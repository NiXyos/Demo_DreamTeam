using dreamteam_mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace dreamteam_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Personnage(int id)
        {
            List<PersonnageModel> lst = new PersonnageModel().GetListePersonnages();
            List<AbilityModel> lstmap = new AbilityModel().GetListeAbilitys();
            foreach (PersonnageModel personnage in lst)
            {
                if (personnage.Id == id)
                {
                    ViewBag.Perso = personnage;
                }
            }
            isConnected();
            return View();
        }

        
            public IActionResult Index()
        {
            ViewBag.Connecte = false;
            List<PersonnageModel> lstperso = new PersonnageModel().GetListePersonnages();
            if (lstperso.Count < 1)
            {
                return View();
            }

            List<MapModel> lstmap = new MapModel().GetListeMaps();
            List<WeaponModel> lstweapon = new WeaponModel().GetListeWeapons();
            ViewBag.Persos = lstperso;
            ViewBag.Maps = lstmap;
            ViewBag.Weapons = lstweapon;
            isConnected();
            return View();
        }

        public IActionResult Authentification()
        {
            //Méthode permettant de savoir si une personne est co afin d'afficher le bon bouton dans la barre de menu
            isConnected();
            return View();
        }

        public IActionResult Deconnexion()
        {
            //Lors de la deconnexion suppression des variables de sessions
            //Et rechargement de la page index
            Index();
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("role");
            isConnected();
            //return View("Index");
            return View();
        }

        public IActionResult Login(string UserName, string Password)
        {
            //Appel de la mthode lançant la requete http
            var response = ApiConnector.Login(UserName, Password);
            //Si c'est un succes on recupére le token et on essaye de récupérer le role
            if (response.Result.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("token", response.Result.Content.ReadAsStringAsync().Result);
                var response2 = ApiConnector.Roles(UserName, Password);
                if (response2.Result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("role", response2.Result.Content.ReadAsStringAsync().Result);
                }
                //Puis on redirige sur la page d'accueil avec un message de succès
                Index();
                isConnected();
                ViewBag.Message = "Connection Succes";

                return View("Index");
            }
            else
            {
                //Sinon on dit que le login à fail pour cause d'user not found
                Console.WriteLine("User not found");
                isConnected();
                ViewBag.Erreur = "Login fail : " + response.Result.ReasonPhrase;
                //Et on retourne sur la meme page
                return View("Authentification");
            }
        }

        public IActionResult AddMap()
        {
            //Verification que l'utilisateur est un admin avant de lui afficher la page d'ajout
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                isConnected();
                return View();
            }
            else
            {
                //Sinon message explicatif
                Index();
                isConnected();
                ViewBag.Message = "Seul les admins peuvent ajouter des maps";
                return View("Index");
            }
        }
        public IActionResult AjoutMap(string Name, string Place, string MapUrl)
        {
            //Appel de la méthode lançant la requete http de création de map en lui passant le token pour vérification des droits
            var response = ApiConnector.PostMap(Name, Place, MapUrl, HttpContext.Session.GetString("token"));
            //Si création réussi on retourne sur la page d'index avec un message de succès
            if (response.Result.IsSuccessStatusCode)
            {
                Index();
                isConnected();
                ViewBag.Message = "Creation reussi";
                return View("Index");
            }
            else
            {
                //Sinon on retourne sur la page de création avec un message d'erreur
                Console.WriteLine("Echec création");
                isConnected();
                ViewBag.Erreur = "Creation failed : " + response.Result.ReasonPhrase;
                return View("AddMap");
            }
        }

        public IActionResult ModifMap(int Id)
        {
            //On vérifie que l'utilisateur est un admin pour autoriser la modif d'une map
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                //On récupère d'abord les infos de la map que l'on veut modifier
                var response = ApiConnector.GetAMap(Id);
                Console.WriteLine(response);
                isConnected();
                if (response.Result.IsSuccessStatusCode)
                {
                    //Si la récupération fonctionne, on va passer l'objet à la page addMap pour afficher les bonnes infos
                    ViewBag.Modif = true;
                    MapModel uneMap = JsonConvert.DeserializeObject<MapModel>(response.Result.Content.ReadAsStringAsync().Result);
                    ViewBag.Name = uneMap.Name;
                    ViewBag.Place = uneMap.Place;
                    ViewBag.MapUrl = uneMap.MapUrl;
                    ViewBag.Id = Convert.ToString(uneMap.Id);
                    return View("AddMap");
                }
                else
                {
                    //Sinon message d'échec
                    Console.WriteLine("Echec récupération map");
                    isConnected();
                    Index();
                    ViewBag.Erreur = "Erreur recuperation map : " + response.Result.ReasonPhrase;
                    return View("Index");
                }
            }
            else
            {
                Index();
                isConnected();
                ViewBag.Message = "Seul les admins peuvent modifier des maps";
                return View("Index");
            }
            
        }

        public IActionResult PutMap(string Id ,string Name, string Place, string MapUrl)
        {
            //Appel à la méthode de modification  de la map
            var response = ApiConnector.PutMap(Id,Name, Place, MapUrl, HttpContext.Session.GetString("token"));
            if (response.Result.IsSuccessStatusCode)
            {
                Index();
                isConnected();
                ViewBag.Message = "Update reussi";
                return View("Index");
            }
            else
            {
                Console.WriteLine("Echec update");
                isConnected();
                ViewBag.Erreur = "Update failed : " + response.Result.ReasonPhrase;
                ModifMap(Convert.ToInt32(Id));
                return View("AddMap");
            }
        }

        public IActionResult SuppressionMap(int Id)
        {
            //Meme fonctionnement mais pour la suppression
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                var response = ApiConnector.DeleteMap(Id, HttpContext.Session.GetString("token"));
                if (response.Result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Suppression reussi";
                }
                else
                {
                    ViewBag.Message = "Suppression échouée : " + response.Result.ReasonPhrase;
                }
                Index();
                isConnected();
                return View("Index");
            }
            else
            {
                Index();
                isConnected();
                ViewBag.Message = "Seul les admins peuvent supprimer des maps";
                return View("Index");
            }
        }

        public IActionResult Map(int id)
        {
            //Récupération des infos d'une map
            var response = ApiConnector.GetAMap(id);
            if (response.Result.IsSuccessStatusCode)
            {
                isConnected();
                ViewBag.Map = JsonConvert.DeserializeObject<MapModel>(response.Result.Content.ReadAsStringAsync().Result);

                return View();
            }
            else
            {
                Console.WriteLine("Echec récupération map");
                isConnected();
                Index();
                ViewBag.Erreur = "Erreur recuperation map : " + response.Result.ReasonPhrase;
                return View("Index");
            }


        }

        public IActionResult Weapon(int id)
        {
            List<WeaponModel> lst = new WeaponModel().GetListeWeapons();
            foreach (WeaponModel weapon in lst)
            {
                if (weapon.Id == id)
                {
                    ViewBag.Weapon = weapon;
                }
            }
            isConnected();
            return View();
        }

        public void isConnected()
        {
            //Permet de savoir si un utilisateur est connecté ou non
            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("token")))
            {
                ViewBag.Connecte = true;
            }
            else
            {
                ViewBag.Connecte = false;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

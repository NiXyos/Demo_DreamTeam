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
            //PersonnageModel perso = new PersonnageModel().GetPersonnage(id);

            var response = ApiConnector.GetAPersonnage(id);

            if (response.Result.IsSuccessStatusCode)
            {
                ViewBag.Perso = JsonConvert.DeserializeObject<PersonnageModel>(response.Result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction("Index");
            }

            isConnected();
            return View();
        }

        public IActionResult AddPersonnage()
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
        public IActionResult AjoutPersonnage(string Name, string Country, string Desc, string IconUrl)
        {
            //Appel de la méthode lançant la requete http de création de map en lui passant le token pour vérification des droits
            var response = ApiConnector.PostPersonnage(Name, Country, Desc, IconUrl, HttpContext.Session.GetString("token"));
            //Si création réussi on retourne sur la page d'index avec un message de succès
            if (response.Result.IsSuccessStatusCode)
            {
                TempData["Message"] = "Creation reussie";
                return RedirectToAction("Index");
            }
            else
            {
                //Sinon on retourne sur la page de création avec un message d'erreur
                //Console.WriteLine("Echec création");
                isConnected();
                ViewBag.Erreur = "Creation failed : " + response.Result.ReasonPhrase;
                return View("AddPersonnage");
            }
        }

        public IActionResult Index()
        {
            ViewBag.Connecte = false;
            List<PersonnageModel> lstperso = new PersonnageModel().GetListePersonnages();
            if (lstperso.Count < 1)
            {
                ViewBag.Erreur = "Erreur pendant la récupération des données personnage";
                return View("Index");
            }

            List<MapModel> lstmap = new MapModel().GetListeMaps();
            List<WeaponModel> lstweapon = new WeaponModel().GetListeWeapons();
            ViewBag.Persos = lstperso;
            ViewBag.Maps = lstmap;
            ViewBag.Weapons = lstweapon;

            isConnected();
            ViewBag.Erreur = (TempData.ContainsKey("Erreur") && !TempData["Erreur"].Equals("")) ? TempData["Erreur"] : null;
            ViewBag.Message = TempData.ContainsKey("Message") && !TempData["Message"].Equals("") ? TempData["Message"] : null;

            return View("Index");
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
            //Index();
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("role");
            ViewBag.Role = "";
            isConnected();
            //return View("Index");
            return RedirectToAction("Index");
        }
        public IActionResult Login(string UserName, string Password)
        {
            if (!TempData.ContainsKey("Erreur")) TempData.Add("Erreur", "");
            if (!TempData.ContainsKey("Message")) TempData.Add("Message", "");

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
                    ViewBag.Message = "Connection Succes";
                    TempData["Message"] = "Connection Succes";
                }
                else
                {
                    ViewBag.Erreur = "Retrieving role fail : " + response2.Result.ReasonPhrase;
                    TempData["Erreur"] = "Retrieving role fail : " + response2.Result.ReasonPhrase;
                    return RedirectToAction("Index");
                }
                TempData.Save();
                //Puis on redirige sur la page d'accueil avec un message de succès
                //Index();
                //isConnected();
                //return View("Index");
                return RedirectToAction("Index");
            }
            else
            {
                //Sinon on dit que le login à fail pour cause d'user not found
                Console.WriteLine("User not found");
                isConnected();
                ViewBag.Erreur = "Login fail : " + response.Result.ReasonPhrase;
                TempData["Erreur"] = "Login fail : " + response.Result.ReasonPhrase;
                TempData.Save();
                //Et on retourne sur la meme page
                //return View("Authentification");
                return RedirectToAction("Authentification");
            }
        }

        // -------- MAPS ------
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
                //Index();
                isConnected();
                //return View("Index");
                return RedirectToAction("Index");
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

        //---------- WEAPONS -------
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
        public IActionResult AddWeapon()
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
        public IActionResult AjoutWeapon(string Name, string Category, string Cost, string Description, string WeaponUrl)
        {
            //Appel de la méthode lançant la requete http de création de map en lui passant le token pour vérification des droits
            var response = ApiConnector.PostWeapon(Name, Category, Cost, Description, WeaponUrl, HttpContext.Session.GetString("token"));
            //Si création réussi on retourne sur la page d'index avec un message de succès
            if (response.Result.IsSuccessStatusCode)
            {
                TempData["Message"] = "Creation reussie";                
                return RedirectToAction("Index");
            }
            else
            {
                //Sinon on retourne sur la page de création avec un message d'erreur
                //Console.WriteLine("Echec création");
                isConnected();
                ViewBag.Erreur = "Creation failed : " + response.Result.ReasonPhrase;
                return View("AddWeapon");
            }
        }

        public void isConnected()
        {
            //Permet de savoir si un utilisateur est connecté ou non
            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString("token")))
            {
                ViewBag.Connecte = true;
                ViewBag.Role = HttpContext.Session.GetString("role");
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

        //OTHERS
        public IActionResult SetItem()
        {
            //Verification que l'utilisateur est un admin avant de lui afficher la page d'ajout
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                isConnected();
                return View("SetPersonnage");  //ON DOIT RETOURNER LA BONNE VUE SELON L'ITEM
            }
            else
            {
                //Sinon message explicatif
                isConnected();
                ViewBag.Message = "Seul les admins peuvent ajouter des éléments";
                return RedirectToAction("Index");
            }
        }
    }
}

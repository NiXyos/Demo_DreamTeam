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
            isConnected();
            return View();
        }

        public IActionResult Deconnexion()
        {
            Index();
            HttpContext.Session.Remove("token");
            isConnected();
            return View("Index");
        }

        public IActionResult Login(string UserName, string Password)
        {
            var response = ApiConnector.Login(UserName, Password);
            if (response.Result.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("token", response.Result.Content.ReadAsStringAsync().Result);
                Index();
                isConnected();
                ViewBag.Message = "Connection Succes";

                return View("Index");
            }
            else
            {
                Console.WriteLine("User not found");
                isConnected();
                ViewBag.Erreur = "Login fail : " + response.Result.ReasonPhrase;
                return View("Authentification");
            }
        }

        public IActionResult AddMap()
        {
            isConnected();
            return View();
        }
        public IActionResult AjoutMap(string Name, string Place, string MapUrl)
        {
            var response = ApiConnector.PostMap(Name, Place, MapUrl, HttpContext.Session.GetString("token"));
            if (response.Result.IsSuccessStatusCode)
            {
                Index();
                isConnected();
                ViewBag.Message = "Creation reussi";
                return View("Index");
            }
            else
            {
                Console.WriteLine("Echec création");
                isConnected();
                ViewBag.Erreur = "Creation failed : " + response.Result.ReasonPhrase;
                return View("AddMap");
            }
        }

        public IActionResult ModifMap(int Id)
        {
            var response = ApiConnector.GetAMap(Id);
            Console.WriteLine(response);
            isConnected();
            if (response.Result.IsSuccessStatusCode)   
            {
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
                Console.WriteLine("Echec récupération map");
                isConnected();
                Index();
                ViewBag.Erreur = "Erreur recuperation map : " + response.Result.ReasonPhrase;
                return View("Index");
            }
            
        }

        public IActionResult PutMap(string Id ,string Name, string Place, string MapUrl)
        {
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

        public IActionResult Map(int id)
        {
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

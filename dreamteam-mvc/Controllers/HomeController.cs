using dreamteam_mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            return View("Index");
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
            var token = ApiConnector.Login(UserName, Password);
            if (token.Result != null)
            {
                HttpContext.Session.SetString("token", token.Result);
                Index();
                isConnected();
                return View("Index");
            }
            else
            {
                Console.WriteLine("User not found");
                isConnected();
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
            var test = ApiConnector.PostMap(Name, Place, MapUrl, HttpContext.Session.GetString("token"));
            Index();
            isConnected();
            return View("Index");
        }

        public IActionResult ModifMap(int Id)
        {
            var map = ApiConnector.GetAMap(Id);
            //var test = ApiConnector.PostMap(Name, Place, MapUrl, HttpContext.Session.GetString("token"));
            //Index();
            isConnected();
            return View("AddMap");
        }

        public IActionResult DeleteMap(int Id)
        {
            var test = ApiConnector.DeleteMap(Id, HttpContext.Session.GetString("token"));
            Index();
            isConnected();
            return View("Index");
        }

        public IActionResult Map(int id)
        {
            List<MapModel> lst = new MapModel().GetListeMaps();
            foreach (MapModel map in lst)
            {
                if (map.Id == id)
                {
                    ViewBag.Map = map;
                }
            }
            isConnected();
            return View();
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

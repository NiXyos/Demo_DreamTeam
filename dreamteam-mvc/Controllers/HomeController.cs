using dreamteam_mvc.Models;
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
        private readonly AuthentificationModel _authentificationModel;

        public HomeController(ILogger<HomeController> logger, AuthentificationModel authentificationModel)
        {
            _logger = logger;
            _authentificationModel = authentificationModel;
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
            return View();
        }

        public IActionResult Index()
        {
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
            return View();
        }

        public IActionResult Authentification()
        {
            //AuthentificationModel.

            return View();
        }

        public IActionResult Login()
        {

            return View();
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

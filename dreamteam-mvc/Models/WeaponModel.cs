using Newtonsoft.Json;
using System.Collections.Generic;

namespace dreamteam_mvc.Models
{
    public class WeaponModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }
        public int DomagesId { get; set; }
        public int WeaponUrl { get; set; }
        public List<WeaponModel> GetListeWeapons()
        {
            string weaponstr = ApiConnector.GetWeapon().Result.ToString();
            List<WeaponModel> weapon = JsonConvert.DeserializeObject<List<WeaponModel>>(weaponstr) as List<WeaponModel>;
            return weapon;
        }
    }
}

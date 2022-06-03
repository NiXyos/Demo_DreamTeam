using Newtonsoft.Json;
using System.Collections.Generic;

namespace dreamteam_mvc.Models
{
    public class UltimateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Orbs { get; set; }
        public string IconUrl { get; set; }
        public int CharacterId { get; set; }

        public List<UltimateModel> GetListePersonnages()
        {
            string ultimatestr = ApiConnector.GetUltimate().Result.ToString();
            List<UltimateModel> ultimate = JsonConvert.DeserializeObject<List<UltimateModel>>(ultimatestr) as List<UltimateModel>;
            return ultimate;
        }
    }
}

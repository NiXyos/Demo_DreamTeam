using Newtonsoft.Json;
using System.Collections.Generic;


namespace dreamteam_mvc.Models
{
    public class AbilityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        public string IconUrl { get; set; }
        public bool IsSignature { get; set; }
        public int Damages { get; set; }
        public int Range { get; set; }
        public int Cooldown { get; set; }
        public int Duration { get; set; }
        public PersonnageModel Charact { get; set; }

        public List<AbilityModel> GetListeAbilitys()
        {
            string abilitystr = ApiConnector.GetAbility().Result.ToString();
            List<AbilityModel> ability = JsonConvert.DeserializeObject<List<AbilityModel>>(abilitystr) as List<AbilityModel>;
            return ability;
        }
    }
}

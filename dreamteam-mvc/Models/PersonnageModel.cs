using Newtonsoft.Json;
using System.Collections.Generic;

namespace dreamteam_mvc.Models
{
    public class PersonnageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public int Ultld { get; set; }
        //public AbilityModel[] Abilities { get; set; }
        public UltimateModel Ult { get; set; }
        public string PicUrl { get; set; }

        public List<PersonnageModel> GetListePersonnages() 
        {
            try
            {
                string persostr = ApiConnector.GetPersonnage().Result.ToString();
                List<PersonnageModel> perso = JsonConvert.DeserializeObject<List<PersonnageModel>>(persostr) as List<PersonnageModel>;
                return perso;
            }
            catch (System.Exception)
            {

                return new List<PersonnageModel>();
            }
            
        }
    }

}

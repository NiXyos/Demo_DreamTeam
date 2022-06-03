using Newtonsoft.Json;
using System.Collections.Generic;

namespace dreamteam_mvc.Models
{
    public class MapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string MapUrl { get; set; }

        public List<MapModel> GetListeMaps()
        {
            string mapstr = ApiConnector.GetMap().Result.ToString();
            List<MapModel> map = JsonConvert.DeserializeObject<List<MapModel>>(mapstr) as List<MapModel>;
            return map;
        }
    }
}

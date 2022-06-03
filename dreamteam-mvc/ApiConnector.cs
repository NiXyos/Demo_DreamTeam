using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace dreamteam_mvc
{
    public static class ApiConnector
    {
        public static async Task<string> GetPersonnage()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", "myUser" },
                { "password", "myPassword" }
            };

            string url = @"https://apivalorant.azurewebsites.net/api/characters";
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsStringAsync().Result;
            
        }
        public static async Task<string> GetWeapon()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", "myUser" },
                { "password", "myPassword" }
            };

            string url = @"https://apivalorant.azurewebsites.net/api/weapons";
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsStringAsync().Result;

        }

        public static async Task<string> GetMap()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", "myUser" },
                { "password", "myPassword" }
            };

            string url = @"https://apivalorant.azurewebsites.net/api/maps";
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsStringAsync().Result;

        }

        public static async Task<string> GetAbility()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", "myUser" },
                { "password", "myPassword" }
            };

            string url = @"https://apivalorant.azurewebsites.net/api/abilitys";
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsStringAsync().Result;

        }


    }
}

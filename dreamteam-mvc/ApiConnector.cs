using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            throw new Exception("Aucune donnée n'a été retrouvé !!!");
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

        public static async Task<string> GetUltimate()
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", "myUser" },
                { "password", "myPassword" }
            };

            string url = @"https://apivalorant.azurewebsites.net/api/ultimates";
            var response = await client.GetAsync(url);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response.Content.ReadAsStringAsync().Result;

        }

        public static async Task<string> Login(string UserName, string Password)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", UserName },
                { "password", Password }
            };
            string url = @"https://apivalorant.azurewebsites.net/api/login";
            string Serialized = JsonConvert.SerializeObject(values);
            HttpContent content = new StringContent(Serialized, System.Text.Encoding.Unicode, "application/json");
            //var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var token = response.Content.ReadAsStringAsync().Result;
                return token;
            }
            else
            {
                return null;
            }
            //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            

            //throw new Exception("Aucune donnée n'a été retrouvé !!!");
        }


    }
}

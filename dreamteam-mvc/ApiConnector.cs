using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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

        public static async Task<HttpResponseMessage> Login(string UserName, string Password)
        {
            //Création de la requete http Post de login
            HttpClient client = new HttpClient();
            //le dictionnaire contient les élément que l'on veut envoyer dans le body de la requete
            var values = new Dictionary<string, string>
            {
                { "username", UserName },
                { "password", Password }
            };
            string url = @"https://apivalorant.azurewebsites.net/api/login";
            //On converti le dictionnaire en string au format json
            string Serialized = JsonConvert.SerializeObject(values);
            //On crée le content de la requete à partir du string json
            HttpContent content = new StringContent(Serialized, System.Text.Encoding.Unicode, "application/json");
            //On lance la requete
            var response = await client.PostAsync(url, content);
            //Si tout se passe bien on retourne la réponse, pour pouvoir traiter les messages d'erreurs
            return response;
        }

        public static async Task<HttpResponseMessage> Roles(string UserName, string Password)
        {
            //Lancement d'une requete similaire à celle de login, mais permettant de récupérer le role au lieu du token
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "username", UserName },
                { "password", Password }
            };
            string url = @"https://apivalorant.azurewebsites.net/api/roles";
           // string url = @"https://localhost:44348/api/roles";
            string Serialized = JsonConvert.SerializeObject(values);
            HttpContent content = new StringContent(Serialized, System.Text.Encoding.Unicode, "application/json");
            var response = await client.PostAsync(url, content);
            return response;
        }

        public static async Task<HttpResponseMessage> PostMap(string Name, string Place, string MapUrl, string token)
        {
            //Lancement de la requete permettant l'ajout d'une map
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "name", Name },
                { "place", Place },
                { "mapUrl", MapUrl }
            };
            string url = @"https://apivalorant.azurewebsites.net/api/maps";
            string Serialized = JsonConvert.SerializeObject(values);
            HttpContent content = new StringContent(Serialized, System.Text.Encoding.Unicode, "application/json");
            //Passage du token en tant que bearer authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync(url, content);
            return response;
        }

        public static async Task<HttpResponseMessage> DeleteMap(int Id, string token)
        {
            //Lancement de la requete permettant la suppression d'une map
            HttpClient client = new HttpClient();
            string url = @"https://apivalorant.azurewebsites.net/api/maps/" + Id;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync(url);
                return response;
        }

        public static async Task<HttpResponseMessage> GetAMap(int Id)
        {
            //Lancement de la requete permettant de récupérer les informations d'une map
            HttpClient client = new HttpClient();
            string url = @"https://apivalorant.azurewebsites.net/api/maps/" + Id;
            var response = await client.GetAsync(url);
            //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                return response;
        }

        public static async Task<HttpResponseMessage> PutMap(string Id,string Name, string Place, string MapUrl, string token)
        {
            //Lancement de la requete http permettant la modification d'une map
            HttpClient client = new HttpClient();
            /*var values = new Dictionary<string, string>
            {
                { "name", Name },
                { "place", Place },
                { "mapUrl", MapUrl }
            };*/
            

            string url = @"https://apivalorant.azurewebsites.net/api/maps/" + Id;
            //Etant donné que le put doit recevoir aussi l'id de la map dans le body, et que l'id est un string, la méthode du dictionnaire n'est pas possible
            // création d'un objet json correspondant au body
            var json = new JObject(new JProperty("id", Convert.ToInt32(Id)), new JProperty("name", Name), new JProperty("place", Place), new JProperty("mapUrl", MapUrl));
            //Puis application du même process
            HttpContent content = new StringContent(json.ToString(), System.Text.Encoding.Unicode, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PutAsync(url, content);
            return response;
        }


    }
}

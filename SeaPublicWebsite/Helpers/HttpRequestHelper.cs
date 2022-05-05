using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SeaPublicWebsite.Helpers
{
    public static class HttpRequestHelper
    {
        public static T SendGetRequest<T>(string baseAddress, string path, AuthenticationHeaderValue auth)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Authorization = auth;
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = httpClient.GetAsync(path).Result;

            if (!response.IsSuccessStatusCode) return default;
            
            var bodyString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(bodyString);
        }

        public static string ConvertToBase64(string username, string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        }
    }
}
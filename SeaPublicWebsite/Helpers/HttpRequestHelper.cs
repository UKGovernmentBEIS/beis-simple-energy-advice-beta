using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SeaPublicWebsite.Helpers
{
    public static class HttpRequestHelper
    {
        public static T SendGetRequest<T>(RequestParameters parameters)
        {
            return SendRequest<T>(RequestType.Get, parameters);
        }
        
        public static T SendPostRequest<T>(RequestParameters parameters)
        {
            return SendRequest<T>(RequestType.Post, parameters);
        }

        public static string ConvertToBase64(string username, string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        }
        private static T SendRequest<T>(RequestType requestType, RequestParameters parameters)
        {
            var httpClient = SetupHttpClient(parameters);
            var response = requestType switch
            {
                RequestType.Get => httpClient.GetAsync(parameters.Path).Result,
                RequestType.Post => httpClient.PostAsync(parameters.Path, parameters.Body).Result,
                _ => throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null)
            };
            return ConvertResponseToObject<T>(response);
        }

        private static HttpClient SetupHttpClient(RequestParameters parameters)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(parameters.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = parameters.Auth;
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return httpClient;
        }

        private static T ConvertResponseToObject<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return default;
            
            var bodyString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(bodyString);
        }
    }

    public class RequestParameters
    {
        public string BaseAddress { get; set; }
        public string Path { get; set; }
        public AuthenticationHeaderValue Auth { get; set; }
        public HttpContent Body { get; set; }
    }

    internal enum RequestType
    {
        Get,
        Post,
    }
}
// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Models.EnergyEfficiency;

var testBreRequest = new BreRequest()
{
    epc = new RequestEpc()
    {
        postcode = "A12 3BC",
        isConnectedtoMainsGas = true,
        propertyType = "house",
        builtForm = "mid-terrace",
        floorLevel = null,
        heatLossCorridor = "no corridor",
        numberHabitableRooms = 7,
        totalFloorArea = "151.26",
        wallsDescription = "Solid brick, as built, no insulation (assumed)",
        roofDescription = "Pitched, no insulation (assumed)",
        multiGlazeProportion = "30",
        glazedArea = "Normal",
        mainheatDescription = "boiler and radiators, mains gas",
        mainheatcontDescription = "Programmer, no room thermostat",
        numberOpenFireplaces = "1",
        lowEnergyLighting = "80",
        solarWaterHeatingFlag = "N",
        photoSupply = "0",
        windTurbineCount = "0",
    },
    construction_date = "C",
    heating_fuel = "26",
    electricity_tariff = 1,
    num_storeys = 2,
    hot_water_cylinder = true,
    condensing_boiler = true,
    heating_pattern_type = 3,
    normal_days_off_hours = new[] {7, 9},
    living_room_temperature = 22,
    showers_per_week = 0,
    baths_per_week = 7,
    measures = true,
    measures_package = new[] {"A", "W1", "G", "U"},
};

string request = JsonConvert.SerializeObject(testBreRequest);

const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
Random random = new();
var nonce = new string(Enumerable.Repeat(chars, 64)
    .Select(s => s[random.Next(s.Length)]).ToArray());

string created = DateTime.Now.ToUniversalTime().ToString
    (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";


var username = "softwire0.0.1";

var password = "9b052a18df10baade8c7448f3";

String sha256_hash(String value) {
    using (SHA256 hash = SHA256.Create()) {
        return String.Concat(hash
            .ComputeHash(Encoding.UTF8.GetBytes(value))
            .Select(item => item.ToString("x2")));
    }
}
string token = sha256_hash(password + nonce + username + created);
var wsse_header = string.Join("", "WSSE UsernameToken ", "Token=\"", token, "\", ", "Nonce=\"", nonce, "\", ", "Username=\"", username, "\", ", "Created=\"", created, "\"");

using (var httpClient = new HttpClient())
{
    
    httpClient.DefaultRequestHeaders.Add("Authorization", wsse_header);
    httpClient.BaseAddress = new Uri("https://uat.brewebserv.com");

    string path = "/bemapi/energy_use";
    StringContent stringContent = new(request);

    HttpResponseMessage response = httpClient.PostAsync(path, stringContent).Result;

    if (response.IsSuccessStatusCode)
    {
        string bodyString = response.Content.ReadAsStringAsync().Result;
        JObject measures = JObject.FromObject(JObject.Parse(bodyString)["measures"]);

        IList<Recommendation> recommendations = new List<Recommendation>();
        foreach (JProperty prop in measures.Properties())
        {
            var value = prop.Value;
            int minInstallCost = (int) value["min_installation_cost"];
            int maxInstallCost = (int) value["max_installation_cost"];
            int saving = (int) value["cost_saving"];
            int lifetime = (int) value["lifetime"];

            Recommendation recommendation = new()
            {
                Key = RecommendationKey.AddLoftInsulation,
                Title = prop.Name,
                MinInstallCost = minInstallCost,
                MaxInstallCost = maxInstallCost,
                Saving = saving,
                LifetimeSaving = lifetime * saving,
                Lifetime = lifetime,
                Summary = prop.Name
            };
            recommendations.Add(recommendation);
        }
        
        foreach (var recommendation in recommendations)
        {
            Console.WriteLine(recommendation.Title);
        }
    }
}
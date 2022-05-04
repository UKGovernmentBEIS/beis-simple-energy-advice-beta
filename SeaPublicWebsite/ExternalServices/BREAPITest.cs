// // See https://aka.ms/new-console-template for more information
//
// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Linq;
// using System.Net.Http;
// using System.Security.Cryptography;
// using System.Text;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using SeaPublicWebsite.DataModels;
// using SeaPublicWebsite.ExternalServices.Models;
// using SeaPublicWebsite.Models.EnergyEfficiency;
//
// var exampleCase3Request = new BreRequest
// {
//     epc = new RequestEpc
//     {
//         postcode = "A12 3BC",
//         isConnectedtoMainsGas = true,
//         propertyType = "house",
//         builtForm = "mid-terrace",
//         floorLevel = null,
//         heatLossCorridor = "no corridor",
//         numberHabitableRooms = 7,
//         totalFloorArea = "151.26",
//         wallsDescription = "Solid brick, as built, no insulation (assumed)",
//         roofDescription = "Pitched, no insulation (assumed)",
//         multiGlazeProportion = "30",
//         glazedArea = "Normal",
//         mainheatDescription = "boiler and radiators, mains gas",
//         mainheatcontDescription = "Programmer, no room thermostat",
//         numberOpenFireplaces = "1",
//         lowEnergyLighting = "80",
//         solarWaterHeatingFlag = "N",
//         photoSupply = "0",
//         windTurbineCount = "0"
//     },
//     construction_date = "C",
//     heating_fuel = "26",
//     electricity_tariff = 1,
//     num_storeys = 2,
//     hot_water_cylinder = true,
//     condensing_boiler = true,
//     heating_pattern_type = 3,
//     normal_days_off_hours = new[] {7, 9},
//     living_room_temperature = 22,
//     showers_per_week = 0,
//     baths_per_week = 7,
//     measures = true,
//     measures_package = new[] {"A", "W1", "G", "U"}
// };
//
// var request = JsonConvert.SerializeObject(exampleCase3Request);
//
// const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
// Random random = new();
// var nonce = new string(Enumerable.Repeat(chars, 64)
//     .Select(s => s[random.Next(s.Length)]).ToArray());
//
// var created = DateTime.Now.ToUniversalTime().ToString
//     (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";
//
//
// var username = "softwire0.0.1";
//
// var password = "9b052a18df10baade8c7448f3";
//
// string sha256_hash(string value)
// {
//     using (var hash = SHA256.Create())
//     {
//         return string.Concat(hash
//             .ComputeHash(Encoding.UTF8.GetBytes(value))
//             .Select(item => item.ToString("x2")));
//     }
// }
//
// var token = sha256_hash(password + nonce + username + created);
// var wsse_header = string.Join("", "WSSE UsernameToken ", "Token=\"", token, "\", ", "Nonce=\"", nonce, "\", ",
//     "Username=\"", username, "\", ", "Created=\"", created, "\"");
//
// using (var httpClient = new HttpClient())
// {
//     httpClient.DefaultRequestHeaders.Add("Authorization", wsse_header);
//     httpClient.BaseAddress = new Uri("https://uat.brewebserv.com");
//
//     var path = "/bemapi/energy_use";
//     StringContent stringContent = new(request);
//
//     var response = httpClient.PostAsync(path, stringContent).Result;
//
//     if (response.IsSuccessStatusCode)
//     {
//         var bodyString = response.Content.ReadAsStringAsync().Result;
//         var measures = JObject.FromObject(JObject.Parse(bodyString)["measures"]);
//
//         IList<Recommendation> recommendations = new List<Recommendation>();
//         foreach (var prop in measures.Properties())
//         {
//             var recommendationDictionary = new Dictionary<string, Recommendation>
//             {
//                 {
//                     "A", new Recommendation
//                     {
//                         Key = RecommendationKey.AddLoftInsulation,
//                         Title = "Add some loft insulation",
//                         Summary = "Increase the level of insulation in your loft to the recommended level of 300mm"
//                     }
//                 },
//                 {
//                     "B", new Recommendation
//                     {
//                         Key = RecommendationKey.InsulateCavityWalls,
//                         Title = "Insulate your cavity walls",
//                         Summary = "Inject insulation into the cavity in your external walls"
//                     }
//                 },
//                 {
//                     "G", new Recommendation
//                     {
//                         Key = RecommendationKey.UpgradeHeatingControls,
//                         Title = "Upgrade your heating controls",
//                         Summary = "Fit a programmer, thermostat and thermostatic radiator valves"
//                     }
//                 },
//                 {
//                     "O3", new Recommendation
//                     {
//                         Key = RecommendationKey.FitNewWindows,
//                         Title = "Fit new windows",
//                         Summary = "Replace old single glazed windows with new double or triple glazing"
//                     }
//                 },
//                 {
//                     "U", new Recommendation
//                     {
//                         Key = RecommendationKey.SolarElectricPanels,
//                         Title = "Fit solar electric panels",
//                         Summary = "Install PV panels on your roof to generate electricity"
//                     }
//                 },
//             };
//             var value = prop.Value;
//             var minInstallCost = (int) value["min_installation_cost"];
//             var maxInstallCost = (int) value["max_installation_cost"];
//             var saving = (int) value["cost_saving"];
//             var lifetime = (int) value["lifetime"];
//
//             Recommendation recommendation = new()
//             {
//                 Key = recommendationDictionary[prop.Name].Key,
//                 Title = recommendationDictionary[prop.Name].Title,
//                 MinInstallCost = minInstallCost,
//                 MaxInstallCost = maxInstallCost,
//                 Saving = saving,
//                 LifetimeSaving = lifetime * saving,
//                 Lifetime = lifetime,
//                 Summary = recommendationDictionary[prop.Name].Summary,
//             };
//             recommendations.Add(recommendation);
//         }
//
//         foreach (var recommendation in recommendations) Console.WriteLine($"{recommendation.Title}: {recommendation.Summary}");
//     }
// }
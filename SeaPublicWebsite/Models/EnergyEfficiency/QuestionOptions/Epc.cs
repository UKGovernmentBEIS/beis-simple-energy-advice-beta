using System;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public class Epc
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string EpcId { get; set; }
        public string BuildingReference { get; set; }
        public string InspectionDate { get; set; }
        public PropertyType? PropertyType { get; set; }
        public HeatingType? HeatingType { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }


        public int? GetHouseNumber() {
            var houseNumberFromFirstLine = Epc.GetIntegerFromStartOfString(this.Address1);
            var houseNumberFromSecondLine = Epc.GetIntegerFromStartOfString(this.Address2);
            return houseNumberFromFirstLine ?? houseNumberFromSecondLine;
        }

        private static int? GetIntegerFromStartOfString(string input) {
            var regex = new Regex("^[0-9]+", RegexOptions.IgnoreCase);
            var regexMatches = regex.Match(input);

            var number = 0;
            var result = false;

            if (regexMatches.Success)
            {
                var numberAsString = regexMatches.Groups[0].Value;
                result = Int32.TryParse(numberAsString, out number);
            }

            return result ? number : null;
        }
}
}


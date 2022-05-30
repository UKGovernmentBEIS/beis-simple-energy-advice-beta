using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using GovUkDesignSystem.Helpers;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public class Epc
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string EpcId { get; set; }
        public string InspectionDate { get; set; }
        public PropertyType? PropertyType { get; set; }
        public HeatingType? HeatingType { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }
        public FloorConstruction? FloorConstruction { get; set; }
        public FloorInsulated? FloorInsulated { get; set; }
        public HomeAge? ConstructionAgeBand { get; set; }
    }

    public class EpcInformation
    {
        public string EpcId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }

        public EpcInformation FixFormatting()
        {
            var tI = new CultureInfo("en-GB", false).TextInfo;
            Address1 = tI.ToTitleCase(Address1.ToLower().Replace(",", ""));
            Address2 = tI.ToTitleCase(Address2.ToLower().Replace(",", ""));
            return this;
        }

        public static List<EpcInformation> SortEpcsInformation(List<EpcInformation> epcsInformation)
        {
            epcsInformation.Sort(SortEpcsInformationByHouseNumberOrAlphabetically);
            return epcsInformation;
        }
        
        private static int SortEpcsInformationByHouseNumberOrAlphabetically(EpcInformation epcInformation1, EpcInformation epcInformation2)
        {
            return (epcInformation1.GetHouseNumber(), epcInformation2.GetHouseNumber()) switch
            {
                (null, null) => SortEpcsInformationAlphabetically(epcInformation1, epcInformation2),
                (null, _) => 1,
                (_, null) => -1,
                var (houseNumber1, houseNumber2) => houseNumber1.Value - houseNumber2.Value switch
                {
                    <0 => -1,
                    >0 => 1,
                    _ => SortEpcsInformationAlphabetically(epcInformation1, epcInformation2)
                }
            };
        }

        private static int SortEpcsInformationAlphabetically(EpcInformation epcInformation1, EpcInformation epcInformation2)
        {
            return string.Compare(epcInformation1.Address2, epcInformation2.Address2, StringComparison.OrdinalIgnoreCase) == 0
                ? string.Compare(epcInformation1.Address1, epcInformation2.Address1, StringComparison.OrdinalIgnoreCase)
                : string.Compare(epcInformation1.Address2, epcInformation2.Address2, StringComparison.OrdinalIgnoreCase);
        }
        
        
        private int? GetHouseNumber() {
            var houseNumberFromFirstLine = GetIntegerFromStartOfString(Address1);
            var houseNumberFromSecondLine = GetIntegerFromStartOfString(Address2);
            return houseNumberFromFirstLine ?? houseNumberFromSecondLine;
        }

        private int? GetIntegerFromStartOfString(string input) {
            var regex = new Regex("^[0-9]+", RegexOptions.IgnoreCase);
            var regexMatches = regex.Match(input);

            var number = 0;
            var result = false;

            if (regexMatches.Success)
            {
                var numberAsString = regexMatches.Groups[0].Value;
                result = int.TryParse(numberAsString, out number);
            }

            return result ? number : null;
        }
    }
}


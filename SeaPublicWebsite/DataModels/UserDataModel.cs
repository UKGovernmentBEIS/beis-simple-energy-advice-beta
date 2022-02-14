using System;
using System.Collections.Generic;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.DataModels
{
    public class UserDataModel
    {
        public string Reference { get; set; }
        public OwnershipStatus? OwnershipStatus { get; set; }
        public Country? Country { get; set; }
        public UserGoal? UserGoal { get; set; }
        public List<UserInterests> UserInterests { get; set; }
        public string Postcode { get; set; }
        public string EpcLmkKey { get; set; }
        public HomeType? HomeType { get; set; }
        public int YearBuilt { get; set; }
        public WallType? WallType { get; set; }
        public RoofConstruction? RoofConstruction { get; set; }
    }
}
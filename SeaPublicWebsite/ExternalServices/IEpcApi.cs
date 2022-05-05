using System.Collections.Generic;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public interface IEpcApi
    { 
        public List<Epc> GetEpcsForPostcode(string postcode);
    }
}
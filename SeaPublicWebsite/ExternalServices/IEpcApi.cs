using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public interface IEpcApi
    { 
        public Task<List<Epc>> GetEpcsForPostcode(string postcode);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.ExternalServices
{
    public interface IEpcApi
    { 
        public Task<List<Epc>> GetEpcsForPostcode(string postcode);
    }
}
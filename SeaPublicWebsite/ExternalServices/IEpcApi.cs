using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.ExternalServices
{
    public interface IEpcApi
    {
        public Task<List<EpcInformation>> GetEpcsInformationForPostcodeAndBuildingNameOrNumber(string postcode,
            string buildingNameOrNumber = null);
        public Task<Epc> GetEpcForId(string epcId);
    }
}
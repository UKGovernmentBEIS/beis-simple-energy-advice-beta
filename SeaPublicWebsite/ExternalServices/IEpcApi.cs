using System.Collections.Generic;
using System.Threading.Tasks;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public interface IEpcApi
    {
        public Task<List<EpcInformation>> GetEpcsInformationForPostcode(string postcode);
        public Task<Epc> GetEpcForId(string epcId);
    }
}
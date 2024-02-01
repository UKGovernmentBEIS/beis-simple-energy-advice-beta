using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc
{
    public interface IEpcApi
    {
        public Task<List<EpcSearchResult>> GetEpcsInformationForPostcodeAndBuildingNameOrNumber(string postcode,
            string buildingNameOrNumber = null);
        public Task<EpbEpcAssessmentDto> GetEpcDtoForId(string epcId);
        public Task<Epc> GetEpcForId(string epcId);
    }
}
using System.Net;
using System.Net.Http.Headers;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Common;
using SeaPublicWebsite.BusinessLogic.Models;
using Microsoft.Extensions.Logging;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.HistoricEngland
{
    public class HistoricEnglandApi
    {
        private readonly ILogger<HistoricEnglandApi> logger;
        
        public HistoricEnglandApi(ILogger<HistoricEnglandApi> logger)
        {
            this.logger = logger;
        }

        public async Task<List<HistoricEnglandSearchResult>> GetListedBuildingsForAddress(string address)
        {
            // var query = "Name%20%3D%20'247,%20FRATTON%20ROAD'";
            var query = "where=Name%20%3D%20'CHURCH%20OF%20ST%20JAMES'&outFields=*&returnGeometry=false&outSR=&f=json";
            HistoricEnglandDto response = null;
            try
            {
                response = await HttpRequestHelper.SendGetRequestAsync<HistoricEnglandDto>(
                    new RequestParameters()
                    {
                        BaseAddress = "https://services-eu1.arcgis.com",
                        Path =
                            $"/ZOdPfBS3aqqDYPUQ/arcgis/rest/services/National_Heritage_List_for_England_NHLE/FeatureServer/0/query?{query}"
                    });
            }
            catch (ApiException e)
            {
                if (e.StatusCode is not HttpStatusCode.NotFound)
                {
                    logger.LogError("There was an error sending a request to the Historic England api: {}", e.Message);
                }

                return new List<HistoricEnglandSearchResult>();
            }

            var listedBuildings = response.Features.Select(feature =>
                new HistoricEnglandSearchResult(
                    feature.Attributes.ObjectId,
                    feature.Attributes.ListEntry,
                    feature.Attributes.Name,
                    feature.Attributes.Grade,
                    feature.Attributes.ListDate,
                    feature.Attributes.AmendDate,
                    feature.Attributes.CaptureScale,
                    feature.Attributes.Hyperlink)).ToList();

            return listedBuildings;
        }
    }   
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Services.EnergyEfficiency.Epc;

public class EpcFilterService
{
    private readonly IEpcApi epcApi;

    public EpcFilterService(IEpcApi epcApi)
    {
        this.epcApi = epcApi;
    }
    
    public async Task<List<EpcSearchResult>> FilterEpcsNotUniqueByAddressToMostRecent(IEnumerable<EpcSearchResult> searchResults)
    {
        
        var epcSearchResultsGroupedByAddress = searchResults.GroupBy(
            epcSearchResult => new
            {
                epcSearchResult.Address1, 
                epcSearchResult.Address2, 
                epcSearchResult.Postcode
            });


        var epcSearchResultsNotUniqueByAddress = epcSearchResultsGroupedByAddress
            .Where(grouping => grouping.Count() > 1)
            .SelectMany(grouping => grouping);
        var epcSearchResultsUniqueByAddress = epcSearchResultsGroupedByAddress
            .Where(grouping => grouping.Count() == 1)
            .SelectMany(grouping => grouping);
            
        var epcSearchResultsWithAdditionalEpcInformationTaskList = epcSearchResultsNotUniqueByAddress.Select(async epc => new KeyValuePair<EpcSearchResult, BusinessLogic.Models.Epc>(epc, await epcApi.GetEpcForId(epc.EpcId)));

        var epcSearchResultsWithAdditionalEpcInformation = await Task.WhenAll(epcSearchResultsWithAdditionalEpcInformationTaskList);

        var epcSearchResultsNotUniqueByAddressWithOlderDuplicatesRemoved = epcSearchResultsWithAdditionalEpcInformation
            .Where(epcDtoPair => epcDtoPair.Value.IsLatestAssessmentForAddress)
            .Select(epcDtoPair => epcDtoPair.Key);

        return epcSearchResultsUniqueByAddress.Concat(epcSearchResultsNotUniqueByAddressWithOlderDuplicatesRemoved).ToList();
    }
}

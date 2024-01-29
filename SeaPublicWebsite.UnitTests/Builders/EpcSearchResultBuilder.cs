using SeaPublicWebsite.BusinessLogic.Models;

namespace Tests.Builders;

public class EpcSearchResultBuilder
{
    private EpcSearchResult epcSearchResult;

    public EpcSearchResultBuilder(string id)
    {
        epcSearchResult = new EpcSearchResult(
            id,
            $"Address {id} line 1",
             $"Address {id} line 2",
             $"Postcode {id}"
        );
    }

    public EpcSearchResult Build()
    {
        return epcSearchResult;
    }

    public EpcSearchResultBuilder WithAddress(string address1, string address2)
    {
        epcSearchResult.Address1 = address1;
        epcSearchResult.Address2 = address2;
        return this;
    }
    
    public EpcSearchResultBuilder WithPostcode(string postcode)
    {
        epcSearchResult.Postcode = postcode;
        return this;
    }
}
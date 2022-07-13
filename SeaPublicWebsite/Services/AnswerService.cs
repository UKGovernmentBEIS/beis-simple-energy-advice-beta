using System;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.BusinessLogic.Services;
using SeaPublicWebsite.DataStores;
using SeaPublicWebsite.ExternalServices;

namespace SeaPublicWebsite.Services;

public class AnswerService
{
    private readonly PropertyDataStore propertyDataStore;
    private readonly QuestionFlowService questionFlowService;
    private readonly IEpcApi epcApi;
    
    public AnswerService(
        PropertyDataStore propertyDataStore,
        QuestionFlowService questionFlowService,
        IEpcApi epcApi)
    {
        this.epcApi = epcApi;
        this.questionFlowService = questionFlowService;
        this.propertyDataStore = propertyDataStore;
    }
    
    public async Task<QuestionFlowStep> UpdateOwnershipStatus(string reference, OwnershipStatus? ownershipStatus)
    {
        return await UpdatePropertyData(
            p => { p.OwnershipStatus = ownershipStatus; },
            reference,
            QuestionFlowStep.OwnershipStatus);
    }
    
    public async Task<QuestionFlowStep> UpdateCountry(string reference, Country? country)
    {
        return await UpdatePropertyData(
            p => { p.Country = country; },
            reference,
            QuestionFlowStep.Country);
    }
    
    public async Task<QuestionFlowStep> UpdateSearchForEpc(string reference, SearchForEpc? searchForEpc)
    {
        return await UpdatePropertyData(
            propertyData =>
            {
                propertyData.SearchForEpc = searchForEpc;
                propertyData.EpcDetailsConfirmed = null;
                propertyData.Epc = null;
                propertyData.PropertyType = null;
                propertyData.YearBuilt = null;
            },
            reference,
            QuestionFlowStep.FindEpc);
    }
    
    public async Task<QuestionFlowStep> SetEpc(string reference, string epcId)
    {
        var epc = epcId == null ? null : await epcApi.GetEpcForId(epcId);

        return await UpdatePropertyData(
            p => { p.Epc = epc; },
            reference,
            QuestionFlowStep.ConfirmAddress);
    }

    public async Task<QuestionFlowStep> ConfirmEpcDetails(string reference, EpcDetailsConfirmed? confirmed)
    {
        return await UpdatePropertyData(
            propertyData =>
            {
                propertyData.EpcDetailsConfirmed = confirmed;
                Epc epc = propertyData.Epc;
                if (confirmed == EpcDetailsConfirmed.Yes)
                {
                    propertyData.PropertyType = epc.PropertyType;
                    propertyData.HouseType = epc.HouseType;
                    propertyData.BungalowType = epc.BungalowType;
                    propertyData.FlatType = epc.FlatType;
                    propertyData.YearBuilt = epc.ConstructionAgeBand switch
                    {
                        HomeAge.Pre1900 => YearBuilt.Pre1930,
                        HomeAge.From1900To1929 => YearBuilt.Pre1930,
                        HomeAge.From1930To1949 => YearBuilt.From1930To1966,
                        HomeAge.From1950To1966 => YearBuilt.From1930To1966,
                        HomeAge.From1967To1975 => YearBuilt.From1967To1982,
                        HomeAge.From1976To1982 => YearBuilt.From1967To1982,
                        HomeAge.From1983To1990 => YearBuilt.From1983To1995,
                        HomeAge.From1991To1995 => YearBuilt.From1983To1995,
                        HomeAge.From1996To2002 => YearBuilt.From1996To2011,
                        HomeAge.From2003To2006 => YearBuilt.From1996To2011,
                        HomeAge.From2007To2011 => YearBuilt.From1996To2011,
                        HomeAge.From2012ToPresent => YearBuilt.From2012ToPresent,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            },
            reference,
            QuestionFlowStep.ConfirmEpcDetails);
    }
    
    public async Task<QuestionFlowStep> UpdatePropertyType(
        string reference,
        PropertyType? propertyType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.PropertyType = propertyType; },
            reference,
            QuestionFlowStep.PropertyType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateHouseType(
        string reference,
        HouseType? houseType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.HouseType = houseType; },
            reference,
            QuestionFlowStep.HouseType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateBungalowType(
        string reference,
        BungalowType? bungalowType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.BungalowType = bungalowType; },
            reference,
            QuestionFlowStep.BungalowType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateFlatType(
        string reference,
        FlatType? flatType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.FlatType = flatType; },
            reference,
            QuestionFlowStep.FlatType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateYearBuilt(
        string reference,
        YearBuilt? yearBuilt,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.YearBuilt = yearBuilt; },
            reference,
            QuestionFlowStep.HomeAge,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateWallConstruction(
        string reference,
        WallConstruction? wallConstruction,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.WallConstruction = wallConstruction; },
            reference,
            QuestionFlowStep.WallConstruction,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateCavityWallInsulation(
        string reference,
        CavityWallsInsulated? cavityWallsInsulated,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.CavityWallsInsulated = cavityWallsInsulated; },
            reference,
            QuestionFlowStep.CavityWallsInsulated,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateSolidWallInsulation(
        string reference,
        SolidWallsInsulated? solidWallsInsulated,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.SolidWallsInsulated = solidWallsInsulated; },
            reference,
            QuestionFlowStep.SolidWallsInsulated,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateFloorConstruction(
        string reference,
        FloorConstruction? floorConstruction,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.FloorConstruction = floorConstruction; },
            reference,
            QuestionFlowStep.FloorConstruction,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateFloorInsulated(
        string reference,
        FloorInsulated? floorInsulated,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.FloorInsulated = floorInsulated; },
            reference,
            QuestionFlowStep.FloorInsulated,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateRoofConstruction(
        string reference,
        RoofConstruction? roofConstruction,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.RoofConstruction = roofConstruction; },
            reference,
            QuestionFlowStep.RoofConstruction,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateLoftSpace(
        string reference,
        LoftSpace? loftSpace,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.LoftSpace = loftSpace; },
            reference,
            QuestionFlowStep.LoftSpace,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateLoftAccess(
        string reference,
        LoftAccess? loftAccess,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.LoftAccess = loftAccess; },
            reference,
            QuestionFlowStep.LoftAccess,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateRoofInsulated(
        string reference,
        RoofInsulated? roofInsulated,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.RoofInsulated = roofInsulated; },
            reference,
            QuestionFlowStep.RoofInsulated,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateGlazingType(
        string reference,
        GlazingType? glazingType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.GlazingType = glazingType; },
            reference,
            QuestionFlowStep.GlazingType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateHasOutdoorSpace(
        string reference,
        HasOutdoorSpace? hasOutdoorSpace,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.HasOutdoorSpace = hasOutdoorSpace; },
            reference,
            QuestionFlowStep.OutdoorSpace,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateHeatingType(
        string reference,
        HeatingType? heatingType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.HeatingType = heatingType; },
            reference,
            QuestionFlowStep.HeatingType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateOtherHeatingType(
        string reference,
        OtherHeatingType? otherHeatingType,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.OtherHeatingType = otherHeatingType; },
            reference,
            QuestionFlowStep.OtherHeatingType,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateHasHotWaterCylinder(
        string reference,
        HasHotWaterCylinder? hasHotWaterCylinder,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.HasHotWaterCylinder = hasHotWaterCylinder; },
            reference,
            QuestionFlowStep.HotWaterCylinder,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateNumberOfOccupants(
        string reference,
        int? numberOfOccupants,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.NumberOfOccupants = numberOfOccupants; },
            reference,
            QuestionFlowStep.NumberOfOccupants,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateHeatingPattern(
        string reference,
        HeatingPattern? heatingPattern,
        int? hoursOfHeatingMorning,
        int? hoursOfHeatingEvening,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p =>
            {
                p.HeatingPattern = heatingPattern;
                p.HoursOfHeatingMorning = hoursOfHeatingMorning;
                p.HoursOfHeatingEvening = hoursOfHeatingEvening;
            },
            reference,
            QuestionFlowStep.HeatingPattern,
            entryPoint);
    }
    
    public async Task<QuestionFlowStep> UpdateTemperature(
        string reference,
        decimal? temperature,
        QuestionFlowStep? entryPoint)
    {
        return await UpdatePropertyData(
            p => { p.Temperature = temperature; },
            reference,
            QuestionFlowStep.Temperature,
            entryPoint);
    }
    
    private async Task<QuestionFlowStep> UpdatePropertyData(
        Action<PropertyData> update,
        string reference,
        QuestionFlowStep currentPage,
        QuestionFlowStep? entryPoint = null)
    {
        var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
            
        // If entryPoint is set then the user is editing their answers (and if HasSeenRecommendations then they have
        // already generated recommendations that may now need to change), so we need to take a copy of the current
        // answers
        if ((entryPoint is not null || propertyData.HasSeenRecommendations) && propertyData.UneditedData is null)
        {
            propertyData.CreateUneditedData();
        }
        
        update(propertyData);
        propertyData.ResetUnusedFields();
        
        var nextStep = questionFlowService.NextStep(currentPage, propertyData, entryPoint);
            
        // If the user is going back to the answer summary page or the check your unchangeable answers page then they
        // finished editing and we can get rid of the old answers
        if ((entryPoint is not null || propertyData.HasSeenRecommendations) &&
            (nextStep == QuestionFlowStep.AnswerSummary ||
             nextStep == QuestionFlowStep.CheckYourUnchangeableAnswers))
        {
            propertyData.CommitEdits();
        }
        await propertyDataStore.SavePropertyDataAsync(propertyData);

        return nextStep;
    }
}
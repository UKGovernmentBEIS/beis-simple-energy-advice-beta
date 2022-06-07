using Npgsql;
using SeaPublicWebsite.Data.Helpers;

namespace SeaPublicWebsite.Data.DataStores;

public class PropertyDataStore
{
    private NpgsqlConnection connection;
    private const string CONNECTION_STRING = "UserId=postgres;Password=postgres;Server=localhost;Port=5432;Database=seadev;Integrated Security=true;Pooling=true";

    public PropertyDataStore()
    {
        connection = new NpgsqlConnection(CONNECTION_STRING);
        connection.Open();
    }
    public PropertyData LoadPropertyData(string reference)
    {
        return new PropertyData();
    }

    public bool IsReferenceValid(string reference)
    {
        return true;
    }

    public async Task SavePropertyData(PropertyData propertyData)
    {
        string commandText = $"INSERT INTO \"PropertyData\" (\"Id\", \"Reference\", \"Postcode\", \"EpcLmkKey\", \"HouseNameOrNumber\") VALUES (@propertyDataId, @reference, @postcode, @epcLmkKey, @houseNameOrNumber)";
        await using (var cmd = new NpgsqlCommand(commandText, connection))
        {
            cmd.Parameters.AddWithValue("propertyDataId", propertyData.PropertyDataId);
            cmd.Parameters.AddWithValue("reference", propertyData.Reference);
            cmd.Parameters.AddWithValue("postcode", propertyData.Postcode);
            cmd.Parameters.AddWithValue("epcLmkKey", propertyData.EpcLmkKey);
            cmd.Parameters.AddWithValue("houseNameOrNumber", propertyData.HouseNameOrNumber);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public string GenerateNewReferenceAndSaveEmptyPropertyData()
    {
        string reference;
        do
        {
            reference = RandomHelper.Generate8DigitReference();
        } while (IsReferenceValid(reference));

        PropertyData propertyData = new()
        {
            Reference = reference
        };
        SavePropertyData(propertyData);

        return reference;
    }
}
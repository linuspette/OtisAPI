using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using OtisAPI.Model.ViewModels.CosmosDb;

namespace OtisAPI.Services;

public interface ICosmosDbService
{
    public Task<List<CosmosElevatorDataViewModel>> GetAllDataAsync();
    public Task<List<CosmosElevatorDataViewModel>> GetElevatorDataAsync(string deviceId);
}
public class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;

    public CosmosDbService(IConfiguration configuration)
    {
        _cosmosClient = new CosmosClient(
            accountEndpoint: configuration["OtisCosmosDbUri"],
            authKeyOrResourceToken: configuration["OtisCosmosDbAccessKey"]
        );
        Initialize().ConfigureAwait(false);
    }

    public async Task Initialize()
    {
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(id: "LpSmartDevices");
    }

    public async Task<List<CosmosElevatorDataViewModel>> GetAllDataAsync()
    {
        List<CosmosElevatorDataViewModel> returnData = new List<CosmosElevatorDataViewModel>();

        var container = _cosmosClient.GetContainer("LpSmartDevices", "data");

        var query = new QueryDefinition(
            query: "SELECT * FROM data"
        );

        using FeedIterator<dynamic> feed = container.GetItemQueryIterator<dynamic>(
            queryDefinition: query);

        while (feed.HasMoreResults)
        {
            FeedResponse<dynamic> response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                returnData.Add(JsonConvert.DeserializeObject<CosmosElevatorDataViewModel>(JsonConvert.SerializeObject(item)));
            }
        }

        return returnData ?? null!;
    }

    public async Task<List<CosmosElevatorDataViewModel>> GetElevatorDataAsync(string deviceId)
    {
        List<CosmosElevatorDataViewModel> returnData = new List<CosmosElevatorDataViewModel>();

        var container = _cosmosClient.GetContainer("LpSmartDevices", "data");

        var query = new QueryDefinition(
            query: "SELECT * FROM data d WHERE d.deviceId = @deviceId"
        ).WithParameter("@deviceId", deviceId);

        using FeedIterator<dynamic> feed = container.GetItemQueryIterator<dynamic>(
            queryDefinition: query);

        while (feed.HasMoreResults)
        {
            FeedResponse<dynamic> response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                returnData.Add(JsonConvert.DeserializeObject<CosmosElevatorDataViewModel>(JsonConvert.SerializeObject(item)));
            }
        }


        return returnData ?? null!;
    }
}
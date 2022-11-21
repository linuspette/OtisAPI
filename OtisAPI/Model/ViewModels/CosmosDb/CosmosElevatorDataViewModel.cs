using Newtonsoft.Json;

namespace OtisAPI.Model.ViewModels.CosmosDb;

public class CosmosElevatorDataViewModel
{
    [JsonProperty("deviceId")] public string DeviceId { get; set; } = string.Empty;
    [JsonProperty("deviceName")] public string DeviceName { get; set; } = string.Empty;
    [JsonProperty("deviceType")] public string DeviceType { get; set; } = string.Empty;
    [JsonProperty("localTimeStamp")] public DateTime LocalTimeStamp { get; set; }
    [JsonProperty("data")] public dynamic Data { get; set; } = null!;
}
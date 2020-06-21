using System.Text.Json.Serialization;

namespace FlightMobileWeb.Results
{
    public interface IResult
    {
        [JsonPropertyName("result_type")]

        public string ResultType { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }


    }
}

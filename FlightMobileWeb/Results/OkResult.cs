using System.Text.Json.Serialization;

namespace FlightMobileWeb.Results
{
    public class OkResult : IResult
    {
        [JsonPropertyName("result_type")]
        public string ResultType { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

        public OkResult(string msg)
        {
            ResultType = "Ok";
            Message = msg;
        }
    }
}

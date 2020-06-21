using System.Text.Json.Serialization;

namespace FlightGearApp.Results
{
    public class InvalidResult : IResult
    {
        [JsonPropertyName("result_type")]
        public string ResultType { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; }

        public InvalidResult(string msg)
        {
            ResultType = "InvalidCommand";
            Message = msg;
        }
    }
}

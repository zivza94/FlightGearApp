using System.Text.Json.Serialization;

namespace FlightMobileWeb.Results
{
    public class ServerErrorResult: IResult
    {
        [JsonPropertyName("result_type")]
        public string ResultType { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

        public ServerErrorResult(string msg)
        {
            ResultType = "ServerError";
            Message = msg;
        }
    }
}

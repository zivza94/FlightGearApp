namespace FlightGearApp.Results
{
    public class ServerErrorResult: IResult
    {
        public string ResultType { get; set; }
        public string Message { get; set; }

        public ServerErrorResult(string msg)
        {
            ResultType = "ServerError";
            Message = msg;
        }
    }
}

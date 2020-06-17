namespace FlightGearApp.Results
{
    public class OkResult : IResult
    {
        public string ResultType { get; set; }
        public string Message { get; set; }

        public OkResult(string msg)
        {
            ResultType = "Ok";
            Message = msg;
        }
    }
}

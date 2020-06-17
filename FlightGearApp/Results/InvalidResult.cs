namespace FlightGearApp.Results
{
    public class InvalidResult : IResult
    {   
        public string ResultType { get; set; }
        public string Message { get; set; }

        public InvalidResult(string msg)
        {
            ResultType = "InvalidCommand";
            Message = msg;
        }
    }
}

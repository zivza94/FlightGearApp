using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ClientSide;
using FlightGearApp.Results;
using Microsoft.Extensions.Configuration;

namespace FlightGearApp
{
    public class FlightGearClient
    {
        private readonly BlockingCollection<AsyncCommand> _queue;
        private readonly Client _client;
        private readonly IConfiguration _configuration;
        public string Ip { get; set; }
        public int Port { get; set; }
        public FlightGearClient(IConfiguration conf)
        {
            _configuration = conf;
            _queue = new BlockingCollection<AsyncCommand>();
            _client = new Client();
            Start();
        }

        public Task<IResult> Execute(Command cmd)
        {
            AsyncCommand asyncCmd = new AsyncCommand(cmd);
            _queue.Add(asyncCmd);
            return asyncCmd.Task;
        }

        public void Start()
        {
            Task.Factory.StartNew(ProccessCommands);
        }

        public void ProccessCommands()
        {
            if (!_client.Connected)
            {
                string ip = _configuration.GetValue<string>("Connections:Tcp:Ip");
                int port = _configuration.GetValue<int>("Connections:Tcp:Port");
                _client.Connect(ip, port);
                _client.Write("data\n");
            }
            foreach (AsyncCommand asyncCommand in _queue.GetConsumingEnumerable())
            {
                IResult res;
                try
                {
                    _client.Write(GetSendCommand(asyncCommand));
                    string recv = _client.Read();
                     
                     if (!ValidOperation(recv, asyncCommand.Command))
                     {
                         res = new ServerErrorResult("server didn't update");
                     }
                     else
                     {
                         res = new OkResult("Successfully sent command");
                     }
                }
                catch(Exception ex)
                {
                    res = new ServerErrorResult(ex.Message);
                }
                
                
                asyncCommand.Completion.SetResult(res);

            }
        }

        public string GetSendCommand(AsyncCommand asyncCommand)
        {
            string sendStr = CommandToString(asyncCommand.Command);
            sendStr += "get /controls/flight/rudder \n" +
                       "get /controls/flight/elevator \n" +
                       "get /controls/flight/aileron \n" +
                       "get /controls/engines/current-engine/throttle \n";
            return sendStr;
        }
        private string CommandToString(Command command)
        {
            string str = "";
            str += "set /controls/flight/rudder " + command.Rudder + "\n";
            str += "set /controls/flight/elevator " + command.Elevator + "\n";
            str += "set /controls/flight/aileron " + command.Aileron + "\n";
            str += "set /controls/engines/current-engine/throttle " + command.Throttle + "\n";
            return str;
        }

        private bool ValidOperation(string recv, Command send)
        {
            string[] values = recv.Split('\n');
            if (Math.Abs(send.Rudder - double.Parse(values[0])) > 0.0001)
            {
                return false;
            }
            if (Math.Abs(send.Elevator - double.Parse(values[1])) > 0.0001)
            {
                return false;
            }
            if (Math.Abs(send.Aileron - double.Parse(values[2])) > 0.0001)
            {
                return false;
            }
            if (Math.Abs(send.Throttle - double.Parse(values[3])) > 0.0001)
            {
                return false;
            }
            return true;
        }
    }
}

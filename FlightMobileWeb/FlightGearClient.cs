using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ClientSide;
using FlightMobileWeb.Results;
using Microsoft.Extensions.Configuration;

namespace FlightMobileWeb
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
            string ip = _configuration.GetValue<string>("Host");
            int port = _configuration.GetValue<int>("Connections:Tcp:Port");
            ConnectTcpConnection(ip, port);
            foreach (AsyncCommand asyncCommand in _queue.GetConsumingEnumerable())
            {
                IResult res;
                string recv;
                if (!ConnectTcpConnection(ip, port))
                {
                    res = new ServerErrorResult("couldn't connect to simulator");
                    asyncCommand.Completion.SetResult(res);
                    continue;
                }
                //try to write and read
                try
                {
                    _client.Write(GetSendCommand(asyncCommand));
                    recv = _client.Read();
                }
                //timeout
                catch (TimeoutException ex)
                {
                    res = new ServerErrorResult(ex.Message);
                    asyncCommand.Completion.SetResult(res);
                    continue;
                }
                //another error
                catch
                {
                    res = new ServerErrorResult(
                        "couldn't communicate with Flight Gear Simulator");
                    asyncCommand.Completion.SetResult(res);
                    continue;
                }
                //valid that send success
                if (!ValidOperation(recv, asyncCommand.Command))
                {
                    res = new ServerErrorResult("server didn't update");
                }
                else
                {
                    res = new OkResult("Successfully sent command");
                }
                asyncCommand.Completion.SetResult(res);

            }
        }

        public bool ConnectTcpConnection(string ip, int port)
        {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect(ip, port);
                    _client.Write("data\n");
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
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

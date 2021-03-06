﻿using System.Threading.Tasks;
using FlightMobileWeb.Results;

namespace FlightMobileWeb
{
    public class AsyncCommand
    {
        public Command Command { get; set; }
        public Task<IResult> Task { get =>Completion.Task;}
        public TaskCompletionSource<IResult> Completion{ get; private set; }

        public AsyncCommand(Command command)
        {
            Command = command;
            Completion = new TaskCompletionSource<IResult>();
        }


    }
}

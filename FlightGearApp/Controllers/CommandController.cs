using System;
using System.Threading.Tasks;
using FlightGearApp.Results;
using Microsoft.AspNetCore.Mvc;

namespace FlightGearApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly FlightGearClient _client;
        public CommandController(FlightGearClient client)
        {
            _client = client;
        }

        [HttpPost]
        public async Task<ActionResult<IResult>> PostFlightPlan([FromBody]Command controllers)
        {
            string valid = controllers.Valid();
            if (valid != "OK")
            {
                return BadRequest(new InvalidResult(valid));
            }
            IResult res = await _client.Execute(controllers);
            Console.WriteLine(res + "\n " + controllers.ToString());
            return Ok(res);
        }
    }
}
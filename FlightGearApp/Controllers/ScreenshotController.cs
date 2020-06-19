using System;
using System.Net.Http;
using System.Threading.Tasks;
using FlightGearApp.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlightGearApp.Controllers
{
    [Route("/screenshot")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly string _url;
        public ScreenshotController(IConfiguration conf,IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("screenshot");
            _client.Timeout = TimeSpan.FromSeconds(10);
            string ip = conf.GetValue<string>("Host");
            string port = conf.GetValue<string>("Connections:Http:port");
            _url = "http://"+ip+":"+port+"/screenshot";
        }

        [HttpGet]
        public async Task<ActionResult<IResult>> GetScreenshot()
        {
            //request from simulator the screenshot
            HttpResponseMessage response;
            try
            {
                 response = await _client.GetAsync(_url);
            }
            catch
            {
                return BadRequest(new ServerErrorResult("couldn't get the screen shot from the simulator"));
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new ServerErrorResult("couldn't get the screen shot from the simulator"));
            }
            var content = response.Content;
            //read the response
            var screenshot = await content.ReadAsByteArrayAsync();
            //return the image as a file
            return File(screenshot, "image/jpeg");
        } 
    }
}
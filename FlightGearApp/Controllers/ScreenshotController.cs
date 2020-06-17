using System.Net.Http;
using System.Threading.Tasks;
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
            _url = conf.GetValue<string>("Connections:Http:Url");
            _url += "/screenshot";
        }

        [HttpGet]
        public async Task<ActionResult> GetScreenshot()
        {
            //request from simulator the screenshot
            HttpResponseMessage response = await _client.GetAsync(_url);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("couldn't get the screen shot from the simulator");
            }
            var content = response.Content;
            //read the response
            var screenshot = await content.ReadAsByteArrayAsync();
            //return the image as a file
            return File(screenshot, "image/jpeg");
        } 
    }
}
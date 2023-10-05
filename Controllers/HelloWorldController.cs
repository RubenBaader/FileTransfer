using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FileTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private readonly string FilePath = "data.txt";

        [HttpGet(Name = "GetHelloWorld")]
        public async Task<IActionResult> GetHelloWorld()
        {
            try
            {
                string FileData = await System.IO.File.ReadAllTextAsync(FilePath);

                var response = new {Hello = FileData };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost (Name = "PostHelloWorld")]
        public async Task<IActionResult> PostHelloWorld([FromBody] string Payload )
        {
            try
            {
                string Data = Payload.ToString();

                await System.IO.File.WriteAllTextAsync( FilePath, Data );

                return Ok("Data saved!");
            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

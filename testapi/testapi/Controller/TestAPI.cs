using Microsoft.AspNetCore.Mvc;

namespace testapi.Controller
{
    [ApiController]
    [Route("api/test")]
    public class TestAPI : ControllerBase
    {
        private readonly string _data;

        public TestAPI()
        {
            _data = "test data";
        }

        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("This is data: " + _data);
        }

        [HttpGet]
        [Route("id")]
        public IActionResult GetTestId()
        {
            return Ok("This is data id: " + _data);
        }
    }
}

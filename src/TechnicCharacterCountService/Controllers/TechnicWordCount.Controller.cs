using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Microsoft.Extensions.Logging;

namespace CharacterCountService.Controllers
{
    [Route("api/[controller]")]
    public class TechnicWordCountController : Controller
    {
        readonly ILogger<TechnicWordCountController> _logger;

        public TechnicWordCountController(ILogger<TechnicWordCountController> logge)
        {
            _logger = logge;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets the Count of Words in the given string
        /// </summary>
        /// <returns></returns>       
        [HttpPost("{wordCountString}")]
        [SwaggerOperation("CalculateBirthday")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(int), "Count of characters in string")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(int), "Exception")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [Route("api/WordCount/{wordCountString}", Name = "WordCount")]
        public int WordCount(string wordCountString)
        {
            var machine = Environment.GetEnvironmentVariable("COMPUTERNAME");
            _logger.LogInformation("Hello from {MachineName}!", machine);
            _logger.LogInformation("Getting count of characters in " + wordCountString);
            return wordCountString.Length;
        }
    }
}

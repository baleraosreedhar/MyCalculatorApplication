using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Threading;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.ServiceFabric.Services.Client;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace TechnicBirthdayAgeService.Controllers
{
    [Route("api/[controller]")]
    public class BirthdayCalculatorController : Controller
    {
        readonly Microsoft.Extensions.Logging.ILogger<BirthdayCalculatorController> _logger;

        public BirthdayCalculatorController(ILogger<BirthdayCalculatorController> logge)
        {
            _logger = logge;
        }

        /// <summary>
        /// Get operation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetValues")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<string>), "Birthday Details")]
        [Route("api/Get", Name = "Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Azure Fabric Statefull service Values controller GET", "Welcome to reliable collection framework example" };
        }

        [HttpPost("{birthdatevalue}")]
        [SwaggerOperation("CalculateBirthday")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(BirthdayCelebration), "Birthday Details")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(BirthdayCelebration), "Exception")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [Route("api/Calculate/{birthdatevalue}", Name = "Calculate")]
        public IActionResult Calculate(string birthdatevalue)
        {
            BirthdayCelebration birthdayResult = null; ;
            DateTime birthDate = DateTime.MinValue;
            _logger.LogInformation("Processing request for " + birthdatevalue);
            try
            {
                birthDate = Convert.ToDateTime(birthdatevalue);
                if (birthDate > DateTime.MinValue)
                {
                    
                    birthdayResult = new BirthdayCelebration(birthDate);
                    birthdayResult.CountOfGoodThought = (birthdayResult.GoodThought).Length;
                    //birthdayResult.CountOfGoodThought = GetCountOfThoughtsFromWordCountServiceAsync(birthdayResult.GoodThought).Result;
                    return Ok(birthdayResult);
                }
                else
                {
                    return StatusCode(400, new BirthdayCelebration("Invalid Date"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Processing request for " + birthdatevalue + "  "+ ex.Message);
                return StatusCode(400, new BirthdayCelebration(ex.Message));
            }
        }
    }
}

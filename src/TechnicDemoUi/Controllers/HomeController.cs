using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

namespace TechnicDemoUi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<FabricServiceSettings> _balSettings;

        string partitionKind = string.Empty;
        string partitionKey = string.Empty;
        string azureFabricClientConnection = string.Empty;
        string azureApplicationName = string.Empty;
        string azureServiceName = string.Empty;
        string azureResourceName = string.Empty;
        string azureResourceActionName = string.Empty;

        string azureCountServiceName = string.Empty;
        string azureCountServiceResourceName = string.Empty;
        string azureCountServiceResourceActionName = string.Empty;

        string proxyUrl = string.Empty;
        public const string JsonMediaType = "application/json";
        private readonly TimeSpan defaultDnsRefreshTimeout = TimeSpan.FromSeconds(90);
        readonly ILogger<HomeController> _logger;
        public HomeController(IOptions<FabricServiceSettings> balSettings, ILogger<HomeController> logger)
        {
            _logger = logger;
            _balSettings = balSettings;
            partitionKind = _balSettings.Value.PartionKind;
            partitionKey = _balSettings.Value.PartitionKey; ;
            azureFabricClientConnection = _balSettings.Value.AzureFabricClientConnection;
            azureApplicationName = _balSettings.Value.AzureApplicationName;
            azureServiceName = _balSettings.Value.AzureServiceName;
            azureResourceName = _balSettings.Value.AzureResourceName;
            azureResourceActionName = _balSettings.Value.AzureResourceActionName;
            azureCountServiceName = _balSettings.Value.AzureCountServiceName;
            azureCountServiceResourceName = _balSettings.Value.AzureCountServiceResourceName;
            azureCountServiceResourceActionName = _balSettings.Value.AzureCountServiceResourceActionName;

            proxyUrl = $"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}/api/{azureResourceName}/{azureResourceActionName}?PartitionKind={partitionKind}&PartitionKey={partitionKey}";


        }

        public IActionResult Index()
        {
            ViewData["ValuesAction"] = $"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}/{azureResourceName}/get";
            ViewData["ValuesActionWithPartiotionKey"] = $"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}/{azureResourceName}/get?PartitionKind={partitionKind}&PartitionKey={partitionKey}";

            ViewData["CalculatorServiceSwagger"] = $"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}/swagger/v1/swagger.json";
            ViewData["CalculatorServiceSwaggerUI"] = $"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}/swagger/ui";

            List<ParitionCollection> paritionCollection = new List<ParitionCollection>();
            // StringBuilder partitionCollections = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                ServicePartitionInformation inf;

                var url = getPartitionUrl(i, out inf);

                //Console.WriteLine($"partitionId={inf.Id} Url:{url}");
                paritionCollection.Add(new ParitionCollection { PartiotionId = inf.Id.ToString(), PartionUrl = url });

            }
            _logger.LogInformation("Invoking the index file, getting the swagger URI and patitions for age service");
            ViewData["partitionCollections"] = paritionCollection;
            return View(paritionCollection);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> About(string birthdayDate)
        {
            _logger.LogInformation("calculating birthday age details for " + birthdayDate);
            BirthdayCelebration birthday = new BirthdayCelebration();
            ServicePartitionInformation inf;
            var url = getPartitionUrl(1, out inf);
            birthday = await ProcessBirthdayRequest(birthdayDate);
            birthday.CountOfGoodThought = PostProcessServiceCall(birthday.GoodThought).Result;
            birthday.Birthday = birthdayDate;
            ViewData["birthdayData"] = birthday;
            //return RedirectToAction("Contact", "Home", new RouteValueDictionary(birthday));
            //return RedirectToAction("Contact", "Home", new { birthday = birthday });
            return View("Contact",birthday);

        }
            public IActionResult Contact(BirthdayCelebration birthday)
        {
            ViewData["Message"] = "Birthday Processing";
            if (birthday == null || birthday.Birthday == null)
                birthday = (BirthdayCelebration)ViewData["birthdayData"];
            return View(birthday);
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<BirthdayCelebration> ProcessBirthdayRequest(string birthday)
        {
            try
            {
                _logger.LogInformation("Started Processing birth day age service");

                BirthdayCelebration birthdayCelebration = new BirthdayCelebration();
                var client = new HttpClient();

                //client.BaseAddress = new Uri($"{azureFabricClientConnection}/{azureApplicationName}/{azureServiceName}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string ageServiceUri = $"{ azureFabricClientConnection }/{ azureApplicationName}/{ azureServiceName}/api/{azureResourceName}/{azureResourceActionName}/?PartitionKind={partitionKind}&PartitionKey={partitionKey}";
                Uri uri = new Uri(ageServiceUri);
                var request = new
                {
                    birthdatevalue = birthday
                };

                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("birthdatevalue", birthday));

                FormUrlEncodedContent content = new FormUrlEncodedContent(nvc);

                //StringContent content = new StringContent(JsonConvert.SerializeObject(request).ToString(),
                //            Encoding.UTF8, "application/json");

                _logger.LogInformation("Invoking the age service on the fabric cluster  with URI :" + ageServiceUri);
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync(uri, content);
                _logger.LogInformation("Response back from age servie is :" + response.StatusCode);
                // HttpResponseMessage response = await client.PostAsync("/api/{azureResourceName}/{azureResourceActionName}?PartitionKind={partitionKind}&PartitionKey={partitionKey}", content);
                if (response.IsSuccessStatusCode)
                {                    
                    string data = await response.Content.ReadAsStringAsync();
                    birthdayCelebration = JsonConvert.DeserializeObject<BirthdayCelebration>(data);
                    // birthdayCelebration.CountOfGoodThought = GetCountOfThoughtsFromWordCountServiceAsync(birthdayCelebration.GoodThought).Result;
                }

                return birthdayCelebration;
            }
            catch (Exception ex)
            {
                return new BirthdayCelebration { ExceptionMessage = ex.Message };
            }
        }

        private async Task<int> PostProcessServiceCall(string goodthoughts)
        {
            try
            {
                var client = new HttpClient();

                _logger.LogInformation("Started Processing count service");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string countServiceUri = $"{ azureFabricClientConnection }/{ azureApplicationName}/{ azureCountServiceName}/api/{azureCountServiceResourceName}/{azureCountServiceResourceActionName}/?PartitionKind={partitionKind}&PartitionKey={partitionKey}";
                Uri uri = new Uri(countServiceUri);

                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("wordCountString", goodthoughts));

                FormUrlEncodedContent content = new FormUrlEncodedContent(nvc);
                _logger.LogInformation("Invoking count service at :" + countServiceUri);
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync(uri, content);
                _logger.LogInformation("Response back from count servie is :" + response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var length = JsonConvert.DeserializeObject<int>(data);
                    return length;
                }

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception encountered processing count service  :" + ex.Message);

                return 0;
            }
        }

        private string getPartitionUrl(long partitionKey, out ServicePartitionInformation info)
        {
            CancellationTokenSource src = new CancellationTokenSource();

            var resolver = ServicePartitionResolver.GetDefault();

            var partKey = new ServicePartitionKey(partitionKey);

            var partition = resolver.ResolveAsync(new Uri
             ($"fabric:/MyCalculatorApplication/{azureServiceName}"), partKey, src.Token).Result;

            var pEndpoint = partition.GetEndpoint();

            var primaryEndpoint =
            partition.Endpoints.FirstOrDefault(p => p.Role ==
            System.Fabric.ServiceEndpointRole.StatefulPrimary);
            info = partition.Info;
            if (primaryEndpoint != null)
            {
                JObject addresses = JObject.Parse(primaryEndpoint.Address);

                var p = addresses["Endpoints"].First();

                string primaryReplicaAddress = p.First().Value<string>();

                return primaryReplicaAddress;
            }
            else
                return ":(";
        }

    }
}

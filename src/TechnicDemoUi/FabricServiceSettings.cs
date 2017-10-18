using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicDemoUi
{
    public class FabricServiceSettings
    {
        public string AzureFabricClientConnection { get; set; }
        public string AzureFabricClusterConnection { get; set; }
        public string AzureApplicationName { get; set; }
        public string AzureServiceName { get; set; }
        public string AzureResourceName { get; set; }
        public string AzureResourceActionName { get; set; }
        public string PartionKind { get; set; }
        public string PartitionKey { get; set; }
        public string FilterTerms { get; set; }

        public string AzureCountServiceName { get; set; }
        public string AzureCountServiceResourceName { get; set; }
    public string AzureCountServiceResourceActionName { get; set; }
    }
}

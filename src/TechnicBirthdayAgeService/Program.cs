using Microsoft.Diagnostics.EventFlow.Inputs;
using Microsoft.Diagnostics.EventFlow.ServiceFabric;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TechnicBirthdayAgeService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // **** Instantiate log collection via EventFlow
                //using (var diagnosticsPipeline = ServiceFabricDiagnosticPipelineFactory.CreatePipeline("TechnicDemo-MyCalculatorApplication-TechnicBirthdayAgeService"))
                //{

                    // The ServiceManifest.XML file defines one or more service type names.
                    // Registering a service maps a service type name to a .NET type.
                    // When Service Fabric creates an instance of this service type,
                    // an instance of the class is created in this host process.
                    //var factory = new LoggerFactory().AddEventFlow(diagnosticsPipeline);
                    //var logger = new Logger<Main>(factory);

                    ServiceRuntime.RegisterServiceAsync("TechnicBirthdayAgeServiceType",
                    context => new TechnicBirthdayAgeService(context)).GetAwaiter().GetResult();

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TechnicBirthdayAgeService).Name);

                    // Prevents this host process from terminating so services keeps running. 
                    Thread.Sleep(Timeout.Infinite);
                //}
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}

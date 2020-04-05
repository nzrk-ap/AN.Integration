using AN.Integration.Dynamics.EntityProviders;
using AN.Integration.Dynamics.Extensions;
using AN.Integration.Dynamics.Models;
using AN.Integration.Models.Dynamics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using static AN.Integration.Models.Dynamics.DynamicsContext;

namespace AN.Integration.Sender.Messages
{
    public class Send : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public Send(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                var settings = new Settings(service).GetSettings(
                    SBCustomSettingsModel.Fields.ServiceBusExportQueueuUrl,
                    SBCustomSettingsModel.Fields.ServiceBusExportQueueuSasKey);

                settings.EnsureParameterIsSet(nameof(settings.ServiceBusExportQueueuUrl));
                settings.EnsureParameterIsSet(nameof(settings.ServiceBusExportQueueuSasKey));

                var httpClient = new HttpClient()
                {
                    DefaultRequestHeaders = {
                        { "Authorization" , settings.ServiceBusExportQueueuSasKey } }
                };

                var dynamicsContext = new DynamicsContext
                {
                    MessageType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), context.MessageName),
                    UserId = context.UserId,
                    InputParameters = context.InputParameters,
                    PreEntityImages = context.PreEntityImages,
                    PostEntityImages = context.PostEntityImages,
                };

                var knownTypes = new List<Type>() {
                    { typeof(Entity) },
                    { typeof(ConcurrencyBehavior) }
                };

                var serializerSettings = new DataContractJsonSerializerSettings
                {
                    KnownTypes = knownTypes
                };

                using (MemoryStream ms = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(DynamicsContext), serializerSettings);
                    serializer.WriteObject(ms, dynamicsContext);
                    var content = new StringContent(Encoding.UTF8.GetString(ms.ToArray()), Encoding.UTF8, "application/json");
                    httpClient.PostAsync(new Uri(settings.ServiceBusExportQueueuUrl), content).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}

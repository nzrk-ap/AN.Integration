using AN.Integration.Dynamics.EntityProviders;
using AN.Integration.Dynamics.Extensions;
using AN.Integration.Dynamics.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using AN.Integration.Dynamics.Core.DynamicsTypes;
using AN.Integration.Dynamics.Core.Extensions;

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

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", settings.ServiceBusExportQueueuSasKey);

                var knownTypes = new List<Type>()
                {
                    typeof(Entity),
                    typeof(ConcurrencyBehavior)
                };

                var serializerSettings = new DataContractJsonSerializerSettings
                {
                    KnownTypes = knownTypes
                };

                var serializer = new DataContractJsonSerializer(typeof(DynamicsContextCore), serializerSettings);
                StringContent content;
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, GetDynamicsContextCore(context));
                    content = new StringContent(Encoding.UTF8.GetString(ms.ToArray()),
                        Encoding.UTF8, "application/json");
                }

                var result = httpClient.PostAsync(new Uri(settings.ServiceBusExportQueueuUrl), content)
                    .GetAwaiter().GetResult();

                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception($"Send message error:\n {response}");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }

        private static DynamicsContextCore GetDynamicsContextCore(IExecutionContext context)
        {
            var dynamicsContext = new DynamicsContextCore
            {
                MessageType = (DynamicsContextCore.MessageTypeEnum)
                    Enum.Parse(typeof(DynamicsContextCore.MessageTypeEnum), context.MessageName),
                UserId = context.UserId,
                InputParameters = context.InputParameters.ToCollectionCore(),
                PreEntityImages = context.PreEntityImages.ToCollectionCore(),
                PostEntityImages = context.PostEntityImages.ToCollectionCore(),
            };

            return dynamicsContext;
        }
    }
}
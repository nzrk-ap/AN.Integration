using AN.Integration.Dynamics.EntityProviders;
using AN.Integration.Dynamics.Extensions;
using AN.Integration.Dynamics.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using AN.Integration.Dynamics.Core.DynamicsTypes;

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

        public Send()
        {
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

                var contextCore = new DynamicsContextCore
                {
                    MessageType = (DynamicsContextCore.MessageTypeEnum)
                        Enum.Parse(typeof(DynamicsContextCore.MessageTypeEnum), context.MessageName),
                    UserId = context.UserId,
                    InputParameters = context.InputParameters.ToCollectionCore()
                };

                if (context.PreEntityImages.Any())
                {
                    contextCore.PreEntityImages = context.PreEntityImages.ToCollectionCore();
                }

                if (context.PostEntityImages.Any())
                {
                    contextCore.PostEntityImages = context.PostEntityImages.ToCollectionCore();
                }

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", settings.ServiceBusExportQueueuSasKey);

                var result = httpClient.PostAsync(new Uri(settings.ServiceBusExportQueueuUrl),
                        ToStringContent(contextCore)).GetAwaiter().GetResult();

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

        private static StringContent ToStringContent(DynamicsContextCore contextCore)
        {
            var knownTypes = new List<Type>()
            {
                typeof(EntityCore),
                typeof(ReferenceCore),
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
                serializer.WriteObject(ms, contextCore);
                content = new StringContent(Encoding.UTF8.GetString(ms.ToArray()),
                    Encoding.UTF8, "application/json");
            }

            return content;
        }
    }
}
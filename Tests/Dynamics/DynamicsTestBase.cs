using System;
using System.Collections.Generic;
using System.IO;
using FakeXrmEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.WebServiceClient;
using Microsoft.Xrm.Tooling.Connector;

namespace Tests.Dynamics
{
    public abstract class DynamicsTestBase<T>
    {
        protected readonly XrmRealContext XrmRealContext;

        protected DynamicsTestBase()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets<DynamicsTestBase<T>>();

            var configuration = builder.Build();

            Service = new CrmServiceClient(@"AuthType=Office365;
            Username = annaz@antrial31032020.onmicrosoft.com;
            Password = Xerox1779;
            Url = https://antrial31032020.crm4.dynamics.com/");

            XrmRealContext = new XrmRealContext(Service);
        }

        public IOrganizationService Service { get; set; }

        protected List<Entity> GetRecords(QueryExpression query) => GetRecords(query, 5000);

        protected List<Entity> GetRecords(QueryExpression query, int count)
        {
            var moreRecords = true;
            var records = new List<Entity>();

            if (query.PageInfo.Count == 0 || query.PageInfo.Count > count && count < 5000)
            {
                query.PageInfo.Count = count;
                query.PageInfo.PageNumber = 1;
            }

            while (moreRecords)
            {
                var results = Service.RetrieveMultiple(query);

                foreach (var entity in results.Entities)
                {
                    if (records.Count != count)
                    {
                        records.Add(entity);
                    }
                    else
                    {
                        return records;
                    }
                }

                moreRecords = results.MoreRecords;

                if (moreRecords)
                {
                    query.PageInfo.PagingCookie = results.PagingCookie;
                    query.PageInfo.PageNumber++;
                }
            }

            return records;
        }

        private static IOrganizationService GetCrmService(IConfiguration configuration)
        {
            var authResult = AcquireToken(configuration, out var authParameters);
            var serviceUrl = new Uri(authParameters.Resource +
                                     @"XRMServices/2011/Organization.svc/web?SdkClientVersion=9.0");

            IOrganizationService crmService;
            using (var sdkService = new OrganizationWebProxyClient(serviceUrl, false))
            {
                sdkService.HeaderToken = authResult.AccessToken;
                crmService = sdkService;
            }

            return crmService;
        }

        private static AuthenticationResult AcquireToken(IConfiguration configuration,
            out AuthenticationParameters authParameters)
        {
            var dynamicsOps = configuration.GetSection("DynamicsClientOptions");
            var apiUri = new Uri($"{dynamicsOps["Resource"]}/api/data/v{dynamicsOps["ApiVersion"]}");

            authParameters = AuthenticationParameters.CreateFromResourceUrlAsync(apiUri)
                .GetAwaiter().GetResult();
            var authContext = new AuthenticationContext(authParameters.Authority, false);
            var clientCred = new ClientCredential(dynamicsOps["ClientId"],
                dynamicsOps["ClientSecret"]);

            return authContext.AcquireTokenAsync(authParameters.Resource, clientCred).GetAwaiter().GetResult();
        }
    }
}
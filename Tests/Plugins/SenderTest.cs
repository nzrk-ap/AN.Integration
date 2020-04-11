using System;
using System.Collections.Generic;
using AN.Integration.Sender.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Tests.Dynamics;

namespace Tests.Plugins
{
    [TestClass]
    public class SenderTest: DynamicsTestBase<Send>
    {
        [TestMethod]
        public void Send()
        {
            var executionContext = XrmRealContext.GetDefaultPluginContext();

            executionContext.InputParameters = new ParameterCollection
            {
                new KeyValuePair<string, object>("Target", new Entity("contact", new Guid("73A0E5B9-88DF-E311-B8E5-6C3BE5A8B200"))
                {
                    ["lastname"] = "Harrison"
                }),
            };

            executionContext.PreEntityImages = new EntityImageCollection()
            {
              new KeyValuePair<string, Entity>("Image", new Entity("contact")
              {
                  ["lastname"] = "Harris"
              })
            };

            executionContext.PostEntityImages = new EntityImageCollection();

            XrmRealContext.ExecutePluginWith<Send>(executionContext);
        }
    }
}
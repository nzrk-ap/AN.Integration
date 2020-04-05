using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace AN.Integration.Dynamics.Models
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the assocaited controller class
	// If a controller does not exist, create one that inherrits the model.

	public class SBCustomSettingsModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_customsettings";

		#region Attribute Names
		public static class Fields
		{
			public const string
				PrimaryId = "sb_customsettingsid",
				PrimaryName = "sb_name",
				ServiceBusExportQueueuSasKey = "an_servicebusexportqueueusaskey",
				ServiceBusExportQueueuUrl = "an_servicebusexportqueueuurl";

			public static string[] All => new[] { PrimaryId,
				PrimaryName,
				ServiceBusExportQueueuSasKey,
				ServiceBusExportQueueuUrl };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public string ServiceBusExportQueueuSasKey
		{
			get => (string)this[Fields.ServiceBusExportQueueuSasKey];
			set => this[Fields.ServiceBusExportQueueuSasKey] = value; 
		}
		public string ServiceBusExportQueueuUrl
		{
			get => (string)this[Fields.ServiceBusExportQueueuUrl];
			set => this[Fields.ServiceBusExportQueueuUrl] = value; 
		}
		#endregion

		#region Constructors
		protected SBCustomSettingsModel()
			: base(LogicalName) { }
		protected SBCustomSettingsModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected SBCustomSettingsModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected SBCustomSettingsModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected SBCustomSettingsModel(Entity entity, IOrganizationService service)
			: base(entity, service) { }
		#endregion
	}
}
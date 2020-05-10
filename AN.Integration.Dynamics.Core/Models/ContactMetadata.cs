using System;
using AN.Integration.DynamicsCore.CoreTypes;

namespace AN.Integration.DynamicsCore.Models
{
    public static class ContactMetadata
    {
        public const string EntityLogicalName = "contact";

        public static class FirstName
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "firstname";
        }

        public static class LastName
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "lastname";
        }

        public static class ANCode
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "an_code";
        }

        public static class ManagerName
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "managername";
        }
    }
}
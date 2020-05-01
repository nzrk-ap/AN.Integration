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

        public static class SbContactIdNumber
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "sb_contactidnumber";
        }

        public static class MentorId
        {
            public static readonly Type Type = typeof(ReferenceCore);
            public const string LogicalName = "sb_mentorid";
            public const string SchemaName = "sb_MentorID";
        }

        public static class ManagerName
        {
            public static readonly Type Type = typeof(string);
            public const string LogicalName = "managername";
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace AN.Integration.Infrastructure.Dynamics.Utilities
{
    public static class StringUtilities
    {
        public static Guid ParseId(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new Exception("Input content contains no data");
            const string matchPattern =
                @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";
            var match = Regex.Match(content, matchPattern);
            if (!match.Success)
                throw new Exception("Unable to parse Guid from input content");
            return Guid.Parse(match.Value);
        }
    }
}
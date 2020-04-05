using AN.Integration.Dynamics.EntityProviders;
using System;

namespace AN.Integration.Dynamics.Extensions
{
    public static class SettingsExtensions
    {
        public static void EnsureParameterIsSet(this Settings settings, string parameterName)
        {
            var prop = settings.GetType().GetProperty(parameterName);

            if (string.IsNullOrEmpty((string)prop.GetValue(settings)))
            {
                throw new Exception($"{prop.Name} is not set");
            }
        }
    }
}

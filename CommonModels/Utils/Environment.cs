using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utils
{
    public class EnvironmentVars
    {
        public const int Default_Int_Value = int.MinValue;
        public const double Default_Double_Value = Double.MinValue;

        public static string GetEnvironmentVariable(string variableName)
        {
            string envVar = Environment.GetEnvironmentVariable(variableName);
            return string.IsNullOrWhiteSpace(envVar) ? null : envVar;
        }
        public static string GetStringVariable(string variableName, IConfigurationRoot config = null)
        {
            return GetEnvironmentVariable(variableName) ?? (config?[variableName]);
        }
        public static int GetIntVariable(string variableName, IConfigurationRoot config = null)
        {
            if (int.TryParse(GetStringVariable(variableName, config), out int variable))
                return variable;
            else
                return Default_Int_Value;

        }
        public static double GetDoubleVariable(string variableName, IConfigurationRoot config = null)
        {
            if (double.TryParse(GetStringVariable(variableName, config), out double variable))
                return variable;
            else
                return Default_Double_Value;
        }

        public static bool GetBooleanVariable(string variableName, IConfigurationRoot config = null)
        {
            if (bool.TryParse(GetStringVariable(variableName, config), out bool variable))
                return variable;
            else
                return false;
        }
    }
}

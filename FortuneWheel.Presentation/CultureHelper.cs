using System.Globalization;
using System.Reflection;
using System.Resources;

namespace OnlineAuc
{
    public static class CultureHelper
    {
        private static readonly ResourceManager ExceptionManager;

        static CultureHelper()
        {
            string exceptionManagerbaseName = $"OnlineAuc.Resources.Exceptions.Exceptions";
            ExceptionManager = new ResourceManager(exceptionManagerbaseName, Assembly.GetExecutingAssembly());
        }

        public static string Exception(string key, CultureInfo culture, params string[] parameters)
        {
            string str = ExceptionManager.GetString(key, culture);

            if (parameters.Length > 0)
            {
                str = string.Format(str, parameters);
            }

            return str;
        }
    }
}

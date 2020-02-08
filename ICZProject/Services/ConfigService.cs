using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ICZProject.Services
{
    public class ConfigService: IConfigService
    {
        public string MasterPassword
        {
            get
            {
                return Setting<string>("masterPassword");
            }
        }

        public string ProjectsFilePath
        {
            get
            {
                return Setting<string>("projectsFilePath");
            }
        }

        private T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
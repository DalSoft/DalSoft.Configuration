using System.Collections.Generic;

namespace DalSoft.Configuration.Test.Unit
{
    public class ValidConfig : JsonConfig<ValidConfig>
    {
        public ValidConfig()
        {
            ValidList = new List<MyAppConfig>();
        }

        public string Website { get; set; }
        public string DatabaseConnectionString { get; set; }
        public MyAppConfig MyAppConfig { get; set; }
        public List<MyAppConfig> ValidList { get; set; }
    }

    public class MissingConfig : JsonConfig<MissingConfig> { }

    public class InvalidConfig : JsonConfig<InvalidConfig> { }

    public class MyAppConfig
    {
        public string Website { get; set; }
        public string DatabaseConnectionString { get; set; }
    }
}

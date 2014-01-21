# DalSoft.Configuration

The simplest .NET configuration that could possibly work (30 lines of code). Use Json in your .NET config. Use simple POCO classes for all your config.

Works with:

* app.config
* web.config
* serviceConfiguration.cscfg (Azure)

## How to use

Create a config class that inherits from DalSoft.Configuration.JsonConfig:

```cs
public class MyAppConfig : JsonConfig<ValidConfig>
{
  public string Website { get; set; }
  public string DatabaseConnectionString { get; set; }
}
```

Add the following appSettings to your app or web.config:
```xml
<appSettings>
   <add key="ValidConfig" value="
    {
        'Website':'http://dalsoft.co.uk/',
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;'
    }" />
  </appSettings>
```














Thanks to @JamesNK for Json.Net

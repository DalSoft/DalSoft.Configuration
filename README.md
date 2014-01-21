# DalSoft.Configuration

The simplest .NET configuration that could possibly work (30 lines of code). Use Json in your .NET .config files. Use simple POCO classes for all your config.

Works with:

* app.config
* web.config
* serviceConfiguration.cscfg (Azure)

## How to use

Create a config class that inherits from DalSoft.Configuration.JsonConfig:

```cs
public class MyAppConfig : JsonConfig<MyAppConfig>
{
  public string Website { get; set; }
  public string DatabaseConnectionString { get; set; }
}
```

Add the following appSettings to your app or web.config:
```xml
<appSettings>
   <add key="MyAppConfig" value="
    {
        'Website': 'http://dalsoft.co.uk/',
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;'
    }" />
  </appSettings>
```

Now you can access your configuration as simple as calling GetSettings() on the class you just created for example:
```cs
MyAppConfig.GetSettings();
```

> MyAppConfig.GetSettings().Website; will return "http://dalsoft.co.uk/"


## How does it work?

A very simple convention JsonConfig just looks in appSettings keys for the Name of the class you pass as a generic arguement to JsonConfig. In our example JsonConfig<MyAppConfig> JsonConfig looks for <appSettings><add key="MyAppConfig"... in the config and deserializes config - that's it.

## Flexiablity

One of the things I like about this apporach is flexiablity. You can have just one JsonConfig class per solution or one per project it's up to you. You can even have multiple JsonConfig in the same app.config, because really they are just normal appSettings.

If you went with one JsonConiifg class per solution one of the awesome features is you only need to add the Json you need. From our example above if your project only need the DatabaseConnectionString, thats all you need to add.

This would work fine
```xml
<appSettings>
   <add key="MyAppConfig" value="
    {
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;'
    }" />
  </appSettings>
```

A call to MyAppConfig.GetSettings().DatabaseConnectionString; would return "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;".
And a call to MyAppConfig.GetSettings().Website; would return null.

#Composition

Composition is supported and encouraged for grouping your config.

Example JsonConfig:
```cs
public class MyAppConfig : JsonConfig<MyAppConfig>
{
  public string DatabaseConnectionString { get; set; }
  public SmtpConfig SmtpConfig { get; set; }
}

public class SmtpConfig
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
```

Example config:
```xml
<appSettings>
   <add key="MyAppConfig" value="
    {
        'Website': 'http://dalsoft.co.uk/',
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;',
        'SmtpConfig': 
        {           
          'Host': 'smtp.google.com',           
          'UserName':'myusername',           
          'Password': 'mypassword'
        }
    }" />
  </appSettings>
```

A call to MyAppConfig.GetSettings().SmtpConfig.Host; would return "smtp.google.com".





Thanks to @JamesNK for Json.Net

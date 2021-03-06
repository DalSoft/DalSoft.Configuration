# DalSoft.Configuration

> Note ASP.NET Core 1.0 now includes this exact functionality https://weblog.west-wind.com/posts/2015/Jun/03/Strongly-typed-AppSettings-Configuration-in-ASPNET-5

The simplest .NET configuration that could possibly work (30 lines of code). Use Json in your .NET config files. Use simple POCO classes for all your config.

```dos
PM> Install-Package DalSoft.Configuration
```

Works with:

* app.config
* web.config
* serviceConfiguration.cscfg (Azure)

## How to use

Create a class that inherits from DalSoft.Configuration.JsonConfig:

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

Now you can access your configuration by simply calling GetSettings() on the class you just created for example:
```cs
MyAppConfig.GetSettings();
```

> MyAppConfig.GetSettings().Website; will return "http://dalsoft.co.uk/"


## How does it work?

JsonConfig has a very simple convention it just looks in appSettings keys for the Name of the class you passed as a generic argument. 

In our example JsonConfig<MyAppConfig> JsonConfig looks for <appSettings><add key="MyAppConfig"... in the config and deserializes it - that's it.

## Flexibility

One of the things I like about this approach is flexibility. You can have just one derived JsonConfig class per solution or one per project it's up to you. You can even have multiple JsonConfig keys in the same app.config because it just uses normal appSettings.

If you went with one derived JsonConfig class per solution one of the awesome features is you only need to add the Json you need. From our example above if your project only needs the DatabaseConnectionString, thats all you need to add.

This would work fine
```xml
<appSettings>
   <add key="MyAppConfig" value="
    {
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;'
    }" />
  </appSettings>
```

> A call to MyAppConfig.GetSettings().DatabaseConnectionString; would return "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;".
And a call to MyAppConfig.GetSettings().Website; would return null.

# Composition

Composition is supported and encouraged for grouping your config.

Example derived JsonConfig class:
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

> A call to MyAppConfig.GetSettings().SmtpConfig.Host; would return "smtp.google.com".


## Complex Types 

JsonConfig supports the same types as [Json.NET](http://james.newtonking.com/json/help/index.html?topic=html/SerializationGuide.htm)  IList, IEnumerable, IList<T>, Array, IDictionary, IDictionary<TKey, TValue> etc.

### List example

Lets take our previous example but now we want to support mutiple SmtpConfigs.

Example derived JsonConfig class:
```cs
public class MyAppConfig : JsonConfig<MyAppConfig>
{
  public MyAppConfig()
  {
    SmtpConfig = new List<SmtpConfig>();
  }
  
  public string DatabaseConnectionString { get; set; }
  public List<SmtpConfig> SmtpConfig { get; set; }
}

public class SmtpConfig
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
```

> Notice the use of null object in the constructor

Example config:
```xml
<appSettings>
   <add key="MyAppConfig" value="
    {
        'Website': 'http://dalsoft.co.uk/',
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;',
        'SmtpConfig': [
        {           
          'Host': 'smtp.google.com',           
          'UserName':'myusername',           
          'Password': 'mypassword'
        },
        {           
          'Host': 'smtp.yahoo.com',           
          'UserName':'myusername',           
          'Password': 'mypassword'
        }]
    }" />
  </appSettings>
```

> A call to MyAppConfig.GetSettings().SmtpConfig[1].Host; would return "smtp.yahoo.com"


## Azure confg .cscfg files

This is as easy as it gets replace:

```xml
<appSettings><add key="MyAppConfig" value="... 
```

with: 

```xml
<ConfigurationSettings><Setting name="MyAppConfig" value="...
```

Our example using Azure ServiceConfiguration.cscfg

```xml
<ConfigurationSettings>
   <add name="MyAppConfig" value="
    {
        'Website': 'http://dalsoft.co.uk/',
        'DatabaseConnectionString': 'Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;'
    }" />
  </ConfigurationSettings>
```

> MyAppConfig.GetSettings().Website; will return "http://dalsoft.co.uk/"
> .cscfg config settings will always take priority over app pr web.config


## IoC

DalSoft.Configuration is so lightweight I sugguest injecting as is without creating any extra abstractions. You should inject by binding to ```() => MyAppConfig.GetSettings()``` directly. In your tests you can just provide the configuration required using app.config. BTW there is no reason why you can't add your own interfaces over any class derived from JsonConfig it would just be overkill. You don't need to worry about multiple calls to GetSettings() see caching below.

## GetSettings() and caching
Since version 1.0.1 after the first call to GetSettings() all subsequent calls are returned from a static field. That means you can call GetSettings() as many times as you like from the same AppDomain - and it will be as quick as it could possibly be.

Made possible by [@JamesNK](https://twitter.com/JamesNK)'s Json.Net

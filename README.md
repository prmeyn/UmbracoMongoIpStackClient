# Setup procedure

The `appsettings.json` file should contain your API settings for your https://ipstack.com/ account
```json
{
	"ipStack": {
		"apiPrefix": "http://api.ipstack.com/",
		"apiPostfix": "?access_key=6a11110882d90f---YOUR-API-KEY---d8993fc3c0ce902170"
	}
}
```

```csharp
using UmbracoMongoIpStackClient;

var ipStackRootObject = IpStackClientService.GetGeoInfo(ipAddress: "5.186.64.220");
```

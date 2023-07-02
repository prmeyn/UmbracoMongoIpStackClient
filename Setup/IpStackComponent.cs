using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;

namespace UmbracoMongoIpStackClient.Setup
{
	public sealed class IpStackComponent : IComponent
	{
		private readonly IConfiguration _config;

		public IpStackComponent(IConfiguration config)
		{
			_config = config;
		}

		public void Initialize()
		{
			var ipStackConfigPath = "ipStack";
			IpStackClientService.Initialize(
				baseAddress: new Uri(_config.GetValue<string>($"{ipStackConfigPath}:apiPrefix")),
				apiKeyParameters: _config.GetValue<string>($"{ipStackConfigPath}:apiPostfix")
			);
		}

		public void Terminate()
		{
		}
	}
}

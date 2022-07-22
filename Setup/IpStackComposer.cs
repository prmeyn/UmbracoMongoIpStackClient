using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using UmbracoMongoDbClient.Setup;

namespace UmbracoMongoIpStackClient.Setup
{
	[ComposeAfter(typeof(MongoDbComposer))]
	public class IpStackComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.AddComponent<IpStackComponent>();
		}
	}
}

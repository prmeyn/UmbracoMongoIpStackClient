using MongoDB.Bson.Serialization.Attributes;

namespace UmbracoMongoIpStackClient.DTOs
{
	public class IpStackRootObject
	{
		[BsonId]
		public string? ip { get; set; }
		public string? type { get; set; }
		public string? continent_code { get; set; }
		public string? continent_name { get; set; }
		public string? country_code { get; set; }
		public string? country_name { get; set; }
		public string? region_code { get; set; }
		public string? region_name { get; set; }
		public string? city { get; set; }
		public string? zip { get; set; }
		public float? latitude { get; set; }
		public float? longitude { get; set; }
		public Location? location { get; set; }
		public DateTimeOffset? ResponseTimeStamp { get; set; }
	}
}

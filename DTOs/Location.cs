using Newtonsoft.Json;

namespace UmbracoMongoIpStackClient.DTOs
{
	public class Location
	{
		[JsonProperty("geoname_id")] public int? GeonameId { get; set; }
		[JsonProperty("capital")] public string? Capital { get; set; }
		[JsonProperty("languages")] public Language[]? Languages { get; set; }
		[JsonProperty("country_flag")] public string? CountryFlag { get; set; }
		[JsonProperty("country_flag_emoji")] public string? CountryFlagEmoji { get; set; }
		[JsonProperty("country_flag_emoji_unicode")] public string? CountryFlagEmojiUnicode { get; set; }
		[JsonProperty("calling_code")] public string? CallingCode { get; set; }
		[JsonProperty("is_eu")] public bool? IsEU { get; set; }
	}
}

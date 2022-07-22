using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using UmbracoMongoDbClient;
using UmbracoMongoIpStackClient.DTOs;

namespace UmbracoMongoIpStackClient
{
	public static class IpStackClientService
	{
		private static readonly HttpClient client = new();
		private static string _ipStackDataDatabaseName;
		private static readonly string _ipStackDataCollectionName = "IpStackData";
		private static string _apiKeyParameters;
		private static ConcurrentDictionary<string, IpStackRootObject> _inMemoryCache;

		public static void Initialize(Uri baseAddress, string apiKeyParameters)
		{
			_ipStackDataDatabaseName = MethodBase.GetCurrentMethod().DeclaringType.Name;
			client.BaseAddress = baseAddress;
			_apiKeyParameters = apiKeyParameters;
			var database = MongoDBClientConnection.GetDatabase(_ipStackDataDatabaseName);
			var collection = database.GetCollection<BsonDocument>(_ipStackDataCollectionName);

			_inMemoryCache = new ConcurrentDictionary<string, IpStackRootObject>(collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync().Result.ToDictionary(v => v.GetValue(0).ToString(), v => BsonSerializer.Deserialize<IpStackRootObject>(v)));
			new Thread(WatchForCollectionChanges).Start();
		}

		private static void WatchForCollectionChanges()
		{
			var database = MongoDBClientConnection.GetDatabase(_ipStackDataDatabaseName);
			var collection = database.GetCollection<BsonDocument>(_ipStackDataCollectionName);
			using (var cursor = collection.Watch())
			{
				_inMemoryCache = new ConcurrentDictionary<string, IpStackRootObject>(collection.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync().Result.ToDictionary(v => v.GetValue(0).ToString(), v => BsonSerializer.Deserialize<IpStackRootObject>(v)));
			}
		}

		private async static Task<IpStackRootObject> GetIpStackInfo(string ip)
		{
			if (_inMemoryCache.TryGetValue(ip, out IpStackRootObject value) && value != null && (DateTime.UtcNow - value.ResponseTimeStamp).Value.TotalDays < 5)
			{
				return value;
			}
			var response = await client.GetAsync($"{ip}{_apiKeyParameters}");
			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();
			var responseValue = JsonConvert.DeserializeObject<IpStackRootObject>(responseBody);
			responseValue.ResponseTimeStamp = DateTime.UtcNow;
			if (string.IsNullOrWhiteSpace(responseValue?.ip))
			{
				if (value != null)
				{
					return value;
				}
				else
				{
					responseValue.ip = ip;
					return responseValue;
				}
			}
			var database = MongoDBClientConnection.GetDatabase(_ipStackDataDatabaseName);
			var collection = database.GetCollection<BsonDocument>(_ipStackDataCollectionName);
			if (value == null)
			{
				try { collection.InsertOne(responseValue.ToBsonDocument()); } catch { }
			}
			else
			{
				collection.ReplaceOne(
					filter: Builders<BsonDocument>.Filter.Eq("_id", ip),
					responseValue.ToBsonDocument());
			}
			return responseValue;
		}

		public static IpStackRootObject GetGeoInfo(string ipAddress)
		{
			return GetIpStackInfo(ip: ipAddress).Result;
		}
	}
}
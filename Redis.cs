using StackExchange.Redis;

namespace CachingLibrary
{
    public static class Redis
    {
        private const string ConnectionString = "localhost:6379,password=2350576Ff,allowAdmin=true";
        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConnectionString);
        /*private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("34.124.248.118:6379,password=2350576Ff");*/
        public static readonly IDatabase db = redis.GetDatabase();
        public static readonly IServer server = redis.GetServer("localhost:6379");
    }
}

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDo_Server.Entities;

namespace ToDo_Server.Service
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ToDoItem> _itemCollection;

        public MongoDbService(IOptions<MongoDBSetting> mongoDBSetting)
        {
            MongoClient client = new MongoClient(mongoDBSetting.Value.ConnectionURI);
            IMongoDatabase mongoDatabase = client.GetDatabase(mongoDBSetting.Value.DatabaseName);
            _itemCollection = mongoDatabase.GetCollection<ToDoItem>(
                mongoDBSetting.Value.CollectionName
            );
        }

        public async Task<ToDoItem> CreateItem(ToDoItem item)
        {
            await _itemCollection.InsertOneAsync(item);
            return item;
        }

        public async Task<List<ToDoItem>> GetAsync()
        {
            return await _itemCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<ToDoItem> UpdateAsync(string id, ToDoItem item)
        {
            return await _itemCollection.FindOneAndReplaceAsync(item => item.Id == id, item);
        }

        public async Task RemoveAsync(string id)
        {
            await _itemCollection.DeleteOneAsync(item => item.Id == id);
        }
    }
}

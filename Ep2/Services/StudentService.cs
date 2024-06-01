using Ep2.Models;
using Microsoft.Azure.Cosmos;

namespace Ep2.Services
{
    public class StudentService
    {
        private readonly CosmosClient _client;
        private Container _container;
        public StudentService(IConfiguration configuration)
        {
            var url = configuration["AzureCosmosDb:Url"];
            var key = configuration["AzureCosmosDb:Key"];
            _client = new CosmosClient(accountEndpoint: url,
                authKeyOrResourceToken: key);
        }
        public async Task InitializeAsync()
        {
           var database = await _client.CreateDatabaseIfNotExistsAsync("SMS");
           _container = await database.Database.CreateContainerIfNotExistsAsync("students", "/id");
        }
        public async Task Insert(Student model)
        {
            await _container.CreateItemAsync(model);
        }
        public async Task<IEnumerable<Student>> GetAll()
        {
            await InitializeAsync();
            var respones = _container.GetItemLinqQueryable<Student>(true);
            return respones.AsEnumerable();
        }
        public async Task<Student> Get(string id)
        {
            await InitializeAsync();
            return await _container.ReadItemAsync<Student>(id,PartitionKey.None);
        }
        public async Task Delete(string id)
        {
            await InitializeAsync();
            await _container.DeleteItemAsync<Student>(id,PartitionKey.None);
        }
    }
}

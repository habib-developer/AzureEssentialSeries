using Ep2.Models;
using Microsoft.Azure.Cosmos;

namespace Ep2.Services
{
    public class StudentService
    {
        private readonly Container _container;
        public StudentService(IConfiguration configuration,CosmosClient cosmosClient)
        {
            var databaseName = configuration["AzureCosmosDb:DatabaseName"];
            var containerName = configuration["AzureCosmosDb:ContainerName"];

            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task Insert(Student model)
        {
            await _container.CreateItemAsync(model);
        }
        public async Task<IEnumerable<Student>> GetAll()
        {
            var respones = _container.GetItemLinqQueryable<Student>(true);
            return respones.AsEnumerable();
        }
        public async Task<Student> Get(string id)
        {
            return await _container.ReadItemAsync<Student>(id,PartitionKey.None);
        }
        public async Task Delete(string id)
        {
            await _container.DeleteItemAsync<Student>(id,PartitionKey.None);
        }
    }
}

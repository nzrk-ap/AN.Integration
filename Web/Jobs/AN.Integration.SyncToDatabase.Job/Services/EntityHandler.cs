//using System.Threading.Tasks;
//using AN.Integration.Database.Client;
//using AN.Integration.Database.Models.Models;

//namespace AN.Integration.SyncToDatabase.Job.Services
//{
//    internal sealed class EntityHandler : IHandler
//    {
//        private readonly IDatabaseClient _databaseClient;

//        public EntityHandler(IDatabaseClient databaseClient)
//        {
//            _databaseClient = databaseClient;
//        }

//        public async Task UpsertAsync(IDatabaseTable model)
//        {
//            await _databaseClient.UpsertAsync(model);
//        }

//        public async Task DeleteAsync(IDatabaseTable model)
//        {
//            await _databaseClient.DeleteAsync(model);
//        }
//    }
//}

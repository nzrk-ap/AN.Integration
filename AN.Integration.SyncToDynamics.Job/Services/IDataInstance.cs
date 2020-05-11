namespace AN.Integration.SyncToDynamics.Job.Services
{
    public interface IDataInstance
    {
        T GetInstanceForUpsert<T>(object message);

        T GetInstanceForDelete<T>(object body);
    } 
}

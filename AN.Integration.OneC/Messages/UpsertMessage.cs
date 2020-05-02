namespace AN.Integration.OneC.Messages
{
    public class UpsertMessage<T>
    {
        public UpsertMessage(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
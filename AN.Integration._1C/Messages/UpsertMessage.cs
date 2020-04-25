namespace AN.Integration._1C.Messages
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
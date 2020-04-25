namespace AN.Integration._1C.Messages
{
    public class DeleteMessage<T>
    {
        public DeleteMessage(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
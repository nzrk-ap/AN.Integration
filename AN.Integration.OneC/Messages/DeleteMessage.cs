namespace AN.Integration.OneC.Messages
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
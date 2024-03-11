namespace InvitationCommandService.Domain.Exceptions
{
    public class OperationException : Exception
    {
        public OperationException(string messsage) : base(messsage) { }

    }
}

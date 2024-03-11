namespace InvitationCommandService.Application.Abstraction
{
    public interface IServiceBusRepository
    {
        public Task PublicMessage();
    }
}

namespace InvitationQueryService.Application.Abstraction
{
    public interface IServiceBusRepository
    {
        public Task PublicMessage();
    }
}

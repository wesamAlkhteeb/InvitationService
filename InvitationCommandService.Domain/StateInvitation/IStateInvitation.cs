namespace InvitationQueryService.Domain.StateInvitation
{
    public interface IStateInvitation
    {
        void Send();
        void Cancel();
        void Accept();
        void Reject();
        void Remove();
        void Leave();
        void Join();
        void ChangePermissions();
        void Empty();
    }
}

using InvitationQueryService.Domain.Exceptions;

namespace InvitationQueryService.Domain.StateInvitation
{
    public class SendInvitationState : IStateInvitation
    {
        public void Send() => throw new OperationException("You already send invitation.");
        public void Accept() => throw new OperationException("you can't send because the member exists in subscription.");
        public void Cancel() { }
        public void Reject() { }
        public void Join() => throw new OperationException("you can't send because the member exists in subscription.");
        public void ChangePermissions() => throw new OperationException("you can't send because the member exists in subscription.");
        public void Leave() { }
        public void Remove() { }
        public void Empty() { }
    }
}

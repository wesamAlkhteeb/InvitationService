using InvitationCommandService.Domain.Exceptions;

namespace InvitationCommandService.Domain.StateInvitation
{
    public class CancelInvitationState : IStateInvitation
    {
        public void Send() { }
        public void Accept() => throw new OperationException("You can't cancel because you already exists in subscription and accept invitation.");
        public void Cancel() => throw new OperationException("You can't cancel because you don't have invitation.");
        public void Reject() => throw new OperationException("You can't cancel because your invitation has reject.");
        public void Join() => throw new OperationException("You can't cancel because you already exists in subscription and accept invitation.");
        public void ChangePermissions() => throw new OperationException("You can't cancel because you already exists in subscription.");
        public void Leave() => throw new OperationException("You can't cancel because we didn't send invitation you.");
        public void Remove() => throw new OperationException("You can't accept because we didn't send invitation you.");

        public void Empty() => throw new OperationException("You can't accept because we didn't send invitation you.");
    }
}

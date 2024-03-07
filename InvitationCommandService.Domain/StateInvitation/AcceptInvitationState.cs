using InvitationQueryService.Domain.Exceptions;

namespace InvitationQueryService.Domain.StateInvitation
{
    public class AcceptInvitationState : IStateInvitation
    {
        public void Send() { }
        public void Accept() => throw new OperationException("You can't accept because you already exists in subscription.");
        public void Cancel() => throw new OperationException("You can't accept because you have not invitation.");
        public void Reject() => throw new OperationException("You can't accept because your invitation has reject.");
        public void Join() => throw new OperationException("You can't accept because you already exists in subscription.");
        public void ChangePermissions() => throw new OperationException("You can't accept because you already exists in subscription.");
        public void Leave() => throw new OperationException("You can't accept because we didn't send invitation you.");
        public void Remove() => throw new OperationException("You can't accept because we didn't send invitation you.");

        public void Empty() => throw new OperationException("You can't accept because we didn't send invitation you.");
    }
}

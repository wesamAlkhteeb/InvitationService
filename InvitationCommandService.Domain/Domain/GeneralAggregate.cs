using InvitationCommandService.Domain.StateInvitation;

namespace InvitationCommandService.Domain.Domain
{
    public class GeneralAggregate : Aggregate
    {
        private GeneralAggregate(IStateInvitation stateInvitation, int subscribtionId, int memberId)
        {
            this.State = stateInvitation;
            GenerateAggregateId(subscribtionId, memberId);
        }
        public static Aggregate GenerateAggregate(IStateInvitation stateInvitation, int subscribtionId, int memberId)
        {
            Aggregate aggregate = new GeneralAggregate(stateInvitation, subscribtionId, memberId);

            return aggregate;
        }

        public override void CanDoEvent()
        {
            Check();
        }
    }
}

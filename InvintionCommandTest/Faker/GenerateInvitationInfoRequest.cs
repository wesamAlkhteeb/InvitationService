using Bogus;
using InvitationCommandTest;


namespace InvintionCommandTest.Faker
{
    public class GenerateInvitationInfoRequest : Faker<InvitationInfoRequest>
    {
        public GenerateInvitationInfoRequest()
        {
            RuleFor(x => x.AccountId, f => f.Random.Int(1, 4));
            RuleFor(x => x.MemberId, f => f.Random.Int(1, 4));
            RuleFor(x => x.SubscriptionId, f => f.Random.Int(1, 4));
            RuleFor(x => x.UserId, f => f.Random.Int(1, 4));
        }
    }
}

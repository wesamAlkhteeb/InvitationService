using Bogus;
using Google.Protobuf.Collections;
using InvitationCommandTest;
using Microsoft.Identity.Client;


namespace InvintionCommandTest.Faker
{
    public class GeneratePermission : Faker<Permissions>
    {
        public GeneratePermission(int id)
        {
            RuleFor(p => p.Id, id);
            RuleFor(p => p.Name, f => f.Name.FirstName());
        }
    }

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

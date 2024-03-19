using Bogus;

namespace InvitationQueryTest.Faker
{
    public class GenerateOwnerSubscription : Faker<OwnerSubscription>
    {
        public GenerateOwnerSubscription(int ownerId)
        {
            RuleFor(x => x.Page, 1);
            RuleFor(x => x.OwnerId, ownerId);
        }
    }
}

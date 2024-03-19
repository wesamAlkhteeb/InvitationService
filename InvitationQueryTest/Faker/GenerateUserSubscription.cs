using Bogus;

namespace InvitationQueryTest.Faker
{
    public class GenerateUserSubscription : Faker<UserSubscription>
    {
        public GenerateUserSubscription(int memberId)
        {
            RuleFor(x => x.Page, 1);
            RuleFor(x => x.UserId, memberId);
        }
    }
}

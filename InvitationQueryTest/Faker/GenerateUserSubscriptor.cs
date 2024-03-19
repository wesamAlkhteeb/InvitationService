using Bogus;
namespace InvitationQueryTest.Faker
{
    public class GenerateUserSubscriptor : Faker<UserSubscriptor>
    {
        public GenerateUserSubscriptor(int subscriptionId)
        {
            RuleFor(x => x.Page, 1);
            RuleFor(x => x.SubscriptionId, subscriptionId);
        }
    }
}
using Bogus;
namespace InvitationQueryTest.Faker
{
    public class GeneratePermissionPage : Faker<PermissionPage>
    {
        public GeneratePermissionPage ()
        {
            RuleFor(x => x.NumberPage, 1);
        }
    }
}



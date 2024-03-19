using Bogus;
namespace InvitationQueryTest.Faker
{
    public class GeneratePermissionId : Faker<PermissionId>
    {
        public GeneratePermissionId(int id)
        {
            RuleFor(x => x.Id, 1);
        }
    }
}



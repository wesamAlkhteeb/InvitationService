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
}

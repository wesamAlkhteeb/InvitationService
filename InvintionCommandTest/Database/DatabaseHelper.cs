using InvitationCommandService.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;


namespace InvintionCommandTest.Database
{
    public class DatabaseHelper
    {

        public static void CheckEvent(WebApplicationFactory<Program> factory,string nameEvent , int sequence)
        {

            var scope = factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var @event = database.Events.Where(@event => @event.Type == nameEvent).Last();
            Assert.NotNull(@event);
            Assert.Equal(sequence, @event.Sequence);

            var outbox = database.Outboxes.Where(outbox => outbox.Id == @event.Id);
            Assert.NotNull(outbox);
        }
    }
}

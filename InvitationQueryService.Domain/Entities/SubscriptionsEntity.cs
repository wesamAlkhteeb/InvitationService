namespace InvitationQueryService.Domain.Entities
{
    public class SubscriptionsEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }
    }
}

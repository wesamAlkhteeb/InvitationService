

namespace InvitationQueryService.Domain.Models.Response
{
    public class UsersInSubscriptionResponseModel
    {
        public required int UserId { get; set; }
        public required int Id { get; set; }
        public required string Status { get; set; }
    }
}

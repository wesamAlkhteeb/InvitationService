using InvitationQueryService.Domain.Models;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Accept
{
    public class AcceptInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime dateTime { get; set; }
        public required InfoModel Data { get; set; }

    }
}

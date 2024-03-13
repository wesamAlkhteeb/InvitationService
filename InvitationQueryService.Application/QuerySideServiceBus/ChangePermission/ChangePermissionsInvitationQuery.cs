using InvitationQueryService.Domain.Models;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.ChangePermission
{
    public class ChangePermissionsInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateTime { get; set; }
        public DataInfoModel Data { get; set; }
    }
}

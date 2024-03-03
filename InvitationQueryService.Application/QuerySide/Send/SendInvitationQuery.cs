using InvitationQueryService.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.QuerySide.Send
{
    public class SendInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateTime{ get; set; }
        public DataInfoModel Data { get;set; }
    }
}

/*{"id":1,
	"aggregateId":"2-90",
	"sequence":1,
	"dateTime":"2024-02-28T12:47:04.7096166Z",
	"data":{
        "Info":{ "UserId":1,"SubscriptionId":90,"MemberId":2,"AccountId":1},
	    "Permissions":[
                { "Id":1,"Name":"Transfer"},{ "Id":2,"Name":"PurchaseCards"}]
    }}
*/
using Grpc.Core;
using InvitationCommandService;
using InvitationCommandService.Application.CommandHandler.Accept;
using InvitationCommandService.Application.CommandHandler.Cancel;
using InvitationCommandService.Application.CommandHandler.ChangePermissions;
using InvitationCommandService.Application.CommandHandler.Join;
using InvitationCommandService.Application.CommandHandler.Leave;
using InvitationCommandService.Application.CommandHandler.Reject;
using InvitationCommandService.Application.CommandHandler.Remove;
using InvitationCommandService.Application.CommandHandler.Send;
using InvitationCommandService.Presentation.DTOExtension;
using MediatR;

namespace InvitationCommandService.Services
{
    public class InvitationService : Invitation.InvitationBase
    {
        private readonly IMediator mediator;

        public InvitationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override async Task<Response> SendInvitationToMember(InvitationRequest request, ServerCallContext context)
        {
            SendInvitationCommand sendInvitation = await request.ConvertTOSendInvitationCommand();
            int id = await mediator.Send(sendInvitation);
            return new Response
            {
                Message = "Invitation has Sended.",
                Id = id
            };
        }

        public override async Task<Response> Accept(InvitationInfoRequest request, ServerCallContext context)
        {
            AcceptInvitationCommand acceptInvitation = await request.ConvertTOAcceptInvitationCommand();
            int id = await mediator.Send(acceptInvitation);
            return new Response
            {
                Message = "Member has Accepted.",
                Id = id
            };
        }

        public override async Task<Response> Cancel(InvitationInfoRequest request, ServerCallContext context)
        {
            CancelInvitationCommand cancelInvitation = await request.ConvertTOCancelInvitationCommand();
            int id = await mediator.Send(cancelInvitation);
            return new Response
            {
                Message = "Member has Canceled.",
                Id = id
            };
        }

        public override async Task<Response> Reject(InvitationInfoRequest request, ServerCallContext context)
        {
            RejectInvitationCommand rejectInvitation = await request.ConvertTORejectInvitationCommand();
            int id = await mediator.Send(rejectInvitation);
            return new Response
            {
                Message = "Member has Rejected.",
                Id = id
            };
        }

        public override async Task<Response> JoinMemberByAdmin(InvitationRequest request, ServerCallContext context)
        {
            JoinInvitationCommand joinInvitationCommand = await request.ConvertTOJoinInvitationCommand();
            int id = await mediator.Send(joinInvitationCommand);
            return new Response
            {
                Message = "Member has Joined.",
                Id = id
            };
        }

        public override async Task<Response> ChangePermissions(InvitationRequest request, ServerCallContext context)
        {
            ChangePermissionInvitationCommand joinInvitationCommand = await request.ConvertTOChangePermissionInvitationCommand();
            int id = await mediator.Send(joinInvitationCommand);
            return new Response
            {
                Message = "Member permissions in invitation has changed.",
                Id = id
            };
        }

        public override async Task<Response> LeaveMember(InvitationInfoRequest request, ServerCallContext context)
        {
            LeaveInvitationCommand leaveInvitation = await request.ConvertTOLeaveInvitationCommand();
            int id = await mediator.Send(leaveInvitation);
            return new Response
            {
                Message = "Member has Leaved.",
                Id = id
            };
        }

        public override async Task<Response> RemoveMember(InvitationInfoRequest request, ServerCallContext context)
        {
            RemoveInvitationCommand rejectInvitation = await request.ConvertTORemoveInvitationCommand();
            int id = await mediator.Send(rejectInvitation);
            return new Response
            {
                Message = "Membear has removed.",
                Id = id
            };
        }
    }
}

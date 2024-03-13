using FluentValidation;
using FluentValidation.Results;
using InvitationCommandService.Application.CommandHandler;
using InvitationCommandService.Domain.Model;
using InvitationCommandService.Domain.Model.Command;
using InvitationCommandService.Presentation.Validation;



namespace InvitationCommandService.Presentation.DTOExtension
{
    public static class InvitationDTO
    {
        public static async Task<SendInvitationCommand> ConvertTOSendInvitationCommand(this InvitationRequest invitation)
        {
            await ValidInvtation(invitation);
            SendInvitationCommand sendInvitation = new SendInvitationCommand(
                accountId: invitation.InvitationInfo.AccountId,
                memberId: invitation.InvitationInfo.MemberId,
                userId: invitation.InvitationInfo.UserId,
                subscriptionId: invitation.InvitationInfo.SubscriptionId,
                Permissions: new List<PermissionsModel>()
                );
            for (int i = 0; i < invitation.Permissions.Count(); i++)
            {
                sendInvitation.Permissions.Add(new PermissionsModel
                {
                    Id = invitation.Permissions[i].Id,
                    Name = invitation.Permissions[i].Name,
                });
            }
            return sendInvitation;
        }

        public static async Task<AcceptInvitationCommand> ConvertTOAcceptInvitationCommand(this InvitationInfoRequest invitation)
        {
            await ValidInvtationInfo(invitation);
            AcceptInvitationCommand sendInvitation = new AcceptInvitationCommand(
                accountId: invitation.AccountId,
                memberId: invitation.MemberId,
                userId: invitation.UserId,
                subscriptionId: invitation.SubscriptionId
                );
            return sendInvitation;
        }

        public static async Task<CancelInvitationCommand> ConvertTOCancelInvitationCommand(this InvitationInfoRequest invitation)
        {
            await ValidInvtationInfo(invitation);
            CancelInvitationCommand cancelInvitation = new CancelInvitationCommand(
                accountId: invitation.AccountId,
                memberId: invitation.MemberId,
                userId: invitation.UserId,
                subscriptionId: invitation.SubscriptionId
                );
            return cancelInvitation;
        }

        public static async Task<RejectInvitationCommand> ConvertTORejectInvitationCommand(this InvitationInfoRequest invitation)
        {
            await ValidInvtationInfo(invitation);
            RejectInvitationCommand rejectInvitation = new RejectInvitationCommand(
                accountId: invitation.AccountId,
                memberId: invitation.MemberId,
                userId: invitation.UserId,
                subscriptionId: invitation.SubscriptionId
                );
            return rejectInvitation;
        }

        public static async Task<JoinInvitationCommand> ConvertTOJoinInvitationCommand(this InvitationRequest invitation)
        {
            await ValidInvtation(invitation);
            JoinInvitationCommand joinInvitation = new JoinInvitationCommand(
                accountId: invitation.InvitationInfo.AccountId,
                memberId: invitation.InvitationInfo.MemberId,
                userId: invitation.InvitationInfo.UserId,
                subscriptionId: invitation.InvitationInfo.SubscriptionId,
                Permissions: new List<PermissionsModel>()
                );
            for (int i = 0; i < invitation.Permissions.Count(); i++)
            {
                joinInvitation.Permissions.Add(new PermissionsModel
                {
                    Id = invitation.Permissions[i].Id,
                    Name = invitation.Permissions[i].Name,
                });
            }
            return joinInvitation;
        }

        public static async Task<ChangePermissionInvitationCommand> ConvertTOChangePermissionInvitationCommand(this InvitationRequest invitation)
        {
            await ValidInvtation(invitation);
            ChangePermissionInvitationCommand changePermissionsInvitation = new ChangePermissionInvitationCommand(
                accountId: invitation.InvitationInfo.AccountId,
                memberId: invitation.InvitationInfo.MemberId,
                userId: invitation.InvitationInfo.UserId,
                subscriptionId: invitation.InvitationInfo.SubscriptionId,
                Permissions: new List<PermissionsModel>()
                );
            for (int i = 0; i < invitation.Permissions.Count(); i++)
            {
                changePermissionsInvitation.Permissions.Add(new PermissionsModel
                {
                    Id = invitation.Permissions[i].Id,
                    Name = invitation.Permissions[i].Name,
                });
            }
            return changePermissionsInvitation;
        }

        public static async Task<LeaveInvitationCommand> ConvertTOLeaveInvitationCommand(this InvitationInfoRequest invitation)
        {
            await ValidInvtationInfo(invitation);
            LeaveInvitationCommand leaveInvitation = new LeaveInvitationCommand(
                accountId: invitation.AccountId,
                memberId: invitation.MemberId,
                userId: invitation.UserId,
                subscriptionId: invitation.SubscriptionId
                );
            return leaveInvitation;
        }

        public static async Task<RemoveInvitationCommand> ConvertTORemoveInvitationCommand(this InvitationInfoRequest invitation)
        {
            await ValidInvtationInfo(invitation);
            RemoveInvitationCommand removeInvitation = new RemoveInvitationCommand(
                accountId: invitation.AccountId,
                memberId: invitation.MemberId,
                userId: invitation.UserId,
                subscriptionId: invitation.SubscriptionId
                );
            return removeInvitation;
        }

        private static async Task ValidInvtation(InvitationRequest invitation)
        {
            InvitationRequestValidation validation = new InvitationRequestValidation();
            ValidationResult result = await validation.ValidateAsync(invitation);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        private static async Task ValidInvtationInfo(InvitationInfoRequest invitation)
        {
            InvitationInfoRequestValidation validation = new InvitationInfoRequestValidation();
            ValidationResult result = await validation.ValidateAsync(invitation);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }

}

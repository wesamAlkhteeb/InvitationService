using FluentValidation;

namespace InvitationCommandService.Presentation.Validation
{
    public class InvitationRequestValidation : AbstractValidator<InvitationRequest>
    {
        public InvitationRequestValidation()
        {
            RuleFor(inv => inv.InvitationInfo.AccountId).GreaterThan(0);
            RuleFor(inv => inv.InvitationInfo.MemberId).GreaterThan(0);
            RuleFor(inv => inv.InvitationInfo.SubscriptionId).GreaterThan(0);
            RuleFor(inv => inv.InvitationInfo.UserId).GreaterThan(0);
            RuleFor(inv => inv.Permissions.Count()).GreaterThan(0);
        }
    }
}

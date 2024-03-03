using FluentValidation;

namespace InvitationCommandService.Presentation.Validation
{
    public class InvitationInfoRequestValidation : AbstractValidator<InvitationInfoRequest>
    {
        public InvitationInfoRequestValidation()
        {
            RuleFor(inv => inv.AccountId).GreaterThan(0);
            RuleFor(inv => inv.MemberId).GreaterThan(0);
            RuleFor(inv => inv.SubscriptionId).GreaterThan(0);
            RuleFor(inv => inv.UserId).GreaterThan(0);
        }
    }
}

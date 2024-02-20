using Grpc.Core;
using InvitationCommandService;

namespace InvitationCommandService.Services
{
    public class InvitationService:Invitation.InvitationBase
    {
        private readonly ILogger<InvitationService> _logger;
        public InvitationService(ILogger<InvitationService> logger)
        {
            _logger = logger;
        }

    }
}

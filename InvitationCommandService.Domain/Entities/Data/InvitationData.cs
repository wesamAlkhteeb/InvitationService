using InvitationCommandService.Domain.Model;

namespace InvitationCommandService.Domain.Entities.Data
{
    public class InvitationData
    {
        public required InvitationInfoData Info { get; set; }
        public required List<PermissionsModel> Permissions { get; set; }
    }


}

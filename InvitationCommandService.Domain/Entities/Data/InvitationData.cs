using InvitationQueryService.Domain.Model;

namespace InvitationQueryService.Domain.Entities.Data
{
    public class InvitationData
    {
        public required InvitationInfoData Info { get; set; }
        public required List<PermissionsModel> Permissions { get; set; }
    }


}

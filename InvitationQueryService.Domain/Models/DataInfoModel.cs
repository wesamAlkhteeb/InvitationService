namespace InvitationQueryService.Domain.Models
{
    public class DataInfoModel
    {
        public required InfoModel Info { get; set; }
        public required List<PermissionModel> Permissions { get; set; }
    }
}
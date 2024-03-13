namespace InvitationCommandService.Domain
{
    public enum EventType
    {
        ChangePermissionEvent, 
        LeaveEvent,
        RemoveEvent,
        JoinEvent, 
        RejectEvent, 
        AcceptEvent,
        CancelEvent,
        SendEvent
    }
}

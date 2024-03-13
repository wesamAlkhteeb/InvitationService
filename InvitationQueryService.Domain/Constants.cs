namespace InvitationQueryService.Domain
{
    public class Constants
    {
        public static int COUNT_ITEM_IN_PAGE = 5;
    }
    public enum InvitationState
    {
        Pending, Joined, Out
    }


    // I don't remember types of Subscription so I set A,B,C
    public enum SubscriptionType
    {
        A,B,C  
    }
}

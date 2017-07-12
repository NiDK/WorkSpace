namespace PwC.C4.Infrastructure.Logger
{
    public static class BehaviorLogWrapper
    {

        private static readonly C4LogServiceClient C4Client = null;

        static BehaviorLogWrapper()
        {
            if (C4Client == null)
            {
                C4Client = new C4LogServiceClient();
            }
        }


        public static void Log(string appcode,string userId, string description)
        {
            Log(appcode, "", "", "", userId, description);
        }

        public static void Log(string appcode, string optionType, string optionId, string method, string userId, string description)
        {
            C4Client.Log_ForUserBehavior_Insert(appcode,optionType, optionId, method, userId, description);
        }
    }
}

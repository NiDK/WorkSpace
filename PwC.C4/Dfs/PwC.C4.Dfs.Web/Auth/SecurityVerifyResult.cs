namespace PwC.C4.Dfs.Web.Auth
{
    public enum SecurityVerifyResult
    {
        Success = 1,    
        InvalidUrl = 2,
        UserNotLogin = 3,
        AppCodeMismatch = 4,
        NoUserIdFound = 5,
        UserMismatch = 6,
        NoEmailFound = 7,
        RequiredParameterNotFound = 8,
        Expired = 9,
    }
}

namespace PwC.C4.Dfs.Common.Model.Enums
{
    public enum SecurityLevel
    {
        Public = 1,    // No security verification
        Protected = 2, // URL integrity, email and captcha verification
        Private = 3,   // URL integrity, Login, user verification
    }
}

using System;

namespace PwC.C4.Dfs.Web.Services
{
    internal static class Executor
    {
        public static void Execute(bool condition, Action @true, Action @false)
        {
            if (condition) @true(); else @false();
        }

        public static void Execute(Action @try, Action<Exception> @catch)
        {
            try
            {
                @try();
            }
            catch (Exception ex)
            {
                @catch(ex);
            }
        }
    }
}
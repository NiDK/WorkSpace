using System.Threading;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Client.Helper
{
    public class ZeroActionCounter
    {
        private int count;
        private ManualResetEvent zeroEvent;

        public ZeroActionCounter(int initValue)
        {
            ArgumentHelper.AssertPositive(initValue);
            this.count = initValue;
            zeroEvent = new ManualResetEvent(false);
        }

        public void Decrement()
        {
            if (Interlocked.Decrement(ref count) == 0)
                zeroEvent.Set();
        }

        public bool WaitForZero()
        {
            return zeroEvent.WaitOne();
        }

        public bool WaitForZero(int timeoutMilliseconds)
        {
            return zeroEvent.WaitOne(timeoutMilliseconds);
        }
    }
}

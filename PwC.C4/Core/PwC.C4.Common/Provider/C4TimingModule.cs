using System;
using System.Diagnostics;
using System.Web;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Common.Provider
{
    public class C4TimingModule : IHttpModule
    {
        public void Dispose()
        {
        }
        static readonly LogWrapper log = new LogWrapper();
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        void OnBeginRequest(object sender, System.EventArgs e)
        {
            var stopwatch = new Stopwatch();
            HttpContext.Current.Items["Stopwatch"] = stopwatch;
            stopwatch.Start();
        }

        private void OnEndRequest(object sender, System.EventArgs e)
        {
            var stopwatch =
                   (Stopwatch)HttpContext.Current.Items["Stopwatch"];
            stopwatch.Stop();

            var ts = stopwatch.Elapsed;
            var elapsedTime = String.Format("{0}ms", ts.TotalMilliseconds);

            log.Debug("Page:" + HttpContext.Current.Request.RawUrl + " render time:" + elapsedTime);
        }
    }
}
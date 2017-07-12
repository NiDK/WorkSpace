using System;
using System.Diagnostics;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Web.Services
{
    internal class PerfCounterDefinition
    {
        public string Name { get; set; }
        public string Help { get; set; }
        public PerformanceCounterType Type { get; set; }

        public PerfCounterDefinition() { }
        public PerfCounterDefinition(string name, string help, PerformanceCounterType type)
        {
            ArgumentHelper.AssertNotEmpty(name);
            ArgumentHelper.AssertNotEmpty(help);
            this.Name = name;
            this.Help = help;
            this.Type = type;
        }
    }

    internal class PerfCounters
    {
        private static readonly LogWrapper logger = new LogWrapper();

        #region counter definitions

        public static readonly string PerfCategoryName = "PwC C4 Dfs";
        public static readonly string PerfCategoryHelp = "Performance Counters for PwC C4 Dfs";

        public static readonly PerfCounterDefinition[] PerfCounterDefinitions = {
            new PerfCounterDefinition("Invalid Req/Sec", "Invalid requests per second", PerformanceCounterType.RateOfCountsPerSecond32),

            new PerfCounterDefinition("Download/Sec", "Successful downloads per second", PerformanceCounterType.RateOfCountsPerSecond32),
            new PerfCounterDefinition("Download Bytes/Sec", "Successfully downloaded bytes per second", PerformanceCounterType.RateOfCountsPerSecond32),

            new PerfCounterDefinition("Avg Download Bytes", "Average bytes downloaded per successful download", PerformanceCounterType.AverageCount64),
            new PerfCounterDefinition("Avg Download Bytes Base", "Avg download bytes base", PerformanceCounterType.AverageBase),

            new PerfCounterDefinition("Avg Download Time", "Average time in milliseconds per successful download takes", PerformanceCounterType.AverageCount64),
            new PerfCounterDefinition("Avg Download Time Base", "Avg Download Time Base", PerformanceCounterType.AverageBase),

            new PerfCounterDefinition("Not Found (404)/Sec", "Download requested file not found (404) per second", PerformanceCounterType.RateOfCountsPerSecond32),

            new PerfCounterDefinition("Delete/Sec", "Deletes per second", PerformanceCounterType.RateOfCountsPerSecond32),

            new PerfCounterDefinition("Upload/Sec", "Uploads per second", PerformanceCounterType.RateOfCountsPerSecond32),
            new PerfCounterDefinition("Upload Bytes/Sec", "Successfully uploaded bytes per second", PerformanceCounterType.RateOfCountsPerSecond32),

            new PerfCounterDefinition("Avg Upload Bytes", "Average bytes uploaded per successful upload", PerformanceCounterType.AverageCount64),
            new PerfCounterDefinition("Avg Upload Bytes Base", "Avg upload bytes base", PerformanceCounterType.AverageBase),

            new PerfCounterDefinition("Avg Upload Time", "Average time in milliseconds per successful upload takes", PerformanceCounterType.AverageCount64),
            new PerfCounterDefinition("Avg Upload Time Base", "Avg Upload Time Base", PerformanceCounterType.AverageBase),

            new PerfCounterDefinition("Invalid Security Token/Sec", "Invalid security tokens received per second", PerformanceCounterType.RateOfCountsPerSecond32),
            new PerfCounterDefinition("Empty Upload/Sec", "Upload requests with no file per second", PerformanceCounterType.RateOfCountsPerSecond32),
            new PerfCounterDefinition("Too Large Upload/Sec", "Upload requests with too large file per second", PerformanceCounterType.RateOfCountsPerSecond32)
        };

        public enum PerfCounterIndex : int
        {
            InvalidRequest = 0,
            Download = 1,
            DownloadBytesPerSecond = 2,
            AvgDownloadBytes = 3,
            AvgDownloadBytesBase = 4,
            AvgDownloadTime = 5,
            AvgDownloadTimeBase = 6,
            FileNotFound = 7,
            Delete = 8,
            Upload = 9,
            UploadBytesPerSecond = 10,
            AvgUploadBytes = 11,
            AvgUploadBytesBase = 12,
            AvgUploadTime = 13,
            AvgUploadTimeBase = 14,
            InvalidSecurityToken = 15,
            EmptyUpload = 16,
            TooLargeUpload = 17,
        }

        #endregion

        #region singleton

        private static PerfCounters instance;

        static PerfCounters()
        {
            instance = new PerfCounters();
            instance.Initialize();
        }

        public static PerfCounters Instance
        {
            get { return instance; }
        }

        #endregion

        private PerformanceCounter[] counters;

        private bool initialized;
        private void Initialize()
        {
            initialized = PerformanceCounterCategory.Exists(PerfCategoryName) ? true : InstallCounters();
            try
            {
                if (initialized)
                {
                    int count = PerfCounterDefinitions.Length;

                    counters = new PerformanceCounter[count];
                    for (int i = 0; i < count; ++i)
                    {
                        counters[i] = new PerformanceCounter(PerfCategoryName, PerfCounterDefinitions[i].Name, false);
                    }

                    ResetCounters();
                }
            }
            catch (Exception ex)
            {
                logger.HandleException(ex, "DfsPerfCounters");
                initialized = false;
            }
        }

        private void ResetCounters()
        {
            if (initialized)
            {
                foreach (var counter in counters)
                    counter.RawValue = 0;
            }
        }

        public void CountInvalidRequest()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.InvalidRequest].Increment();
        }

        public void CountFileNotFound()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.FileNotFound].Increment();
        }

        public void CountDownload(long downloadBytes, long downloadTicks)
        {
            if (initialized)
            {
                counters[(int)PerfCounterIndex.Download].Increment();
                counters[(int)PerfCounterIndex.DownloadBytesPerSecond].IncrementBy(downloadBytes);
                counters[(int)PerfCounterIndex.AvgDownloadBytes].IncrementBy(downloadBytes);
                counters[(int)PerfCounterIndex.AvgDownloadBytesBase].Increment();

                int milliseconds = (int)(((double)downloadTicks / Stopwatch.Frequency) * 1000);
                counters[(int)PerfCounterIndex.AvgDownloadTime].IncrementBy(milliseconds);
                counters[(int)PerfCounterIndex.AvgDownloadTimeBase].Increment();
            }
        }

        public void CountInvalidSecurityToken()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.InvalidSecurityToken].Increment();
        }

        public void CountDelete()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.Delete].Increment();
        }

        public void CountEmptyUpload()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.EmptyUpload].Increment();
        }

        public void CountTooLargeUpload()
        {
            if (initialized)
                counters[(int)PerfCounterIndex.TooLargeUpload].Increment();
        }

        public void CountUpload(int uploadBytes, long uploadTicks)
        {
            if (initialized)
            {
                counters[(int)PerfCounterIndex.Upload].Increment();
                counters[(int)PerfCounterIndex.UploadBytesPerSecond].IncrementBy(uploadBytes);
                counters[(int)PerfCounterIndex.AvgUploadBytes].IncrementBy(uploadBytes);
                counters[(int)PerfCounterIndex.AvgUploadBytesBase].Increment();

                int milliseconds = (int)(((double)uploadTicks / Stopwatch.Frequency) * 1000);
                counters[(int)PerfCounterIndex.AvgUploadTime].IncrementBy(milliseconds);
                counters[(int)PerfCounterIndex.AvgUploadTimeBase].Increment();
            }
        }

        #region install / remove counters

        public static bool InstallCounters()
        {
            try
            {
                logger.Info("Creating Performance Counter Category " + PerfCategoryName);

                var counterDataCollection = new CounterCreationDataCollection();
                foreach (var counter in PerfCounterDefinitions)
                {
                    counterDataCollection.Add(new CounterCreationData(counter.Name, counter.Help, counter.Type));
                    logger.Info("Creating Performance Counter " + counter.Name);
                }

                PerformanceCounterCategory.Create(PerfCategoryName, PerfCategoryHelp,
                    PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

                return true;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error creating Perfomance Counter Category {0}: {1}. Performance Counter Category will not be used.",
                    PerfCategoryName, ex);
                return false;
            }
        }

        public static void RemoveCounters()
        {
            try
            {
                logger.Info("Removing Performance Counter Category " + PerfCategoryName);
                PerformanceCounter.CloseSharedResources();
                PerformanceCounterCategory.Delete(PerfCategoryName);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Error removing Perfomance Counter Category {0}: {1}",
                    PerfCategoryName, ex);
            }
        }

        #endregion
    }
}

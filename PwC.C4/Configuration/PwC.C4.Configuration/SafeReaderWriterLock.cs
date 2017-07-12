using System;
using System.Threading;

namespace PwC.C4.Configuration
{
    public interface ISafeLock : IDisposable
    {
        void AcquireLock();
    }

    public class SafeReaderWriterLock
    {
        SafeUpgradeReaderLock upgradableReaderLock;
        SafeReaderLock readerLock;
        SafeWriterLock writerLock;
        
        ReaderWriterLockSlim locker;

        public SafeReaderWriterLock()
        {
            locker = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion);
            upgradableReaderLock = new SafeUpgradeReaderLock(locker);
            readerLock = new SafeReaderLock(locker);
            writerLock = new SafeWriterLock(locker);
        }

        public ISafeLock AcquireReaderLock()
        {
            readerLock.AcquireLock();
            return readerLock;
        }

        public ISafeLock AcquireWriterLock()
        {
            writerLock.AcquireLock();
            return writerLock;
        }

        public ISafeLock AcquireUpgradeableReaderLock()
        {
            upgradableReaderLock.AcquireLock();
            return upgradableReaderLock;
        }

       
    }

    class SafeReaderLock : ISafeLock
    {
        ReaderWriterLockSlim locker;
        public SafeReaderLock(ReaderWriterLockSlim locker)
        {
            this.locker = locker;
        }         

        #region IDisposable 成员

        public void Dispose()
        {
            locker.ExitReadLock();
        }

        #endregion

        #region ISafeLock 成员

        public void AcquireLock()
        {
            locker.EnterReadLock();
        }

        #endregion
    }

    class SafeWriterLock : ISafeLock
    {
        ReaderWriterLockSlim locker;
        public SafeWriterLock(ReaderWriterLockSlim locker)
        {
            this.locker = locker;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            locker.ExitWriteLock();
        }

        #endregion

        #region ISafeLock 成员

        public void AcquireLock()
        {
            locker.EnterWriteLock();

        }

        #endregion
    }

    class SafeUpgradeReaderLock : ISafeLock
    {
        ReaderWriterLockSlim locker;
        public SafeUpgradeReaderLock(ReaderWriterLockSlim locker)
        {
            this.locker = locker;
        }
        #region IDisposable 成员

        public void Dispose()
        {
            locker.ExitUpgradeableReadLock();           
        }

        #endregion

        #region ISafeLock 成员

        public void AcquireLock()
        {
            locker.EnterUpgradeableReadLock();
        }

        #endregion
    }
}

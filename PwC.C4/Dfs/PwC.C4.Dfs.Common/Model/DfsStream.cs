using System;
using System.IO;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Common.Model
{
    internal class DfsStream : Stream
    {
        #region Properties

        private DfsBlockInfo[] _blocks;

        private byte[] _data;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        private long _length;
        public override long Length
        {
            get { return _length; }
        }

        private long _position;
        public override long Position
        {
            get { return _position; }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ctor

        public DfsStream(long length, DfsBlockInfo[] blocks, byte[] data)
        {
            _length = length == 0 ? data.Length : length;
            _blocks = blocks;
            _data = data;
        }

        #endregion

        //public override int Read(byte[] buffer, int offset, int count)
        //{
        //    EnsureBufferLength(buffer, offset, count);
        //    EnsureStreamLength();

        //    FetchData();

        //    int read = Math.Min(_data.Length - _dataIndex, count);

        //    Buffer.BlockCopy(_data, _dataIndex, buffer, offset, read);
        //    _dataIndex += read;
        //    _position += read;

        //    return read;
        //}

        //private void FetchData()
        //{
        //    if (_dataIndex == _data.Length)
        //    {
        //        var path = DfsPath.Parse(_blocks[_blockIndex++].Path);
        //        _data = DfsClientHelper.RetrieveRecord(path.Keyspace, path.AppCode, path.FileId).Data;
        //        _dataIndex = 0;
        //    }
        //}

        private void EnsureStreamLength()
        {
            if (_position == _length) throw new EndOfStreamException();
        }

        private void EnsureBufferLength(byte[] buffer, int offset, int count)
        {
            ArgumentHelper.AssertNotNull(buffer, "buffer");
            ArgumentHelper.AssertInRange("offset", offset, 0, buffer.Length - 1);
            ArgumentHelper.AssertPositive(count, "count");

            if (offset + count > buffer.Length) throw new ArgumentOutOfRangeException("Buffer is not big enough");
        }

        #region NotImplemented

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

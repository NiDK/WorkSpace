using System;
using System.IO;
using Newtonsoft.Json;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Common.Model
{
    public class DfsOperationResult
    {
        public bool Succeeded { get; set; }
        public string DfsPath { get; set; }
        public string ErrorMessage { get; set; }

        #region ctor

        public DfsOperationResult() { }

        public DfsOperationResult(bool succeeded)
        {
            this.Succeeded = succeeded;
        }

        public DfsOperationResult(bool succeeded, string dfsPath)
        {
            this.Succeeded = succeeded;
            this.DfsPath = dfsPath;
        }

        public DfsOperationResult(string dfsPath, string errorMessage)
        {
            this.Succeeded = false;
            this.DfsPath = dfsPath;
            this.ErrorMessage = errorMessage;
        }

        public DfsOperationResult(string errorMessage)
        {
            this.Succeeded = false;
            this.ErrorMessage = errorMessage;
        }

        #endregion

        #region Serialization

        private static readonly JsonSerializer _serializer = new JsonSerializer();
        public static readonly Type Type = typeof(DfsOperationResult);

        public string ToJson()
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }

        public static DfsOperationResult FromJson(string json)
        {
            ArgumentHelper.AssertNotEmpty(json, "json");

            using (var reader = new StringReader(json))
            {
                return (DfsOperationResult)_serializer.Deserialize(reader, Type);
            }
        }

        #endregion

        #region Helper

        public static DfsOperationResult Success()
        {
            return new DfsOperationResult(true);
        }

        public static DfsOperationResult Success(DfsPath dfsPath)
        {
            return new DfsOperationResult(true, dfsPath.ToString());
        }

        public static DfsOperationResult Success(string dfsPath)
        {
            return new DfsOperationResult(true, dfsPath);
        }

        public static DfsOperationResult Fail(DfsPath dfsPath, string errorMessage)
        {
            return new DfsOperationResult(dfsPath.ToString(), errorMessage);
        }

        public static DfsOperationResult Fail(string dfsPath, string errorMessage)
        {
            return new DfsOperationResult(dfsPath, errorMessage);
        }

        public static DfsOperationResult Fail(string errorMessage)
        {
            return new DfsOperationResult(errorMessage);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace PwC.C4.Infrastructure.Helper
{
    public static class FileHelper
    {
        public static string CheckFileExistReturnNewPath(string savePath,string fileName,out string newFileName)
        {
            var filePath = Path.Combine(savePath, fileName);
            var fileExist = System.IO.File.Exists(filePath);
            if (fileExist)
            {
                var dotIndex = fileName.LastIndexOf(".", System.StringComparison.Ordinal);
                var allName = fileName.Substring(0, dotIndex);
                var extName = fileName.Substring(dotIndex, fileName.Length - dotIndex);
                fileName = allName + '-' + DateTime.Now.ToString("fff") + extName;
                filePath = Path.Combine(savePath, fileName);
            }
            newFileName = fileName;
            return filePath;
        }

        public static bool CheckFileExist(string savePath)
        {
            var fileExist = System.IO.File.Exists(savePath);
            return fileExist;
        }

        public static bool CheckFileExist(string savePath, string fileName)
        {
            var filePath = Path.Combine(savePath, fileName);
            var fileExist = System.IO.File.Exists(filePath);
            return fileExist;
        }
    }





}

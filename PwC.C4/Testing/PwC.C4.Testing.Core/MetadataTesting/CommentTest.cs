using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Common.Provider;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Testing.Core.MetadataTesting
{
    [TestClass]
    public class CommentTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //new Comment and reply to comments/reply to reply.  append the information into the Inspire Mial Box.
            var mailTo ="chenhui.yu@cn.pwc.com";
            var leaderName = "wuha";
            var dic = new Dictionary<string, string>
                            {
                                {"Message", "Message"},
                                {"Leader", leaderName},
                                {"Submitter", "Chenhui yu"},
                                {"SendDate", DateTime.Now.ToString("dd/MM/yyyy")}
                            };

           // var result = EmailServiceProvider.SingleEmailSend("Comment", mailTo, null, null, dic);
        }

        [TestMethod]
        public void TestInsertMailWithAttachments()
        {
            var mailTo = "chenhui.yu@cn.pwc.com";
            var leaderName = "wuha";
            var dic = new Dictionary<string, string>
            {
                {"Message", leaderName},
                {"Submitter", "Chenhui yu"},
                {"SendDate", DateTime.Now.ToString("dd/MM/yyyy")}
            };
            var attas = new List<MailAttachment>();
            FileStream file = new FileStream("C:\\Avatar.JPG", FileMode.Open);
            FileStream file1 = new FileStream("C:\\Chrysanthemum.jpg", FileMode.Open);
            //FileStream file2 = new FileStream("C:\\Source Code-Inspire-2015-12-07.zip", FileMode.Open);
            
            attas.Add(new MailAttachment()
            {
                Content = StreamHelper.StreamToBytes(file),
                Name = "Avatar1",
                FileName = "Avatar1.JPG",
                LinkedResourceFlag = false,
                MimeType = MimeMapping.GetMimeMapping("Avatar1.JPG")
        });
            attas.Add(new MailAttachment()
            {
                Content = StreamHelper.StreamToBytes(file1),
                Name = "Chrysanthemum1",
                FileName = "Chrysanthemum1.jpg",
                LinkedResourceFlag = false,
                MimeType = MimeMapping.GetMimeMapping("Chrysanthemum1.jpg")
            });
            //attas.Add(new MailAttachment()
            //{
            //    Content = StreamHelper.StreamToBytes(file2),
            //    Name = "Source Code-Inspire-2015-12-07",
            //    FileName = "Source Code-Inspire-2015-12-07.zip",
            //    LinkedResourceFlag = false,
            //    MimeType = MimeMapping.GetMimeMapping("Source Code-Inspire-2015-12-07.zip")
            //});

            var result = EmailServiceProvider.SingleEmailSend("PrivateMessage", mailTo, null, attas, null,
                null, null, null, dic);
            // string str2 = File.ReadAllText(@"c:\MailQueueModel.txt", Encoding.ASCII);
            // var mm = JsonHelper.Deserialize<MailQueueModel>(str2);
            //var data = PwC.C4.DataService.Persistance.MailMasterDao.InsertToMailQueueWithAttachment(mm, attas);
        }
        [TestMethod]
        public void TestInsertAtta()
        {
            //FileStream file1 = new FileStream("C:\\Users\\Chenhui Yu\\Downloads\\jenkins-1.624.zip", FileMode.Open);

            //var dfs =
            //    C4.Dfs.Client.Dfs.Store(
            //        new DfsItem("Report", "jenkins-1.624.zip", file1, "utf-8", "InspireC4"), "");

            var dfs = C4.Dfs.Client.Dfs.Get("dfs://Report/InspireC4/f02266f66d4d4ee294e12ec99f580904.zip");

            //var d = new MailAttachment()
            //{
            //    Content = dfs.FileDataBytes,
            //    Name = "jenkins-1.624",
            //    FileName = "jenkins-1.624.zip",
            //    LinkedResourceFlag = false,
            //    MimeType = MimeMapping.GetMimeMapping("jenkins-1.624.zip")
            //};
            //var di= EmailServiceProvider.InsertMailAttachment("dfs://Image/InspireC4/faa482fb518a48c6b7e7ddebab7e77fc.jpg","DfsTest",false);
        }
    }
}

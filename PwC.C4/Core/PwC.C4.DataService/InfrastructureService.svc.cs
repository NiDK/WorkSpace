using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Helpers;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Persistance;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.DataService
{
    public class InfrastructureService : IInfrastructure
    {
        private static readonly LogWrapper Log = new LogWrapper();
        public int InsertToMailQueue(MailQueueModel model)
        {
            return MailMasterDao.InsertToMailQueue(model);
        }

        public int InsertToMailQueueBatch(List<MailQueueModel> model)
        {
            return MailMasterDao.InsertToMailQueueBatch(model);
        }
         
        public StaffInfo Staff_Get(string staffId)
        {
            return StaffMasterDao.GetStaffInfo(staffId);
        }

        public string InsertInvitation(Invitation invitation, IList<InvitationRole> invitationRoles)
        {
            try
            {
                var roleType = "";
                var roles = invitationRoles.ToTable(out roleType);
                if (roleType == "-2")
                {
                    return "-2";
                }
                else
                {
                    var inv = invitation.ToTable();
                    var result = InvitationDao.AddInvitation(inv, roles);
                    switch (result)
                    {
                        case "Error":
                            return "-1";
                        case "None":
                            return "";
                        default:
                            if (invitation.EmailClientType != "Notes") return "";
                            var mq = new MailQueueModel
                            {
                                MailFrom = AppSettings.Instance.GetConfigSettings("CalendarMailFrom"),
                                MailTo = AppSettings.Instance.GetConfigSettings("CalendarNotesMailBox"),
                                ImmediateFlag = "Y",
                                ReplyTo = "",
                                SendDate = DateTime.Now,
                                Subject = result,
                                SubmitBy = "System",
                                AppCode = AppSettings.Instance.GetConfigSettings("CalendarAppCode")
                            };
                            var rst = MailMasterDao.InsertToMailQueue(mq);
                            return rst > 0 ? result : "-1";
                    }
                }
            }
            catch (Exception ee)
            {
                Log.Error(
                    "InsertInvitation error,invitation:" + JsonHelper.Serialize(invitation) + ",invitationRoles:" +
                    JsonHelper.Serialize(invitationRoles), ee);
                return "-1";
            }

            
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IInfrastructure
    {

        #region MailMaster

        [OperationContract]
        int InsertToMailQueue(MailQueueModel model);

        [OperationContract]
        int InsertToMailQueueBatch(List<MailQueueModel> model);

        #endregion

        #region StaffBank

        [OperationContract]
        StaffInfo Staff_Get(string staffId);

        #endregion

        #region invitation
        [OperationContract]
        string InsertInvitation(Invitation invitation, IList<InvitationRole> invitationRoles);
        #endregion


    }


}

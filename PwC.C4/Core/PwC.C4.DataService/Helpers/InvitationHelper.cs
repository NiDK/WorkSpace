using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;

namespace PwC.C4.DataService.Helpers
{

    public static class InvitationHelper
    {
        public static DataTable ToTable(this Invitation invitation)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("SendFlag", typeof(string));
            dataTable.Columns.Add("InvitationType", typeof(string));
            dataTable.Columns.Add("Emailclienttype", typeof(string));
            dataTable.Columns.Add("SendDate", typeof(DateTime));
            dataTable.Columns.Add("InvitationStartTime", typeof(DateTime));
            dataTable.Columns.Add("InvitationEndTime", typeof(DateTime));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Location", typeof(string));
            dataTable.Columns.Add("InvitationtimeZone", typeof(int));
            dataTable.Columns.Add("CreatedBy", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("AppCode", typeof(string));
            var dataRow = dataTable.NewRow();
            dataRow["SendFlag"] = invitation.SendFlag;
            dataRow["InvitationType"] = invitation.InvitationType;
            dataRow["Emailclienttype"] = invitation.EmailClientType;
            if (invitation.SendDate != null)
            {
                dataRow["SendDate"] = invitation.SendDate;
            }
            var dtst = Convert.ToDateTime(invitation.InvitationStartTime);
            dataRow["InvitationStartTime"] = dtst.ToString("yyyy-MM-dd HH:mm");
            var dtet = Convert.ToDateTime(invitation.InvitationEndTime);
            dataRow["InvitationEndTime"] = dtet.ToString("yyyy-MM-dd HH:mm");
            dataRow["Subject"] = invitation.Subject;
            dataRow["Description"] = invitation.Description;
            dataRow["Location"] = invitation.Location;
            dataRow["InvitationtimeZone"] = invitation.InvitationtimeZone;
            dataRow["CreatedBy"] = invitation.CreatedBy;
            dataRow["CreatedDate"] = invitation.CreatedDate;
            dataRow["AppCode"] = invitation.AppCode;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }

        public static DataTable ToTable(this IList<InvitationRole> roles,out string type)
        {
            var dtRoles = new DataTable();
            dtRoles.Columns.Add("Roletype", typeof(string));
            dtRoles.Columns.Add("RoleEmail", typeof(string));
            dtRoles.Columns.Add("ISRequired", typeof(string));
            foreach (var p in roles)
            {
                if (p.RoleType == "Participant" || p.RoleType == "Chairman")
                {
                    var drRoles = dtRoles.NewRow();
                    drRoles["Roletype"] = p.RoleType;
                    drRoles["RoleEmail"] = p.RoleEmail;
                    drRoles["ISRequired"] = p.IsRequired;
                    dtRoles.Rows.Add(drRoles);
                }
                else
                {
                    type = "-2";
                }
            }
            type = "";
            return dtRoles;
        }

    }
}

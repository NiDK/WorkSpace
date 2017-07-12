﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    public class StaffMasterDao
    {
        internal static StaffInfo GetStaffInfo(string staffId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<StaffInfo>(db, "dbo.vwStaffMaster_GetByStaffId",
                parameters => parameters.AddWithValue("@staffId", staffId), MapperStaffMaster);
            return myentity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where">start with word : 'AND'</param>
        /// <returns></returns>
        internal static List<StaffInfo> GetStaffList(string where)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            List<StaffInfo> list = SafeProcedure.ExecuteAndGetInstanceList<StaffInfo>(db,
                "dbo.vwStaffMaster_List",
                MapperStaffMaster,
                new SqlParameter[]
                {
                    new SqlParameter("@Where", where)
                }
                );
            return list;
        }

        private static void MapperStaffMaster(IRecord record, StaffInfo entity)
        {

            entity.StaffId = record.Get<string>("StaffId").Trim();
            entity.CountryCode = record.Get<string>("CountryCode");
            entity.StaffInitial = record.Get<string>("StaffInitial");
            entity.StaffName = record.Get<string>("StaffName");
            entity.FirstName = record.Get<string>("FirstName");
            entity.LastName = record.Get<string>("LastName");
            entity.DivCode = record.Get<string>("DivCode");
            entity.DivName = record.Get<string>("DivName");
            entity.GroupCode = record.Get<string>("GroupCode");
            entity.GroupName = record.Get<string>("GroupName");
            entity.GradeCode = record.Get<string>("GradeCode");
            entity.GradeName = record.Get<string>("GradeName");
            entity.JobTitle = record.Get<string>("JobTitle");
            entity.PhoneNo = record.Get<string>("PhoneNo");
            entity.OfficeBuilding = record.Get<string>("OfficeBuilding");
            entity.OfficeFloor = record.Get<string>("OfficeFloor");
            entity.SecName = record.Get<string>("SecName");
            entity.OfficePIN = record.Get<string>("SecName");
            entity.LoginID = record.Get<string>("LoginID");
            entity.LoginContext = record.Get<string>("LoginContext");
            entity.PowerStaffCode = record.Get<string>("PowerStaffCode");
            entity.PowerGroupCode = record.Get<string>("PowerGroupCode");
            entity.PowerGradeCode = record.Get<string>("PowerGradeCode");
            entity.TermFlag = record.Get<string>("TermFlag");
            entity.TermDate = record.Get<DateTime?>("TermDate");
            entity.Email = record.Get<string>("Email");
            entity.GUID = record.Get<string>("GUID");
            entity.LDAP_DistName = record.Get<string>("LDAP_DistName");
            entity.PwCEntityID = record.Get<string>("PwCEntityID");
            entity.localExpatFlag = record.Get<string>("localExpatFlag");
            entity.ExpatFlag = record.Get<string>("ExpatFlag");
            entity.INetEmail = record.Get<string>("INetEmail");
            entity.NotesID = record.Get<string>("NotesID");
            entity.BU = record.Get<string>("BU");
            entity.BUDesc = record.Get<string>("BUDesc");
            entity.SubService = record.Get<string>("SubService");
            entity.SubServiceDesc = record.Get<string>("SubServiceDesc");
            entity.JoinDate = record.Get<DateTime?>("JoinDate");
            entity.IsProfessional = record.Get<string>("IsProfessional");
            entity.JobCode = record.Get<string>("JobCode");
            entity.NativeName = record.Get<string>("NativeName");
            entity.FullName = record.Get<string>("FullName");
            entity.NameInEng = record.Get<string>("NameInEng");
            entity.GivenName = record.Get<string>("NameInEng");
            entity.PreferredName = record.Get<string>("PreferredName");
            entity.TerritoryEffectiveDate = record.Get<DateTime?>("TerritoryEffectiveDate");
            entity.OfficeCityName = record.Get<string>("OfficeCityName");
            entity.HRID = record.Get<string>("HRID");
            entity.CountryOffice = record.Get<string>("CountryOffice");
            entity.OUCode = record.Get<string>("OUCode");
            entity.Status = record.Get<string>("Status");
            //entity.ContractYearEnd = record.Get<DateTime?>("ContractYearEnd"));
            entity.IDTerritory = record.Get<string>("IDTerritory");
            entity.PowerIDStatus = record.Get<string>("PowerIDStatus");
            entity.LoS = record.Get<string>("LoS");
            entity.LoSDesc = record.Get<string>("LoSDesc");
            entity.Department = record.Get<string>("Department");
            entity.SalaryCategory = record.Get<string>("SalaryCategory");
            entity.JobGrade = record.Get<string>("JobGrade");
            entity.EmployeeCategory = record.Get<string>("EmployeeCategory");
            entity.Role = record.Get<string>("Role");
            entity.Sex = record.Get<string>("Sex");
            entity.SchoolUniversity = record.Get<string>("SchoolUniversity");
            entity.Majority = record.Get<string>("Majority");
            entity.MajorityCategory = record.Get<string>("MajorityCategory");

            entity.Pic = record.GetOrDefault<byte[]>("Pic", null);
            entity.PicType = record.Get<string>("PicType");
            entity.City = record.Get<string>("City");
            entity.ChineseName = record.Get<string>("ChineseName");
        }
    }
}

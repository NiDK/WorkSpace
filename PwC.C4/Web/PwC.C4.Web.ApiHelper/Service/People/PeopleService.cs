using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Ajax.Utilities;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Web.ApiHelper.Models;
using PwC.C4.Web.ApiHelper.Models.ApiModel;

namespace PwC.C4.Web.ApiHelper.Service.People
{
    public static class PeopleService
    {
        private static readonly string Stn = ConfigurationManager.AppSettings["DataSourceTableName"];
        private static readonly string Stp = ConfigurationManager.AppSettings["DataSourcePicTableName"];
        private static readonly string So = ConfigurationManager.AppSettings["DefaultOrderColumn"];
        private static readonly string Som = ConfigurationManager.AppSettings["DefaultOrderMethod"];
        private static readonly string Ssc= ConfigurationManager.AppSettings["DefaultSearchColumn"];
        private static readonly string Sdk = ConfigurationManager.AppSettings["DefaultDataKey"];
        private static readonly string Sps = ConfigurationManager.AppSettings["DefaultPageSize"];
        private static readonly string Sdp = ConfigurationManager.AppSettings["DataPickerProps"];
        private static readonly string Sss = ConfigurationManager.AppSettings["EnableSplitSearch"];
        
        internal static List<StaffInfo> GetStaffList(string tableName,string cols,int pageSize,int pageIndex,string orderBy,string where,out int totalCount)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var rowcountParameter = new SqlParameter("@recordTotal", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var result = SafeProcedure.ExecuteAndGetInstanceList<StaffInfo>(db,
                    "dbo.Common_Paging", MapperStaffMaster, new SqlParameter[]
                    {
                        new SqlParameter("@viewName", tableName),
                        new SqlParameter("@fieldName", cols),
                        new SqlParameter("@pageSize", pageSize),
                        new SqlParameter("@pageNo", pageIndex),
                        new SqlParameter("@orderString", orderBy),
                        new SqlParameter("@whereString", where),
                        new SqlParameter("@keyName", "staffId"),
                        rowcountParameter
                    }
                    );
                totalCount = (int)rowcountParameter.Value;
                return result;
            }
            catch (Exception ex)
            {
                totalCount = 0;
                return new List<StaffInfo>();
            }
        }

        private static void MapperStaffMaster(IRecord record, StaffInfo entity)
        {
            entity.StaffId = record.GetExist("StaffId") ? record.Get<string>("StaffId").Trim() : "";
            entity.CountryCode = record.GetExist("CountryCode") ? record.Get<string>("CountryCode") : "";
            entity.StaffInitial = record.GetExist("StaffInitial") ? record.Get<string>("StaffInitial") : "";
            entity.StaffName = record.GetExist("StaffName") ? record.Get<string>("StaffName") : "";
            entity.FirstName = record.GetExist("FirstName") ? record.Get<string>("FirstName") : "";
            entity.LastName = record.GetExist("LastName") ? record.Get<string>("LastName") : "";
            entity.DivCode = record.GetExist("DivCode") ? record.Get<string>("DivCode") : "";
            entity.DivName = record.GetExist("DivName") ? record.Get<string>("DivName") : "";
            entity.GroupCode = record.GetExist("GroupCode") ? record.Get<string>("GroupCode") : "";
            entity.GroupName = record.GetExist("GroupName") ? record.Get<string>("GroupName") : "";
            entity.GradeCode = record.GetExist("GradeCode") ? record.Get<string>("GradeCode") : "";
            entity.GradeName = record.GetExist("GradeName") ? record.Get<string>("GradeName") : "";
            entity.JobTitle = record.GetExist("JobTitle") ? record.Get<string>("JobTitle") : "";
            entity.PhoneNo = record.GetExist("PhoneNo") ? record.Get<string>("PhoneNo") : "";
            entity.OfficeBuilding = record.GetExist("OfficeBuilding") ? record.Get<string>("OfficeBuilding") : "";
            entity.OfficeFloor = record.GetExist("OfficeFloor") ? record.Get<string>("OfficeFloor") : "";
            entity.SecName = record.GetExist("SecName") ? record.Get<string>("SecName") : "";
            entity.OfficePIN = record.GetExist("SecName") ? record.Get<string>("SecName") : "";
            entity.LoginID = record.GetExist("LoginID") ? record.Get<string>("LoginID") : "";
            entity.LoginContext = record.GetExist("LoginContext") ? record.Get<string>("LoginContext") : "";
            entity.PowerStaffCode = record.GetExist("PowerStaffCode") ? record.Get<string>("PowerStaffCode") : "";
            entity.PowerGroupCode = record.GetExist("PowerGroupCode") ? record.Get<string>("PowerGroupCode") : "";
            entity.PowerGradeCode = record.GetExist("PowerGradeCode") ? record.Get<string>("PowerGradeCode") : "";
            entity.TermFlag = record.GetExist("TermFlag") ? record.Get<string>("TermFlag") : "";
            if (record.GetExist("TermDate"))
            {
                var jd = record.Get<DateTime?>("TermDate") ?? DateTime.MinValue;
                entity.TermDate = jd.ToString("yyyy-MM-dd HH:mm:ss");
            }
           
            entity.Email = record.GetExist("Email") ? record.Get<string>("Email") : "";
            entity.GUID = record.GetExist("GUID") ? record.Get<string>("GUID") : "";
            entity.LDAP_DistName = record.GetExist("LDAP_DistName") ? record.Get<string>("LDAP_DistName") : "";
            entity.PwCEntityID = record.GetExist("PwCEntityID") ? record.Get<string>("PwCEntityID") : "";
            entity.localExpatFlag = record.GetExist("localExpatFlag") ? record.Get<string>("localExpatFlag") : "";
            entity.ExpatFlag = record.GetExist("ExpatFlag") ? record.Get<string>("ExpatFlag") : "";
            entity.INetEmail = record.GetExist("INetEmail") ? record.Get<string>("INetEmail") : "";
            entity.NotesID = record.GetExist("NotesID") ? record.Get<string>("NotesID") : "";
            entity.BU = record.GetExist("BU") ? record.Get<string>("BU") : "";
            entity.BUDesc = record.GetExist("BUDesc") ? record.Get<string>("BUDesc") : "";
            entity.SubService = record.GetExist("SubService") ? record.Get<string>("SubService") : "";
            entity.SubServiceDesc = record.GetExist("SubServiceDesc") ? record.Get<string>("SubServiceDesc") : "";
            if (record.GetExist("JoinDate"))
            {
                var jd = record.Get<DateTime?>("JoinDate") ?? DateTime.MinValue;
                entity.JoinDate = jd.ToString("yyyy-MM-dd HH:mm:ss");
            }
            
            entity.IsProfessional = record.GetExist("IsProfessional") ? record.Get<string>("IsProfessional") : "";
            entity.JobCode = record.GetExist("JobCode") ? record.Get<string>("JobCode") : "";
            entity.NativeName = record.GetExist("NativeName") ? record.Get<string>("NativeName") : "";
            entity.FullName = record.GetExist("FullName") ? record.Get<string>("FullName") : "";
            entity.NameInEng = record.GetExist("NameInEng") ? record.Get<string>("NameInEng") : "";
            entity.GivenName = record.GetExist("NameInEng") ? record.Get<string>("NameInEng") : "";
            entity.PreferredName = record.GetExist("PreferredName") ? record.Get<string>("PreferredName") : "";
            if (record.GetExist("TerritoryEffectiveDate"))
            {
                var jd = record.Get<DateTime?>("TerritoryEffectiveDate") ?? DateTime.MinValue;
                entity.TerritoryEffectiveDate = jd.ToString("yyyy-MM-dd HH:mm:ss");
            }
          
            entity.OfficeCityName = record.GetExist("OfficeCityName") ? record.Get<string>("OfficeCityName") : "";
            entity.HRID = record.GetExist("HRID") ? record.Get<string>("HRID") : "";
            entity.CountryOffice = record.GetExist("CountryOffice") ? record.Get<string>("CountryOffice") : "";
            entity.OUCode = record.GetExist("OUCode") ? record.Get<string>("OUCode") : "";
            entity.Status = record.GetExist("Status") ? record.Get<string>("Status") : "";
            //entity.ContractYearEnd = record.GetExist("ContractYearEnd") ? record.Get<DateTime?>("ContractYearEnd")): "";
            entity.IDTerritory = record.GetExist("IDTerritory") ? record.Get<string>("IDTerritory") : "";
            entity.PowerIDStatus = record.GetExist("PowerIDStatus") ? record.Get<string>("PowerIDStatus") : "";
            entity.LoS = record.GetExist("LoS") ? record.Get<string>("LoS") : "";
            entity.LoSDesc = record.GetExist("LoSDesc") ? record.Get<string>("LoSDesc") : "";
            entity.Department = record.GetExist("Department") ? record.Get<string>("Department") : "";
            entity.SalaryCategory = record.GetExist("SalaryCategory") ? record.Get<string>("SalaryCategory") : "";
            entity.JobGrade = record.GetExist("JobGrade") ? record.Get<string>("JobGrade") : "";
            entity.EmployeeCategory = record.GetExist("EmployeeCategory") ? record.Get<string>("EmployeeCategory") : "";
            entity.Role = record.GetExist("Role") ? record.Get<string>("Role") : "";
            entity.Sex = record.GetExist("Sex") ? record.Get<string>("Sex") : "";
            entity.SchoolUniversity = record.GetExist("SchoolUniversity") ? record.Get<string>("SchoolUniversity") : "";
            entity.Majority = record.GetExist("Majority") ? record.Get<string>("Majority") : "";
            entity.MajorityCategory = record.GetExist("MajorityCategory") ? record.Get<string>("MajorityCategory") : "";

            entity.Pic = record.GetExist("Pic") ? record.GetOrDefault<byte[]>("Pic", null) : new byte[0];
            entity.PicType = record.GetExist("PicType") ? record.Get<string>("PicType") : "";
            entity.City = record.GetExist("City") ? record.Get<string>("City") : "";
            entity.ChineseName = record.GetExist("ChineseName") ? record.Get<string>("ChineseName") : "";
            if (record.GetExist("Count"))
            {
                var jd = record.GetOrDefault<int>("Count",0);
                entity.Count = jd.ToString();
            }
            if (record.GetExist("Type"))
            {
                var jd = record.GetOrDefault<int>("Type", 1);
                entity.Type = jd.ToString();
            }
         
        }

        public static List<StaffInfo> StaffBankList(FetchModel f, string type, out int totalcount)
        {
            f.O = string.IsNullOrEmpty(f.O) ? So : f.O;
            f.Om = string.IsNullOrEmpty(f.Om) ? Som : f.Om;
            f.Qp = string.IsNullOrEmpty(f.Qp) ? Ssc : f.Qp;
            f.Kn = string.IsNullOrEmpty(f.Kn) ? Sdk : f.Kn;
            var ps = f.L ?? int.Parse(Sps);
            var pi = f.I ?? 1;
            var props = Sdp.Split(new string[] {","},
                    StringSplitOptions.RemoveEmptyEntries).ToList();
            var cols = f.P ?? new List<string>();
            var newCols = cols.Intersect(props).ToList();

            var cl = "*";
            cl = string.Join(",", newCols);
            cl = string.IsNullOrEmpty(cl) ? "*" : cl;
            var where = "";

            if (!string.IsNullOrEmpty(f.K))
            {
                var stIds = new List<string>();
                f.KeyArray.ForEach(s =>
                {
                    stIds.Add("'" + s + "'");
                });
                where =f.Kn+ " in (" + string.Join(",", stIds) + ")";
            }
            else if (!string.IsNullOrEmpty(f.Q))
            {

                var searchItem = f.Qp.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                var searchText = new List<string>();
                var kText = new List<string>();
                searchItem.ForEach(s =>
                {
                    searchText.Add(s + " like '%{0}%'");
                });
                var st = "(" + string.Join(" or ", searchText) + ")";

                var words = f.Q.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 1 || Sss == "false")
                {
                    kText.Add(string.Format(st, f.Q));
                }
                else if(words.Length >1 && Sss == "true")
                {
                    kText = words.Select(word => string.Format(st, word)).ToList();
                    kText.Add(string.Format(st, f.Q));
                }

                if (kText.Count > 0)
                {
                    where = string.Join(" or ", kText);
                }
                
                if (f.Ig != null && f.Ig.Any() && f.Ig[0] != null)
                {
                    var stIds = new List<string>();
                    f.Ig.ForEach(s =>
                    {
                        stIds.Add("'" + s + "'");
                    });
                    where = where + " and "+ f.Kn + " not in (" +
                            string.Join(",", stIds) + ")";
                }

            }
            else if (f.Isd ?? false)
            {
                where = "1=1";
            }
            else
            {
                where = "1=2";
            }
            where = where + GetSearchType(type);
            var order = f.O + " " + f.Om;
            var datas = GetStaffList(Stn, cl, ps, pi, order, where, out totalcount);
            return datas;
        }

        public static List<StaffInfo> GetStaffPic(FetchModel f, string type)
        {
            var where = "";
            if (!string.IsNullOrEmpty(f.K))
            {
                var stIds = new List<string>();
                f.KeyArray.ForEach(s =>
                {
                    stIds.Add("'" + s + "'");
                });
                where = f.Kn + " in (" + string.Join(",", stIds) + ")";
            }
            where = where + GetSearchType(type);
            int totalcount = 0;
            var datas = GetStaffList(Stp, string.Join(",",f.P),f.KeyArray.Count, 1, "staffid asc", where, out totalcount);
            return datas;
        }

        public static string GetSearchType(string type)
        {
            if (type.ToLower() == "staff")
            {
                return " and type = 1";
            }
            else if (type.ToLower() == "group")
            {
                return " and type = 2";
            }
            else
            {
                return "";
            }
        }

    }
}

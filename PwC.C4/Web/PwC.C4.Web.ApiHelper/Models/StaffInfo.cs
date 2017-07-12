using System;
using System.Runtime.Serialization;

namespace PwC.C4.Web.ApiHelper.Models
{
    [DataContract]
    public class StaffInfo
    {
        [DataMember]
        public string StaffId { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string StaffInitial { get; set; }
        [DataMember]
        public string StaffName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string DivCode { get; set; }
        [DataMember]
        public string DivName { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string GradeCode { get; set; }
        [DataMember]
        public string GradeName { get; set; }
        [DataMember]
        public string JobTitle { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public string OfficeBuilding { get; set; }
        [DataMember]
        public string OfficeFloor { get; set; }
        [DataMember]
        public string SecName { get; set; }
        [DataMember]
        public string OfficePIN { get; set; }
        [DataMember]
        public string LoginID { get; set; }
        [DataMember]
        public string LoginContext { get; set; }
        [DataMember]
        public string PowerStaffCode { get; set; }
        [DataMember]
        public string PowerGroupCode { get; set; }
        [DataMember]
        public string PowerGradeCode { get; set; }
        [DataMember]
        public string TermFlag { get; set; }
        [DataMember]
        public string TermDate { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        public string LDAP_DistName { get; set; }
        [DataMember]
        public string PwCEntityID { get; set; }
        [DataMember]
        public string localExpatFlag { get; set; }
        [DataMember]
        public string ExpatFlag { get; set; }
        [DataMember]
        public string INetEmail { get; set; }
        [DataMember]
        public string NotesID { get; set; }
        [DataMember]
        public string BU { get; set; }
        [DataMember]
        public string BUDesc { get; set; }
        [DataMember]
        public string SubService { get; set; }
        [DataMember]
        public string SubServiceDesc { get; set; }
        [DataMember]
        public string JoinDate { get; set; }
        [DataMember]
        public string IsProfessional { get; set; }
        [DataMember]
        public string JobCode { get; set; }
        [DataMember]
        public string NativeName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string NameInEng { get; set; }
        [DataMember]
        public string GivenName { get; set; }
        [DataMember]
        public string PreferredName { get; set; }
        [DataMember]
        public string TerritoryEffectiveDate { get; set; }
        [DataMember]
        public string OfficeCityName { get; set; }
        [DataMember]
        public string HRID { get; set; }
        [DataMember]
        public string CountryOffice { get; set; }
        [DataMember]
        public string OUCode { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string ContractYearEnd { get; set; }
        [DataMember]
        public string IDTerritory { get; set; }
        [DataMember]
        public string PowerIDStatus { get; set; }
        [DataMember]
        public string LoS { get; set; }
        [DataMember]
        public string LoSDesc { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string SalaryCategory { get; set; }
        [DataMember]
        public string JobGrade { get; set; }
        [DataMember]
        public string EmployeeCategory { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public string Sex { get; set; }
        [DataMember]
        public string SchoolUniversity { get; set; }
        [DataMember]
        public string Majority { get; set; }
        [DataMember]
        public string MajorityCategory { get; set; }

        [DataMember]
        public byte[] Pic { get; set; }
        [DataMember]
        public string PicType { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ChineseName { get; set; }
        [DataMember]
        public string Count { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}

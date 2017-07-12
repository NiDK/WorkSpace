using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PwC.C4.Membership.Model
{
    internal class VProfileReturn<T>
    {
        public DateTime CreateTime { get; set; }
        public string StatusMsg { get; set; }
        public T Data { get; set; }
    }

    internal class GetUserReturnData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OrganizationCode { get; set; }
        public string Phone { get; set; }
        public string StaffCode { get; set; }
    }

    internal class VerifyReturnData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OrganizationCode { get; set; }
        public string Phone { get; set; }
        public string StaffCode { get; set; }
        public List<Dictionary<string,object>> Roles { get; set; }
        [JsonIgnore]
        public KeyValuePair<string, string>[] RolesKv
        {
            get
            {
                if (this.Roles == null)
                {
                    return new KeyValuePair<string, string>[0];
                }
                var role = new KeyValuePair<string, string>[this.Roles.Count];
                for (int i = 0; i < this.Roles.Count;i++)
                {   role[i] =
                        new KeyValuePair<string, string>(this.Roles[i].ContainsKey("RoleID") ? this.Roles[i]["RoleID"].ToString() : "",
                            this.Roles[i].ContainsKey("RoleName") ? this.Roles[i]["RoleName"].ToString() : "");
                }

                return role;
            }
        }

        public List<Dictionary<string, object>> Groups { get; set; }

        [JsonIgnore]
        public KeyValuePair<string, string>[] GroupsKv
        {
            get
            {
                if (this.Groups == null)
                {
                    return new KeyValuePair<string, string>[0];
                }
                var group = new KeyValuePair<string, string>[this.Groups.Count];
                for (int i = 0; i < this.Groups.Count; i++)
                {
                    group[i] =
                        new KeyValuePair<string, string>(this.Groups[i].ContainsKey("GroupID") ? this.Groups[i]["GroupID"].ToString() : "",
                            this.Groups[i].ContainsKey("GroupName") ? this.Groups[i]["GroupName"].ToString() : "");
                }

                return group;
            }
        }
    }

    internal static class GetRolesReturnData
    {


        public static List<string> ToRolesList(this List<Dictionary<string, object>> roles)
        {
            var list = new List<string>();
            if (roles == null)
                return list;
            roles.ForEach(c =>
            {
                if (c.ContainsKey("RoleName"))
                {
                    var role = c["RoleName"].ToString();
                    if (!list.Contains(role))
                        list.Add(role);
                }
            });
            return list;
        }
        public static KeyValuePair<string, string>[] ToRolesKv(this List<Dictionary<string, object>> roles)
        {
            if (roles == null)
                return new KeyValuePair<string, string>[0];
            var role = new KeyValuePair<string, string>[roles.Count];
            for (int i = 0; i < roles.Count; i++)
            {
                role[i] =
                    new KeyValuePair<string, string>(roles[i].ContainsKey("RoleID") ? roles[i]["RoleID"].ToString() : "",
                        roles[i].ContainsKey("RoleName") ? roles[i]["RoleName"].ToString() : "");
            }

            return role;
        }
    }


    internal static class GetGroupReturnData
    {
        public static List<string> ToGroupsList(this List<Dictionary<string, object>> groups)
        {
            
            var list = new List<string>();
            if (groups == null)
                return list;
            groups.ForEach(c =>
            {
                if (c.ContainsKey("GroupName"))
                {
                    var role = c["GroupName"].ToString();
                    if (!list.Contains(role))
                        list.Add(role);
                }
            });
            return list;
        }

        public static KeyValuePair<string, string>[] ToGroupsKv(this List<Dictionary<string, object>> groups)
        {
            if (groups == null)
                return new KeyValuePair<string, string>[0];
            var group = new KeyValuePair<string, string>[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                group[i] =
                    new KeyValuePair<string, string>(groups[i].ContainsKey("GroupID") ? groups[i]["GroupID"].ToString() : "",
                        groups[i].ContainsKey("GroupName") ? groups[i]["GroupName"].ToString() : "");
            }

            return group;
        }
    }


    internal class GetUserByIdModel
    {
        public string UserId { get; set; }
        public string ApiAccess { get; set; }
    }

    internal class OrgCodeStaffCodeModel
    {
        public string OrganizationCode { get; set; }
        public string StaffCode { get; set; }
        public string ApiAccess { get; set; }
    }


    internal class GetRolesByAppCodeOrgCodeStaffCode
    {
        public string ApplicationCode { get; set; }
        public string OrganizationCode { get; set; }
        public string StaffCode { get; set; }
        public string ApiAccess { get; set; }
    }


    internal class VerifyModel
    {
        public string Token { get; set; }
        public string ApiAccess { get; set; }
        public bool WithRole { get; set; }
        public bool WithGroup { get; set; }
    }
}

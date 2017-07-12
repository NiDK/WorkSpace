using System;
using System.Collections.Generic;

using PwC.C4.DataService.Model;

namespace PwC.C4.Common.Provider
{
    public static class StaffBankProvider
    {
        private static readonly C4CommonServiceClient C4Client = null;
        static StaffBankProvider()
        {
            if (C4Client == null)
            {
                C4Client = new C4CommonServiceClient();
            }
        }

        public static StaffInfo GetStaffInfoByStaffId(string staffId)
        {
            return C4Client.Staff_Get(staffId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where">start word should be - 'AND'</param>
        /// <returns></returns>
        public static List<StaffInfo> GetStaffListBy(string where)
        {
            return C4Client.Staff_GetList(where);
        }
    }
}

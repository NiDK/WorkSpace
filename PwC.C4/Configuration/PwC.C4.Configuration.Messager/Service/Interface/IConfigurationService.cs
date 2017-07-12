using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Configuration.Messager.Model;

namespace PwC.C4.Configuration.Messager.Service.Interface
{
    public interface IConfigurationService
    {

        int ConfigurationDetail_Create(ConfigurationDetail detail);

        int ConfigurationType_Create(ConfigurationType type);

        int ConfigurationDetail_GetMinor(Guid configId, string appCode, short major);

        ConfigurationDetail ConfigurationDetail_GetEntity(Guid configId, string appCode, short major, int minor, string method);

        ConfigurationDetail ConfigurationDetail_GetEntityById(Guid id, string method);

        ConfigurationDetail ConfigurationDetail_GetEntityByLastMinor(Guid configId, string appCode, short major, string method);

        int ConfigurationDetail_AddNewMinor(ConfigurationDetail detail);

        int ConfigurationDetail_AddNewMajor(ConfigurationDetail detail);

        List<ConfigurationType> ConfigurationType_ListByPaging(int index,  out int totalCount);

        List<ConfigurationDetail> ConfigurationDetail_ListByPaging(Guid configId,int index,  out int totalCount);

        List<ConfigurationDetail> ConfigurationDetail_ListByAppCode(Guid configId,string appCode, int major, int index,out int totalCount);

        PageModel<ConfigurationType> GetAllConfig(int index);

        int RecoveryConfig(Guid configId, string appCode, short major, Guid id);

        bool SaveFileToServer(ConfigurationDetail detail);

        void ConfigurationTruncateTable();

        int ConfigurationDetail_CreateApplication(Guid configTypeId, string appCode);

        void InitConfigurationByNotification();
    }
}

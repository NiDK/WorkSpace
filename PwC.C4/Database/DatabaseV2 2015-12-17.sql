USE [PwCC4Base]
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Workflow', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Workflow'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Preference', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Preference'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_UserBehavior', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_UserBehavior'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_Metadata', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_Metadata'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_Exception', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_Exception'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'EmailTemplates', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailTemplates'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'EmailParameters', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailParameters'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DataSourceType', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataSourceType'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DataSourceDetail', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataSourceDetail'

GO
/****** Object:  View [dbo].[vwStaffMaster]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster]'))
DROP VIEW [dbo].[vwStaffMaster]
GO
/****** Object:  View [dbo].[vwLegalEntity]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwLegalEntity]'))
DROP VIEW [dbo].[vwLegalEntity]
GO
/****** Object:  View [dbo].[vwDataSourceCheck]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDataSourceCheck]'))
DROP VIEW [dbo].[vwDataSourceCheck]
GO
/****** Object:  Table [dbo].[Workflow]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow]') AND type in (N'U'))
DROP TABLE [dbo].[Workflow]
GO
/****** Object:  Table [dbo].[Preference]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference]') AND type in (N'U'))
DROP TABLE [dbo].[Preference]
GO
/****** Object:  Table [dbo].[Log_UserBehavior]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_UserBehavior]') AND type in (N'U'))
DROP TABLE [dbo].[Log_UserBehavior]
GO
/****** Object:  Table [dbo].[Log_Metadata]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Metadata]') AND type in (N'U'))
DROP TABLE [dbo].[Log_Metadata]
GO
/****** Object:  Table [dbo].[Log_Exception]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Exception]') AND type in (N'U'))
DROP TABLE [dbo].[Log_Exception]
GO
/****** Object:  Table [dbo].[HtmlSnippet]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HtmlSnippet]') AND type in (N'U'))
DROP TABLE [dbo].[HtmlSnippet]
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplates]') AND type in (N'U'))
DROP TABLE [dbo].[EmailTemplates]
GO
/****** Object:  Table [dbo].[EmailParameters]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters]') AND type in (N'U'))
DROP TABLE [dbo].[EmailParameters]
GO
/****** Object:  Table [dbo].[DataSourceType]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSourceType]') AND type in (N'U'))
DROP TABLE [dbo].[DataSourceType]
GO
/****** Object:  Table [dbo].[DataSourceDetail]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSourceDetail]') AND type in (N'U'))
DROP TABLE [dbo].[DataSourceDetail]
GO
/****** Object:  StoredProcedure [dbo].[Workflow_InsertWorkflow]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_InsertWorkflow]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Workflow_InsertWorkflow]
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetLastUnactivedByDataId]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetLastUnactivedByDataId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Workflow_GetLastUnactivedByDataId]
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetLastUnactived]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetLastUnactived]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Workflow_GetLastUnactived]
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetAcitveByRecordId]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetAcitveByRecordId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Workflow_GetAcitveByRecordId]
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetAcitveByFormId]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetAcitveByFormId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Workflow_GetAcitveByFormId]
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_List]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_List]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[vwStaffMaster_List]
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_GetStaffByDataId]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_GetStaffByDataId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[vwStaffMaster_GetStaffByDataId]
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_GetByStaffId]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_GetByStaffId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[vwStaffMaster_GetByStaffId]
GO
/****** Object:  StoredProcedure [dbo].[usp_workflow_get_last_unactived_by_data_id]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_workflow_get_last_unactived_by_data_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_workflow_get_last_unactived_by_data_id]
GO
/****** Object:  StoredProcedure [dbo].[usp_GlobalTS_GetLegalEntity]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GlobalTS_GetLegalEntity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GlobalTS_GetLegalEntity]
GO
/****** Object:  StoredProcedure [dbo].[Preference_Set]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Set]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Preference_Set]
GO
/****** Object:  StoredProcedure [dbo].[Preference_Get]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Preference_Get]
GO
/****** Object:  StoredProcedure [dbo].[Preference_Delete]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Preference_Delete]
GO
/****** Object:  StoredProcedure [dbo].[Log_UserBehavior_Insert]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_UserBehavior_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Log_UserBehavior_Insert]
GO
/****** Object:  StoredProcedure [dbo].[Log_Metadata_Insert]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Metadata_Insert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Log_Metadata_Insert]
GO
/****** Object:  StoredProcedure [dbo].[insert_MAILQUEUE]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_MAILQUEUE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[insert_MAILQUEUE]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplates_List]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplates_List]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplates_List]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Update]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_Update]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_ListByPaging]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_ListByPaging]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_ListByPaging]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetById]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_GetById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_GetById]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetByCode]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_GetByCode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_GetByCode]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Delete]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_Delete]
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Create]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailTemplate_Create]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Update]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_Update]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_ListByPaging]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_ListByPaging]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_ListByPaging]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_List]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_List]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_List]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetById]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_GetById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_GetById]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetByCode]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_GetByCode]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_GetByCode]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Delete]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_Delete]
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Create]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Create]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[EmailParameters_Create]
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDetails]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSource_GetDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DataSource_GetDetails]
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDataSource]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSource_GetDataSource]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DataSource_GetDataSource]
GO
/****** Object:  UserDefinedTableType [dbo].[StrIdTable]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'StrIdTable' AND ss.name = N'dbo')
DROP TYPE [dbo].[StrIdTable]
GO
/****** Object:  UserDefinedTableType [dbo].[GuidIdTable]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'GuidIdTable' AND ss.name = N'dbo')
DROP TYPE [dbo].[GuidIdTable]
GO
/****** Object:  User [UAT\ECNPExpAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\ECNPExpAppID')
DROP USER [UAT\ECNPExpAppID]
GO
/****** Object:  User [UAT\PwCCMFAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCCMFAppID')
DROP USER [UAT\PwCCMFAppID]
GO
/****** Object:  User [UAT\PwCFRSAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCFRSAppID')
DROP USER [UAT\PwCFRSAppID]
GO
/****** Object:  User [UAT\PwCMOFAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCMOFAppID')
DROP USER [UAT\PwCMOFAppID]
GO
USE [master]
GO
/****** Object:  Database [PwCC4Base]    Script Date: 17/12/2015 17:21:40 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'PwCC4Base')
DROP DATABASE [PwCC4Base]
GO
/****** Object:  Database [PwCC4Base]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PwCC4Base')
BEGIN
CREATE DATABASE [PwCC4Base]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PwCC4Base', FILENAME = N'D:\SQL2012\MSSQL11.MSSQLSERVER\MSSQL\DATA\PwCC4Base.mdf' , SIZE = 21504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PwCC4Base_log', FILENAME = N'D:\SQL2012\MSSQL11.MSSQLSERVER\MSSQL\DATA\PwCC4Base_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [PwCC4Base] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PwCC4Base].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PwCC4Base] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PwCC4Base] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PwCC4Base] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PwCC4Base] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PwCC4Base] SET ARITHABORT OFF 
GO
ALTER DATABASE [PwCC4Base] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PwCC4Base] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [PwCC4Base] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PwCC4Base] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PwCC4Base] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PwCC4Base] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PwCC4Base] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PwCC4Base] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PwCC4Base] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PwCC4Base] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PwCC4Base] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PwCC4Base] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PwCC4Base] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PwCC4Base] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PwCC4Base] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PwCC4Base] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PwCC4Base] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PwCC4Base] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PwCC4Base] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PwCC4Base] SET  MULTI_USER 
GO
ALTER DATABASE [PwCC4Base] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PwCC4Base] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PwCC4Base] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PwCC4Base] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PwCC4Base', N'ON'
GO
USE [PwCC4Base]
GO
/****** Object:  User [UAT\PwCMOFAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCMOFAppID')
CREATE USER [UAT\PwCMOFAppID] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [UAT\PwCFRSAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCFRSAppID')
CREATE USER [UAT\PwCFRSAppID] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [UAT\PwCCMFAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\PwCCMFAppID')
CREATE USER [UAT\PwCCMFAppID] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [UAT\ECNPExpAppID]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'UAT\ECNPExpAppID')
CREATE USER [UAT\ECNPExpAppID] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [UAT\PwCMOFAppID]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [UAT\PwCFRSAppID]
GO
ALTER ROLE [db_datareader] ADD MEMBER [UAT\PwCCMFAppID]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [UAT\PwCCMFAppID]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [UAT\ECNPExpAppID]
GO
/****** Object:  UserDefinedTableType [dbo].[GuidIdTable]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'GuidIdTable' AND ss.name = N'dbo')
CREATE TYPE [dbo].[GuidIdTable] AS TABLE(
	[Id] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[StrIdTable]    Script Date: 17/12/2015 17:21:40 ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'StrIdTable' AND ss.name = N'dbo')
CREATE TYPE [dbo].[StrIdTable] AS TABLE(
	[Id] [varchar](100) NULL
)
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDataSource]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSource_GetDataSource]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-4-22
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DataSource_GetDataSource]
	@AppCode varchar(50)
AS
BEGIN
	select [Name],[Key],[Value],d.[Order] from [dbo].[DataSourceType] t inner join [dbo].[DataSourceDetail] d on t.[Id]=d.[DataSourceTypeId]
	where AppCode=@AppCode and t.State=0 and d.State=0
	order by d.[Order]
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDetails]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSource_GetDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-04-17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DataSource_GetDetails]
	@appCode varchar(50),
	@dataSourceType varchar(50),
	@group varchar(50)
AS
BEGIN

if @group = ''''
	begin
	set @group=null
	end

	select [Key],Value from [dbo].[DataSourceType] t inner join [dbo].[DataSourceDetail] d
	on t.Id = d.DataSourceTypeId
	where t.[AppCode]=@appCode and [Name]=@dataSourceType and [Group]=isnull(@group,[Group]) and  t.State=0 and d.State=0
	order by d.[Order] asc
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Create]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_Create]
	@AppCode varchar(50),
	@Group varchar(50),
	@ParameterCode varchar(50),
	@ParameterName nvarchar(50),
	@Assembly nvarchar(200),
	@ParameterType int,
	@Content nvarchar(max),
	@CreateBy varchar(100),
	@ModifyBy varchar(100),
	@Id int output
AS
BEGIN
	declare @existId int

	if @Group =''''
	begin
		set @Group=null
	end

	select top 1 @existId=[ParameterID] from [dbo].[EmailParameters] 
	where appcode=@AppCode and [Group]=ISNULL(@Group,[Group]) and [ParameterID]=@ParameterCode

	if @existId=1
	begin 
		set @Id = -1
	end
	else
	begin

		   INSERT INTO [dbo].[EmailParameters]
           ([AppCode]
           ,[Group]
           ,[ParameterCode]
           ,[ParameterName]
           ,[Assembly]
           ,[ParameterType]
           ,[Content]
           ,[IsDeleted]
           ,[CreateDate]
           ,[ModifyDate]
           ,[CreateBy]
           ,[ModifyBy])
     VALUES
           (@AppCode
           ,@Group
           ,@ParameterCode
           ,@ParameterName
           ,@Assembly
           ,@ParameterType
           ,@Content
           ,0
           ,GETDATE()
           ,GETDATE()
           ,@CreateBy
           ,@ModifyBy)

	set @Id=@@IDENTITY

	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Delete]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_Delete]
	@AppCode varchar(50),
	@ParameterID int,
	@ModifyBy varchar(100)
AS
BEGIN
	update [dbo].EmailParameters
	set IsDeleted=1,ModifyBy=@ModifyBy,ModifyDate=getdate()
	where AppCode=@AppCode and ParameterID=@ParameterID
END







' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetByCode]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_GetByCode]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_GetByCode] 
	@AppCode varchar(50),
	@Group varchar(50),
	@ParameterCode [StrIdTable] readonly
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	select * from [EmailParameters]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) and ParameterCode in (select id from @ParameterCode)

END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetById]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_GetById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_GetById] 
	@AppCode varchar(50),
	@ParameterId int
AS
BEGIN

	select top 1 * from [EmailParameters]
	where AppCode=@AppCode and ParameterID=@ParameterId

END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_List]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_List]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_List]
	@AppCode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	select * from [dbo].[EmailParameters]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) 
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_ListByPaging]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_ListByPaging]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_ListByPaging]
	@AppCode varchar(50),
	@Group varchar(50),
	@ParameterName nvarchar(100),
	@PageIndex int,
	@PageSize int,
	@TotalCount int output
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	if @PageSize =-1
	begin
		select  * from [dbo].EmailParameters 
		where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [ParameterName] like ''%''+@ParameterName+''%''
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by CreateDate desc) as ''rowNumber'', * from [dbo].EmailParameters 
	where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [ParameterName] like ''%''+@ParameterName+''%''
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Update]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_Update]
	@ParameterID int,
	@AppCode varchar(50),
	@Group varchar(50),
	@ParameterCode varchar(50),
	@ParameterName nvarchar(50),
	@Assembly nvarchar(200),
	@ParameterType int,
	@Content nvarchar(max),
	@ModifyBy varchar(100)
AS
BEGIN
	declare @existId int

	if @Group =''''
	begin
		set @Group=null
	end

	if @ParameterID =''''
	begin
		set @ParameterID=null
	end

	select top 1 @existId=ParameterID from [dbo].[EmailParameters] 
	where appcode=@AppCode and [Group]=ISNULL(@Group,[Group]) and ParameterCode=@ParameterCode

	if (@existId=@ParameterID) OR ( @existId is null and @ParameterID is not null)
	begin 
		UPDATE [dbo].[EmailParameters]
    SET
      [Group] = @Group
      ,[ParameterCode] = @ParameterCode
      ,[ParameterName] = @ParameterName
      ,[Assembly] = @Assembly
      ,[ParameterType] = @ParameterType
      ,[Content] = @Content
      ,[ModifyDate] = GETDATE()
      ,[ModifyBy] = @ModifyBy
    WHERE ParameterID=@ParameterID

		select @@ROWCOUNT
	end
	else
	begin
	select -1
	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Create]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Create]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_Create]
		   @AppCode varchar(50),
           @Group varchar(50),
           @TemplateCode varchar(50),
           @TemplateName nvarchar(100),
           @MailFrom varchar(60),
           @MailReplyTo varchar(400),
           @MailCc varchar(400),
           @MailBcc varchar(400),
           @MailOrganisation varchar(60),
           @MailSubject nvarchar(1000),
           @MailContent ntext,
		   @MailSubmitBy varchar(60),
		   @IsImmediate bit,
           @CreateBy varchar(100),
           @ModifyBy varchar(100),
		   @Id int output
AS
BEGIN
	declare @existId int

	if @Group =''''
	begin
		set @Group=null
	end

	select top 1 @existId=[TemplateID] from [dbo].[EmailTemplates] 
	where appcode=@AppCode and [Group]=ISNULL(@Group,[Group]) and TemplateCode=@TemplateCode

	if @existId=1
	begin 
		set @Id = -1
	end
	else
	begin
INSERT INTO [dbo].[EmailTemplates]
           ([AppCode]
           ,[Group]
           ,[TemplateCode]
           ,[TemplateName]
           ,[MailFrom]
           ,[MailReplyTo]
           ,[MailCc]
           ,[MailBcc]
           ,[MailOrganisation]
           ,[MailSubject]
           ,[MailContent]
           ,[MailSubmitBy]
		   ,IsImmediate
           ,[CreateDate]
           ,[ModifyDate]
           ,[CreateBy]
           ,[ModifyBy]
           ,[IsDeleted])
     VALUES
           (@AppCode,
			@Group,
			@TemplateCode,
			@TemplateName,
			@MailFrom,
			@MailReplyTo,
			@MailCc,
			@MailBcc,
			@MailOrganisation,
			@MailSubject,
			@MailContent,
			@MailSubmitBy,
			@IsImmediate,
			getdate(),
			getdate(),
			@CreateBy,
			@ModifyBy,
			0)

		   set @Id=@@IDENTITY
	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Delete]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_Delete]
	@AppCode varchar(50),
	@TemplateId int,
	@ModifyBy varchar(100)
AS
BEGIN
	update [dbo].[EmailTemplates]
	set IsDeleted=1,ModifyBy=@ModifyBy,ModifyDate=getdate()
	where AppCode=@AppCode and TemplateId=@TemplateId
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetByCode]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_GetByCode]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_GetByCode] 
	@AppCode varchar(50),
	@Group varchar(50),
	@TemplateCode [StrIdTable] readonly
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	select * from EmailTemplates
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateCode]in (select id from @TemplateCode)

END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetById]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_GetById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_GetById] 
	@AppCode varchar(50),
	@TemplateId int
AS
BEGIN

	select top 1 * from EmailTemplates
	where AppCode=@AppCode and TemplateId=@TemplateId

END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_ListByPaging]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_ListByPaging]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_ListByPaging]
	@AppCode varchar(50),
	@Group varchar(50),
	@TemplateName nvarchar(100),
	@PageIndex int,
	@PageSize int,
	@TotalCount int output
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	if @PageSize =-1
	begin
		select  * from [dbo].[EmailTemplates] 
		where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateName] like ''%''+@TemplateName+''%''
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by CreateDate desc) as ''rowNumber'', * from [dbo].[EmailTemplates] 
	where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateName] like ''%''+@TemplateName+''%''
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Update]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplate_Update]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_Update]
		   @TemplateID int,
		   @AppCode			 varchar(50),
           @TemplateCode 	 varchar(50),
           @TemplateName 	 nvarchar(100),
           @MailFrom 		 varchar(60),
           @MailReplyTo 	 varchar(400),
           @MailCc 			 varchar(400),
           @MailBcc 		 varchar(400),
           @MailOrganisation varchar(60),
           @MailSubject 	 nvarchar(1000),
           @MailContent 	 ntext,
           @MailSubmitBy 	 varchar(60),
		   @IsImmediate		 bit,
           @ModifyBy 		 varchar(100)
AS
BEGIN
	declare @existId int

	if @TemplateID =''''
	begin
		set @TemplateID=null
	end

	select top 1 @existId=[TemplateID] from [dbo].[EmailTemplates] 
	where appcode=@AppCode and TemplateCode=@TemplateCode

	if (@existId=@TemplateID) OR ( @existId is null and @TemplateID is not null)
	begin 
		update [EmailTemplates] set 
		TemplateCode = @TemplateCode,
		TemplateName = @TemplateName,
		MailFrom = @MailFrom,
		MailReplyTo = @MailReplyTo,
		MailCc = @MailCc,
		MailBcc = @MailBcc,
		MailOrganisation = @MailOrganisation,
		MailSubject = @MailSubject,
		MailContent = @MailContent,
		MailSubmitBy = @MailSubmitBy,
		IsImmediate=@IsImmediate,
		ModifyBy = @ModifyBy,
		ModifyDate = getdate()

		where TemplateID=@TemplateID

		select @@ROWCOUNT
	end
	else
	begin
	select 0
	end
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[EmailTemplates_List]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplates_List]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplates_List]
	@AppCode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''''
	begin
		set @Group=null
	end

	select * from [dbo].[EmailTemplates]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) 
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[insert_MAILQUEUE]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_MAILQUEUE]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[insert_MAILQUEUE]
	(@mAPPCODE	 	[varchar](20),
	 @mCC	 		[varchar](400),
	 @mREPLYTO 		[varchar](400),
	 @mBCC	 	[varchar](1000),
	 @mORGANISATION 	[varchar](60),
	 @mSUBJECT	 	[nvarchar](1000),
	 @mSUBMITBY		[varchar](60),
	 @mSUBMITDATE 	[datetime],
	 @mPOSTEDFLAG 	[char](1),
	 @mSENDDATE 	[datetime],
	 @mIMMEDIATEFLAG 	[varchar](1),
	 @mMTO 		[varchar](400),
	 @mMFROM 		[varchar](60),
	@mContent 		ntext,
	@encode			varchar(12) = ''utf-8''
)

AS
begin
	
	if @mSUBMITBY is null or len(isnull(@mSUBMITBY,''''))=0
	    begin
		set @mSUBMITBY =''pwchk.com Web Master''
	    end

	if @mMFROM is null or len(isnull(@mMFROM,''''))=0
	    begin
		set @mMFROM = ''pwcwebmaster@hk.pwcglobal.com''
            end

	if @mCC is null or len(isnull(@mCC,''''))=0
            begin
		set @mCC = null
	    end
           
        if @mREPLYTo is null or len(isnull(@mREPLYTO,''''))=0
            begin
		set @mREPLYTO = @mMFROM
	    end

	if @mBCC is null or len(isnull(@mBCC,''''))=0
	    begin
		set @mBCC = null
	    end

	if @mORGANISATION is null or len(isnull(@mORGANISATION,''''))=0
	    begin
		set @mORGANISATION = ''PricewaterhouseCoopers''
	    end
	 


        if @mSUBMITDATE is null or convert(varchar,isnull(@mSUBMITDATE,''''),103)=''01/01/1900''
	    begin
	        set @mSUBMITDATE = convert(datetime, getdate())
            end    
	else
            begin
		set @mSUBMITDATE = convert(datetime, @mSUBMITDATE)
	    end
           
        if @mPOSTEDFLAG is null or len(isnull(@mPOSTEDFLAG,''''))=0 
	    begin
		set @mPOSTEDFLAG = ''N''
	    end      
        	

        if @mSENDDATE is null or convert(varchar,isnull(@mSENDDATE,''''),103)=''01/01/1900'' 
	    begin
		set @mSENDDATE = convert(datetime,getdate())
            end
        else
 	    begin
	        set @mSENDDATE = convert(datetime, @mSENDDATE)
            end

	if @mIMMEDIATEFLAG is null or len(isnull(@mIMMEDIATEFLAG,''''))=0
	    begin
		set @mIMMEDIATEFLAG = ''N''
	    end




--BEGIN TRAN

INSERT INTO [MailMaster].[dbo].[MAILQUEUE] 
	 ( [APPCODE],
	 [CC],
	 [REPLYTO],
	 [BCC],
	 [ORGANISATION],
	 [SUBJECT],
	 [SUBMITBY],
	 [SUBMITDATE],
	 [POSTEDFLAG],
	 [SENDDATE],
	 [IMMEDIATEFLAG],
	 [MTO],
	 [MFROM],
	[CONTENT],
	ENCODE) 
 
VALUES 
	( @mAPPCODE,
	 @mCC,
	 @mREPLYTO,
	 @mBCC,
	 @mORGANISATION,
	 @mSUBJECT,
	 @mSUBMITBY,
	 @mSUBMITDATE,
	 @mPOSTEDFLAG,
	 @mSENDDATE,
	 @mIMMEDIATEFLAG,
	 @mMTO,
	 @mMFROM,
	@mContent,
	@encode)


--COMMIT

end





' 
END
GO
/****** Object:  StoredProcedure [dbo].[Log_Metadata_Insert]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Metadata_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Log_Metadata_Insert]
	@AppCode varchar(50),
	@Metadata varchar(50),
	@DataId uniqueidentifier,
	@Method int,
	@JsonData ntext,
	@UserId varchar(50),
	@IpAddress varchar(50),
	@MachineName varchar(50)
AS
BEGIN
	INSERT INTO [dbo].Log_Metadata
           ([AppCode]
           ,[Metadata]
           ,[DataId]
           ,[Method]
           ,[JsonData]
           ,[UserId]
           ,[ActionDate]
           ,[IpAddress]
           ,[MachineName])
     VALUES
           (@AppCode
           ,@Metadata
           ,@DataId
           ,@Method
           ,@JsonData
           ,@UserId
           ,GETDATE()
           ,@IpAddress
           ,@MachineName)
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[Log_UserBehavior_Insert]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_UserBehavior_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Log_UserBehavior_Insert]
	@AppCode  varchar(50),
	@OptionType varchar(50),
	@OptionId varchar(350),
	@Method varchar(50),
	@Description ntext,
	@UserId varchar(50),
	@IpAddress varchar(50),
	@MachineName varchar(50)
AS
BEGIN
	INSERT INTO [dbo].[Log_UserBehavior]
           ([AppCode]
           ,[OptionType]
           ,[OptionId]
           ,[Method]
           ,[Description]
           ,[UserId]
           ,[ActionDate]
           ,[IpAddress]
           ,[MachineName])
     VALUES
           (@AppCode
           ,@OptionType
           ,@OptionId
           ,@Method
           ,@Description
           ,@UserId
           ,GETDATE()
           ,@IpAddress
           ,@MachineName)
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[Preference_Delete]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Delete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Preference_Delete]
    @appcode varchar(256),
	@key varchar(256)
AS
BEGIN
	delete from Preference where appcode=@appcode and [key]=@key
END


' 
END
GO
/****** Object:  StoredProcedure [dbo].[Preference_Get]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Preference_Get]
    @appcode varchar(256),
	@key varchar(256)
AS
BEGIN
	select Value from Preference where appcode=@appcode and [key]=@key
END









' 
END
GO
/****** Object:  StoredProcedure [dbo].[Preference_Set]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference_Set]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Preference_Set]
@appcode varchar(256),
	@key varchar(256),
	@value	nvarchar(Max)
AS
BEGIN
	declare @isExist int
	select @isExist=count(0) from Preference where appcode=@appcode and [key]=@key
	if @isExist = 0
	begin
		insert into Preference([AppCode],[key],value) values(@appcode,@key,@value)
	end
	else
	begin
		update Preference set value=@value where  appcode=@appcode and [key]=@key
	end
	select @@rowcount 
END









' 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GlobalTS_GetLegalEntity]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GlobalTS_GetLegalEntity]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GlobalTS_GetLegalEntity]
AS
BEGIN
	declare @tempTable table
	(
		PwCEntityID varchar(10),
		EntityDescription varchar(100)
	)
	insert into @tempTable select * from vwLegalEntity

	insert into @tempTable(PwCEntityID,EntityDescription) values (''Others'',''Others'')

	select * from @tempTable
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_workflow_get_last_unactived_by_data_id]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_workflow_get_last_unactived_by_data_id]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[usp_workflow_get_last_unactived_by_data_id]
	@appcode varchar(150),
	@entity_name varchar(150),
	@record_id uniqueidentifier
AS
BEGIN
	select top 1 * from [dbo].[Workflow]
	where AppCode=@appcode and EntityName=@entity_name and RecordId=@record_id and [IsActive]=0
	order by CreateTime desc
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_GetByStaffId]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_GetByStaffId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[vwStaffMaster_GetByStaffId] 
	@staffId varchar(10)
AS
BEGIN
	select top 1 * from [dbo].[vwStaffMaster]
	where StaffID=@staffId
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_GetStaffByDataId]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_GetStaffByDataId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[vwStaffMaster_GetStaffByDataId]
	@dataId uniqueidentifier
AS

BEGIN
declare @staffId  varchar(50)
select @staffId = [CreateBy] from [dbo].[App_Default]
	where RecordId=@dataId
	select [StaffName] from [dbo].[vwStaffMaster]
	where [StaffID]=@staffId
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_List]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster_List]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[vwStaffMaster_List]
	@where nvarchar(max)
AS
BEGIN
	declare @sql nvarchar(max)
	set @sql = ''select * from [dbo].[vwStaffMaster]	where 1=1 ''+@where
	exec(@sql)
END






' 
END
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetAcitveByFormId]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetAcitveByFormId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Workflow_GetAcitveByFormId]
	@appCode varchar(50),
	@entityName varchar(50),
	@formId int
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and EntityName=@entityName and FormId=@formId and isActive=1
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetAcitveByRecordId]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetAcitveByRecordId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Workflow_GetAcitveByRecordId]
	@appCode varchar(50),
	@entityName varchar(50),
	@recordId uniqueidentifier
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and EntityName=@entityName and RecordId=@recordId and isActive=1
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetLastUnactived]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetLastUnactived]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Workflow_GetLastUnactived]
	@AppCode varchar(150),
	@workflowCode varchar(150),
	@InstanceId int
AS
BEGIN
	select top 1 * from [dbo].[Workflow]
	where AppCode=@appcode and [WorkflowCode]=@workflowCode and [InstanceId]=@InstanceId and [IsActive]=0
	order by CreateTime desc
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[Workflow_GetLastUnactivedByDataId]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_GetLastUnactivedByDataId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[Workflow_GetLastUnactivedByDataId]
	@AppCode varchar(150),
	@entityName varchar(150),
	@recordId uniqueidentifier
AS
BEGIN
	select top 1 * from [dbo].[Workflow]
	where AppCode=@appcode and EntityName=@entityName and RecordId=@recordId and [IsActive]=0
	order by CreateTime desc
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[Workflow_InsertWorkflow]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow_InsertWorkflow]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Workflow_InsertWorkflow]
	@appCode varchar(150),
	@entityName varchar(150),
	@workflowCode varchar(150),
	@actionCode varchar(150),
	@formId int,
	@recordId uniqueidentifier,
	@instanceId int,
	@userId varchar(50),
	@userRole varchar(50),
	@status varchar(50),
	@comment nvarchar(500),
    @Id int output
AS
BEGIN
	
	update [dbo].[Workflow] set isActive=0
	where formid=@formId and RecordId=@recordId

	INSERT INTO [dbo].[Workflow]
           ([AppCode]
           ,[EntityName]
		   ,WorkflowCode
		   ,ActionCode
           ,[FormId]
           ,[RecordId]
           ,[InstanceId]
           ,[UserId]
           ,[UserRole]
           ,[Status]
           ,[CreateTime]
		   ,Comment,
		   IsActive)
     VALUES
           (@appCode
           ,@entityName
		   ,@workflowCode
		   ,@actionCode
           ,@formId
           ,@recordId
           ,@instanceId
           ,@userId
           ,@userRole
           ,@status
           ,getdate()
		   ,@comment,
		   1)
	set @Id=@@IDENTITY
END

' 
END
GO
/****** Object:  Table [dbo].[DataSourceDetail]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSourceDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataSourceDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[DataSourceTypeId] [uniqueidentifier] NULL,
	[Group] [varchar](50) NULL,
	[Key] [nvarchar](50) NULL,
	[Value] [nvarchar](500) NULL,
	[Order] [int] NULL,
	[State] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_DataSourceDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataSourceType]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSourceType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataSourceType](
	[Id] [uniqueidentifier] NOT NULL,
	[AppCode] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Desc] [nvarchar](500) NULL,
	[Order] [int] NULL,
	[State] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_DataSourceType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailParameters]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailParameters]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EmailParameters](
	[ParameterID] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
	[ParameterCode] [varchar](50) NOT NULL,
	[ParameterName] [nvarchar](50) NOT NULL,
	[Assembly] [nvarchar](200) NULL,
	[ParameterType] [int] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateBy] [varchar](100) NULL,
	[ModifyBy] [varchar](100) NULL,
 CONSTRAINT [PK_EmailParameter_1] PRIMARY KEY CLUSTERED 
(
	[ParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailTemplates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EmailTemplates](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
	[TemplateCode] [varchar](50) NOT NULL,
	[TemplateName] [nvarchar](100) NULL,
	[MailFrom] [varchar](60) NULL,
	[MailReplyTo] [varchar](400) NULL,
	[MailCc] [varchar](400) NULL,
	[MailBcc] [varchar](400) NULL,
	[MailOrganisation] [varchar](60) NULL,
	[MailSubject] [nvarchar](1000) NULL,
	[MailContent] [ntext] NULL,
	[MailSubmitBy] [varchar](60) NULL,
	[IsImmediate] [bit] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateBy] [varchar](100) NULL,
	[ModifyBy] [varchar](100) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[TemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HtmlSnippet]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HtmlSnippet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HtmlSnippet](
	[Id] [uniqueidentifier] NOT NULL,
	[AppCode] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Group] [varchar](50) NULL,
	[Html] [nvarchar](max) NULL,
	[Description] [varchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyBy] [varchar](50) NULL,
	[ModifyTime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_Exception]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log_Exception](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [nvarchar](50) NULL,
	[Type] [varchar](255) NULL,
	[Date] [datetime] NULL,
	[StaffId] [nvarchar](50) NULL,
	[Thread] [nvarchar](50) NULL,
	[Level] [nvarchar](255) NULL,
	[Logger] [nvarchar](255) NULL,
	[Message] [nvarchar](max) NULL,
	[Exception] [nvarchar](max) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Log_Exception] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_Metadata]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_Metadata]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log_Metadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NULL,
	[Metadata] [varchar](50) NULL,
	[DataId] [uniqueidentifier] NULL,
	[Method] [int] NULL,
	[JsonData] [ntext] NULL,
	[UserId] [varchar](50) NULL,
	[ActionDate] [datetime] NULL,
	[IpAddress] [varchar](50) NULL,
	[MachineName] [varchar](50) NULL,
 CONSTRAINT [PK_Log_Metadata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_UserBehavior]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log_UserBehavior]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log_UserBehavior](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NULL,
	[OptionType] [varchar](50) NULL,
	[OptionId] [varchar](350) NULL,
	[Method] [varchar](50) NULL,
	[Description] [ntext] NULL,
	[UserId] [varchar](50) NULL,
	[ActionDate] [datetime] NULL,
	[IpAddress] [varchar](50) NULL,
	[MachineName] [varchar](50) NULL,
 CONSTRAINT [PK_Log_UserBehavior] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Preference]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preference]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Preference](
	[AppCode] [varchar](256) NOT NULL,
	[Key] [varchar](256) NOT NULL,
	[Value] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Workflow]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Workflow]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Workflow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](150) NULL,
	[EntityName] [varchar](150) NULL,
	[WorkflowCode] [varchar](150) NULL,
	[ActionCode] [varchar](150) NULL,
	[FormId] [int] NULL,
	[RecordId] [uniqueidentifier] NULL,
	[InstanceId] [int] NULL,
	[UserId] [varchar](50) NULL,
	[UserRole] [varchar](50) NULL,
	[Status] [varchar](150) NULL,
	[Comment] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vwDataSourceCheck]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDataSourceCheck]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwDataSourceCheck]
AS
SELECT        TOP (100) PERCENT dbo.DataSourceType.Name, dbo.DataSourceType.AppCode, dbo.DataSourceDetail.[Group], dbo.DataSourceDetail.[Key], 
                         dbo.DataSourceDetail.Value, dbo.DataSourceDetail.[Order]
FROM            dbo.DataSourceDetail INNER JOIN
                         dbo.DataSourceType ON dbo.DataSourceDetail.DataSourceTypeId = dbo.DataSourceType.Id






' 
GO
/****** Object:  View [dbo].[vwLegalEntity]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwLegalEntity]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vwLegalEntity]
AS
SELECT A.PwCEntityID,A.EntityDescription 
FROM   StaffBank.dbo.PwCEntityCode AS A where len(EntityDescription)>0 and (PwCEntityID like ''CN%'' or PwCEntityID like ''HK%'')
                        






' 
GO
/****** Object:  View [dbo].[vwStaffMaster]    Script Date: 17/12/2015 17:21:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwStaffMaster]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vwStaffMaster]
AS
SELECT        A.StaffID, A.CountryCode, A.StaffInitial, A.StaffName, A.FirstName, A.LastName, A.DivCode, A.DivName, A.GroupCode, A.GroupName, A.GradeCode, A.GradeName, 
                         A.JobTitle, A.PhoneNo, A.OfficeBuilding, A.OfficeFloor, A.SecName, A.OfficePIN, A.LoginID, A.LoginContext, A.PowerStaffCode, A.PowerGroupCode, A.PowerGradeCode, 
                         A.TermFlag, A.TermDate, A.Email, A.GUID, A.LDAP_DistName, A.PwCEntityID, A.localExpatFlag, A.ExpatFlag, A.INetEmail, A.NotesID, A.BU, A.BUDesc, A.SubService, 
                         A.SubServiceDesc, A.JoinDate, A.IsProfessional, A.JobCode, A.NativeName, A.FullName, A.NameInEng, A.GivenName, A.PreferredName, A.TerritoryEffectiveDate,
                             (SELECT        OfficeCityName
                               FROM            StaffBank.dbo.WorkOfficeCity
                               WHERE        (OfficeCityCode = B.OfficeCity)) AS OfficeCityName, B.HRID, B.CountryOffice, B.OUCode, B.Status, B.ContractYearEnd, B.IDTerritory, B.PowerIDStatus, 
                         B.LoS, B.Department, B.SalaryCategory, B.JobGrade, B.EmployeeCategory, B.Role, B.Sex, B.SchoolUniversity, B.Majority, B.MajorityCategory
FROM            StaffBank.dbo.vwStaffMaster AS A INNER JOIN
                         StaffBank.dbo.vwStaffBankExtension AS B ON A.StaffID = B.StaffID





' 
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DataSourceDetail', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据源详情,datasource type id,以及具体key,value数据项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataSourceDetail'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DataSourceType', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典数据源的大类,具体数据项在DatasourceDetail表中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataSourceType'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'EmailParameters', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email 参数表,可配置对应的翻译接口等' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailParameters'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'EmailTemplates', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮件模板表,用于存储邮件模板以及邮件参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailTemplates'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_Exception', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统异常或者Debug信息存储表,已迁移到PwCC4Base库中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_Exception'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_Metadata', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Metadata数据实时镜像表,所有关于Metadata的变更都会再次保存一份副本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_Metadata'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Log_UserBehavior', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户行为日志表,已迁移到PwCC4Base库中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log_UserBehavior'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Preference', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户行为及配置表,用于存储用户自定义设置的列表项等' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Preference'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Workflow', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作流信息设置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Workflow'
GO
USE [master]
GO
ALTER DATABASE [PwCC4Base] SET  READ_WRITE 
GO

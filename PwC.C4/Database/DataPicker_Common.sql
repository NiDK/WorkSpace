USE [PwCC4Base]
GO
/****** Object:  Table [dbo].[DataPicker_Staff]    Script Date: 2016/6/13 02:12:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataPicker_Staff](
	[StaffID] [nvarchar](100) NULL,
	[CountryCode] [varchar](10) NULL,
	[StaffInitial] [char](6) NULL,
	[StaffName] [nvarchar](100) NULL,
	[FirstName] [varchar](60) NULL,
	[LastName] [char](25) NULL,
	[DivCode] [varchar](20) NULL,
	[DivName] [varchar](40) NULL,
	[GroupCode] [varchar](20) NULL,
	[GroupName] [varchar](40) NULL,
	[GradeCode] [varchar](20) NULL,
	[GradeName] [varchar](40) NULL,
	[JobTitle] [char](50) NULL,
	[PhoneNo] [char](20) NULL,
	[OfficeBuilding] [varchar](50) NULL,
	[OfficeFloor] [char](3) NULL,
	[SecName] [char](25) NULL,
	[OfficePIN] [char](15) NULL,
	[LoginID] [varchar](50) NULL,
	[LoginContext] [varchar](50) NULL,
	[PowerStaffCode] [char](10) NULL,
	[PowerGroupCode] [char](10) NULL,
	[PowerGradeCode] [char](10) NULL,
	[TermFlag] [varchar](1) NULL,
	[TermDate] [datetime] NULL,
	[Email] [nvarchar](150) NULL,
	[GUID] [varchar](40) NULL,
	[LDAP_DistName] [varchar](400) NULL,
	[PwCEntityID] [varchar](10) NULL,
	[localExpatFlag] [char](1) NULL,
	[ExpatFlag] [char](1) NULL,
	[INetEmail] [varchar](390) NULL,
	[NotesID] [varchar](100) NULL,
	[BU] [nvarchar](60) NULL,
	[BUDesc] [nvarchar](120) NULL,
	[SubService] [nvarchar](60) NULL,
	[SubServiceDesc] [nvarchar](120) NULL,
	[JoinDate] [datetime] NULL,
	[IsProfessional] [char](1) NULL,
	[JobCode] [nvarchar](60) NULL,
	[NativeName] [nvarchar](40) NULL,
	[FullName] [nvarchar](2000) NULL,
	[NameInEng] [nvarchar](2000) NULL,
	[GivenName] [nvarchar](4000) NULL,
	[PreferredName] [nvarchar](2000) NULL,
	[TerritoryEffectiveDate] [datetime] NULL,
	[OfficeCityName] [nvarchar](500) NULL,
	[HRID] [nchar](12) NULL,
	[CountryOffice] [nvarchar](30) NULL,
	[OUCode] [nvarchar](60) NULL,
	[Status] [nvarchar](10) NULL,
	[ContractYearEnd] [datetime] NULL,
	[IDTerritory] [varchar](10) NULL,
	[PowerIDStatus] [nvarchar](40) NULL,
	[LoS] [nvarchar](60) NULL,
	[Department] [nvarchar](60) NULL,
	[SalaryCategory] [nvarchar](60) NULL,
	[JobGrade] [nvarchar](60) NULL,
	[EmployeeCategory] [nvarchar](60) NULL,
	[Role] [nchar](40) NULL,
	[Sex] [nchar](50) NULL,
	[SchoolUniversity] [nvarchar](120) NULL,
	[Majority] [nvarchar](120) NULL,
	[MajorityCategory] [nchar](20) NULL,
	[City] [nvarchar](100) NULL,
	[ChineseName] [nvarchar](60) NULL,
	[LoSDesc] [nvarchar](120) NULL,
	[Count] [int] NULL,
	[Type] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataPicker_StaffPic]    Script Date: 2016/6/13 02:12:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataPicker_StaffPic](
	[StaffID] [char](10) NOT NULL,
	[Pic] [image] NULL,
	[PicType] [varchar](5) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Common_Paging]    Script Date: 2016/6/13 02:12:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Common_Paging]
	@recordTotal INT OUTPUT,
	@viewName VARCHAR(800),
    @fieldName VARCHAR(800) = '*',
    @keyName VARCHAR(200) = 'Id',
    @pageSize INT = 20,
    @pageNo INT =1,
    @orderString VARCHAR(200), 
    @whereString VARCHAR(800) = '1=1'  
AS
BEGIN


     DECLARE @tempCount NVARCHAR(max),@tempMain NVARCHAR(max),@countIndex int
	 set @countIndex = (@pageNo -1) * @pageSize

     SET @tempCount = 'SELECT @recordTotal = COUNT(*) FROM (SELECT '+@keyName+' FROM '+@viewName+' WHERE '+@whereString+') AS my_temp'
     EXECUTE sp_executesql @tempCount,N'@recordTotal INT OUTPUT',@recordTotal OUTPUT
     SET @tempMain = 'SELECT '+@fieldName+' FROM '+@viewName+' where '+@whereString+' order by '+@orderString+' OFFSET '+ cast(@countIndex as varchar(8))+' ROWS FETCH NEXT '+cast(@pageSize as varchar(8))+' ROWS ONLY'
     EXECUTE (@tempMain)
END

GO

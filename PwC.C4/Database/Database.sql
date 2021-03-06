USE [master]
GO
/****** Object:  Database [PwCRushFramework]    Script Date: 23/04/2015 20:07:46 ******/
CREATE DATABASE [PwCRushFramework] ON  PRIMARY 
( NAME = N'PwCRushFramework', FILENAME = N'D:\SQL2008R2\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PwCRushFramework.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PwCRushFramework_log', FILENAME = N'D:\SQL2008R2\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PwCRushFramework_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PwCRushFramework] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PwCRushFramework].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PwCRushFramework] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PwCRushFramework] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PwCRushFramework] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PwCRushFramework] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PwCRushFramework] SET ARITHABORT OFF 
GO
ALTER DATABASE [PwCRushFramework] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PwCRushFramework] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [PwCRushFramework] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PwCRushFramework] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PwCRushFramework] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PwCRushFramework] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PwCRushFramework] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PwCRushFramework] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PwCRushFramework] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PwCRushFramework] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PwCRushFramework] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PwCRushFramework] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PwCRushFramework] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PwCRushFramework] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PwCRushFramework] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PwCRushFramework] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PwCRushFramework] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PwCRushFramework] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PwCRushFramework] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PwCRushFramework] SET  MULTI_USER 
GO
ALTER DATABASE [PwCRushFramework] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PwCRushFramework] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PwCRushFramework', N'ON'
GO
USE [PwCRushFramework]
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDataSource]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-4-22
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DataSource_GetDataSource]
	@AppCode varchar(50)
AS
BEGIN
	select [Name],[Key],[Value],d.[Order] from [dbo].[DataSourceType] t inner join [dbo].[DataSourceDetail] d on t.[Id]=d.[DataSourceTypeId]
	where appcode=@AppCode and t.State=0 and d.state=0
	order by d.[order]
END

GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDetails]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-04-17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DataSource_GetDetails]
	@appCode varchar(50),
	@dataSourceType varchar(50)
AS
BEGIN
	select [Key],Value from [dbo].[DataSourceType] t inner join [dbo].[DataSourceDetail] d
	on t.Id = d.DataSourceTypeId
	where appcode=@appCode and [Name]=@dataSourceType and  t.State=0 and d.state=0
	order by d.[Order] asc
END

GO
/****** Object:  StoredProcedure [dbo].[Metadata_CheckDataExist]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-04-17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Metadata_CheckDataExist]
	@tableName varchar(50),
	@pkColumn varchar(50),
	@dataId uniqueidentifier
AS
BEGIN
	declare @select nvarchar(4000)
	set @select = 'select count(0) from '+ @tableName +' where '+@pkColumn+'='''+CONVERT(char(255), @dataId) +''''
	exec(@select)
END

GO
/****** Object:  StoredProcedure [dbo].[Metadata_GetDataSet]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[Metadata_GetDataSet]
	@TableName varchar(80),   
	@Columns varchar(1000),---   
	@Where varchar(1000),---带and连接   
	@OrderCol varchar(100), -- 排序字段   
	@OrderType varchar(10),--asc:顺序，desc:倒序   
	@OtherOrder varchar(100),--其他排序
	@Start varchar(10), --   
	@Size varchar(10), -- 
	@TotalCount int output
AS
BEGIN
	declare @select nvarchar(4000),@count nvarchar(4000)
	if @Size=-1
	begin
	set @select = 'select ' + @Columns + ' from '+ @TableName+' where 1=1 '+@Where +'order by ' +@OrderCol +' '+@OrderType+' '+@OtherOrder
	end
	else
	begin
	set @select = 'select ' + @Columns + ' from (select *,ROW_NUMBER() over(order by ' + @OrderCol + ' '+ @OrderType+ ' '+@OtherOrder+' ) as rowNumber from ' 
	+ @TableName + ' where 1=1 ' + @Where + ' ) temp where rowNumber between '+@Start+' and '+ @Size
	end
	exec(@select)

	set @count='select @totalcount=count(0) from '+@TableName+ ' where 1=1 '+@Where
	exec sp_executesql @count,N'@totalcount int output',@totalcount output

	--select @totalcount

END




GO
/****** Object:  StoredProcedure [dbo].[Metadata_GetEntity]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Metadata_GetEntity]
	@tableName varchar(50),
	@columns varchar(500),
	@pkColumn varchar(50),
	@dataId uniqueidentifier
AS
BEGIN
	declare @select nvarchar(4000)
	set @select = 'select '+ @columns +' from '+ @tableName +' where '+@pkColumn+'='''+CONVERT(char(255), @dataId) +''''
	exec(@select)
END

GO
/****** Object:  StoredProcedure [dbo].[Preference_Get]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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




GO
/****** Object:  StoredProcedure [dbo].[Preference_Set]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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




GO
/****** Object:  Table [dbo].[DataSourceDetail]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataSourceDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[DataSourceTypeId] [uniqueidentifier] NULL,
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

GO
/****** Object:  Table [dbo].[DataSourceType]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Entity_Goods]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Entity_Goods](
	[GoodsId] [uniqueidentifier] NOT NULL,
	[GoodsType] [varchar](200) NULL,
	[GoodsName] [nvarchar](500) NULL,
	[GoodsState] [int] NULL,
	[GoodsPrice] [float] NULL,
	[IsDeleted] [bit] NULL,
	[CreateDate] [datetime] NULL,
	[ModifyDate] [datetime] NULL,
	[CreateBy] [varchar](100) NULL,
	[ModifyBy] [varchar](100) NULL,
 CONSTRAINT [PK_Entity_Goods] PRIMARY KEY CLUSTERED 
(
	[GoodsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExceptionLog](
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
 CONSTRAINT [PK_ErrorTrack] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Preference]    Script Date: 23/04/2015 20:07:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Preference](
	[AppCode] [varchar](256) NOT NULL,
	[Key] [varchar](256) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_Preference] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'6c22f129-39ca-9993-3f10-a320e9192b9e', N'e4dfde70-b985-5258-a4d9-88537303a938', N'0', N'OK', 0, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3205e825-3216-c951-fb02-e4ab8ef6f09c', N'e4dfde70-b985-5258-a4d9-88537303a938', N'1', N'Bad', 1, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e4dfde70-b985-5258-a4d9-88537303a938', N'PwC.C4.Web', N'Single', N'GoodsState', N'State', 0, 0, N'System', NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'1170044e-b94d-41cd-ac2b-0bf839e062c1', N'1|C4|2', N'dsadf12321', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7f529618-7f6e-43ca-a67c-4a48ba9b6c88', N'1|C4|3', N'21213saxzxc', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'34341480-c07e-4e95-b8b7-5397bdce174d', N'1PwC.C4.Web', N'xzcvzxc', 1, 1.1, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'08e93267-cf3d-4638-a824-58a48ed04370', N'1|C4|2|C4|3', N'this is Goods Name', 0, 0.5545, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7414d210-8ea2-4b8e-b117-6ebc52a256e6', N'2|C4|3', N'ghjghjffsdfg', 0, 0, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'635b8ec9-d842-4a4f-9d12-774646ae5a40', N'1', N'sdfgwetre', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'ba609744-36a6-47d7-8efb-7a1fadc1b875', N'2', N'bbbvv', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'680c6d36-7cef-4dba-b1d6-c1cc1f4cc85f', N'1', N'ccvvcvsdgsd', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'f35a1935-0df7-478f-99af-eabde53c908d', N'3', N'sadsdfgs', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ExceptionLog] ON 

GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (1, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00A09255 AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 23', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (2, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00A0926E AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 33', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (3, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00AA3614 AS DateTime), N'(null)', N'13', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (4, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00AA3656 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (5, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00AA3681 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (6, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00C384C3 AS DateTime), N'(null)', N'6', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (7, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00C384EE AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (8, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00C38505 AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (9, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FAFD4A AS DateTime), N'(null)', N'15', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (10, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB5221 AS DateTime), N'(null)', N'23', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (11, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB79CB AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (12, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBB29F AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (13, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEA6A AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (14, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEC7B AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (15, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBFE50 AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (16, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC4C0C AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (17, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC9379 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (18, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCA054 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (19, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCB229 AS DateTime), N'(null)', N'24', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (20, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCBB80 AS DateTime), N'(null)', N'32', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (21, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCEBA8 AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (22, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FD3450 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (23, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201090281 AS DateTime), N'(null)', N'33', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (24, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201095F07 AS DateTime), N'(null)', N'10', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (25, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300AD8C55 AS DateTime), N'(null)', N'50', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (26, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300B1332B AS DateTime), N'(null)', N'52', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
GO
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (27, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48301495ECE AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.Data.SqlClient.SqlException (0x80131904): An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 223
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149
ClientConnectionId:f5377f60-d244-4663-90df-9e5fdda0ec2a', 0)
GO
SET IDENTITY_INSERT [dbo].[ExceptionLog] OFF
GO
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityCol-ListPage', N'[{"Name":"GoodsId","Label":"Goods Id","SortName":"GoodsId","ShortName":"gid","Width":"130px","Order":0,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsType","Label":"Goods Type","SortName":"GoodsType","ShortName":"gt","Width":"130px","Order":1,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsName","Label":"Goods Name","SortName":"GoodsName","ShortName":"gn","Width":"130px","Order":2,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsState","Label":"Goods State","SortName":"GoodsState","ShortName":"gs","Width":"130px","Order":2,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsPrice","Label":"Goods Price","SortName":"GoodsPrice","ShortName":"gp","Width":"130px","Order":3,"Sortable":true,"Searchable":true,"Visable":true}]')
GO
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityColName-ListPage', N'["GoodsId","GoodsType","GoodsName","GoodsState","GoodsPrice"]')
GO
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityConfigCol-ListPage', N'[{"IsChecked":true,"Name":"GoodsId","Label":"Goods Id"},{"IsChecked":true,"Name":"GoodsType","Label":"Goods Type"},{"IsChecked":true,"Name":"GoodsName","Label":"Goods Name"},{"IsChecked":true,"Name":"GoodsState","Label":"Goods State"},{"IsChecked":true,"Name":"GoodsPrice","Label":"Goods Price"}]')
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-App-Key]    Script Date: 23/04/2015 20:07:46 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-App-Key] ON [dbo].[Preference]
(
	[AppCode] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [PwCRushFramework] SET  READ_WRITE 
GO

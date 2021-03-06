USE [master]
GO
/****** Object:  Database [PwCRushFramework]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[DataSource_GetDataSource]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[DataSource_GetDetails]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[EmailEmailParameters_Delete]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailEmailParameters_Delete]
	@AppCode varchar(50),
	@EmailParameterID int,
	@ModifyBy varchar(100)
AS
BEGIN
	update [dbo].EmailParameters
	set IsDeleted=1,ModifyBy=@ModifyBy,ModifyDate=getdate()
	where AppCode=@AppCode and [EmailParameterID]=@EmailParameterID
END


GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Create]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[EmailTemplate_Create]
	@AppCode varchar(50),
	@Group varchar(50),
	@EmailTemplateCode varchar(50),
	@TemplateName nvarchar(100),
	@EmailSubject nvarchar(100),
	@EmailContent nvarchar(100),
	@SendFrom nvarchar(100),
	@ReplyTo nvarchar(100),
	@CreateBy nvarchar(100),
	@ModifyBy nvarchar(100),
	@Id int output
AS
BEGIN
	declare @existId int

	if @Group =''
	begin
		set @Group=null
	end

	select top 1 @existId=[TemplateID] from [dbo].[EmailTemplates] 
	where appcode=@AppCode and [Group]=ISNULL(@Group,[Group]) and EmailTemplateCode=@EmailTemplateCode

	if @existId=1
	begin 
		set @Id = -1
	end
	else
	begin
		INSERT INTO [dbo].[EmailTemplates]
           ([AppCode]
           ,[Group]
           ,[EmailTemplateCode]
           ,[TemplateName]
           ,[EmailSubject]
           ,[EmailContent]
           ,[SendFrom]
           ,[ReplyTo]
           ,[CreateDate]
           ,[ModifyDate]
           ,[CreateBy]
           ,[ModifyBy])
     VALUES
           (@AppCode
           ,@Group
           ,@EmailTemplateCode
           ,@TemplateName
           ,@EmailSubject
           ,@EmailContent
           ,@SendFrom
           ,@ReplyTo
           ,GETDATE()
           ,GETDATE()
           ,@CreateBy
           ,@ModifyBy)

		   set @Id=@@IDENTITY
	end
END

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Delete]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Get]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_Get] 
	@AppCode varchar(50),
	@Group varchar(50),
	@TemplateCode varchar(50)
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select top 1 * from EmailTemplate
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) and [EmailTemplateCode]=@TemplateCode

END

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_List]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_List]
	@AppCode varchar(50),
	@Group varchar(50),
	@TemplateName nvarchar(100),
	@PageIndex int,
	@PageSize int,
	@TotalCount int output
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	if @PageSize =-1
	begin
		select  * from [dbo].[EmailTemplates] 
		where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateName] like '%'+@TemplateName+'%'
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by CreateDate desc) as 'rowNumber', * from [dbo].[EmailTemplates] 
	where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateName] like '%'+@TemplateName+'%'
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Update]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplate_Update]
	@TemplateID int,
	@AppCode varchar(50),
	@Group varchar(50),
	@EmailTemplateCode varchar(50),
	@TemplateName nvarchar(100),
	@EmailSubject nvarchar(100),
	@EmailContent nvarchar(100),
	@SendFrom nvarchar(100),
	@ReplyTo nvarchar(100),
	@ModifyBy nvarchar(100)
AS
BEGIN
	declare @existId int

	if @Group =''
	begin
		set @Group=null
	end

	if @TemplateID =''
	begin
		set @TemplateID=null
	end

	select top 1 @existId=[TemplateID] from [dbo].[EmailTemplates] 
	where appcode=@AppCode and [Group]=ISNULL(@Group,[Group]) and EmailTemplateCode=@EmailTemplateCode

	if (@existId=@TemplateID) OR ( @existId is null and @TemplateID is not null)
	begin 
		update [EmailTemplates] set 
		[Group]=@Group,
		EmailTemplateCode=@EmailTemplateCode,
		TemplateName=@TemplateName,
		EmailSubject=@EmailSubject,
		EmailContent=@EmailContent,
		SendFrom=@SendFrom,
		ReplyTo=@ReplyTo,
		ModifyDate=getdate(),
		ModifyBy=@ModifyBy

		where TemplateID=@TemplateID

		select @@ROWCOUNT
	end
	else
	begin
	select 0
	end
END

GO
/****** Object:  StoredProcedure [dbo].[Metadata_CheckDataExist]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_GetDataSet]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_GetEntity]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_Insert]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Metadata_Insert]
	@tableName varchar(50),
	@cols varchar(2000),
	@vals nvarchar(max)
AS
BEGIN
	declare @insertSql nvarchar(max)

	set @insertSql = 'insert into '+@tableName+' ('+@cols+') values ('+@vals+')'

	exec(@insertSql)
END

GO
/****** Object:  StoredProcedure [dbo].[Metadata_Update]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Metadata_Update]
	@tableName varchar(50),
	@pkName varchar(50),
	@dataId uniqueidentifier,
	@vals nvarchar(max)
AS
BEGIN
	declare @updateSql nvarchar(max)

	set @updateSql = 'update '+@tableName+' set '+@vals+' where '+ @pkName+' = '''+convert(nvarchar,@dataId)+''''

	exec(@updateSql)
END

GO
/****** Object:  StoredProcedure [dbo].[MetadataLog_Insert]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MetadataLog_Insert]
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
	INSERT INTO [dbo].[MetadataLog]
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

GO
/****** Object:  StoredProcedure [dbo].[Preference_Get]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  StoredProcedure [dbo].[Preference_Set]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  Table [dbo].[DataSourceDetail]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  Table [dbo].[DataSourceType]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  Table [dbo].[EmailParameters]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailParameters](
	[EmailParameterID] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
	[ParameterCode] [varchar](50) NOT NULL,
	[ParameterName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[ParameterType] [varchar](10) NULL,
	[Content] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateBy] [varchar](100) NULL,
	[ModifyBy] [varchar](100) NULL,
 CONSTRAINT [PK_EmailParameter_1] PRIMARY KEY CLUSTERED 
(
	[EmailParameterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailTemplates](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[AppCode] [varchar](50) NOT NULL,
	[Group] [varchar](50) NULL,
	[EmailTemplateCode] [varchar](50) NOT NULL,
	[TemplateName] [nvarchar](100) NULL,
	[EmailSubject] [nvarchar](1000) NULL,
	[EmailContent] [ntext] NULL,
	[SendFrom] [nvarchar](50) NULL,
	[ReplyTo] [nvarchar](50) NULL,
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Entity_Goods]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  Table [dbo].[ExceptionLog]    Script Date: 27/04/2015 18:00:00 ******/
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
/****** Object:  Table [dbo].[MetadataLog]    Script Date: 27/04/2015 18:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MetadataLog](
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
 CONSTRAINT [PK_MetadataLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Preference]    Script Date: 27/04/2015 18:00:00 ******/
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
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3205e825-3216-c951-fb02-e4ab8ef6f09c', N'e4dfde70-b985-5258-a4d9-88537303a938', N'1', N'Bad', 1, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e4dfde70-b985-5258-a4d9-88537303a938', N'PwC.C4.Web', N'Single', N'GoodsState', N'State', 0, 0, N'System', NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'1170044e-b94d-41cd-ac2b-0bf839e062c1', N'1|C4|2', N'dsadf12321', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7f529618-7f6e-43ca-a67c-4a48ba9b6c88', N'1|C4|3', N'21213saxzxc', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'34341480-c07e-4e95-b8b7-5397bdce174d', N'1PwC.C4.Web', N'xzcvzxc', 1, 1.1, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'08e93267-cf3d-4638-a824-58a48ed04370', N'1|C4|2|C4|3', N'this is Goods Name', 0, 0.5545, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7414d210-8ea2-4b8e-b117-6ebc52a256e6', N'2|C4|3', N'ghjghjffsdfg', 0, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'635b8ec9-d842-4a4f-9d12-774646ae5a40', N'1', N'sdfgwetre', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'ba609744-36a6-47d7-8efb-7a1fadc1b875', N'2', N'bbbvv', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'680c6d36-7cef-4dba-b1d6-c1cc1f4cc85f', N'1', N'ccvvcvsdgsd', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'f35a1935-0df7-478f-99af-eabde53c908d', N'3', N'sadsdfgs', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ExceptionLog] ON 

INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (1, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00A09255 AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 23', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (2, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00A0926E AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 33', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (3, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00AA3614 AS DateTime), N'(null)', N'13', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (4, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00AA3656 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (5, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00AA3681 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (6, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00C384C3 AS DateTime), N'(null)', N'6', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (7, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00C384EE AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (8, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00C38505 AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (9, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FAFD4A AS DateTime), N'(null)', N'15', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (10, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB5221 AS DateTime), N'(null)', N'23', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (11, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB79CB AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (12, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBB29F AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (13, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEA6A AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (14, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEC7B AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (15, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBFE50 AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (16, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC4C0C AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (17, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC9379 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (18, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCA054 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (19, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCB229 AS DateTime), N'(null)', N'24', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (20, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCBB80 AS DateTime), N'(null)', N'32', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (21, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCEBA8 AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (22, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FD3450 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (23, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201090281 AS DateTime), N'(null)', N'33', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (24, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201095F07 AS DateTime), N'(null)', N'10', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (25, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300AD8C55 AS DateTime), N'(null)', N'50', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (26, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300B1332B AS DateTime), N'(null)', N'52', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (27, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48301495ECE AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.Data.SqlClient.SqlException (0x80131904): An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 223
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149
ClientConnectionId:f5377f60-d244-4663-90df-9e5fdda0ec2a', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (28, N'PwC.C4.Web', N'PwC.C4.Common.Exceptions.JavascriptException', CAST(0x0000A48500FD14C8 AS DateTime), N'(null)', N'5', N'ERROR', N'PwC.C4.Web.Controller.Common.CommonApiController', N'GetColumnsForConfig Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]}', N'PwC.C4.Common.Exceptions.JavascriptException: GetColumnsForConfig Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]} ---> System.Exception: Fetch Error
   --- End of inner exception stack trace ---', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (29, N'PwC.C4.Web', N'PwC.C4.Common.Exceptions.JavascriptException', CAST(0x0000A48500FD14C8 AS DateTime), N'(null)', N'14', N'ERROR', N'PwC.C4.Web.Controller.Common.CommonApiController', N'GetTableColumns Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]}', N'PwC.C4.Common.Exceptions.JavascriptException: GetTableColumns Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]} ---> System.Exception: Fetch Error
   --- End of inner exception stack trace ---', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (30, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A48700C166F9 AS DateTime), N'(null)', N'14', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With context info: Instance type: PwC.C4.Model.GoodsInfo 
 
 With inner exception message: The incorrect number of parameters were supplied to the procedure dbo.Metadata_GetDataSet.  The number supplied was: 10.  The number expected is: 9. ---> System.ArgumentException: The incorrect number of parameters were supplied to the procedure dbo.Metadata_GetDataSet.  The number supplied was: 10.  The number expected is: 9.
   at PwC.C4.Infrastructure.Data.CommandFactory.AssertParameterCount(Int32 numProcedureParameters, Int32 numPassedParameters, Int32 returnValueOffset, String procedureName) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandFactory.cs:line 109
   at PwC.C4.Infrastructure.Data.CommandFactory.MapParameters(SqlCommand command, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandFactory.cs:line 31
   at PwC.C4.Infrastructure.Data.CommandFactory.CreateCommand(SqlConnection connection, String databaseInstanceName, String commandName, Object[] parameterValues) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandFactory.cs:line 180
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 108
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1040
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1062
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (31, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F250DB AS DateTime), N'(null)', N'10', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With parameters: {@@RETURN_VALUE}={}{@@TableName}={@TotalCount}{@@Columns}={@TableName}{@@Where}={@Columns}{@@OrderCol}={@Where}{@@OrderType}={@OrderCol}{@@OtherOrder}={@OrderType}{@@Start}={@OtherOrder}{@@Size}={@Start}{@@TotalCount}={@Size}
 With inner exception message: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Object must implement IConvertible.
   at System.Convert.ChangeType(Object value, Type conversionType, IFormatProvider provider)
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   --- End of inner exception stack trace ---
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   at System.Data.SqlClient.SqlParameter.GetCoercedValue()
   at System.Data.SqlClient.SqlParameter.Validate(Int32 index, Boolean isCommandProc)
   at System.Data.SqlClient.SqlCommand.SetUpRPCParameters(_SqlRPC rpc, Int32 startCount, Boolean inSchema, SqlParameterCollection parameters)
   at System.Data.SqlClient.SqlCommand.BuildRPC(Boolean inSchema, SqlParameterCollection parameters, _SqlRPC& rpc)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 113
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 122
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1062
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (32, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F3315A AS DateTime), N'(null)', N'29', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With parameters: {@@RETURN_VALUE}={}{@@TableName}={@TableName}{@@Columns}={@Columns}{@@Where}={@Where}{@@OrderCol}={@OrderCol}{@@OrderType}={@OrderType}{@@OtherOrder}={@OtherOrder}{@@Start}={@Start}{@@Size}={@Size}{@@TotalCount}={@TotalCount}
 With inner exception message: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Object must implement IConvertible.
   at System.Convert.ChangeType(Object value, Type conversionType, IFormatProvider provider)
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   --- End of inner exception stack trace ---
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   at System.Data.SqlClient.SqlParameter.GetCoercedValue()
   at System.Data.SqlClient.SqlParameter.Validate(Int32 index, Boolean isCommandProc)
   at System.Data.SqlClient.SqlCommand.SetUpRPCParameters(_SqlRPC rpc, Int32 startCount, Boolean inSchema, SqlParameterCollection parameters)
   at System.Data.SqlClient.SqlCommand.BuildRPC(Boolean inSchema, SqlParameterCollection parameters, _SqlRPC& rpc)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 113
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 122
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1062
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (33, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F450EA AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With parameters: {@@RETURN_VALUE}={}{@@TableName}={@TableName}{@@Columns}={@Columns}{@@Where}={@Where}{@@OrderCol}={@OrderCol}{@@OrderType}={@OrderType}{@@OtherOrder}={@OtherOrder}{@@Start}={@Start}{@@Size}={@Size}{@@TotalCount}={@TotalCount}
 With inner exception message: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Object must implement IConvertible.
   at System.Convert.ChangeType(Object value, Type conversionType, IFormatProvider provider)
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   --- End of inner exception stack trace ---
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   at System.Data.SqlClient.SqlParameter.GetCoercedValue()
   at System.Data.SqlClient.SqlParameter.Validate(Int32 index, Boolean isCommandProc)
   at System.Data.SqlClient.SqlCommand.SetUpRPCParameters(_SqlRPC rpc, Int32 startCount, Boolean inSchema, SqlParameterCollection parameters)
   at System.Data.SqlClient.SqlCommand.BuildRPC(Boolean inSchema, SqlParameterCollection parameters, _SqlRPC& rpc)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 113
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 122
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1062
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (34, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F84749 AS DateTime), N'(null)', N'55', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With parameters: {@@RETURN_VALUE}={}{@@TableName}={@TableName}{@@Columns}={@Columns}{@@Where}={@Where}{@@OrderCol}={@OrderCol}{@@OrderType}={@OrderType}{@@OtherOrder}={@OtherOrder}{@@Start}={@Start}{@@Size}={@Size}{@@TotalCount}={@TotalCount}
 With inner exception message: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Failed to convert parameter value from a SqlParameter to a Int32. ---> System.InvalidCastException: Object must implement IConvertible.
   at System.Convert.ChangeType(Object value, Type conversionType, IFormatProvider provider)
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   --- End of inner exception stack trace ---
   at System.Data.SqlClient.SqlParameter.CoerceValue(Object value, MetaType destinationType, Boolean& coercedToDataFeed, Boolean& typeChanged, Boolean allowStreaming)
   at System.Data.SqlClient.SqlParameter.GetCoercedValue()
   at System.Data.SqlClient.SqlParameter.Validate(Int32 index, Boolean isCommandProc)
   at System.Data.SqlClient.SqlCommand.SetUpRPCParameters(_SqlRPC rpc, Int32 startCount, Boolean inSchema, SqlParameterCollection parameters)
   at System.Data.SqlClient.SqlCommand.BuildRPC(Boolean inSchema, SqlParameterCollection parameters, _SqlRPC& rpc)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 113
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 122
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, Object[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1062
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (35, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FA7F00 AS DateTime), N'(null)', N'17', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (36, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FB07B5 AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (37, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FC3143 AS DateTime), N'(null)', N'25', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (38, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FC40B8 AS DateTime), N'(null)', N'30', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (39, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FCD9E6 AS DateTime), N'(null)', N'26', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (40, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A4870100DD93 AS DateTime), N'(null)', N'63', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetDataSet 
 
 With context info: Instance type: PwC.C4.Model.GoodsInfo 
 
 With inner exception message: The SqlParameter is already contained by another SqlParameterCollection. ---> System.ArgumentException: The SqlParameter is already contained by another SqlParameterCollection.
   at System.Data.SqlClient.SqlParameterCollection.Validate(Int32 index, Object value)
   at System.Data.SqlClient.SqlParameterCollection.Add(Object value)
   at System.Data.SqlClient.SqlParameterCollection.Add(SqlParameter value)
   at PwC.C4.Infrastructure.Data.CommandFactory.CreateCommand(SqlConnection connection, String databaseInstanceName, String commandName, SqlParameter[] parameterValues) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandFactory.cs:line 196
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, SqlParameter[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 137
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, SqlParameter[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1038
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndMetadata[T](Database database, String procedureName, RecordMapper`1 recordMapper, SqlParameter[] parameters) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 1060
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 118
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (41, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48701055887 AS DateTime), N'(null)', N'27', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'get GetEntity error,table name:Entity_Goods,PK Name:GoodsId,DataId:f35a1935-0df7-478f-99af-eabde53c908d,columns:[]', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetEntity 
 
 With parameters: {@@tableName}={Entity_Goods}{@@pkName}={GoodsId}{@@dataId}={f35a1935-0df7-478f-99af-eabde53c908d}{@@columns}={*}
 With inner exception message: Procedure or function ''Metadata_GetEntity'' expects parameter ''@pkColumn'', which was not supplied. ---> System.Data.SqlClient.SqlException: Procedure or function ''Metadata_GetEntity'' expects parameter ''@pkColumn'', which was not supplied.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 80
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 90
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndHydrateInstance[T](T objectInstance, Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 744
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndGetInstance[T](Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 839
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetEntity(String tableName, String pkName, Guid dataId, IList`1 properties) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 162', 0)
INSERT [dbo].[ExceptionLog] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (42, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A487010809B2 AS DateTime), N'(null)', N'47', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'get GetEntity error,table name:Entity_Goods,PK Name:GoodsId,DataId:f35a1935-0df7-478f-99af-eabde53c908d,columns:[]', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetEntity 
 
 With parameters: {@@tableName}={Entity_Goods}{@@pkName}={GoodsId}{@@dataId}={f35a1935-0df7-478f-99af-eabde53c908d}{@@columns}={*}
 With inner exception message: Procedure or function ''Metadata_GetEntity'' expects parameter ''@pkColumn'', which was not supplied. ---> System.Data.SqlClient.SqlException: Procedure or function ''Metadata_GetEntity'' expects parameter ''@pkColumn'', which was not supplied.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior)
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 80
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 90
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndHydrateInstance[T](T objectInstance, Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 744
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndGetInstance[T](Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 839
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetEntity(String tableName, String pkName, Guid dataId, IList`1 properties) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 162', 0)
SET IDENTITY_INSERT [dbo].[ExceptionLog] OFF
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityCol-ListPage', N'[{"Name":"GoodsId","Label":"Goods Id","SortName":"GoodsId","ShortName":"gid","Width":"130px","Order":0,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsType","Label":"Goods Type","SortName":"GoodsType","ShortName":"gt","Width":"130px","Order":1,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsName","Label":"Goods Name","SortName":"GoodsName","ShortName":"gn","Width":"130px","Order":2,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsState","Label":"Goods State","SortName":"GoodsState","ShortName":"gs","Width":"130px","Order":3,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsPrice","Label":"Goods Price","SortName":"GoodsPrice","ShortName":"gp","Width":"130px","Order":4,"Sortable":true,"Searchable":true,"Visable":true}]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityColName-ListPage', N'["GoodsId","GoodsType","GoodsName","GoodsState","GoodsPrice"]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityConfigCol-ListPage', N'[{"IsChecked":true,"Name":"GoodsId","Label":"Goods Id"},{"IsChecked":true,"Name":"GoodsType","Label":"Goods Type"},{"IsChecked":true,"Name":"GoodsName","Label":"Goods Name"},{"IsChecked":true,"Name":"GoodsState","Label":"Goods State"},{"IsChecked":true,"Name":"GoodsPrice","Label":"Goods Price"}]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Testings', N'WulalaWula')
/****** Object:  Index [NonClusteredIndex-Type-State]    Script Date: 27/04/2015 18:00:01 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Type-State] ON [dbo].[DataSourceDetail]
(
	[DataSourceTypeId] ASC,
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-AppCode-Name]    Script Date: 27/04/2015 18:00:01 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-AppCode-Name] ON [dbo].[DataSourceType]
(
	[AppCode] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-App-Key]    Script Date: 27/04/2015 18:00:01 ******/
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

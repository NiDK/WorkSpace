USE [master]
GO
/****** Object:  Database [PwCRushFramework]    Script Date: 30/04/2015 11:44:32 ******/
CREATE DATABASE [PwCRushFramework] ON  PRIMARY 
( NAME = N'PwCRushFramework', FILENAME = N'D:\SQL2008R2\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PwCRushFramework.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
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
/****** Object:  UserDefinedTableType [dbo].[StrIdTable]    Script Date: 30/04/2015 11:44:32 ******/
CREATE TYPE [dbo].[StrIdTable] AS TABLE(
	[Id] [varchar](100) NULL
)
GO
/****** Object:  StoredProcedure [dbo].[DataSource_GetDataSource]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[DataSource_GetDetails]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[EmailParameters_Create]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

	if @Group =''
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

GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Delete]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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


GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetByCode]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
	if @Group =''
	begin
		set @Group=null
	end

	select * from [EmailParameters]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) and ParameterCode in (select id from @ParameterCode)

END

GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_GetById]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_List]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailParameters_List]
	@AppCode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from [dbo].[EmailParameters]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) 
END

GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_ListByPaging]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
	if @Group =''
	begin
		set @Group=null
	end

	if @PageSize =-1
	begin
		select  * from [dbo].EmailParameters 
		where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [ParameterName] like '%'+@ParameterName+'%'
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by CreateDate desc) as 'rowNumber', * from [dbo].EmailParameters 
	where appcode = @AppCode and [Group]=ISNULL(@Group,[Group]) and [ParameterName] like '%'+@ParameterName+'%'
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END

GO
/****** Object:  StoredProcedure [dbo].[EmailParameters_Update]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

	if @Group =''
	begin
		set @Group=null
	end

	if @ParameterID =''
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

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Create]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

	if @Group =''
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

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Delete]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetByCode]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
	if @Group =''
	begin
		set @Group=null
	end

	select * from EmailTemplate
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) and [TemplateCode]in (select id from @TemplateCode)

END

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_GetById]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[EmailTemplate_GetById] 
	@AppCode varchar(50),
	@TemplateId int
AS
BEGIN

	select top 1 * from EmailTemplate
	where AppCode=@AppCode and TemplateId=@TemplateId

END

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplate_ListByPaging]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
/****** Object:  StoredProcedure [dbo].[EmailTemplate_Update]    Script Date: 30/04/2015 11:44:32 ******/
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

	if @TemplateID =''
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

GO
/****** Object:  StoredProcedure [dbo].[EmailTemplates_List]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EmailTemplates_List]
	@AppCode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from [dbo].[EmailTemplates]
	where AppCode=@AppCode and [Group]=ISNULL(@Group,[Group]) 
END

GO
/****** Object:  StoredProcedure [dbo].[Log_Metadata_Insert]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
/****** Object:  StoredProcedure [dbo].[Log_UserBehavior_Insert]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

GO
/****** Object:  StoredProcedure [dbo].[Metadata_CheckDataExist]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_GetDataSet]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_GetEntity]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_Insert]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[Metadata_Update]    Script Date: 30/04/2015 11:44:32 ******/
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
	@dataId varchar(100),
	@vals nvarchar(max)
AS
BEGIN
	declare @updateSql nvarchar(max)

	set @updateSql = 'update '+@tableName+' set '+@vals+' where '+ @pkName+' = '''+@dataId+''''

	exec(@updateSql)
END

GO
/****** Object:  StoredProcedure [dbo].[Preference_Get]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[Preference_Set]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_GetByStaffId]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

GO
/****** Object:  StoredProcedure [dbo].[vwStaffMaster_List]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[vwStaffMaster_List]
	@where nvarchar(max)
AS
BEGIN
	declare @sql nvarchar(max)
	set @sql = 'select * from [dbo].[vwStaffMaster]	where 1=1 '+@where
	exec(@sql)
END

GO
/****** Object:  Table [dbo].[DataSourceDetail]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataSourceType]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  Table [dbo].[EmailParameters]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 30/04/2015 11:44:32 ******/
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Entity_Goods]    Script Date: 30/04/2015 11:44:32 ******/
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
/****** Object:  Table [dbo].[Entity_PartnerConference]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Entity_PartnerConference](
	[RecordId] [uniqueidentifier] NOT NULL,
	[IsAttend] [bit] NULL,
	[IsAttendAssurance] [bit] NULL,
	[IsAttendTax] [bit] NULL,
	[IsAttendAdvisory] [bit] NULL,
	[IsAccommodation] [bit] NULL,
	[IsSmoking] [bit] NULL,
	[CheckInDate] [datetime] NULL,
	[CheckOutDate] [datetime] NULL,
	[DocumentName] [nvarchar](150) NULL,
	[DocumentType] [varchar](50) NULL,
	[DocumentNumber] [varchar](100) NULL,
	[CountryofResidence] [nvarchar](150) NULL,
	[DeparturetoMacao] [varchar](50) NULL,
	[MacaoFerrySchedule] [varchar](50) NULL,
	[DeparturetoHongKong] [varchar](50) NULL,
	[HongKongFerrySchedule] [varchar](50) NULL,
	[CarTransfer] [bit] NULL,
	[ArrivalAndDeparture] [varchar](50) NULL,
	[MealsType] [varchar](50) NULL,
	[SpecifyMeals] [nvarchar](300) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[ModifyBy] [varchar](50) NULL,
	[ModifyDate] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Entity_PartnerConference] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_Exception]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_Metadata]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log_UserBehavior]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Preference]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Preference](
	[AppCode] [varchar](256) NOT NULL,
	[Key] [varchar](256) NOT NULL,
	[Value] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vwDataSourceCheck]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwDataSourceCheck]
AS
SELECT        TOP (100) PERCENT dbo.DataSourceType.Name, dbo.DataSourceType.AppCode, dbo.DataSourceDetail.[Group], dbo.DataSourceDetail.[Key], 
                         dbo.DataSourceDetail.Value, dbo.DataSourceDetail.[Order]
FROM            dbo.DataSourceDetail INNER JOIN
                         dbo.DataSourceType ON dbo.DataSourceDetail.DataSourceTypeId = dbo.DataSourceType.Id

GO
/****** Object:  View [dbo].[vwStaffMaster]    Script Date: 30/04/2015 11:44:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwStaffMaster]
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

GO
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'640b9c95-7b16-4b8b-9b07-03f93331b76f', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'1', N'(Dep) 1000  (Arr) 1100', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'19f2fd36-7478-4835-8526-063d289098ae', N'1064eb34-8bef-4ffe-92fb-ad6f79a8fe30', N'', N'0', N'Beef', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'a972ac56-b20e-45f7-b539-07f0b233ab6c', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'0', N'(Dep) 1430  (Arr) 1530', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'613fe41b-9c09-4a27-9afa-082e51b4fb77', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'9', N'(Dep) 1530  (Arr) 1630', 9, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'f960c688-04c5-4a05-a091-0c5d0ac21aff', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'23', N'(Dep) 2230  (Arr) 2330', 23, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'87cfb765-c04e-4309-befd-0e720fe3ce5c', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'11', N'(Dep) 2000  (Arr) 2100', 11, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'893dcafa-a603-4ad4-9f27-0fd14efeca33', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'6', N'(Dep) 1730  (Arr) 1830', 6, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'acf35d17-e885-41c4-9034-106219e61dd5', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'17', N'(Dep) 2300  (Arr) 2400', 17, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e5eb5bf2-259d-4bbb-b222-119ec647357a', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'21', N'(Dep) 2130  (Arr) 2230', 21, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'1e542b92-3752-4bd5-a90d-14fb7730e2cd', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'18', N'(Dep) 2000  (Arr) 2100', 18, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'31331ede-d6c8-4712-88c3-17aed2fe3f3e', N'afa4ea67-6023-4eb5-a20d-8da2d5e65708', N'', N'0', N'Friday 03 July', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'8f997e03-2414-46b1-84ec-1a98cdd72248', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'11', N'(Dep) 1630  (Arr) 1730', 11, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'618a1f32-5d39-4f47-8484-1b04915e7671', N'fec3924b-39fb-4b0e-8888-785a334d2100', N'', N'true', N'I will attend the Advisory LOS meeting', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'5a2db307-9c64-47c7-a5f5-1faaec736de6', N'042254aa-9739-45c0-84e9-4795ecd6f72e', N'', N'false', N'I will NOT attend the Partner Conference', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5F6 AS DateTime), CAST(0x0000A48A00C0C5F6 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'01e430a6-51b4-40ac-9999-22624fff4143', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'3', N'(Dep) 1600  (Arr) 1700', 3, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'65ee59b6-c191-45be-8c35-24f9bd9bf74e', N'1064eb34-8bef-4ffe-92fb-ad6f79a8fe30', N'', N'2', N'Vegetarian', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'1fd78744-5d68-4c27-b9e8-25d73a3f7f7d', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'14', N'(Dep) 1800  (Arr) 1900', 14, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'b4debe2c-d61e-44da-8675-2793ae6bb1b4', N'679f58fd-466f-4526-9339-27ad98110a39', N'', N'0', N'One-way (arrival only)', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'97492af5-a446-438b-a790-3662039ec0f4', N'afa4ea67-6023-4eb5-a20d-8da2d5e65708', N'', N'1', N'Saturday 04 July', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'7081d80e-d53f-4787-b5af-36d739c12ab9', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'22', N'(Dep) 2200  (Arr) 2300', 22, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'28ccd68f-c3fd-49ba-b4d5-396840e5945c', N'326ec8a6-596c-4341-a1eb-b80356055cdd', N'', N'false', N'I will NOT attend the Assurance LOS meeting', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'56311608-9103-4974-bf99-39e00d61eaf8', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'5', N'(Dep) 1700  (Arr) 1800', 5, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'19a3fa01-e68d-4a88-845d-39eae9f05e07', N'1064eb34-8bef-4ffe-92fb-ad6f79a8fe30', N'', N'3', N'Other', 3, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e9d39b5a-4f19-4751-a6ee-3b36098f515a', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'16', N'(Dep) 2230  (Arr) 2330', 16, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'bb3ee850-77a2-4742-b187-3c9fc3991cb5', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'15', N'(Dep) 2200  (Arr) 2300', 15, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'828b9977-fc67-4595-90cc-3f62c73a3053', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'2', N'(Dep) 1030  (Arr) 1130', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'9b112873-51d5-407c-bb08-40d1b74c3012', N'8a004fb7-d3ab-4811-a3c0-25ef2302fe2d', N'', N'true', N'I prefer a non-smoking room', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'aa7c9a8a-1b19-4624-beb4-41a8064028ba', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'15', N'(Dep) 1700  (Arr) 1800', 15, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'799fa624-7aaf-42d0-945a-43c39dace9c3', N'fec3924b-39fb-4b0e-8888-785a334d2100', N'', N'false', N'I will NOT attend Advisory LOS meeting', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'dad8b6e9-6b90-4c73-a69f-44df3d31e714', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'3', N'(Dep) 1100  (Arr) 1200', 3, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'c2e93372-a297-42fe-87f4-478ccb12f850', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'24', N'(Dep) 2300  (Arr) 2400', 24, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'09bab028-4dc6-46df-803f-478e37e025ff', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'15', N'(Dep) 1830  (Arr) 1930', 15, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'bff49d1b-3b24-470f-b6f6-47be5615c85e', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'4', N'(Dep) 1300  (Arr) 1400', 4, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'a3bdecf3-396d-4689-a85d-480e7bd0ef45', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'5', N'(Dep) 1370  (Arr) 1470', 5, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'539a9223-2689-4bda-9e46-49366821be42', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'2', N'(Dep) 1200  (Arr) 1300', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'd68534e1-b614-432d-a06e-5ad4d5d072d2', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'11', N'(Dep) 1500  (Arr) 1600', 11, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'965f7f07-f04d-4d1a-8f4a-606bede86bfb', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'0', N'(Dep) 1100  (Arr) 1200', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'31c25da0-702c-4eaf-9b7f-6080468b9803', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'8', N'(Dep) 1830  (Arr) 1930', 8, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'c07fce83-f123-4020-adc5-6aae2fbc8eaf', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'9', N'(Dep) 1900  (Arr) 2000', 9, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'0ea408f0-5438-4081-b8f1-6b73572ded86', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'10', N'(Dep) 1600  (Arr) 1700', 10, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'78895394-45e5-4bf0-9034-6e11425fcdbe', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'2', N'(Dep) 1530  (Arr) 1630', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'9e1307b4-ef25-4d9c-9f6e-73c4b30e376c', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'5', N'(Dep) 1200  (Arr) 1300', 5, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'f56d21d0-44b5-499e-a9f1-74e1d42210af', N'9b86b29d-3b61-45a4-9c47-9bb7e34a2ab9', N'', N'true', N'I need accommodation', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'0866b56a-5743-4f97-bcdb-77b99148d494', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'6', N'(Dep) 1400  (Arr) 1500', 6, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3ccea0f3-4cb8-43fa-abdb-7962982bb04c', N'679f58fd-466f-4526-9339-27ad98110a39', N'', N'1', N'One-way (departure only)', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3f0f08c9-1713-4749-9954-7ee7b5e7d972', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'4', N'(Dep) 1630  (Arr) 1730', 4, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'bebe1af0-32c9-41f1-9976-810b78e05c15', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'12', N'(Dep) 1530  (Arr) 1630', 12, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'0b0f2d3f-b518-494a-9202-8535275d0228', N'9b86b29d-3b61-45a4-9c47-9bb7e34a2ab9', N'', N'false', N'I do not need accommodation', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e618361d-2047-4a96-9c49-86b377efafc9', N'a2ac1641-50ae-4c31-a20e-d59ed4660943', N'', N'false', N'I will NOT attend the Tax LOS meeting', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'73d0fb88-67e2-492b-9e73-87dbbcac33b5', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'17', N'(Dep) 1930  (Arr) 2030', 17, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'ba33542d-8296-4fda-9020-924b43a87e72', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'19', N'(Dep) 2030  (Arr) 2130', 19, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'61209f6c-0d47-40bb-ac00-958ca1a40ca5', N'679f58fd-466f-4526-9339-27ad98110a39', N'', N'2', N'Two-way (arrival & departure)', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'b8c3bda3-b7f2-4535-bd28-96ae8c05bff2', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'7', N'(Dep) 1800  (Arr) 1900', 7, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e55bc1cf-1728-4337-8f29-980a9e18eacc', N'1064eb34-8bef-4ffe-92fb-ad6f79a8fe30', N'', N'1', N'Fish and /or Prawn', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'59b89560-1dcd-4899-acb5-9ce725af90b2', N'd0329da1-e376-4e28-9a3c-5e1bbcf7ec61', N'', N'0', N'No Reply', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'1896df5a-769f-4ac4-9fb8-9f6dce66f2c5', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'0', N'(Dep) 0930  (Arr) 1030', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'6c22f129-39ca-9993-3f10-a320e9192b9e', N'e4dfde70-b985-5258-a4d9-88537303a938', NULL, N'0', N'OK', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'62e78458-335d-4256-87f0-a37d5c35854d', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'9', N'(Dep) 1400  (Arr) 1500', 9, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'03d181c3-9e96-4759-bb3f-a42fb46ec2fd', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'8', N'(Dep) 1370  (Arr) 1470', 8, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'2adc54ba-a442-4573-b086-a818ab2bd6ae', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'12', N'(Dep) 1700  (Arr) 1800', 12, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'8f9614d4-4b50-41ab-b505-a949d489bbbc', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'1', N'(Dep) 1130  (Arr) 1230', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'aaf6ea4f-9492-4c1d-9b63-aca5ddd8baf6', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'13', N'(Dep) 1600  (Arr) 1700', 13, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'8142a39e-a251-4cfa-b326-aecf6b036bd5', N'bd40eb4e-60a2-4eb4-a75d-1e971ad285cc', N'', N'1', N'None', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'0e5569fc-905b-423e-a23d-b21f95299152', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'16', N'(Dep) 1900  (Arr) 2000', 16, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e9ebc3b3-3a4c-4f08-8d82-b2def59314ed', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'13', N'(Dep) 1730  (Arr) 1830', 13, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'17a33356-f931-468b-aa55-b49738b98435', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'10', N'(Dep) 1930  (Arr) 2030', 10, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'cbc252ee-fa96-4ee0-a340-b55ddc71f768', N'bd94d1db-35e4-4c4b-820a-943d7f0ace42', N'', N'true', N'I need car transfer', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'b00fee9d-6c4b-4adf-a0b6-b5a282dd85e7', N'a2ac1641-50ae-4c31-a20e-d59ed4660943', N'', N'true', N'I will attend the Tax LOS meeting', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'f051f74b-fccc-40b9-ac4c-b83629ebe7ed', N'326ec8a6-596c-4341-a1eb-b80356055cdd', N'', N'true', N'I will attend the Assurance LOS meeting', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e76bba1b-1470-4755-909d-ba45489c1d8f', N'afa4ea67-6023-4eb5-a20d-8da2d5e65708', N'', N'2', N'None', 2, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'0b770cb7-46a6-4d4d-ad79-bc244aaf434d', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'20', N'(Dep) 2100  (Arr) 2200', 20, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'c5999160-a19b-4cfd-81a3-bc69fda2d92e', N'042254aa-9739-45c0-84e9-4795ecd6f72e', N'', N'true', N'I will attend the Partner Conference', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C562 AS DateTime), CAST(0x0000A48A00C0C562 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'a8c007e9-93d7-4269-a0c2-bd99565424ad', N'bd94d1db-35e4-4c4b-820a-943d7f0ace42', N'', N'false', N'I do not need car transfer', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'ff62cea7-a389-4082-972e-c64f783c5dd8', N'8a004fb7-d3ab-4811-a3c0-25ef2302fe2d', N'', N'false', N'I prefer a smoking room', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'074e9c9f-d0db-46b9-9118-c884e9b9c9ab', N'bd40eb4e-60a2-4eb4-a75d-1e971ad285cc', N'', N'0', N'Wednesday 01 July', 0, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'a3f402a1-ffc6-4821-ae55-c8c2b1805813', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'14', N'(Dep) 1630  (Arr) 1730', 14, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'54339c5b-5816-4267-94c9-ca47fca56565', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'14', N'(Dep) 2130  (Arr) 2230', 14, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'4d557c87-c81e-4689-bcfe-d0e98faf4d9b', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'7', N'(Dep) 1430  (Arr) 1530', 7, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'b763a5d9-a1aa-4905-a5e3-d27cc4e77e7e', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'7', N'(Dep) 1300  (Arr) 1400', 7, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'f121d598-29d8-4b18-8f3b-da8a7dc8cabe', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'4', N'(Dep) 1130  (Arr) 1230', 4, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'd27c9669-16ce-4d0d-9e02-df14c4a22a3a', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'13', N'(Dep) 2100  (Arr) 2200', 13, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'706212dc-5f03-456b-a3aa-e33b505540c3', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'12', N'(Dep) 2030  (Arr) 2130', 12, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3205e825-3216-c951-fb02-e4ab8ef6f09c', N'e4dfde70-b985-5258-a4d9-88537303a938', NULL, N'1', N'Bad', 1, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'6fb387ab-3004-4e4d-be51-e7d58ad2a923', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'6', N'(Dep) 1230  (Arr) 1330', 6, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'4e526027-b908-4afa-99c8-e94672503a5a', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'0', N'1', N'(Dep) 1500  (Arr) 1600', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'3f2ace65-be06-4be3-b526-eb659627fc68', N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'1', N'10', N'(Dep) 1430  (Arr) 1530', 10, 0, N'System', N'System', CAST(0x0000A48A00C0C600 AS DateTime), CAST(0x0000A48A00C0C600 AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'554ed6c8-9a12-4698-bd3e-f68b39b8c64b', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'8', N'(Dep) 1500  (Arr) 1600', 8, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'08060a94-93cc-4ca7-99d2-f8e6aa902c42', N'd0329da1-e376-4e28-9a3c-5e1bbcf7ec61', N'', N'1', N'Submited', 1, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceDetail] ([Id], [DataSourceTypeId], [Group], [Key], [Value], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'8f422a70-952d-4920-bd09-f9771ecb7a88', N'ab919792-dab2-43ca-a766-875f18835695', N'', N'3', N'(Dep) 1230  (Arr) 1330', 3, 0, N'System', N'System', CAST(0x0000A48A00C0C5FF AS DateTime), CAST(0x0000A48A00C0C5FF AS DateTime))
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'bd40eb4e-60a2-4eb4-a75d-1e971ad285cc', N'PartnerConference', NULL, N'DeparturetoMacao', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'8a004fb7-d3ab-4811-a3c0-25ef2302fe2d', N'PartnerConference', NULL, N'IsSmoking', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'679f58fd-466f-4526-9339-27ad98110a39', N'PartnerConference', NULL, N'ArrivalAndDeparture', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'042254aa-9739-45c0-84e9-4795ecd6f72e', N'PartnerConference', N'Single', N'AttendPartnerConference', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'd0329da1-e376-4e28-9a3c-5e1bbcf7ec61', N'PartnerConference', NULL, N'PCStatus', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'fec3924b-39fb-4b0e-8888-785a334d2100', N'PartnerConference', NULL, N'AttendAdvisoryMeeting', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'ab919792-dab2-43ca-a766-875f18835695', N'PartnerConference', NULL, N'MacaoFerrySchedule', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'e4dfde70-b985-5258-a4d9-88537303a938', N'PwC.C4.Web', N'Single', N'GoodsState', N'State', 0, 0, N'System', NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'afa4ea67-6023-4eb5-a20d-8da2d5e65708', N'PartnerConference', NULL, N'DeparturetoHongKong', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'bd94d1db-35e4-4c4b-820a-943d7f0ace42', N'PartnerConference', NULL, N'CarTransfer', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'9b86b29d-3b61-45a4-9c47-9bb7e34a2ab9', N'PartnerConference', NULL, N'Accommodation', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'1064eb34-8bef-4ffe-92fb-ad6f79a8fe30', N'PartnerConference', NULL, N'MealsType', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'326ec8a6-596c-4341-a1eb-b80356055cdd', N'PartnerConference', NULL, N'AttendAssuranceMeeting', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'c199a9f3-0371-4602-9380-c7b6df7cd87f', N'PartnerConference', NULL, N'HongKongFerrySchedule', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[DataSourceType] ([Id], [AppCode], [Type], [Name], [Desc], [Order], [State], [CreateBy], [ModifyBy], [CreateTime], [ModifyTime]) VALUES (N'a2ac1641-50ae-4c31-a20e-d59ed4660943', N'PartnerConference', NULL, N'AttendTaxMeeting', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'1170044e-b94d-41cd-ac2b-0bf839e062c1', N'1|C4|2', N'dsadf12321', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7f529618-7f6e-43ca-a67c-4a48ba9b6c88', N'1|C4|3', N'21213saxzxc', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'34341480-c07e-4e95-b8b7-5397bdce174d', N'1PwC.C4.Web', N'xzcvzxc', 1, 1.1, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'08e93267-cf3d-4638-a824-58a48ed04370', N'1|C4|2|C4|3', N'this is Goods Name', 0, 0.5545, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'7414d210-8ea2-4b8e-b117-6ebc52a256e6', N'2|C4|3', N'ghjghjffsdfg', 0, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'635b8ec9-d842-4a4f-9d12-774646ae5a40', N'1', N'sdfgwetre', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'ba609744-36a6-47d7-8efb-7a1fadc1b875', N'2', N'bbbvv', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'840f348e-2f32-43eb-a9ec-a19ce3cc1f1b', N'1|C4|2|C4|3', N'8888565645', 1, 0.223, 0, CAST(0x0000A4870129827C AS DateTime), CAST(0x0000A48701356A98 AS DateTime), NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'680c6d36-7cef-4dba-b1d6-c1cc1f4cc85f', N'1', N'ccvvcvsdgsd', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Entity_Goods] ([GoodsId], [GoodsType], [GoodsName], [GoodsState], [GoodsPrice], [IsDeleted], [CreateDate], [ModifyDate], [CreateBy], [ModifyBy]) VALUES (N'f35a1935-0df7-478f-99af-eabde53c908d', N'3', N'sadsdfgs', 1, 1.5542, 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Log_Exception] ON 

INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (1, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00A09255 AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 23', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (2, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00A0926E AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 33', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (3, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00AA3614 AS DateTime), N'(null)', N'13', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (4, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00AA3656 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (5, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00AA3681 AS DateTime), N'(null)', N'13', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (6, N'PwC.C4.Web', N'(null)', CAST(0x0000A47B00C384C3 AS DateTime), N'(null)', N'6', N'INFO', N'PwC.C4.Web.Controllers.HomeController', N'lalalal 满街爬', N'', 4)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (7, N'PwC.C4.Web', N'System.Collections.Generic.KeyNotFoundException', CAST(0x0000A47B00C384EE AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'Test', N'System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 24', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (8, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A47B00C38505 AS DateTime), N'(null)', N'6', N'ERROR', N'PwC.C4.Web.Controllers.HomeController', N'bool.Parse', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Web.Controllers.HomeController.Index() in c:\Development\Projects\PwC.C4\PwC.C4.Web\Controllers\HomeController.cs:line 34', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (9, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FAFD4A AS DateTime), N'(null)', N'15', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (10, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB5221 AS DateTime), N'(null)', N'23', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (11, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FB79CB AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (12, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBB29F AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (13, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEA6A AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (14, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBEC7B AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (15, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FBFE50 AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (16, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC4C0C AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (17, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FC9379 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (18, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCA054 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (19, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCB229 AS DateTime), N'(null)', N'24', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (20, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCBB80 AS DateTime), N'(null)', N'32', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (21, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FCEBA8 AS DateTime), N'(null)', N'18', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (22, N'PwC.C4.Web', N'System.NotImplementedException', CAST(0x0000A48200FD3450 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.NotImplementedException: The method or operation is not implemented.
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 142
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (23, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201090281 AS DateTime), N'(null)', N'33', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (24, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48201095F07 AS DateTime), N'(null)', N'10', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near ''Equal''.
Incorrect syntax near ''Equal''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cd9a64f9-18ba-46d5-8e8a-b0cd07b6be91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (25, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300AD8C55 AS DateTime), N'(null)', N'50', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (26, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48300B1332B AS DateTime), N'(null)', N'52', N'ERROR', N'PwC.C4.ServiceImp.Service.GoodsInfoService', N'Get GoodsInfo List Error', N'System.Data.SqlClient.SqlException (0x80131904): Invalid column name ''ID''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 218
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 144
   at PwC.C4.ServiceImp.Service.GoodsInfoService.GetGoodsInfoList(IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.ServiceImp\Service\GoodsInfoService.cs:line 48
ClientConnectionId:cdb9c3aa-905a-4237-a7db-1798d9fecc28', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (27, N'PwC.C4.Web', N'System.Data.SqlClient.SqlException', CAST(0x0000A48301495ECE AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.Data.SqlClient.SqlException (0x80131904): An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
An expression of non-boolean type specified in a context where a condition is expected, near ''Or''.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData(String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 223
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149
ClientConnectionId:f5377f60-d244-4663-90df-9e5fdda0ec2a', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (28, N'PwC.C4.Web', N'PwC.C4.Common.Exceptions.JavascriptException', CAST(0x0000A48500FD14C8 AS DateTime), N'(null)', N'5', N'ERROR', N'PwC.C4.Web.Controller.Common.CommonApiController', N'GetColumnsForConfig Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]}', N'PwC.C4.Common.Exceptions.JavascriptException: GetColumnsForConfig Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]} ---> System.Exception: Fetch Error
   --- End of inner exception stack trace ---', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (29, N'PwC.C4.Web', N'PwC.C4.Common.Exceptions.JavascriptException', CAST(0x0000A48500FD14C8 AS DateTime), N'(null)', N'14', N'ERROR', N'PwC.C4.Web.Controller.Common.CommonApiController', N'GetTableColumns Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]}', N'PwC.C4.Common.Exceptions.JavascriptException: GetTableColumns Error ,data:{"Type":"Entity_Goods","Page":"ListPage","TableId":"details","AppCode":"PwC.C4.Web","DataUrl":"/Dashboard/DataList","CustomColumn":{"Enable":false,"CustomColumns":"","CheckBoxColumn":"GoodsId"},"Search":{"SearchAreaId":"searchArea","Enable":true,"SearchColumns":"GoodsName,GoodsType"},"Config":{"Enable":true,"DialogId":"configDialog","Title":"Config table columns for this page"},"EnableExprot":true,"EnableColumnDrag":true,"DefaultOrder":0,"DefaultOrderMethod":"Desc","SearchItems":[]} ---> System.Exception: Fetch Error
   --- End of inner exception stack trace ---', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (30, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A48700C166F9 AS DateTime), N'(null)', N'14', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (31, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F250DB AS DateTime), N'(null)', N'10', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (32, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F3315A AS DateTime), N'(null)', N'29', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (33, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F450EA AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (34, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48700F84749 AS DateTime), N'(null)', N'55', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (35, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FA7F00 AS DateTime), N'(null)', N'17', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (36, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FB07B5 AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (37, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FC3143 AS DateTime), N'(null)', N'25', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (38, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FC40B8 AS DateTime), N'(null)', N'30', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (39, N'PwC.C4.Web', N'System.NullReferenceException', CAST(0x0000A48700FCD9E6 AS DateTime), N'(null)', N'26', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'System.NullReferenceException: Object reference not set to an instance of an object.
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetGetDataSetCommonData[T](String tableName, IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount, String otherOrder) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 143
   at PwC.C4.Metadata.Service.EntityService.GetEntites[T](IList`1 searchItems, String orderCol, String orderType, IList`1 columns, Int32 pageIndex, Int32 pageSize, Int32& totalCount) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Service\EntityService.cs:line 149', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (40, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A4870100DD93 AS DateTime), N'(null)', N'63', N'ERROR', N'PwC.C4.Metadata.Service.EntityService', N'GetEntites<T> Error', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (41, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A48701055887 AS DateTime), N'(null)', N'27', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'get GetEntity error,table name:Entity_Goods,PK Name:GoodsId,DataId:f35a1935-0df7-478f-99af-eabde53c908d,columns:[]', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (42, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException', CAST(0x0000A487010809B2 AS DateTime), N'(null)', N'47', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'get GetEntity error,table name:Entity_Goods,PK Name:GoodsId,DataId:f35a1935-0df7-478f-99af-eabde53c908d,columns:[]', N'PwC.C4.Infrastructure.Exceptions.DatabaseExecutionException: Database Exception Against Database: AppConnStrName 
 
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
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (43, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012958C0 AS DateTime), N'(null)', N'19', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,datas:{"GoodsId":"7ea5af0a-7ab9-412e-b13d-56b489504806","GoodsType":"1|C4|3","GoodsName":"sadsdfgsdd","GoodsState":1,"GoodsPrice":1.5542,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Insert 
 
 With inner exception message: Procedure or function ''Metadata_Insert'' expects parameter ''@vals'', which was not supplied. ---> System.Data.SqlClient.SqlException: Procedure or function ''Metadata_Insert'' expects parameter ''@vals'', which was not supplied.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.InsertMetadata(String tableName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 53', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (44, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012A5B46 AS DateTime), N'(null)', N'29', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"sadsdfgsasdf","GoodsState":1,"GoodsPrice":1222.0,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 87', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (45, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012B4109 AS DateTime), N'(null)', N'28', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"121312","GoodsState":1,"GoodsPrice":1.22,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 87', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (46, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012BF3F0 AS DateTime), N'(null)', N'7', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"121312","GoodsState":1,"GoodsPrice":1.22,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 87', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (47, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012C7A42 AS DateTime), N'(null)', N'19', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 87', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (48, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487012D9341 AS DateTime), N'(null)', N'29', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (49, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487013072A0 AS DateTime), N'(null)', N'20', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Arithmetic overflow error converting expression to data type nvarchar. ---> System.Data.SqlClient.SqlException: Arithmetic overflow error converting expression to data type nvarchar.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (50, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A4870131FDC2 AS DateTime), N'(null)', N'16', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Incorrect syntax near ''1''. ---> System.Data.SqlClient.SqlException: Incorrect syntax near ''1''.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (51, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A487013225B3 AS DateTime), N'(null)', N'21', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"1|C4|2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Incorrect syntax near ''1''. ---> System.Data.SqlClient.SqlException: Incorrect syntax near ''1''.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (52, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A4870132869A AS DateTime), N'(null)', N'39', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Incorrect syntax near ''2''. ---> System.Data.SqlClient.SqlException: Incorrect syntax near ''2''.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (53, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A48701336C00 AS DateTime), N'(null)', N'50', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Incorrect syntax near ''2''. ---> System.Data.SqlClient.SqlException: Incorrect syntax near ''2''.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (54, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A48701339938 AS DateTime), N'(null)', N'62', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'Save metadata error,table name:Entity_Goods,PK Name:GoodsId,DataId:840f348e-2f32-43eb-a9ec-a19ce3cc1f1b,datas:{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"2|C4|3","GoodsName":"dddd","GoodsState":1,"GoodsPrice":1.123,"IsDeleted":false}', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_Update 
 
 With inner exception message: Incorrect syntax near ''2''. ---> System.Data.SqlClient.SqlException: Incorrect syntax near ''2''.
   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, SqlDataReader ds)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean asyncWrite)
   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 144
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, SqlConnection connection, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 162
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper, OutputParameterMapper outputMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 80
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteNonQuery(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 173
   at PwC.C4.Metadata.Persistance.CommonDataDao.UpdateMetadata(String tableName, String pkName, Guid dataId, String modifyUserId, Dictionary`2 datas) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 91', 0)
INSERT [dbo].[Log_Exception] ([Id], [AppCode], [Type], [Date], [StaffId], [Thread], [Level], [Logger], [Message], [Exception], [Status]) VALUES (55, N'PwC.C4.Web', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException', CAST(0x0000A4870133CF90 AS DateTime), N'(null)', N'30', N'ERROR', N'PwC.C4.Metadata.Persistance.CommonDataDao', N'get GetEntity error,table name:Entity_Goods,PK Name:GoodsId,DataId:f35a1935-0df7-478f-99af-eabde53c908d,columns:[]', N'PwC.C4.Infrastructure.Exceptions.SafeProcedureException: Database Exception Against Database: AppConnStrName 
 
 With procedure: dbo.Metadata_GetEntity 
 
 With context info: Instance type: System.Collections.Generic.Dictionary`2[System.String,System.Object] 
 
 With inner exception message: The handle is invalid. (Exception from HRESULT: 0x80070006 (E_HANDLE)) ---> System.Runtime.InteropServices.COMException: The handle is invalid. (Exception from HRESULT: 0x80070006 (E_HANDLE))
   at System.Runtime.InteropServices.Marshal.ThrowExceptionForHRInternal(Int32 errorCode, IntPtr errorInfo)
   at System.Data.ProviderBase.DbConnectionPool.TryGetConnection(DbConnection owningObject, UInt32 waitForMultipleObjectsTimeout, Boolean allowCreate, Boolean onlyOneCheckConnection, DbConnectionOptions userOptions, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionPool.TryGetConnection(DbConnection owningObject, TaskCompletionSource`1 retry, DbConnectionOptions userOptions, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionFactory.TryGetConnection(DbConnection owningConnection, TaskCompletionSource`1 retry, DbConnectionOptions userOptions, DbConnectionInternal oldConnection, DbConnectionInternal& connection)
   at System.Data.ProviderBase.DbConnectionInternal.TryOpenConnectionInternal(DbConnection outerConnection, DbConnectionFactory connectionFactory, TaskCompletionSource`1 retry, DbConnectionOptions userOptions)
   at System.Data.SqlClient.SqlConnection.TryOpenInner(TaskCompletionSource`1 retry)
   at System.Data.SqlClient.SqlConnection.TryOpen(TaskCompletionSource`1 retry)
   at System.Data.SqlClient.SqlConnection.Open()
   at PwC.C4.Infrastructure.Data.CommandCache.GetCommandCopy(SqlConnection connection, String databaseInstanceName, String procedureName) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandCache.cs:line 21
   at PwC.C4.Infrastructure.Data.CommandFactory.CreateParameterMappedCommand(SqlConnection connection, String databaseInstanceName, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\CommandFactory.cs:line 145
   at PwC.C4.Infrastructure.Data.Procedure.Execute(Database database, String procedureName, ParameterMapper parameterMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\Procedure.cs:line 71
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndHydrateInstance[T](T objectInstance, Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 724
   --- End of inner exception stack trace ---
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndHydrateInstance[T](T objectInstance, Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 744
   at PwC.C4.Infrastructure.Data.SafeProcedure.ExecuteAndGetInstance[T](Database database, String procedureName, ParameterMapper parameterMapper, RecordMapper`1 recordMapper) in c:\Development\Projects\PwC.C4\PwC.C4.Infrastructure\Data\SafeProcedure.cs:line 839
   at PwC.C4.Metadata.Persistance.CommonDataDao.GetEntity(String tableName, String pkName, Guid dataId, IList`1 properties) in c:\Development\Projects\PwC.C4\PwC.C4.Metadata\Persistance\CommonDataDao.cs:line 172', 0)
SET IDENTITY_INSERT [dbo].[Log_Exception] OFF
SET IDENTITY_INSERT [dbo].[Log_Metadata] ON 

INSERT [dbo].[Log_Metadata] ([Id], [AppCode], [Metadata], [DataId], [Method], [JsonData], [UserId], [ActionDate], [IpAddress], [MachineName]) VALUES (1, N'PwC.C4.Web', N'Entity_Goods', N'840f348e-2f32-43eb-a9ec-a19ce3cc1f1b', 0, N'{"GoodsId":"840f348e-2f32-43eb-a9ec-a19ce3cc1f1b","GoodsType":"1|C4|2|C4|3","GoodsName":"sadsdfgs555","GoodsState":1,"GoodsPrice":1.5542,"IsDeleted":false}', N'', CAST(0x0000A48701299070 AS DateTime), N'', N'')
INSERT [dbo].[Log_Metadata] ([Id], [AppCode], [Metadata], [DataId], [Method], [JsonData], [UserId], [ActionDate], [IpAddress], [MachineName]) VALUES (2, N'PwC.C4.Web', N'Entity_Goods', N'840f348e-2f32-43eb-a9ec-a19ce3cc1f1b', 1, N'{"GoodsId":"f35a1935-0df7-478f-99af-eabde53c908d","GoodsType":"2|C4|3","GoodsName":"55555","GoodsState":1,"GoodsPrice":1.11,"IsDeleted":false}', N'CN110', CAST(0x0000A487013465FF AS DateTime), N'', N'')
INSERT [dbo].[Log_Metadata] ([Id], [AppCode], [Metadata], [DataId], [Method], [JsonData], [UserId], [ActionDate], [IpAddress], [MachineName]) VALUES (3, N'PwC.C4.Web', N'Entity_Goods', N'840f348e-2f32-43eb-a9ec-a19ce3cc1f1b', 1, N'{"GoodsId":"840f348e-2f32-43eb-a9ec-a19ce3cc1f1b","GoodsType":"1|C4|2|C4|3","GoodsName":"8888565645","GoodsState":1,"GoodsPrice":0.223,"IsDeleted":false}', N'CN110', CAST(0x0000A48701356A9E AS DateTime), N'', N'')
SET IDENTITY_INSERT [dbo].[Log_Metadata] OFF
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityCol-ListPage', N'[{"Name":"GoodsId","Label":"Goods Id","SortName":"GoodsId","ShortName":"gid","Width":"130px","Order":0,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsType","Label":"Goods Type","SortName":"GoodsType","ShortName":"gt","Width":"130px","Order":1,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsName","Label":"Goods Name","SortName":"GoodsName","ShortName":"gn","Width":"130px","Order":2,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsState","Label":"Goods State","SortName":"GoodsState","ShortName":"gs","Width":"130px","Order":3,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsPrice","Label":"Goods Price","SortName":"GoodsPrice","ShortName":"gp","Width":"130px","Order":4,"Sortable":true,"Searchable":true,"Visable":true}]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityColName-ListPage', N'["GoodsId","GoodsType","GoodsName","GoodsState","GoodsPrice"]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Entity_Goods-EntityConfigCol-ListPage', N'[{"IsChecked":true,"Name":"GoodsId","Label":"Goods Id"},{"IsChecked":false,"Name":"GoodsType","Label":"Goods Type"},{"IsChecked":true,"Name":"GoodsName","Label":"Goods Name"},{"IsChecked":false,"Name":"GoodsState","Label":"Goods State"},{"IsChecked":false,"Name":"GoodsPrice","Label":"Goods Price"}]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PwC.C4.Web', N'Testings', N'WulalaWula')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PartnerConference', N'Entity_Goods-EntityColName-ListPage', N'["GoodsId","GoodsName"]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PartnerConference', N'Entity_Goods-EntityCol-ListPage', N'[{"Name":"GoodsId","Label":"Goods Id","SortName":"GoodsId","ShortName":"gid","Width":"130px","Order":0,"Sortable":true,"Searchable":true,"Visable":true},{"Name":"GoodsName","Label":"Goods Name","SortName":"GoodsName","ShortName":"gn","Width":"130px","Order":2,"Sortable":true,"Searchable":true,"Visable":true}]')
INSERT [dbo].[Preference] ([AppCode], [Key], [Value]) VALUES (N'PartnerConference', N'Entity_Goods-EntityConfigCol-ListPage', N'[{"IsChecked":true,"Name":"GoodsId","Label":"Goods Id"},{"IsChecked":false,"Name":"GoodsType","Label":"Goods Type"},{"IsChecked":true,"Name":"GoodsName","Label":"Goods Name"},{"IsChecked":false,"Name":"GoodsState","Label":"Goods State"},{"IsChecked":false,"Name":"GoodsPrice","Label":"Goods Price"}]')
/****** Object:  Index [NonClusteredIndex-Type-State]    Script Date: 30/04/2015 11:44:32 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Type-State] ON [dbo].[DataSourceDetail]
(
	[DataSourceTypeId] ASC,
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-AppCode-Name]    Script Date: 30/04/2015 11:44:32 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-AppCode-Name] ON [dbo].[DataSourceType]
(
	[AppCode] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-App-Key]    Script Date: 30/04/2015 11:44:32 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-App-Key] ON [dbo].[Preference]
(
	[AppCode] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DataSourceDetail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 298
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DataSourceType"
            Begin Extent = 
               Top = 6
               Left = 261
               Bottom = 264
               Right = 431
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwDataSourceCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwDataSourceCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 324
               Right = 241
            End
            DisplayFlags = 280
            TopColumn = 33
         End
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 279
               Bottom = 324
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwStaffMaster'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwStaffMaster'
GO
USE [master]
GO
ALTER DATABASE [PwCRushFramework] SET  READ_WRITE 
GO

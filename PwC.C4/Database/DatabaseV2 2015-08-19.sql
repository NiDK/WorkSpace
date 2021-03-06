USE [master]
GO
/****** Object:  Database [PwCC4DataKeeper]    Script Date: 19/08/2015 17:48:57 ******/
CREATE DATABASE [PwCC4DataKeeper]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PwCC4DataKeeper', FILENAME = N'D:\SQL2012\MSSQL11.MSSQLSERVER\MSSQL\DATA\PwCC4DataKeeper.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PwCC4DataKeeper_log', FILENAME = N'D:\SQL2012\MSSQL11.MSSQLSERVER\MSSQL\DATA\PwCC4DataKeeper_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PwCC4DataKeeper] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PwCC4DataKeeper].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PwCC4DataKeeper] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET ARITHABORT OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [PwCC4DataKeeper] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PwCC4DataKeeper] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PwCC4DataKeeper] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PwCC4DataKeeper] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PwCC4DataKeeper] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET RECOVERY FULL 
GO
ALTER DATABASE [PwCC4DataKeeper] SET  MULTI_USER 
GO
ALTER DATABASE [PwCC4DataKeeper] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PwCC4DataKeeper] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PwCC4DataKeeper] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PwCC4DataKeeper] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PwCC4DataKeeper', N'ON'
GO
USE [PwCC4DataKeeper]
GO
/****** Object:  User [DEV\chenhui yu]    Script Date: 19/08/2015 17:48:57 ******/
CREATE USER [DEV\chenhui yu] FOR LOGIN [DEV\chenhui yu] WITH DEFAULT_SCHEMA=[db_owner]
GO
ALTER ROLE [db_datareader] ADD MEMBER [DEV\chenhui yu]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [DEV\chenhui yu]
GO
/****** Object:  UserDefinedTableType [dbo].[tb_datasource_detail]    Script Date: 19/08/2015 17:48:57 ******/
CREATE TYPE [dbo].[tb_datasource_detail] AS TABLE(
	[id] [uniqueidentifier] NULL,
	[datasource_type_id] [uniqueidentifier] NULL,
	[group] [varchar](50) NULL,
	[key] [nvarchar](50) NULL,
	[value] [nvarchar](500) NULL,
	[order] [int] NULL,
	[state] [int] NULL,
	[create_by] [nvarchar](50) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[tb_guid_id]    Script Date: 19/08/2015 17:48:57 ******/
CREATE TYPE [dbo].[tb_guid_id] AS TABLE(
	[Id] [uniqueidentifier] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[tb_str_id]    Script Date: 19/08/2015 17:48:57 ******/
CREATE TYPE [dbo].[tb_str_id] AS TABLE(
	[Id] [varchar](100) NULL
)
GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_detail_update]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_detail_update]
    @datasource_type_id uniqueidentifier,
	@create_by varchar(50),
	@details [tb_datasource_detail] readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update [dbo].[datasource_detail] 
	set [state]=1,[modify_by]=@create_by,[modify_time]=getdate()
	where [datasource_type_id]=@datasource_type_id and [state]=0

	INSERT INTO [dbo].[datasource_detail]
           ([id]
           ,[datasource_type_id]
           ,[group]
           ,[key]
           ,[value]
           ,[order]
           ,[state]
           ,[create_by]
           ,[create_time]
           ,[modify_by]
           ,[modify_time])
	 select id,@datasource_type_id,[group],[key],[value],[order],0,@create_by,getdate(),@create_by,getdate()
	 from @details
END

GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_get_datasource]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-4-22
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_get_datasource]
	@appcode varchar(50)
AS
BEGIN
	select [name],[key],[value],d.[order] from [dbo].[datasource_type] t inner join [dbo].[datasource_detail] d on t.[Id]=d.[datasource_type_id]
	where appcode=@appcode and t.state=0 and d.state=0
	order by d.[order]
END







GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_get_details]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chenhui Yu
-- Create date: 2015-04-17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_get_details]
	@appcode varchar(50),
	@dataSourceType varchar(50),
	@group varchar(50)
AS
BEGIN

if @group = ''
	begin
	set @group=null
	end

	select [key],value from [dbo].[datasource_type] t inner join [dbo].[datasource_detail] d
	on t.Id = d.datasource_type_id
	where t.[appcode]=@appcode and [name]=@dataSourceType and [group]=isnull(@group,[group]) and  t.state=0 and d.state=0
	order by d.[order] asc
END







GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_type_get_list]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_type_get_list]
	@appcode varchar(50)
AS
BEGIN
	select [id],[type],[name] from [dbo].[datasource_type]
	where ([appcode]='' or [appcode]=@appcode) and [state]=0
	order by [order] asc
END

GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_type_insert]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_type_insert]
	@id uniqueidentifier,
	@appcode varchar(50),
	@type varchar(50),
	@name nvarchar(50),
	@desc nvarchar(500),
	@order int,
	@state int,
	@create_by nvarchar(50)
AS
BEGIN
	declare @is_exist int

	select @is_exist=count(0) from [dbo].[datasource_type]
	where appcode=@appcode and [type]=@type

	if @is_exist=0
	begin
		INSERT INTO [dbo].[datasource_type]
           ([id]
           ,[appcode]
           ,[type]
           ,[name]
           ,[desc]
           ,[order]
           ,[state]
           ,[create_by]
           ,[create_time]
           ,[modify_by]
           ,[modify_time])
     VALUES
           (@id ,
			@appcode,
			@type,
			@name ,
			@desc ,
			@order ,
			@state ,
			@create_by ,
			getdate() ,
			@create_by ,
			getdate() )
		select @@ROWCOUNT
	end
	else
	begin
		select -1
	end
END

GO
/****** Object:  StoredProcedure [dbo].[sp_datasource_type_update]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_datasource_type_update]
	@id uniqueidentifier,
	@name nvarchar(50),
	@desc nvarchar(500),
	@order int,
	@state int,
	@modify_by nvarchar(50)
AS
BEGIN
   UPDATE [dbo].[datasource_type]
   SET 
      [name] = @name
      ,[desc] = @desc
      ,[order] = @order
      ,[state] = @state
      ,[modify_by] = @modify_by
      ,[modify_time] = getdate()
 WHERE [id] = @id

 select @@ROWCOUNT
END

GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_create]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_create]
	@appcode varchar(50),
	@Group varchar(50),
	@parameter_code varchar(50),
	@parameter_name nvarchar(50),
	@assembly nvarchar(200),
	@parameter_type int,
	@content nvarchar(max),
	@create_by varchar(100),
	@modify_by varchar(100),
	@Id int output
AS
BEGIN
	declare @existId int

	if @Group =''
	begin
		set @Group=null
	end

	select top 1 @existId=[parameter_id] from [dbo].[email_parameters] 
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) and [parameter_id]=@parameter_code

	if @existId=1
	begin 
		set @Id = -1
	end
	else
	begin

		   INSERT INTO [dbo].[email_parameters]
           ([appcode]
           ,[group]
           ,[parameter_code]
           ,[parameter_name]
           ,[assembly]
           ,[parameter_type]
           ,[content]
           ,[is_deleted]
           ,[create_time]
           ,[modify_time]
           ,[create_by]
           ,[modify_by])
     VALUES
           (@appcode
           ,@Group
           ,@parameter_code
           ,@parameter_name
           ,@assembly
           ,@parameter_type
           ,@content
           ,0
           ,GETDATE()
           ,GETDATE()
           ,@create_by
           ,@modify_by)

	set @Id=@@IDENTITY

	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_delete]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_delete]
	@appcode varchar(50),
	@parameter_id int,
	@modify_by varchar(100)
AS
BEGIN
	update [dbo].email_parameters
	set is_deleted=1,modify_by=@modify_by,modify_time=getdate()
	where appcode=@appcode and parameter_id=@parameter_id
END








GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_get_by_code]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_get_by_code] 
	@appcode varchar(50),
	@Group varchar(50),
	@parameter_code [tb_str_id] readonly
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from [email_parameters]
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) and parameter_code in (select id from @parameter_code)

END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_get_by_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_get_by_id] 
	@appcode varchar(50),
	@parameter_id int
AS
BEGIN

	select top 1 * from [email_parameters]
	where appcode=@appcode and parameter_id=@parameter_id

END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_list]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_list]
	@appcode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from [dbo].[email_parameters]
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) 
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_list_by_paging]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_list_by_paging]
	@appcode varchar(50),
	@Group varchar(50),
	@parameter_name nvarchar(100),
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
		select  * from [dbo].email_parameters 
		where appcode = @appcode and [group]=ISNULL(@Group,[group]) and [parameter_name] like '%'+@parameter_name+'%'
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by create_time desc) as 'rowNumber', * from [dbo].email_parameters 
	where appcode = @appcode and [group]=ISNULL(@Group,[group]) and [parameter_name] like '%'+@parameter_name+'%'
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_parameters_update]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_parameters_update]
	@parameter_id int,
	@appcode varchar(50),
	@Group varchar(50),
	@parameter_code varchar(50),
	@parameter_name nvarchar(50),
	@assembly nvarchar(200),
	@parameter_type int,
	@content nvarchar(max),
	@modify_by varchar(100)
AS
BEGIN
	declare @existId int

	if @Group =''
	begin
		set @Group=null
	end

	if @parameter_id =''
	begin
		set @parameter_id=null
	end

	select top 1 @existId=parameter_id from [dbo].[email_parameters] 
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) and parameter_code=@parameter_code

	if (@existId=@parameter_id) OR ( @existId is null and @parameter_id is not null)
	begin 
		UPDATE [dbo].[email_parameters]
    SET
      [group] = @Group
      ,[parameter_code] = @parameter_code
      ,[parameter_name] = @parameter_name
      ,[assembly] = @assembly
      ,[parameter_type] = @parameter_type
      ,[content] = @content
      ,[modify_time] = GETDATE()
      ,[modify_by] = @modify_by
    WHERE parameter_id=@parameter_id

		select @@ROWCOUNT
	end
	else
	begin
	select -1
	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_create]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_create]
		   @appcode varchar(50),
           @Group varchar(50),
           @template_code varchar(50),
           @template_name nvarchar(100),
           @mail_from varchar(60),
           @mail_reply_to varchar(400),
           @mail_cc varchar(400),
           @mail_bcc varchar(400),
           @mail_organisation varchar(60),
           @mail_Subject nvarchar(1000),
           @mail_content ntext,
		   @mail_submit_by varchar(60),
		   @is_immediate bit,
           @create_by varchar(100),
           @modify_by varchar(100),
		   @Id int output
AS
BEGIN
	declare @existId int

	if @Group =''
	begin
		set @Group=null
	end

	select top 1 @existId=[template_id] from [dbo].[email_templates] 
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) and template_code=@template_code

	if @existId=1
	begin 
		set @Id = -1
	end
	else
	begin
INSERT INTO [dbo].[email_templates]
           ([appcode]
           ,[group]
           ,[template_code]
           ,[template_name]
           ,[mail_from]
           ,[mail_reply_to]
           ,[mail_cc]
           ,[mail_bcc]
           ,[mail_organisation]
           ,[mail_Subject]
           ,[mail_content]
           ,[mail_submit_by]
		   ,is_immediate
           ,[create_time]
           ,[modify_time]
           ,[create_by]
           ,[modify_by]
           ,[is_deleted])
     VALUES
           (@appcode,
			@Group,
			@template_code,
			@template_name,
			@mail_from,
			@mail_reply_to,
			@mail_cc,
			@mail_bcc,
			@mail_organisation,
			@mail_Subject,
			@mail_content,
			@mail_submit_by,
			@is_immediate,
			getdate(),
			getdate(),
			@create_by,
			@modify_by,
			0)

		   set @Id=@@IDENTITY
	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_delete]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_delete]
	@appcode varchar(50),
	@template_id int,
	@modify_by varchar(100)
AS
BEGIN
	update [dbo].[email_templates]
	set is_deleted=1,modify_by=@modify_by,modify_time=getdate()
	where appcode=@appcode and template_id=@template_id
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_get_by_code]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_get_by_code] 
	@appcode varchar(50),
	@Group varchar(50),
	@template_code [tb_str_id] readonly
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from email_templates
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) and [template_code]in (select id from @template_code)

END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_get_by_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_get_by_id] 
	@appcode varchar(50),
	@template_id int
AS
BEGIN

	select top 1 * from email_templates
	where appcode=@appcode and template_id=@template_id

END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_List]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_List]
	@appcode varchar(50),
	@group varchar(50)
AS
BEGIN
	if @Group =''
	begin
		set @Group=null
	end

	select * from [dbo].[email_templates]
	where appcode=@appcode and [group]=ISNULL(@Group,[group]) 
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_list_by_paging]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_list_by_paging]
	@appcode varchar(50),
	@Group varchar(50),
	@template_name nvarchar(100),
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
		select  * from [dbo].[email_templates] 
		where appcode = @appcode and [group]=ISNULL(@Group,[group]) and [template_name] like '%'+@template_name+'%'
	end
	else
	begin

	select * from 
    (select ROW_NUMBER() over(order by create_time desc) as 'rowNumber', * from [dbo].[email_templates] 
	where appcode = @appcode and [group]=ISNULL(@Group,[group]) and [template_name] like '%'+@template_name+'%'
	) as temp
    where rowNumber between @PageIndex and (@PageIndex+@PageSize)
	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_email_template_update]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_email_template_update]
		   @template_id int,
		   @appcode			 varchar(50),
           @template_code 	 varchar(50),
           @template_name 	 nvarchar(100),
           @mail_from 		 varchar(60),
           @mail_reply_to 	 varchar(400),
           @mail_cc 			 varchar(400),
           @mail_bcc 		 varchar(400),
           @mail_organisation varchar(60),
           @mail_Subject 	 nvarchar(1000),
           @mail_content 	 ntext,
           @mail_submit_by 	 varchar(60),
		   @is_immediate		 bit,
           @modify_by 		 varchar(100)
AS
BEGIN
	declare @existId int

	if @template_id =''
	begin
		set @template_id=null
	end

	select top 1 @existId=[template_id] from [dbo].[email_templates] 
	where appcode=@appcode and template_code=@template_code

	if (@existId=@template_id) OR ( @existId is null and @template_id is not null)
	begin 
		update [email_templates] set 
		template_code = @template_code,
		template_name = @template_name,
		mail_from = @mail_from,
		mail_reply_to = @mail_reply_to,
		mail_cc = @mail_cc,
		mail_bcc = @mail_bcc,
		mail_organisation = @mail_organisation,
		mail_Subject = @mail_Subject,
		mail_content = @mail_content,
		mail_submit_by = @mail_submit_by,
		is_immediate=@is_immediate,
		modify_by = @modify_by,
		modify_time = getdate()

		where template_id=@template_id

		select @@ROWCOUNT
	end
	else
	begin
	select 0
	end
END







GO
/****** Object:  StoredProcedure [dbo].[sp_mail_master_mail_queue_insert]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_mail_master_mail_queue_insert]
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
	@encode			varchar(12) = 'utf-8'
)

AS
begin
	
	if @mSUBMITBY is null or len(isnull(@mSUBMITBY,''))=0
	    begin
		set @mSUBMITBY ='pwchk.com Web Master'
	    end

	if @mMFROM is null or len(isnull(@mMFROM,''))=0
	    begin
		set @mMFROM = 'pwcwebmaster@hk.pwcglobal.com'
            end

	if @mCC is null or len(isnull(@mCC,''))=0
            begin
		set @mCC = null
	    end
           
        if @mREPLYTo is null or len(isnull(@mREPLYTO,''))=0
            begin
		set @mREPLYTO = @mMFROM
	    end

	if @mBCC is null or len(isnull(@mBCC,''))=0
	    begin
		set @mBCC = null
	    end

	if @mORGANISATION is null or len(isnull(@mORGANISATION,''))=0
	    begin
		set @mORGANISATION = 'PricewaterhouseCoopers'
	    end
	 


        if @mSUBMITDATE is null or convert(varchar,isnull(@mSUBMITDATE,''),103)='01/01/1900'
	    begin
	        set @mSUBMITDATE = convert(datetime, getdate())
            end    
	else
            begin
		set @mSUBMITDATE = convert(datetime, @mSUBMITDATE)
	    end
           
        if @mPOSTEDFLAG is null or len(isnull(@mPOSTEDFLAG,''))=0 
	    begin
		set @mPOSTEDFLAG = 'N'
	    end      
        	

        if @mSENDDATE is null or convert(varchar,isnull(@mSENDDATE,''),103)='01/01/1900' 
	    begin
		set @mSENDDATE = convert(datetime,getdate())
            end
        else
 	    begin
	        set @mSENDDATE = convert(datetime, @mSENDDATE)
            end

	if @mIMMEDIATEFLAG is null or len(isnull(@mIMMEDIATEFLAG,''))=0
	    begin
		set @mIMMEDIATEFLAG = 'N'
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






GO
/****** Object:  StoredProcedure [dbo].[sp_vw_staff_master_get_by_staff_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_vw_staff_master_get_by_staff_id] 
	@staffId varchar(10)
AS
BEGIN
	select top 1 * from [dbo].vw_staff_master
	where StaffID=@staffId
END







GO
/****** Object:  StoredProcedure [dbo].[sp_vw_staff_master_list]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_vw_staff_master_list]
	@where nvarchar(max)
AS
BEGIN
	declare @sql nvarchar(max)
	set @sql = 'select * from [dbo].[vw_staff_master] where 1=1 '+@where
	exec(@sql)
END







GO
/****** Object:  StoredProcedure [dbo].[sp_workflow_get_acitve_by_form_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_workflow_get_acitve_by_form_id]
	@appCode varchar(50),
	@entityName varchar(50),
	@formId int
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and entity_name=@entityName and form_id=@formId and is_active=1
END


GO
/****** Object:  StoredProcedure [dbo].[sp_workflow_get_acitve_by_record_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_workflow_get_acitve_by_record_id]
	@appCode varchar(50),
	@entityName varchar(50),
	@recordId uniqueidentifier
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and entity_name=@entityName and record_id=@recordId and is_active=1
END


GO
/****** Object:  StoredProcedure [dbo].[sp_workflow_get_list_by_form_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_workflow_get_list_by_form_id]
	@appCode varchar(50),
	@entityName varchar(50),
	@formId int
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and entity_name=@entityName and form_id=@formId
	order by create_time desc
END


GO
/****** Object:  StoredProcedure [dbo].[sp_workflow_get_list_by_record_id]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[sp_workflow_get_list_by_record_id]
	@appCode varchar(50),
	@entityName varchar(50),
	@recordId uniqueidentifier
AS
BEGIN
	select * from [dbo].[Workflow]
	where AppCode=@appCode and entity_name=@entityName and record_id=@recordId
	order by create_time desc
END


GO
/****** Object:  StoredProcedure [dbo].[sp_workflow_insert_workflow]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_workflow_insert_workflow]
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
	
	update [dbo].[workflow] set is_active=0
	where form_id=@formId or record_id=@recordId

	INSERT INTO [dbo].[workflow]
           ([appcode]
           ,[entity_name]
		   ,workflow_code
		   ,action_code
           ,[form_id]
           ,[record_id]
           ,[instance_id]
           ,[user_id]
           ,[user_role]
           ,[status]
           ,[create_time]
		   ,comment,
		   is_active)
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


GO
/****** Object:  Table [dbo].[datasource_detail]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[datasource_detail](
	[id] [uniqueidentifier] NOT NULL,
	[datasource_type_id] [uniqueidentifier] NULL,
	[group] [varchar](50) NULL,
	[key] [nvarchar](50) NULL,
	[value] [nvarchar](500) NULL,
	[order] [int] NULL,
	[state] [int] NULL,
	[create_by] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[modify_by] [nvarchar](50) NULL,
	[modify_time] [datetime] NULL,
 CONSTRAINT [PK_DataSourceDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[datasource_type]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[datasource_type](
	[id] [uniqueidentifier] NOT NULL,
	[appcode] [varchar](50) NULL,
	[type] [varchar](50) NULL,
	[name] [nvarchar](50) NULL,
	[desc] [nvarchar](500) NULL,
	[order] [int] NULL,
	[state] [int] NULL,
	[create_by] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[modify_by] [nvarchar](50) NULL,
	[modify_time] [datetime] NULL,
 CONSTRAINT [PK_DataSourceType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[email_parameters]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[email_parameters](
	[parameter_id] [int] IDENTITY(1,1) NOT NULL,
	[appcode] [varchar](50) NOT NULL,
	[group] [varchar](50) NULL,
	[parameter_code] [varchar](50) NOT NULL,
	[parameter_name] [nvarchar](50) NOT NULL,
	[assembly] [nvarchar](200) NULL,
	[parameter_type] [int] NOT NULL,
	[content] [nvarchar](max) NULL,
	[is_deleted] [bit] NOT NULL,
	[create_by] [varchar](100) NULL,
	[create_time] [datetime] NOT NULL,
	[modify_by] [varchar](100) NULL,
	[modify_time] [datetime] NOT NULL,
 CONSTRAINT [PK_EmailParameter_1] PRIMARY KEY CLUSTERED 
(
	[parameter_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[email_templates]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[email_templates](
	[template_id] [int] IDENTITY(1,1) NOT NULL,
	[appcode] [varchar](50) NOT NULL,
	[group] [varchar](50) NULL,
	[template_code] [varchar](50) NOT NULL,
	[template_name] [nvarchar](100) NULL,
	[mail_from] [varchar](60) NULL,
	[mail_reply_to] [varchar](400) NULL,
	[mail_cc] [varchar](400) NULL,
	[mail_bcc] [varchar](400) NULL,
	[mail_organisation] [varchar](60) NULL,
	[mail_Subject] [nvarchar](1000) NULL,
	[mail_content] [ntext] NULL,
	[mail_submit_by] [varchar](60) NULL,
	[is_immediate] [bit] NULL,
	[create_by] [varchar](100) NULL,
	[create_time] [datetime] NOT NULL,
	[modify_by] [varchar](100) NULL,
	[modify_time] [datetime] NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[template_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[metadata_applications]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[metadata_applications](
	[application_id] [uniqueidentifier] NOT NULL,
	[application_identity_id] [int] IDENTITY(1,1) NOT NULL,
	[is_enabe_appcentre] [bit] NULL,
	[appcode] [varchar](50) NULL,
	[application_name] [nvarchar](150) NULL,
	[application_description] [nvarchar](500) NULL,
	[application_status] [int] NULL,
	[application_is_offline] [bit] NULL,
	[application_is_deleted] [bit] NULL,
	[application_create_by] [varchar](50) NULL,
	[application_create_time] [datetime] NULL,
	[application_modify_by] [varchar](50) NULL,
	[application_modify_time] [datetime] NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[application_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[metadata_columns]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[metadata_columns](
	[column_id] [uniqueidentifier] NOT NULL,
	[column_identity_id] [int] IDENTITY(1,1) NOT NULL,
	[entity_id] [uniqueidentifier] NULL,
	[column_name] [nvarchar](150) NULL,
	[column_code] [varchar](150) NULL,
	[column_description] [nvarchar](500) NULL,
	[data_type] [int] NULL,
	[data_length] [int] NULL,
	[label] [nvarchar](150) NULL,
	[width] [varchar](50) NULL,
	[order] [int] NULL,
	[visable] [bit] NULL,
	[searchable] [bit] NULL,
	[sortable] [bit] NULL,
	[sort_name] [varchar](150) NULL,
	[short_name] [varchar](50) NULL,
	[data_source_type] [int] NULL,
	[data_source_detail] [varchar](150) NULL,
	[data_source_group_by] [varchar](150) NULL,
	[data_control_type] [int] NULL,
	[is_require] [bit] NULL,
	[invalid_msg] [nvarchar](350) NULL,
	[input_regular] [nvarchar](150) NULL,
	[is_format] [bit] NULL,
	[data_format] [varchar](50) NULL,
	[is_default] [bit] NULL,
	[is_translate] [int] NULL,
	[column_is_deleted] [bit] NULL,
	[column_status] [int] NULL,
	[column_create_by] [varchar](50) NULL,
	[column_create_time] [datetime] NULL,
	[column_modify_by] [varchar](50) NULL,
	[column_modify_time] [datetime] NULL,
 CONSTRAINT [PK_MetadataColumns] PRIMARY KEY CLUSTERED 
(
	[column_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[metadata_entitys]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[metadata_entitys](
	[entity_id] [uniqueidentifier] NOT NULL,
	[entity_identity_id] [int] IDENTITY(1,1) NOT NULL,
	[application_id] [uniqueidentifier] NULL,
	[entity_code] [varchar](150) NULL,
	[entity_name] [nvarchar](150) NULL,
	[entity_escription] [nvarchar](500) NULL,
	[enable_workflow] [bit] NULL,
	[workflow_code] [varchar](150) NULL,
	[entity_is_offline] [bit] NULL,
	[entity_is_deleted] [bit] NULL,
	[entity_status] [int] NULL,
	[entity_create_by] [varchar](50) NULL,
	[entity_create_time] [datetime] NULL,
	[entity_modify_by] [varchar](50) NULL,
	[entity_modify_time] [datetime] NULL,
 CONSTRAINT [PK_Entitys] PRIMARY KEY CLUSTERED 
(
	[entity_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[workflow]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[workflow](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[appcode] [varchar](150) NULL,
	[entity_name] [varchar](150) NULL,
	[workflow_code] [varchar](150) NULL,
	[action_code] [varchar](150) NULL,
	[form_id] [int] NULL,
	[record_id] [uniqueidentifier] NULL,
	[instance_id] [int] NULL,
	[user_id] [varchar](50) NULL,
	[user_role] [varchar](50) NULL,
	[status] [varchar](150) NULL,
	[comment] [nvarchar](500) NULL,
	[is_active] [bit] NULL,
	[create_time] [datetime] NULL,
 CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vw_datasource_check]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_datasource_check]
AS
SELECT        TOP (100) PERCENT dbo.datasource_type.name, dbo.datasource_type.appcode, dbo.datasource_detail.[group], dbo.datasource_detail.[key], 
                         dbo.datasource_detail.value, dbo.datasource_detail.[order]
FROM            dbo.datasource_detail INNER JOIN
                         dbo.datasource_type ON dbo.datasource_detail.datasource_type_id = dbo.datasource_type.id

GO
/****** Object:  View [dbo].[vw_staff_master]    Script Date: 19/08/2015 17:48:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_staff_master]
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
/****** Object:  Index [NonClusteredIndex-Type-State]    Script Date: 19/08/2015 17:48:57 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Type-State] ON [dbo].[datasource_detail]
(
	[datasource_type_id] ASC,
	[state] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-AppCode-Name]    Script Date: 19/08/2015 17:48:57 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-AppCode-Name] ON [dbo].[datasource_type]
(
	[appcode] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-Code]    Script Date: 19/08/2015 17:48:57 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Code] ON [dbo].[email_parameters]
(
	[appcode] ASC,
	[group] ASC,
	[parameter_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-Code]    Script Date: 19/08/2015 17:48:57 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-Code] ON [dbo].[email_templates]
(
	[appcode] ASC,
	[group] ASC,
	[template_code] ASC
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
         Begin Table = "datasource_detail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 280
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "datasource_type"
            Begin Extent = 
               Top = 6
               Left = 266
               Bottom = 205
               Right = 436
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_datasource_check'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_datasource_check'
GO
USE [master]
GO
ALTER DATABASE [PwCC4DataKeeper] SET  READ_WRITE 
GO

USE [PwCC4Base]
GO

DECLARE	@return_value int,@uddt StrIdTable,@appCode varchar(150),@Area varchar(105),@Controller varchar(105),@Action varchar(105),@url varchar(105), @roleId uniqueidentifier, @functionId uniqueidentifier,@hasRight int,@roleCount int

insert into @uddt(Id) values('Admin')
insert into @uddt(Id) values('Viewer')

set	@appCode = N'C4Rush'
		set	@Area = N'Admin'
		set	@Controller = N'Home'
		set	@Action = N'Index'
		set	@url = NULL
		select top 1 @functionId=[Id] from [dbo].[Security_Function]
	where appcode = @appCode and ((Area=isnull(@Area,Area) and Controller=@Controller and [Action]=@Action) or Url=@url)
	and IsDeleted =0

	select @functionId

select Id into #roleIds  from Security_Role
		where AppCode = @appCode and IsDeleted=0 and RoleName in (select Id from @uddt)
		select @roleCount = count(0) from #roleIds
		if @roleCount is not null and @roleCount >0
		begin 
			with funcIds as(
				select [FunctionId] from [dbo].[Security_Rel_FunctionRole]
				where appcode=@appCode and [Role] in (select id from #roleIds) and IsDeleted = 0 and FunctionType=1
				UNION all
				select [FunctionId] from [dbo].[Security_Rel_FunctionGroup]
				where [GroupId] in (
					select functionId from [dbo].[Security_Rel_FunctionRole]
					where appcode=@appCode and [Role] in (select id from #roleIds) and IsDeleted = 0 and FunctionType=2
				)
			)
			select * from  funcIds
			--select @hasRight=count(0) from funcIds where [FunctionId]=@functionId
			--select @hasRight
		end
		else
		begin
			select -2 --Not exist role
		end

		drop table #roleIds
/****** Script for SelectTopNRows command from SSMS  ******/
--select * into DataPicker_Staff from 
with crt as(SELECT  LTRIM(RTRIM([StaffID])) as StaffID
      ,[CountryCode]
      ,[StaffInitial]
      ,[StaffName]
      ,[FirstName]
      ,[LastName]
      ,[DivCode]
      ,[DivName]
      ,[GroupCode]
      ,[GroupName]
      ,[GradeCode]
      ,[GradeName]
      ,[JobTitle]
      ,[PhoneNo]
      ,[OfficeBuilding]
      ,[OfficeFloor]
      ,[SecName]
      ,[OfficePIN]
      ,[LoginID]
      ,[LoginContext]
      ,[PowerStaffCode]
      ,[PowerGroupCode]
      ,[PowerGradeCode]
      ,[TermFlag]
      ,[TermDate]
      ,[Email]
      ,[GUID]
      ,[LDAP_DistName]
      ,[PwCEntityID]
      ,[localExpatFlag]
      ,[ExpatFlag]
      ,[INetEmail]
      ,[NotesID]
      ,[BU]
      ,[BUDesc]
      ,[SubService]
      ,[SubServiceDesc]
      ,[JoinDate]
      ,[IsProfessional]
      ,[JobCode]
      ,[NativeName]
      ,[FullName]
      ,[NameInEng]
      ,[GivenName]
      ,[PreferredName]
      ,[TerritoryEffectiveDate]
      ,[OfficeCityName]
      ,[HRID]
      ,[CountryOffice]
      ,[OUCode]
      ,[Status]
      ,[ContractYearEnd]
      ,[IDTerritory]
      ,[PowerIDStatus]
      ,[LoS]
      ,[Department]
      ,[SalaryCategory]
      ,[JobGrade]
      ,[EmployeeCategory]
      ,[Role]
      ,[Sex]
      ,[SchoolUniversity]
      ,[Majority]
      ,[MajorityCategory]
      ,[City]
      ,[ChineseName]
      ,[LoSDesc]
	  ,1 as [Count],
	  1 as [Type]
  FROM [PwCC4Base].[dbo].[vwStaffMaster]
UNION all
SELECT '@'+LTRIM(RTRIM([GroupName])) as [StaffID]
      ,null as [CountryCode]
      ,null as [StaffInitial]
      ,LTRIM(RTRIM([GroupName])) as  [StaffName]
      ,null as [FirstName]
      ,null as [LastName]
      ,null as [DivCode]
      ,'Group' as [DivName]
      ,null as [GroupCode]
      ,null as [GroupName]
      ,null as [GradeCode]
      ,null as [GradeName]
      ,null as [JobTitle]
      ,null as [PhoneNo]
      ,null as [OfficeBuilding]
      ,null as [OfficeFloor]
      ,null as [SecName]
      ,null as [OfficePIN]
      ,null as [LoginID]
      ,null as [LoginContext]
      ,null as [PowerStaffCode]
      ,null as [PowerGroupCode]
      ,null as [PowerGradeCode]
      ,'N' as [TermFlag]
      ,null as [TermDate]
      ,LTRIM(RTRIM([GroupName])) as [Email]
      ,null as [GUID]
      ,null as [LDAP_DistName]
      ,null as [PwCEntityID]
      ,null as [localExpatFlag]
      ,null as [ExpatFlag]
      ,null as [INetEmail]
      ,null as [NotesID]
      ,null as [BU]
      ,null as [BUDesc]
      ,null as [SubService]
      ,null as [SubServiceDesc]
      ,null as [JoinDate]
      ,null as [IsProfessional]
      ,null as [JobCode]
      ,null as [NativeName]
      ,null as [FullName]
      ,null as [NameInEng]
      ,null as [GivenName]
      ,null as [PreferredName]
      ,null as [TerritoryEffectiveDate]
      ,null as [OfficeCityName]
      ,null as [HRID]
      ,null as [CountryOffice]
      ,null as [OUCode]
      ,null as [Status]
      ,null as [ContractYearEnd]
      ,null as [IDTerritory]
      ,null as [PowerIDStatus]
      ,null as [LoS]
      ,null as [Department]
      ,null as [SalaryCategory]
      ,null as [JobGrade]
      ,null as [EmployeeCategory]
      ,null as [Role]
      ,null as [Sex]
      ,null as [SchoolUniversity]
      ,null as [Majority]
      ,null as [MajorityCategory]
      ,null as [City]
      ,null as [ChineseName]
      ,null as [LoSDesc]
	  ,count(0) as [Count]
	  ,2 as [Type]
  FROM [StaffBank].[dbo].[StaffDomainGroup]
  group by [GroupName]) 

 select * into #mergeStaffInfo from crt where staffid not like '%note a%' and staffid <>''
  
  merge into [dbo].[DataPicker_Staff] as t
  using #mergeStaffInfo as s
  on t.StaffId = s.StaffId
  when matched
  then update set t.[CountryCode] = s.[CountryCode]
,t.[StaffInitial] = s.[StaffInitial]
,t.[StaffName] = s.[StaffName]
,t.[FirstName] = s.[FirstName]
,t.[LastName] = s.[LastName]
,t.[DivCode] = s.[DivCode]
,t.[DivName] = s.[DivName]
,t.[GroupCode] = s.[GroupCode]
,t.[GroupName] = s.[GroupName]
,t.[GradeCode] = s.[GradeCode]
,t.[GradeName] = s.[GradeName]
,t.[JobTitle] = s.[JobTitle]
,t.[PhoneNo] = s.[PhoneNo]
,t.[OfficeBuilding] = s.[OfficeBuilding]
,t.[OfficeFloor] = s.[OfficeFloor]
,t.[SecName] = s.[SecName]
,t.[OfficePIN] = s.[OfficePIN]
,t.[LoginID] = s.[LoginID]
,t.[LoginContext] = s.[LoginContext]
,t.[PowerStaffCode] = s.[PowerStaffCode]
,t.[PowerGroupCode] = s.[PowerGroupCode]
,t.[PowerGradeCode] = s.[PowerGradeCode]
,t.[TermFlag] = s.[TermFlag]
,t.[TermDate] = s.[TermDate]
,t.[Email] = s.[Email]
,t.[GUID] = s.[GUID]
,t.[LDAP_DistName] = s.[LDAP_DistName]
,t.[PwCEntityID] = s.[PwCEntityID]
,t.[localExpatFlag] = s.[localExpatFlag]
,t.[ExpatFlag] = s.[ExpatFlag]
,t.[INetEmail] = s.[INetEmail]
,t.[NotesID] = s.[NotesID]
,t.[BU] = s.[BU]
,t.[BUDesc] = s.[BUDesc]
,t.[SubService] = s.[SubService]
,t.[SubServiceDesc] = s.[SubServiceDesc]
,t.[JoinDate] = s.[JoinDate]
,t.[IsProfessional] = s.[IsProfessional]
,t.[JobCode] = s.[JobCode]
,t.[NativeName] = s.[NativeName]
,t.[FullName] = s.[FullName]
,t.[NameInEng] = s.[NameInEng]
,t.[GivenName] = s.[GivenName]
,t.[PreferredName] = s.[PreferredName]
,t.[TerritoryEffectiveDate] = s.[TerritoryEffectiveDate]
,t.[OfficeCityName] = s.[OfficeCityName]
,t.[HRID] = s.[HRID]
,t.[CountryOffice] = s.[CountryOffice]
,t.[OUCode] = s.[OUCode]
,t.[Status] = s.[Status]
,t.[ContractYearEnd] = s.[ContractYearEnd]
,t.[IDTerritory] = s.[IDTerritory]
,t.[PowerIDStatus] = s.[PowerIDStatus]
,t.[LoS] = s.[LoS]
,t.[Department] = s.[Department]
,t.[SalaryCategory] = s.[SalaryCategory]
,t.[JobGrade] = s.[JobGrade]
,t.[EmployeeCategory] = s.[EmployeeCategory]
,t.[Role] = s.[Role]
,t.[Sex] = s.[Sex]
,t.[SchoolUniversity] = s.[SchoolUniversity]
,t.[Majority] = s.[Majority]
,t.[MajorityCategory] = s.[MajorityCategory]
,t.[City] = s.[City]
,t.[ChineseName] = s.[ChineseName]
,t.[LoSDesc] = s.[LoSDesc]
when not matched
then insert values(s.StaffID
,s.CountryCode
,s.StaffInitial
,s.StaffName
,s.FirstName
,s.LastName
,s.DivCode
,s.DivName
,s.GroupCode
,s.GroupName
,s.GradeCode
,s.GradeName
,s.JobTitle
,s.PhoneNo
,s.OfficeBuilding
,s.OfficeFloor
,s.SecName
,s.OfficePIN
,s.LoginID
,s.LoginContext
,s.PowerStaffCode
,s.PowerGroupCode
,s.PowerGradeCode
,s.TermFlag
,s.TermDate
,s.Email
,s.[GUID]
,s.LDAP_DistName
,s.PwCEntityID
,s.localExpatFlag
,s.ExpatFlag
,s.INetEmail
,s.NotesID
,s.BU
,s.BUDesc
,s.SubService
,s.SubServiceDesc
,s.JoinDate
,s.IsProfessional
,s.JobCode
,s.NativeName
,s.FullName
,s.NameInEng
,s.GivenName
,s.PreferredName
,s.TerritoryEffectiveDate
,s.OfficeCityName
,s.HRID
,s.CountryOffice
,s.OUCode
,s.[Status]
,s.ContractYearEnd
,s.IDTerritory
,s.PowerIDStatus
,s.LoS
,s.Department
,s.SalaryCategory
,s.JobGrade
,s.EmployeeCategory
,s.[Role]
,s.Sex
,s.SchoolUniversity
,s.Majority
,s.MajorityCategory
,s.City
,s.ChineseName
,s.LoSDesc
,s.[Count]
,s.[Type])
when not matched by source
then delete
output $action as [Action],Inserted.StaffId as NewStaffId,deleted.StaffId as DeletedStaffId;

drop table #mergeStaffInfo




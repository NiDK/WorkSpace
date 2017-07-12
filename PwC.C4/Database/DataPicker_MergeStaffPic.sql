with sp as (select [StaffID],[Pic],[PicType]
from [dbo].[vwStaffMaster]
where staffname<>'' and staffname not like '%not a%')

select *  from sp

  merge into [dbo].[DataPicker_StaffPic] as t
  using #mergePic as s
  on t.StaffId = s.StaffId
  when matched
  then update set t.[Pic]=s.[Pic], t.[PicType]=s.[PicType]
  when not matched
then insert values(s.StaffID,s.Pic,s.PicType)
when not matched by source
then delete
output $action as [Action],Inserted.StaffId as NewStaffId,deleted.StaffId as DeletedStaffId;

drop table #mergePic
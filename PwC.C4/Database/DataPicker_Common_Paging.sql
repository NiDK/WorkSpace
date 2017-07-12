USE [PwCC4Base]
GO

/****** Object:  StoredProcedure [dbo].[Common_Paging]    Script Date: 2016/6/12 05:14:47 ******/
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



create procedure [dbo].[init]
    @spid int = null output,
    @version nvarchar(100) = null
AS
set nocount on
begin try
  declare @su sysname,
          @TableName sysname,
          @Sql nvarchar(1000),
          @DevRevision nvarchar(100)
  set @su = system_user

  -- Проверка версии клиента: требуется самая свежая из dbo.tRevisionLog
  select top 1 
         @DevRevision = DevRevision
    from dbo.tRevisionLog
   where EventType  = 'CHANGE_VERSION'
     and ObjectType = 'APPLICATION'
   order by ID desc

  if @version <> isnull(@DevRevision,'') or @version is null
    raiserror('Your version is not valid',16,1)

  set @spid = @@spid

end try
begin catch

  raiserror('Error',16,1)

end catch  

GO
CREATE TRIGGER Payments_DELETE
ON dbo.tPayments
AFTER Delete
AS
begin
declare @MoneyID int;
declare @OrderID int;
declare @DeletedID int;
declare @Summ numeric(10,2);
select @DeletedID=ID, @Summ=Summ FROM deleted;
select @MoneyID=MoneyID from dbo.tPayments WHERE Id = @DeletedID;
select @OrderID=OrderID from dbo.tPayments WHERE Id = @DeletedID;
UPDATE dbo.tOrders
SET SummPayed = SummPayed - @Summ
WHERE Id = @OrderID
UPDATE dbo.tMoney
SET SummRest = SummRest + @Summ
WHERE Id = @MoneyID
end
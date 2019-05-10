CREATE TRIGGER Payments_INSERT
ON dbo.tPayments
AFTER INSERT
AS
begin
declare @MoneyID int;
declare @OrderID int;
declare @InsertedID int;
declare @Summ numeric(10,2);
select @InsertedID=ID, @Summ=Summ FROM inserted;
select @MoneyID=MoneyID from dbo.tPayments WHERE Id = @InsertedID;
select @OrderID=OrderID from dbo.tPayments WHERE Id = @InsertedID;
UPDATE dbo.tOrders
SET SummPayed = SummPayed + @Summ
WHERE Id = @OrderID
UPDATE dbo.tMoney
SET SummRest = SummRest - @Summ
WHERE Id = @MoneyID
end
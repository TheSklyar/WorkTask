CREATE TRIGGER Payments_INSERT
ON dbo.tPaymentMO
AFTER INSERT
AS
begin
declare @MoneyID int;
declare @OrderID int;
declare @InsertedID int;
declare @Summ numeric(10,2);
select @InsertedID=ID, @Summ=Summ FROM inserted;
select @MoneyID=MoneyID from dbo.tPaymentMO WHERE Id = @InsertedID;
select @OrderID=OrderID from dbo.tPaymentMO WHERE Id = @InsertedID;
UPDATE dbo.tOrders
SET SummPayed = SummPayed + @Summ
WHERE Id = @OrderID
UPDATE dbo.tMoney
SET SummRest = SummRest - @Summ
WHERE Id = @MoneyID
UPDATE dbo.tPayments
SET Summ = Summ + @Summ
WHERE Id = @InsertedID
end
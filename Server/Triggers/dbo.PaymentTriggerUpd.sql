CREATE TRIGGER Payments_UPDATE
ON dbo.tPayments
AFTER UPDATE
AS
begin
declare @MoneyID int;
declare @OrderID int;
declare @UpdatedID int;
declare @Summ numeric(10,2);
select @UpdatedID=ID, @Summ=Summ FROM deleted;
select @MoneyID=MoneyID from dbo.tPayments WHERE Id = @UpdatedID;
select @OrderID=OrderID from dbo.tPayments WHERE Id = @UpdatedID;
UPDATE dbo.tOrders
SET SummPayed = SummPayed - @Summ
WHERE Id = @OrderID
UPDATE dbo.tMoney
SET SummRest = SummRest + @Summ
WHERE Id = @MoneyID
select @UpdatedID=ID, @Summ=Summ FROM inserted;
select @MoneyID=MoneyID from dbo.tPayments WHERE Id = @UpdatedID;
select @OrderID=OrderID from dbo.tPayments WHERE Id = @UpdatedID;
UPDATE dbo.tOrders
SET SummPayed = SummPayed + @Summ
WHERE Id = @OrderID
UPDATE dbo.tMoney
SET SummRest = SummRest - @Summ
WHERE Id = @MoneyID
end
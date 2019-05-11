create TRIGGER Payments_DELETE
ON dbo.tPaymentMO
AFTER Delete
AS
begin
UPDATE
    tor
SET
    tor.SummPayed =  tor.SummPayed-summa
FROM
    dbo.tOrders AS tor
    JOIN (select OrderID,  SUM(Summ) as summa from deleted group by OrderID ) AS ins
        ON tor.id = ins.OrderID
UPDATE
    tor
SET
    tor.SummRest =  tor.SummRest+summa
FROM
    dbo.tMoney AS tor
    JOIN (select MoneyID,  SUM(Summ) as summa from deleted group by MoneyID ) AS ins
        ON tor.id = ins.MoneyID
UPDATE
    tor
SET
    tor.Summ =  tor.Summ-ins.Summ
FROM
    dbo.tPayments AS tor
    JOIN deleted AS ins
        ON tor.id = ins.ID
end
create TRIGGER Payments_INSERT
ON dbo.tPaymentMO
AFTER INSERT
AS
begin
UPDATE
    tor
SET
    tor.SummPayed =  tor.SummPayed+ins.Summ
FROM
    dbo.tOrders AS tor
    INNER JOIN inserted AS ins
        ON tor.id = ins.OrderID
UPDATE
    tor
SET
    tor.SummRest =  tor.SummRest-ins.Summ
FROM
    dbo.tMoney AS tor
    INNER JOIN inserted AS ins
        ON tor.id = ins.MoneyID
UPDATE
    tor
SET
    tor.Summ =  tor.Summ+ins.Summ
FROM
    dbo.tPayments AS tor
    INNER JOIN inserted AS ins
        ON tor.id = ins.ID
end
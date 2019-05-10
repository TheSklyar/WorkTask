CREATE TABLE [dbo].[tPaymentMO](
	[ID] [int] not null,
	[OrderID] [int] NOT NULL,
	[MoneyID] [int] NOT NULL,
	[Summ] [numeric](10,2) NOT NULL);


CREATE INDEX ix_payordid ON [dbo].[tPaymentMO]([OrderID]);

CREATE INDEX ix_paymonid ON [dbo].[tPaymentMO]([MoneyID]);

CREATE INDEX ix_paypayid ON [dbo].[tPaymentMO]([ID]);

ALTER TABLE [dbo].[tPaymentMO]
	WITH CHECK ADD CONSTRAINT [tPaymentMO_OrderID_tOrders_ID] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[tOrders]([ID]) ON

UPDATE CASCADE;

ALTER TABLE [dbo].[tPaymentMO]
	WITH CHECK ADD CONSTRAINT [tPaymentMO_ID_tPayments_ID] FOREIGN KEY ([ID]) REFERENCES [dbo].[tPayments]([ID]) ON

UPDATE CASCADE;

ALTER TABLE [dbo].[tPaymentMO]
	WITH CHECK ADD CONSTRAINT [tPaymentMO_MoneyID_tMoney_ID] FOREIGN KEY ([MoneyID]) REFERENCES [dbo].[tMoney]([ID]) ON

UPDATE CASCADE;
CREATE TABLE [dbo].[tPayments](
	[ID] [int] IDENTITY(1,1) not null,
	[OrderID] [int] NOT NULL,
	[MoneyID] [int] NOT NULL,
	[Summ] [numeric](10,2) NOT NULL
 CONSTRAINT [PK_tPayments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE INDEX ix_payordid ON [dbo].[tPayments]([OrderID]);

CREATE INDEX ix_paymonid ON [dbo].[tPayments]([MoneyID]);

ALTER TABLE [dbo].[tPayments]
	WITH CHECK ADD CONSTRAINT [tPayments_OrderID_tOrders_ID] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[tOrders]([ID]) ON

UPDATE CASCADE;

ALTER TABLE [dbo].[tPayments]
	WITH CHECK ADD CONSTRAINT [tPayments_MoneyID_tMoney_ID] FOREIGN KEY ([MoneyID]) REFERENCES [dbo].[tMoney]([ID]) ON

UPDATE CASCADE;

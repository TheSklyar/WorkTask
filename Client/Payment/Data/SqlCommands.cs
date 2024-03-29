﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Data
{
    internal class SqlCommands
    {
        internal static string CountForPages = @"
	        SELECT count(*)
            FROM [dbo].[tOrders] tor
	        WHERE 1 = 1";

        internal static string GridPart1 = @"
            WITH OrderedRecords
            AS (
	            SELECT 
		            row_number() OVER (
			            ORDER BY ";

        internal static string GridByPagePart2 = @"
			            ) AS RowNumber,
		            tor.ID as 'ID',
					tor.[Summ] as 'Сумма'
	            FROM [dbo].[tPayments] tor
	            WHERE 1 = 1
	            )
            SELECT *
            FROM OrderedRecords
            WHERE RowNumber BETWEEN @RowStart
		            AND @RowEnd order by RowNumber";

        internal static string DeleteCard = @"delete from  [dbo].[tPaymentMO] where ID=@DelID;
delete from  [dbo].[tPayments] where ID=@DelID";
        internal static string SaveNew = @"insert into  [dbo].[tPayments] values (0); select @@identity";

        internal static string GetAllIDsByOrder = @"
            SELECT distinct ID FROM [dbo].[tPaymentMO] where OrderID=@ID
            ";

        internal static string GetAllIDsByMoney = @"
            SELECT distinct ID FROM [dbo].[tPaymentMO] where MoneyID=@ID
            ";

        internal static string GetMoneyByID = @"select [Summ]
      ,[SummRest] from dbo.tMoney where ID=@ID";

        internal static string GetOrderByID = @"select [Summ]
      ,[SummPayed] from dbo.tOrders where ID=@ID";

        internal static string GetAllDataByID = @"select [MoneyID],[OrderID]
      ,[Summ] from dbo.[tPaymentMO] where ID=@ID";

        internal static string SaveItem = @"
if(((select top 1 SummRest from dbo.tMoney where ID=@MID)<@Summ) or ((select top 1 (Summ-SummPayed) from dbo.tOrders where ID=@OID)<@Summ))
begin
select 0
end
else
begin
if((select count(*) from dbo.[tPaymentMO] where ID=@ID and OrderID=@OID and MoneyID=@MID and Summ=@Summ)=0)
begin
insert into dbo.[tPaymentMO] values (@ID, @OID, @MID, @Summ);
select 1
end
else
begin 
select 0
end
end
";
        internal static string DelItem = @"
delete from dbo.[tPaymentMO] where ID=@ID and OrderID=@OID and MoneyID=@MID and Summ=@Summ
";
        


    }
}

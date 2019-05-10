using System;
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

        internal static string DeleteCard = @"delete from  [dbo].[tPaymentMO] where ID=@DelID
delete from  [dbo].[tPayments] where ID=@DelID";
        internal static string SaveNew = @"insert into  [dbo].[tPayments] values (@Summ)";
        internal static string GetAllIDsByOrder = @"
            SELECT distinct ID FROM [dbo].[tPaymentMO] where OrderID=@ID
            ";

        internal static string GetAllIDsByMoney = @"
            SELECT distinct ID FROM [dbo].[tPaymentMO] where MoneyID=@ID
            ";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Data
{
    public static class SqlCommands
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
                    CONVERT(VARCHAR(10), tor.Date, 104) as 'Дата',
					tor.[Summ] AS 'Сумма',
					tor.[SummPayed] as 'Сумма оплаты'
	            FROM dbo.tOrders tor
	            WHERE 1 = 1
	            )
            SELECT *
            FROM OrderedRecords
            WHERE RowNumber BETWEEN @RowStart
		            AND @RowEnd order by RowNumber";

        internal static string DeleteCard = @"delete from  dbo.tOrders where ID=@DelID";
    }
}

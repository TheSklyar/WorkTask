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
        internal static string SelectOrderByID = @"select Date, Summ, SummPayed from dbo.tOrders where ID=@ID";
        internal static string Update = @"update dbo.tOrders set Date=@Date, Summ=@Summ, SummPayed=@SummPayed where ID=@ID";
        internal static string SaveNew = @"insert into dbo.tOrders values (@Date, @Summ, @SummPayed)";
    }
}

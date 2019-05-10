using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money.Data
{
    internal static class SqlCommands
    {
        internal static string CountForPages = @"
	        SELECT count(*)
            FROM [dbo].[tMoney] tor
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
					tor.[SummRest] as 'Остаток'
	            FROM dbo.tMoney tor
	            WHERE 1 = 1
	            )
            SELECT *
            FROM OrderedRecords
            WHERE RowNumber BETWEEN @RowStart
		            AND @RowEnd order by RowNumber";

        internal static string DeleteCard = @"delete from  dbo.tMoney where ID=@DelID";
        internal static string SelectOrderByID = @"select Date, Summ, SummRest from dbo.tMoney where ID=@ID";
        internal static string Update = @"update dbo.tMoney set Date=@Date, Summ=@Summ, SummRest=@SummPayed where ID=@ID";
        internal static string SaveNew = @"insert into dbo.tMoney values (@Date, @Summ, @SummPayed)";
    }
}

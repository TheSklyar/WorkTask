using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DB
{
    public static class DBHelper
    {
        public static string CountTotalPagesByFilter(string BaseCommand, Dictionary<int, string> filters)
        {
            string filter = "";
            if ((filters.Count > 0))
            {
                foreach (var item in filters)
                {
                    if (item.Value != "")
                    {
                        filter += " and " + item.Value;
                    }
                }
            }
            return BaseCommand + filter;
        }

        public static string FormSqlGrid(int columnNumber, ListSortDirection? direction,
            Dictionary<int, string> filters, Dictionary<int, string> sortsource, string gridPart1, string gridPart2)
        {
            string result = gridPart1;
            result += sortsource[columnNumber];
            if (direction.HasValue)
            {
                if (direction == ListSortDirection.Ascending)
                {
                    result += " asc ";
                }
                else
                {
                    result += " desc ";
                }
            }
            else
            {
                result += " asc ";
            }
            result += gridPart2;
            string filter = "";
            foreach (var item in filters)
            {
                if (item.Value != "")
                {
                    filter += @"
                                    and " + item.Value;
                }
            }
            result = result.Replace("1 = 1", "1 = 1" + filter);
            return result;
        }
    }
}

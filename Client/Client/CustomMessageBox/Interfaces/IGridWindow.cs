using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helpers.Interfaces
{
    public interface IGridWindow
    {
        void InitTable();
        void Row_DoubleClick(object sender, MouseButtonEventArgs e);
        void UpdateGrid();
        void CountPages();
        void UpdatePageCount();
        void SetFiltersToNull();
        void Filter_Click(object sender, RoutedEventArgs e);
        void Grid_Sort(object sender, DataGridSortingEventArgs e);
        void PrevPage_Click(object sender, RoutedEventArgs e);
        void NextPage_Click(object sender, RoutedEventArgs e);
    }
}

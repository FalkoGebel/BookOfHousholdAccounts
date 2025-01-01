using BohaLibrary;
using BohaWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BohaWpf.Views
{
    /// <summary>
    /// Interaktionslogik für EditCategoriesView.xaml
    /// </summary>
    public partial class EditCategoriesView : Window
    {
        private readonly BookOfHouseholdAccounts _book;

        public EditCategoriesView(BookOfHouseholdAccounts book)
        {
            _book = book;

            InitializeComponent();

            DataContext = new EditCategoriesViewModel(_book);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (CategoriesListView.Items.Count == 0)
                return;

            if (CategoriesListView.ItemContainerGenerator.ContainerFromIndex(0) is not ListViewItem item)
                return;

            item.Focus();

        }
    }
}

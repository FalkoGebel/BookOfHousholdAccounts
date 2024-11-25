using BohaLibrary;
using BohaWpf.ViewModels;
using System.Windows;

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
    }
}

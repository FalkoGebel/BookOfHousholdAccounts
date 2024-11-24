using BohaLibrary;
using BohaWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private BookOfHouseholdAccounts? _book;
        private readonly string _pathFiles = Application.Current.Properties["PathFiles"] as string ?? throw new ArgumentNullException(nameof(_pathFiles));

        [ObservableProperty]
        private string bookName = string.Empty;

        [ObservableProperty]
        private List<string> entryTypes = [Properties.Literals.MainView_EntryTypes_Deposit, Properties.Literals.MainView_EntryTypes_Payout];

        [ObservableProperty]
        private string choosenEntryType = Properties.Literals.MainView_EntryTypes_Deposit;

        [ObservableProperty]
        private List<string> categories = [];

        [RelayCommand]
        private void ChooseBook()
        {
            var chooseBookView = new ChooseBookView();
            chooseBookView.ShowDialog();
            BookName = ((ChooseBookViewModel)chooseBookView.DataContext).ChoosenBook;
            _book = new BookOfHouseholdAccounts(_pathFiles, BookName);
            LoadAndUpdateCategories();
        }

        [RelayCommand]
        private void EditCategories()
        {
            // TODO - implement EditCategories

            MessageBox.Show("Implement EditCategories");
        }

        private void LoadAndUpdateCategories()
        {
            if (_book == null)
                return;

            _book.LoadFromFile();
            Categories = _book.Categories.Where(c => c.EntryType == (ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit ? BookEntryType.Deposit : BookEntryType.Payout))
                                         .Select(c => c.Name)
                                         .ToList();
        }
    }
}
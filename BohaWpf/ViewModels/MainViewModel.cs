using BohaLibrary;
using BohaWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // TODO - open last opened book again, if existing

        private BookOfHouseholdAccounts? _book;
        private readonly string _pathFiles = Application.Current.Properties["PathFiles"] as string ?? throw new ArgumentNullException(nameof(_pathFiles));
        private decimal _amount;

        [ObservableProperty]
        private string bookName = Properties.Literals.MainView_CurrentBookTextBlock_Placeholder;

        [ObservableProperty]
        private List<string> entryTypes = [Properties.Literals.MainView_EntryTypes_Deposit, Properties.Literals.MainView_EntryTypes_Payout];

        [ObservableProperty]
        private string choosenEntryType = Properties.Literals.MainView_EntryTypes_Deposit;

        [ObservableProperty]
        private List<string> categories = [];

        [ObservableProperty]
        private DateTime? _postingDate = DateTime.Now;

        [ObservableProperty]
        private string choosenCategory = string.Empty;

        [ObservableProperty]
        private string memoText = string.Empty;

        [ObservableProperty]
        private string amountInput = string.Empty;

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
            if (_book == null)
                return;

            var editCategoriesView = new EditCategoriesView(_book);
            editCategoriesView.ShowDialog();
            LoadAndUpdateCategories();
        }

        [RelayCommand]
        private void CreateEntry()
        {
            if (_book == null ||
                string.IsNullOrEmpty(ChoosenCategory) ||
                string.IsNullOrEmpty(AmountInput) ||
                PostingDate == null)
                return;

            DateTime today = (DateTime)PostingDate;
            today = new DateTime(today.Year, today.Month, today.Day);

            if (ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit)
                _book.AddDepositBookEntry(ChoosenCategory, _amount, MemoText, today);
            else
                _book.AddPayoutBookEntry(ChoosenCategory, _amount, MemoText, today);
            _book.SaveToFile();
        }

        partial void OnAmountInputChanged(string? oldValue, string newValue)
        {
            if (oldValue == null)
                return;

            if (decimal.TryParse(newValue, out var decimalValue))
                _amount = decimalValue;
            else
                amountInput = oldValue;
        }

        partial void OnChoosenEntryTypeChanged(string? oldValue, string newValue)
        {
            if (oldValue != newValue)
                LoadAndUpdateCategories();
        }

        private void LoadAndUpdateCategories()
        {
            if (_book == null)
                return;

            _book.LoadFromFile();
            Categories = _book.Categories.Where(c => c.EntryType == (ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit
                                                                       ? BookEntryType.Deposit
                                                                       : BookEntryType.Payout))
                                         .Select(c => c.Name)
                                         .ToList();
        }
    }
}
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
        private decimal _amount;

        public MainViewModel()
        {
            BooksOfHouseholdAccounts books = new();
            books.LoadFromFile(_pathFiles);
            if (books.LastBookName == string.Empty)
            {
                if (books.Names.Count != 0)
                    books.LastBookName = books.Names[0];
                else
                    books.AddBook(Properties.Literals.MainView_DefaultBookName);
            }
            SetBook(books.LastBookName);
            AmountInput = "0";
        }

        public bool CreateEntryButtonIsEnabled
        {
            get
            {
                if (_book == null ||
                    string.IsNullOrEmpty(ChoosenCategory) ||
                    string.IsNullOrEmpty(AmountInput) ||
                    _amount == 0 ||
                    PostingDate == null)
                    return false;

                return true;
            }
        }

        public string CurrentMonth
        {
            get
            {
                return CurrentDate.ToString("MMMM yyyy");
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CreateEntryButtonIsEnabled))]
        private string _bookName = string.Empty;

        [ObservableProperty]
        private List<string> _entryTypes = [Properties.Literals.MainView_EntryTypes_Deposit, Properties.Literals.MainView_EntryTypes_Payout];

        [ObservableProperty]
        private string _choosenEntryType = Properties.Literals.MainView_EntryTypes_Deposit;

        [ObservableProperty]
        private List<string> _categories = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CreateEntryButtonIsEnabled))]
        private DateTime? _postingDate = DateTime.Now;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CreateEntryButtonIsEnabled))]
        private string _choosenCategory = string.Empty;

        [ObservableProperty]
        private string _memoText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CreateEntryButtonIsEnabled))]
        private string _amountInput = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentMonth))]
        private DateTime _currentDate = DateTime.Now;

        [RelayCommand]
        private void ChooseBook()
        {
            var chooseBookView = new ChooseBookView();
            chooseBookView.ShowDialog();
            string bookName = ((ChooseBookViewModel)chooseBookView.DataContext).ChoosenBook;
            if (!string.IsNullOrEmpty(bookName))
                SetBook(bookName);
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
                _amount == 0 ||
                PostingDate == null)
                return;

            DateTime today = (DateTime)PostingDate;
            today = new DateTime(today.Year, today.Month, today.Day);

            if (ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit)
                _book.AddDepositBookEntry(ChoosenCategory, _amount, MemoText, today);
            else
                _book.AddPayoutBookEntry(ChoosenCategory, _amount, MemoText, today);
            _book.SaveToFile();

            AmountInput = "0";
            MemoText = string.Empty;
        }

        [RelayCommand]
        private void ChooseMonth()
        {
            var chooseMonthView = new ChooseMonthView(CurrentDate);
            //var chooseMonthView = new ChooseMonthView();
            chooseMonthView.ShowDialog();
            DateTime? choosenDate = ((ChooseMonthViewModel)chooseMonthView.DataContext).ChoosenMonth;

            if (choosenDate != null)
                CurrentDate = choosenDate.Value;
        }

        partial void OnAmountInputChanged(string? oldValue, string newValue)
        {
            if (oldValue == null)
                return;

            if (decimal.TryParse(newValue, out var decimalValue))
                _amount = decimalValue;
            else
                _amountInput = oldValue;
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

        private void SetBook(string bookName)
        {
            BookName = bookName;

            if (BookName != string.Empty)
            {
                _book = new BookOfHouseholdAccounts(_pathFiles, BookName);
                LoadAndUpdateCategories();
            }
        }
    }
}
using BohaLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class ChooseBookViewModel : ObservableObject
    {
        private readonly BooksOfHouseholdAccounts _books;
        private readonly string _pathFiles = Application.Current.Properties["PathFiles"] as string ?? throw new ArgumentNullException(nameof(_pathFiles));

        public string ChoosenBook { get; private set; } = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string>? _names;

        [ObservableProperty]
        private string _selectedBookName = string.Empty;

        [ObservableProperty]
        private string _newBookName = string.Empty;

        public ChooseBookViewModel()
        {
            _books = new BooksOfHouseholdAccounts();
            LoadBooksAndUpdateNames();
        }

        [RelayCommand]
        private void AddBook()
        {
            try
            {
                _books.AddBook(NewBookName);
                _books.SaveToFile(_pathFiles);
                LoadBooksAndUpdateNames();
                NewBookName = string.Empty;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
            }
        }

        [RelayCommand]
        private void ChooseBook(Window window)
        {
            ChoosenBook = SelectedBookName;
            window.Close();
        }

        [RelayCommand]
        private void DeleteBook()
        {
            if (SelectedBookName == null)
                return;

            if (MessageBox.Show(Properties.Literals.ChoosBookView_DeleteConfirmText.Replace("<BOOKNAME>", SelectedBookName),
                                Properties.Literals.ConfirmWindowTitle,
                                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            try
            {
                _books.DeleteBook(SelectedBookName);
                _books.SaveToFile(_pathFiles);
                LoadBooksAndUpdateNames();

                BookOfHouseholdAccounts book = new(_pathFiles, SelectedBookName);
                book.DeleteFile();
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
            }
        }

        private void LoadBooksAndUpdateNames()
        {
            _books.LoadFromFile(_pathFiles);
            Names = new ObservableCollection<string>(_books.Names);
        }
    }
}
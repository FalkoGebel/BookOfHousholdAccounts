using BohaLibrary;
using BohaWpf.Views;
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

        public ChooseBookViewModel()
        {
            _books = new BooksOfHouseholdAccounts();
            LoadBooksAndUpdateNames();
        }

        [ObservableProperty]
        private ObservableCollection<string>? _names;

        [ObservableProperty]
        private string _selectedBookName = string.Empty;

        [ObservableProperty]
        private string _newBookName = string.Empty;

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
            _books.LastBookName = SelectedBookName;
            _books.SaveToFile(_pathFiles);
            window.Close();
        }

        [RelayCommand]
        private void DeleteBook()
        {
            if (SelectedBookName == null)
                return;

            var confirmView = new ConfirmView(Properties.Literals.ChooseBookView_DeleteConfirmText.Replace("<BOOKNAME>", SelectedBookName));
            confirmView.ShowDialog();

            if (((ConfirmViewModel)confirmView.DataContext).Confirmed == false)
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
            Names = [.. _books.Names];
        }
    }
}
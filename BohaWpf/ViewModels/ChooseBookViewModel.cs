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

        [ObservableProperty]
        private ObservableCollection<string>? _names;

        public ChooseBookViewModel()
        {
            _books = new BooksOfHouseholdAccounts();
            LoadBooksAndUpdateNames();
        }

        [RelayCommand]
        private void AddBook()
        {
            if (Names == null)
                throw new ArgumentNullException(nameof(Names));

            try
            {
                // TODO - get book from input dialog

                _books.AddBook("book from AddBook");
                _books.SaveToFile(_pathFiles);
                LoadBooksAndUpdateNames();
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BohaWpf.ViewModels
{
    public partial class ChooseBookViewModel : ObservableObject
    {
        //private ChooseBookView _chooseBookView;

        [ObservableProperty]
        private ObservableCollection<string>? _names = ["book1", "book2", "third book"];

        public ChooseBookViewModel()
        {
            // TODO - load books from file

            Names.Add("book from constructor");
        }

        [RelayCommand]
        private void AddBook()
        {
            // TODO - add new book to list

            Names.Add("book from AddBook");
        }
    }
}
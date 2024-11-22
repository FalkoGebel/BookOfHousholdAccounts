using BohaWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BohaWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = string.Empty;

        [RelayCommand]
        private void ChooseBook()
        {
            var chooseBookView = new ChooseBookView();
            chooseBookView.ShowDialog();
            Name = ((ChooseBookViewModel)chooseBookView.DataContext).ChoosenBook;
        }
    }
}
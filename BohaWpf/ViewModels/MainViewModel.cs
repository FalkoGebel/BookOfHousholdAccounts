using BohaWpf.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BohaWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? name;

        [RelayCommand]
        private void ChooseBook()
        {
            var chooseBookView = new ChooseBookView();
            chooseBookView.ShowDialog();
            //var chooseBookViewModel = new ChooseBookViewModel(["book from MainViewModel"]);
            ////chooseBookViewModel.InitView();
            ////chooseBookViewModel.LoadBooks();
            //chooseBookViewModel.ShowView();
        }
    }
}
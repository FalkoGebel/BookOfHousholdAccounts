using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class ConfirmViewModel : ObservableObject
    {
        public bool Confirmed { get; private set; }

        public ConfirmViewModel(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            _title = Properties.Literals.ConfirmView_Title;
            _message = message;
        }

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        [RelayCommand]
        private void Yes(Window window)
        {
            Confirmed = true;
            window.Close();
        }

        [RelayCommand]
        private void No(Window window)
        {
            window.Close();
        }
    }
}

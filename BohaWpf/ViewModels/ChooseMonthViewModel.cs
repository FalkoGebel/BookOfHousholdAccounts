using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class ChooseMonthViewModel : ObservableObject
    {
        private readonly DateTime _currentMonth;

        public DateTime? ChoosenMonth { get; private set; }

        public ChooseMonthViewModel(DateTime currentMonth)
        {
            _currentMonth = currentMonth;
            SetCurrentMonth();
        }

        [ObservableProperty]
        private DateTime _currentDate;

        [RelayCommand]
        private void Ok(Window window)
        {
            ChoosenMonth = CurrentDate;
            window.Close();
        }

        private void SetCurrentMonth()
        {
            CurrentDate = _currentMonth;
        }
    }
}

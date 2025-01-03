using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace BohaWpf.ViewModels
{
    public partial class ChooseMonthViewModel : ObservableObject
    {
        private DateTime _currentMonth;

        public DateTime? ChoosenMonth { get; private set; }

        public ChooseMonthViewModel(DateTime currentMonth)
        {
            _currentMonth = currentMonth;
            SetCurrentMonth();
        }

        [ObservableProperty]
        private DateTime _selectedDate;

        [RelayCommand]
        private void Ok(Window window)
        {
            ChoosenMonth = SelectedDate;
            window.Close();
        }

        private void SetCurrentMonth()
        {
            SelectedDate = _currentMonth;
        }
    }
}

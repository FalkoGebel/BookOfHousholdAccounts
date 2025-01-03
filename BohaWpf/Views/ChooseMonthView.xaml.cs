using BohaWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BohaWpf.Views
{
    /// <summary>
    /// Interaktionslogik für ChooseMonthView.xaml
    /// </summary>
    public partial class ChooseMonthView : Window
    {
        public ChooseMonthView(DateTime currentDate)
        {
            InitializeComponent();

            DataContext = new ChooseMonthViewModel(currentDate);
        }

        private void Calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            CurrentMonthCalendar.DisplayMode = CalendarMode.Year;
        }
    }
}

using BohaWpf.ViewModels;
using System.Windows;

namespace BohaWpf.Views
{
    /// <summary>
    /// Interaktionslogik für ConfirmView.xaml
    /// </summary>
    public partial class ConfirmView : Window
    {
        private readonly string _message;

        public ConfirmView(string message)
        {
            _message = message;

            InitializeComponent();

            DataContext = new ConfirmViewModel(_message);
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace BohaWpf.Views
{
    /// <summary>
    /// Interaction logic for ChooseBookView.xaml
    /// </summary>
    public partial class ChooseBookView : Window
    {
        public ChooseBookView()
        {
            InitializeComponent();
        }

        private void ChoosBookWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (BookNamesListView.Items.Count == 0)
                return;

            if (BookNamesListView.ItemContainerGenerator.ContainerFromIndex(0) is not ListViewItem item)
                return;

            item.Focus();
        }
    }
}

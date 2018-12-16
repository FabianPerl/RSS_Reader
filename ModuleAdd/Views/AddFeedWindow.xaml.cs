using System.Windows;
using System.Windows.Controls;
using Infrastructure.Models;

namespace ModuleAdd.Views
{
    /// <summary>
    /// Interaction logic for AddFeedFormUserControl
    /// </summary>
    public partial class AddFeedWindow : Window
    {
        public AddFeedWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

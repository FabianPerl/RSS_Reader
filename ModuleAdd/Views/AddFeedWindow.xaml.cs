using System.Windows;

namespace ModuleAdd.Views
{
    /// <summary>
    /// Interaction logic for AddFeedFormUserControl
    /// </summary>
    public partial class AddFeedWindow
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

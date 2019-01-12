using System.Windows;

namespace ModuleAdd.Views
{
    /// <summary>
    /// Interaction logic for AddSource FormUserControl
    /// </summary>
    public partial class AddSourceWindow
    {
        public AddSourceWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

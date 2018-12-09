using System.Windows;

namespace ModuleAdd.Views
{
    /// <summary>
    /// Interaction logic for AddFeedWindow.xaml
    /// </summary>
    public partial class AddFeedWindow : Window
    {
        public AddFeedWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

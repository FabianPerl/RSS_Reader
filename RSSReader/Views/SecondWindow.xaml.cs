using System.Windows;

namespace ModuleAdd.Views
{
    /// <summary>
    /// Interaction logic for AddFeedWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

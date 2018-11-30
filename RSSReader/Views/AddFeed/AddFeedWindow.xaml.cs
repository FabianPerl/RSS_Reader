using System.Windows;

namespace RSSReader.Views.AddFeed
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

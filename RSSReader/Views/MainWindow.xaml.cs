using System.Windows;
using System.Windows.Input;

namespace RSSReader.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void UIElement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AddFeedWindow addFeed = new AddFeedWindow();
            addFeed.Show();
        }
    }
}

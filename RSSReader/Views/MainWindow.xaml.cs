using System.Windows;
using System.Windows.Input;
using ModuleAdd.Views;

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
            new AddFeedWindow().Show();
        }
    }
}

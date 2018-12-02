using System;
using System.Windows;
using System.Windows.Input;
using RSSReader.ViewModels;
using RSSReader.Views.AddFeed;

namespace RSSReader.Views.Shell
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

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

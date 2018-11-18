using System;
using System.Windows.Controls;
using RSSReader.ViewModels;

namespace RSSReader.Views.Shell
{
    /// <summary>
    /// Interaction logic for FeedBoxUserControl
    /// </summary>
    public partial class FeedBoxUserControl : UserControl
    {
        public FeedBoxUserControl()
        {
            InitializeComponent();
            DataContext = new FeedBoxUserControlViewModel();
        }
    }
}

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
        private Source _sourceToChange;

        public AddFeedWindow()
        {
            InitializeComponent();
        }

        public AddFeedWindow(Source source)
        {
            _sourceToChange = source;
            InitializeComponent();
        }
    }
}

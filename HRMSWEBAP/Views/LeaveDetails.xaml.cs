using HRMSWEBAP.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HRMSWEBAP.Views
{
    public partial class LeaveDetails : ChildWindow
    {
        UserWindowViewModel viewmodel;
        public LeaveDetails()
        {
            InitializeComponent();
            viewmodel = new UserWindowViewModel(this);
            DataContext = viewmodel;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}


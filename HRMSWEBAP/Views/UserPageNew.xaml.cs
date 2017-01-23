using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace HRMSWEBAP.Views
{
    public partial class UserPageNew : Page
    {
        public UserPageNew()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cnfrm_pwd_panel.Visibility = Visibility.Visible;
            contentlabel.Visibility = Visibility.Collapsed;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void changeusrphoto_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

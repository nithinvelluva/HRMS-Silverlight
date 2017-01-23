using HRMSWEBAP.Cookies;
using HRMSWEBAP.HrmsReference;
using HRMSWEBAP.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace HRMSWEBAP.Views
{
    public partial class Login : Page
    {
        public LoginViewModel ViewModel;
        HrmsServiceClient svc = new HrmsServiceClient();
        ObservableCollection<UserInfo> uin = new ObservableCollection<UserInfo>();
        public bool rememberusr = false;

        public Login()
        {
            string cookieee = BrowserCookies.GetCookie("CECrd");
            // string cookieee = "CECrd=13;CECrd=2;expires=Thu, 25 Feb 2016 11:17:39 GMT";
            if (cookieee != null && cookieee != "")
            {
                try
                {
                    string[] uidtype = cookieee.Split(';');
                    //string temp = uidtype[1].Split('=')[1];
                    int mytype = Convert.ToInt32(uidtype[1]);
                    switch (mytype)
                    {
                        case 1:
                            {
                                var uri = new Uri("/AdminPage", UriKind.RelativeOrAbsolute);
                                NavigationService.Navigate(uri);
                                break;
                            }
                        case 2:
                            {

                                var uri = new Uri("/UserPage", UriKind.RelativeOrAbsolute);
                                NavigationService.Navigate(uri);
                                break;
                            }
                        default: break;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            else
            {
                InitializeComponent();
                HidePassword();
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void signin_Click(object sender, RoutedEventArgs e)
        {
            busyshow.IsBusy = true;
            string username = Username.Text;
            string password = Pwdtext.Password;

            Err_msg.Text = "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password))
            {
                Err_msg.Text = "Enter username & password!";
            }

            else
            {
                try
                {
                    svc.ValidateUserCompleted -= new EventHandler<ValidateUserCompletedEventArgs>(ValidateUserResult);
                    svc.ValidateUserCompleted += new EventHandler<ValidateUserCompletedEventArgs>(ValidateUserResult);

                    ObservableCollection<UserInfo> logdetails = new ObservableCollection<UserInfo>();
                    UserInfo ui = new UserInfo();

                    ui.UserName = username;
                    ui.Pwd = password;
                    ui.CurrentDate = DateTime.Now.ToString();
                    logdetails.Add(ui);

                    svc.ValidateUserAsync(logdetails);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }

            }
        }

        private void ValidateUserResult(object sender, ValidateUserCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    busyshow.IsBusy = false;
                    //waiticon.Visibility= Visibility.Collapsed;
                    uin = e.Result;

                    if (uin[0].Flag)
                    {
                        Helpers.UserLoginStatus.currentEmpId = uin[0].EmpID;
                        Helpers.UserLoginStatus.currentName = uin[0].Name;
                        Helpers.UserLoginStatus.currentUserName = uin[0].UserName;
                        Helpers.UserLoginStatus.currentPassword = uin[0].Pwd;
                        Helpers.UserLoginStatus.currentEmail = uin[0].Mail;
                        Helpers.UserLoginStatus.currentPhone = uin[0].Number;
                        Helpers.UserLoginStatus.currentUserType = uin[0].Utype;
                        Helpers.UserLoginStatus.currentUserDob = Convert.ToDateTime(uin[0].Dob);
                        Helpers.UserLoginStatus.currentUserImgData = uin[0].ImgData;
                        Helpers.UserLoginStatus.PunchinTime = uin[0].PunchinTime;
                        Helpers.UserLoginStatus.PunchoutTime = uin[0].PunchoutTime;
                        Helpers.UserLoginStatus.currentGender = uin[0].Gender;
                        Helpers.UserLoginStatus.CurrentEmpPhotoPath = uin[0].UserPhotoPath;

                        switch (uin[0].Utype)
                        {
                            case 1:
                                {
                                    var uri = new Uri("/AdminPage", UriKind.Relative);
                                    NavigationService.Navigate(uri);
                                    break;
                                }
                            case 2:
                                {
                                    CheckYearEnding();

                                    var uri = new Uri("/UserPage", UriKind.Relative);
                                    NavigationService.Navigate(uri);
                                    break;
                                }
                            default:
                                {
                                    Err_msg.Text = "*Invalid Entry!";
                                    break;
                                }
                        }

                        App ap = (App)Application.Current;
                        ap.myEId = uin[0].EmpID;
                        MainPage main = (MainPage)Application.Current.RootVisual;
                        main.lgnbtn.Visibility = System.Windows.Visibility.Collapsed;
                        main.lgoutbtn.Visibility = System.Windows.Visibility.Visible;
                        main.homebtn.Visibility = Visibility.Collapsed;

                        //if (rememberusr)
                        //{
                        //    svc.testCoockieCompleted -= new EventHandler<testCoockieCompletedEventArgs>(asc_testCoockieCompleted);
                        //    svc.testCoockieCompleted += new EventHandler<testCoockieCompletedEventArgs>(asc_testCoockieCompleted);
                        //    svc.testCoockieAsync(uin[0].EmpID.ToString());
                        //}
                    }
                    else
                    {
                        Err_msg.Text = "Invalid Entry!";
                        busyshow.IsBusy = false;
                        //waiticon.Visibility = Visibility.Collapsed;
                    }
                }
            }

            catch (Exception ex)
            {

            }

            finally { }
        }

        //private void asc_testCoockieCompleted(object sender, testCoockieCompletedEventArgs e)
        //{
        //    if (e.Result)
        //    {
        //        BrowserCookies.SetCookie("CECrd", uin[0].EmpID.ToString(), uin[0].Utype.ToString());
        //    }
        //}

        private void CheckYearEnding()
        {
            try
            {
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int day = DateTime.Now.DayOfYear;
                //   bool? isLeap;

                if (month == 12)
                {
                    if (day == 366 || day == 365)
                    {
                        MessageBox.Show("Last day of the year");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Username.Text = "";
            Pwdtext.Password = "";
            Err_msg.Text = "";
        }

        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        void ShowPassword()
        {
            //ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\cus.jpg"));
            txtVisiblePasswordbox.Visibility = Visibility.Visible;
            Pwdtext.Visibility = Visibility.Collapsed;
            txtVisiblePasswordbox.Text = Pwdtext.Password;
        }
        void HidePassword()
        {
            //ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\clip.jpg"));
            txtVisiblePasswordbox.Visibility = Visibility.Collapsed;
            Pwdtext.Visibility = Visibility.Visible;
            Pwdtext.Focus();
        }

        private void keepmechkbx_Checked(object sender, RoutedEventArgs e)
        {
            rememberusr = true;
        }      

        private void Pwdtext_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Pwdtext.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
            }
            else
            {
                ImgShowHide.Visibility = Visibility.Collapsed;
            }
        }
    }
}

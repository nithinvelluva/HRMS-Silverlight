using Apex.MVVM;
using HRMSWEBAP.HrmsReference;
using HRMSWEBAP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace HRMSWEBAP.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Login loginwindow;        
        public event PropertyChangedEventHandler PropertyChanged;

        public bool init = false;
        ObservableCollection<UserInfo> uinfo = new ObservableCollection<UserInfo>();

        HrmsServiceClient svc = new HrmsServiceClient();

        public LoginViewModel(Login loginWin)
        {
            loginwindow = loginWin;
            _signInCommand = new Command(ExecuteSignIn);
            _cancelCommand = new Command(ExecuteCancel);

            svc.ValidateUserCompleted -= new EventHandler<ValidateUserCompletedEventArgs>(ValidateUserResult);
            svc.ValidateUserCompleted += new EventHandler<ValidateUserCompletedEventArgs>(ValidateUserResult);

            //svc.GetEmpPunchDetailsCompleted -= new EventHandler<HrmsReference.GetEmpPunchDetailsCompletedEventArgs>(EmpPunchResult);
            //svc.GetEmpPunchDetailsCompleted += new EventHandler<HrmsReference.GetEmpPunchDetailsCompletedEventArgs>(EmpPunchResult);

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(EmpLeaveDetailsResult);
            svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(EmpLeaveDetailsResult);

            svc.GetEmployeeAttendanceDetailsCompleted -= new EventHandler<GetEmployeeAttendanceDetailsCompletedEventArgs>(EmpAttDataResult);
            svc.GetEmployeeAttendanceDetailsCompleted += new EventHandler<GetEmployeeAttendanceDetailsCompletedEventArgs>(EmpAttDataResult);


            svc.GetLeaveStatisticsCompleted -= new EventHandler<HrmsReference.GetLeaveStatisticsCompletedEventArgs>(LeaveStatisticsResult);
            svc.GetLeaveStatisticsCompleted += new EventHandler<HrmsReference.GetLeaveStatisticsCompletedEventArgs>(LeaveStatisticsResult);

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(EmpLeaveDetailsIndiResult);
            svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(EmpLeaveDetailsIndiResult);

        }

        private void EmpLeaveDetailsIndiResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();
        }

        private void LeaveStatisticsResult(object sender, GetLeaveStatisticsCompletedEventArgs e)
        {
            Helpers.EmpDetails.CurrEmpLeaveStatisticsList = e.Result.ToList();
        }

        private void EmpAttDataResult(object sender, GetEmployeeAttendanceDetailsCompletedEventArgs e)
        {
            UserInfo ui = new UserInfo();
            ui = e.Result;
            List<UserInfo> uinfo = new List<UserInfo>();
            uinfo.Add(ui);
            Helpers.EmpDetails.CurrEmpAttendanceDataList = uinfo;
        }

        private void EmpLeaveDetailsResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();
        }

        //private void EmpPunchResult(object sender, GetEmpPunchDetailsCompletedEventArgs e)
        //{
        //    Helpers.UserLoginStatus.EmpDetailsList = e.Result.ToList();
        //}

        private void ValidateUserResult(object sender, ValidateUserCompletedEventArgs e)
        {
            uinfo = e.Result;

            if (uinfo[0].Flag == true)
            {
                HRMSWEBAP.Models.UserInfo userinfo = new Models.UserInfo();
                Helpers.UserLoginStatus.currentEmpId = uinfo[0].EmpID;
                Helpers.UserLoginStatus.currentName = uinfo[0].Name;
                Helpers.UserLoginStatus.currentUserName = uinfo[0].UserName;
                Helpers.UserLoginStatus.currentPassword = uinfo[0].Pwd;
                Helpers.UserLoginStatus.currentEmail = uinfo[0].Mail;
                Helpers.UserLoginStatus.currentPhone = uinfo[0].Number;
                Helpers.UserLoginStatus.currentUserType = uinfo[0].Utype;
                Helpers.UserLoginStatus.currentUserDob = Convert.ToDateTime(uinfo[0].Dob);

                Helpers.UserLoginStatus.PunchinTime = uinfo[0].PunchinTime;
                Helpers.UserLoginStatus.PunchoutTime = uinfo[0].PunchoutTime;

                if (uinfo[0].Utype == 1)
                {
                    //svc.GetEmpPunchDetailsAsync();
                    //svc.GetEmployeeLeaveDetailsAsync(uinfo[0].EmpID, uinfo[0].Utype, null);
                }
                else
                {
                    svc.GetEmployeeAttendanceDetailsAsync(uinfo[0].EmpID);
                    svc.GetLeaveStatisticsAsync(uinfo[0].EmpID);

                    var startDate = Helpers.SupportMethods.GetFirstDayOfMonth(DateTime.Now.Month, DateTime.Now.Year);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    LeaveInfo li = new LeaveInfo();
                    li.FromDate = startDate.ToString();
                    li.ToDate = endDate.ToString();

                    svc.GetEmployeeLeaveDetailsAsync(uinfo[0].EmpID, uinfo[0].Utype, li,false);

                    Helpers.LeaveInfo.LeaveTypes = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].LeaveTypes.ToList();
                    Helpers.LeaveInfo.CasualLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].CasualLeave;
                    Helpers.LeaveInfo.FestiveLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].FestiveLeave;
                    Helpers.LeaveInfo.SickLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].SickLeave;
                }
            }
            else
            {
                Err_msg = "Invaid username or password!";

            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }      

        private string _err_msg;
        public string Err_msg
        {
            get { return _err_msg; }
            set
            {
                _err_msg = value;
                RaisePropertyChanged("Err_msg");
            }
        }

        private Command _signInCommand;
        public Command SignInCommand
        {
            get
            {
                return _signInCommand;
            }
        }

        private Command _cancelCommand;
        public Command CancelCommand
        {
            get { return _cancelCommand; }
        }  

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }

        private void ExecuteSignIn()
        {           
            Err_msg = "";

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                Err_msg = "Enter username & password!";
            }

            else
            {
                try
                {
                    ObservableCollection<UserInfo> logdetails = new ObservableCollection<UserInfo>();
                    UserInfo ui = new UserInfo();

                    ui.UserName = UserName;
                    ui.Pwd = Password;
                    ui.CurrentDate = System.DateTime.Now.ToString();
                    logdetails.Add(ui);

                    svc.ValidateUserAsync(logdetails);                   

                }
                catch (Exception ex)
                {

                }
            }
        }

        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private void ExecuteCancel()
        {
            UserName = string.Empty;
            Password = string.Empty;

            //Reg_succ_msg = string.Empty;
            Err_msg = string.Empty;

        }
    }
}

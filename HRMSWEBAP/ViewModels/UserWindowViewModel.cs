using Apex.MVVM;
using HRMSWEBAP.HrmsReference;
using HRMSWEBAP.Models;
using HRMSWEBAP.Views;
using SilverlightMessageBoxLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;


namespace HRMSWEBAP.ViewModels
{
    public class UserWindowViewModel : INotifyPropertyChanged
    {
        public UserPage window;
        public LeaveDetails leavepage;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool dateflag = false;
        public bool dateSearchFlag = false;
        public bool init = false;
        MemoryStream ms = new MemoryStream();
        byte[] arr = null;
        BitmapImage bImg = null;

        double totalDays = 0.0;
        double activeDays = 0.0;
        double InActiveDays = 0.0;
        double holidays = 0.0;
        double appliedLeaves = 0.0;

        HrmsServiceClient svc = new HrmsServiceClient();

        #region Constructors
        public UserWindowViewModel(UserPage usrwindow)
        {
            window = usrwindow;

            svc.AddAttendanceCompleted -= new EventHandler<AddAttendanceCompletedEventArgs>(AddAttendanceResult);
            svc.AddAttendanceCompleted += new EventHandler<AddAttendanceCompletedEventArgs>(AddAttendanceResult);

            svc.GetEmployeeAttendanceDetailsCompleted -= new EventHandler<GetEmployeeAttendanceDetailsCompletedEventArgs>(GetEmpAttData);
            svc.GetEmployeeAttendanceDetailsCompleted += new EventHandler<GetEmployeeAttendanceDetailsCompletedEventArgs>(GetEmpAttData);

            svc.AddLeaveDetailsCompleted -= new EventHandler<AddLeaveDetailsCompletedEventArgs>(AddLeaveDetailsResult);
            svc.AddLeaveDetailsCompleted += new EventHandler<AddLeaveDetailsCompletedEventArgs>(AddLeaveDetailsResult);

            svc.GetLeaveStatisticsCompleted -= new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetLeaveStatisticsResult);
            svc.GetLeaveStatisticsCompleted += new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetLeaveStatisticsResult);

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);
            svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);

            _punchInCommand = new Command(ExecutePunchInCommand);
            _punchoutCommand = new Command(ExecutePunchoutCommand);
            _goCommand = new Command(ExecuteSearchCommand);
            _applyCommand = new Command(ExecuteApply);
            _GenerateReportCommand = new Command(ExecuteGetReport);
            _LeaveSearchCommand = new Command(ExecuteLeaveSearch);

            EmpID = Helpers.UserLoginStatus.currentEmpId;
            UserName = Helpers.UserLoginStatus.currentUserName;
            Name = Helpers.UserLoginStatus.currentName;
            Password = Helpers.UserLoginStatus.currentPassword;
            PhoneNumber = Helpers.UserLoginStatus.currentPhone;
            Email = Helpers.UserLoginStatus.currentEmail;
            Dob = Helpers.UserLoginStatus.currentUserDob.ToString().Split(' ')[0];

            if (Helpers.UserLoginStatus.currentGender.Equals("M"))
            {
                window.male.IsChecked = true;
            }
            else
            {
                window.female.IsChecked = true;
            }

            if (string.IsNullOrEmpty(Helpers.UserLoginStatus.CurrentEmpPhotoPath))
            {
                arr = (Helpers.UserLoginStatus.currentGender.Equals("M") ? Icons.Resource.male_avatar : Icons.Resource.female_avatar);
                ms = new MemoryStream(arr);
                bImg = new BitmapImage();
                bImg.SetSource(ms);
                ImgData = bImg;

                Helpers.UserLoginStatus.currentUserImgData = arr;
            }
            else
            {
                _UserImageUrl = "http://localhost:56702/" + Helpers.UserLoginStatus.CurrentEmpPhotoPath;

                Uri uri = new Uri(_UserImageUrl);
                Fetch(uri);
            }

            //if (Helpers.UserLoginStatus.currentUserImgData == null)
            //{
            //    arr = Icons.Resource.usravatar;
            //    ms = new MemoryStream(arr);
            //    bImg = new BitmapImage();
            //    bImg.SetSource(ms);
            //   ImgData = bImg;

            //    Helpers.UserLoginStatus.currentUserImgData = arr;
            //}
            //else
            //{
            //    ms = new MemoryStream(Helpers.UserLoginStatus.currentUserImgData);
            //    bImg = new BitmapImage();
            //    bImg.SetSource(ms);
            //    ImgData = bImg;
            //}

            #region Attendance Tab

            CurrentDate = DateTime.Now.ToShortDateString();

            PunchinTime = Helpers.UserLoginStatus.PunchinTime;
            PunchoutTime = Helpers.UserLoginStatus.PunchoutTime;

            ErrMsg = string.Empty;

            if (!string.IsNullOrEmpty(PunchinTime))
            {
                Punchinbtnvisib = System.Windows.Visibility.Collapsed;
                PunchinTime = Convert.ToDateTime(PunchinTime).ToString("MM-dd-yyyy hh:mm:ss tt");
            }
            else
            {
                Punchinbtnvisib = System.Windows.Visibility.Visible;
                Punchoutbtnvisib = System.Windows.Visibility.Collapsed;
            }
            if (!string.IsNullOrEmpty(PunchoutTime))
            {
                Punchoutbtnvisib = System.Windows.Visibility.Collapsed;
                PunchoutTime = Convert.ToDateTime(PunchoutTime).ToString("MM-dd-yyyy hh:mm:ss tt");
            }
            else
            {
                //  Punchoutbtnvisib = System.Windows.Visibility.Visible;
            }

            datagrid_visibility = System.Windows.Visibility.Collapsed;
            search_datagrid_visibility = System.Windows.Visibility.Collapsed;

            svc.GetEmployeeAttendanceDetailsAsync(Helpers.UserLoginStatus.currentEmpId);

            #endregion

            svc.GetLeaveStatisticsAsync(Helpers.UserLoginStatus.currentEmpId);

            List<string> monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames.ToList();
            monthNames.Remove("");
            mnthlst = monthNames;

            string[] typarr = { "Attendance", "Leave" };
            reperttyplst = typarr;

            int[] yeararr = { 2014, 2015, 2016, 2017 };
            yearlst = yeararr;

            window.lvDateTxtBlk.Text = "Showing Reports Of " + mnthlst[DateTime.Now.Month - 1] + " , " + DateTime.Now.Year;
            window.lvDateUpdtTxtBlk.Text = "As Of Date : " + DateTime.Now.ToLongDateString();
        }

        public UserWindowViewModel(LeaveDetails leaveDetails)
        {
            leavepage = leaveDetails;
            leavepage.casual_leave_blk.Text = Helpers.LeaveInfo.CasualLeave.ToString();
            leavepage.sick_leave_blk.Text = Helpers.LeaveInfo.SickLeave.ToString();
            leavepage.festive_leave_blk.Text = Helpers.LeaveInfo.FestiveLeave.ToString();
            leavepage.lossofpay_blk.Text = "0.0";
        }

        #endregion

        public void Fetch(Uri uri)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.OpenReadCompleted += ReadCompleted;
                webClient.OpenReadAsync(uri);
            }
            catch (Exception ex)
            {

            }
        }

        private void ReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                WebClient webClient = (WebClient)sender;
                webClient.OpenReadCompleted -= ReadCompleted;
                Stream stream = e.Result;
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(stream);
                //WriteableBitmap wbmp = new WriteableBitmap(bmp);
                ImgData = bmp;
            }
            catch (Exception ex)
            {

            }
        }

        private void ExecuteLeaveSearch(object obj)
        {
            try
            {
                if (sel_lv_search_month == -1 || sel_lv_search_year == -1)
                {
                    new CustomMessage("*Select Mandatory Fields!", CustomMessage.MessageType.Error, null).Show();
                }
                else if (sel_lv_search_month > -1 && sel_lv_search_year > -1)
                {
                    LeaveInfo li = new LeaveInfo();
                    var strtDate = Helpers.SupportMethods.GetFirstDayOfMonth(sel_lv_search_month + 1, yearlst[sel_lv_search_year]);
                    li.FromDate = strtDate.ToString();
                    li.ToDate = strtDate.AddMonths(1).AddDays(-1).ToString();

                    svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveSearchResult);
                    svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveSearchResult);
                    svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, li,false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LeaveSearchResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                window.lvDateTxtBlk.Text = "Showing Reports Of " + mnthlst[sel_lv_search_month] + " , " + yearlst[sel_lv_search_year];
                window.applied_leave_datagrid.ItemsSource = null;

                if (e.Result.ToList().Count() == 0)
                {
                    MyLeaveErrorMsg = "No Records found!";
                    window.applied_leave_datagrid.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    leaveDetailsList = e.Result.ToList();
                    window.applied_leave_datagrid.ItemsSource = e.Result.ToList();

                    MyLeaveErrorMsg = "";
                    window.applied_leave_datagrid.Visibility = System.Windows.Visibility.Visible;
                }
            }

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveSearchResult);
        }

        private void ExecuteGetReport(object obj)
        {
            try
            {
                if (seltdmnth == -1 || selttyp == -1 || seltdyear == -1)
                {
                    CustomMessage cm = new CustomMessage("*Select mandatory fields!", CustomMessage.MessageType.Error);
                    cm.Show();
                }
                else if (seltdmnth > -1 && selttyp > -1 && seltdyear > -1)
                {
                    //this.window.busyshow.IsBusy = true;

                    int month = seltdmnth + 1;
                    int year = yearlst[seltdyear];

                    if (selttyp == 0)
                    {
                        var startDate = Helpers.SupportMethods.GetFirstDayOfMonth(month, year);
                        var endDate = startDate.AddMonths(1).AddDays(-1);

                        totalDays = DateTime.DaysInMonth(year, month);
                        holidays = totalDays - Helpers.SupportMethods.GetBusinessDaysCount(startDate, endDate);


                        HrmsReference.UserInfo ui = new HrmsReference.UserInfo();
                        ui.StartDate = startDate.ToString();
                        ui.EndDate = endDate.ToString();

                        svc.GetAttendanceDetailsCompleted -= new EventHandler<GetAttendanceDetailsCompletedEventArgs>(AttnReport);
                        svc.GetAttendanceDetailsCompleted += new EventHandler<GetAttendanceDetailsCompletedEventArgs>(AttnReport);
                        svc.GetAttendanceDetailsAsync(Helpers.UserLoginStatus.currentEmpId,ui.StartDate,ui.EndDate,true);

                    }

                    else if (selttyp == 1)
                    {
                        var startDate = Helpers.SupportMethods.GetFirstDayOfMonth(month, year);
                        var endDate = startDate.AddMonths(1).AddDays(-1);

                        LeaveInfo li = new LeaveInfo();
                        li.FromDate = startDate.ToString();
                        li.ToDate = endDate.ToString();

                        svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveReport);
                        svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveReport);


                        svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, li,false);

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LeaveReport(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                double lvcount = 0.0;

                foreach (var item in e.Result)
                {
                    lvcount = lvcount + item.Days;
                }

                InActiveDays = totalDays - activeDays - holidays - lvcount;

                AttendanceReportInfo ar = new AttendanceReportInfo();
                ar.TotalDays = totalDays;
                ar.Holidays = holidays;
                ar.WorkingDays = totalDays - holidays;
                ar.ActiveDays = activeDays;
                ar.LeaveDays = lvcount;
                ar.Month = mnthlst[seltdmnth];
                ar.Year = yearlst[seltdyear].ToString();

                window.cnvContainer.Visibility = System.Windows.Visibility.Visible;
                window.exporttyppanel.Visibility = System.Windows.Visibility.Visible;
                window.reportCalcDate.Visibility = System.Windows.Visibility.Visible;

                ObservableCollection<AttendanceReportInfo> attRprt = new ObservableCollection<AttendanceReportInfo>();
                attRprt.Add(ar);
                AttendanceReport = attRprt;

                window.reportCalcDate.Content = "Report As of " + DateTime.Now.ToShortDateString();
                window.nametxt.Text = Helpers.UserLoginStatus.currentName;
                window.yrtxt.Text = ar.Year;
                window.mnthtxt.Text = ar.Month;
                window.wrkdystxt.Text = ar.WorkingDays.ToString();
                window.holidystxt.Text = ar.Holidays.ToString();
                window.attndtxt.Text = ar.ActiveDays.ToString();
                window.lvtxt.Text = ar.LeaveDays.ToString();
                window.wrkhrtxt.Text = (ar.ActiveDays * 8).ToString();

                PrintTitle = "Monthly Report - " + mnthlst[seltdmnth] + "," + yearlst[seltdyear];
            }

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveReport);
        }

        private void AttnReport(object sender, GetAttendanceDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //if (e.Result.Count != 0)
                //{
                    window.reporterrormsg.Content = "";

                    ObservableCollection<HrmsReference.UserInfo> atinfo = new ObservableCollection<HrmsReference.UserInfo>();
                    atinfo = e.Result;

                    ObservableCollection<AttendanceReportInfo> attRprt = new ObservableCollection<AttendanceReportInfo>();
                    AttendanceReportInfo ar = new AttendanceReportInfo();
                    activeDays = e.Result.Count;
                    //InActiveDays = totalDays - activeDays - holidays;

                    //ar.TotalDays = totalDays;
                    //ar.Holidays = holidays;
                    //ar.WorkingDays = totalDays - holidays;
                    //ar.ActiveDays = activeDays;
                    //ar.LeaveDays = InActiveDays;
                    //ar.Month = mnthlst[seltdmnth];
                    //ar.Year = yearlst[seltdyear].ToString();

                    attRprt.Add(ar);

                    AttendanceReport = attRprt;

                    //this.window.reportdetails.Content = "Attendance Report for " + this.window.mnthcmbbx.SelectedItem.ToString() + "," + this.window.yearcmbbx.SelectedItem.ToString();
                    //this.window.reportgrid.Visibility = System.Windows.Visibility.Visible;
                    //// this.window.exporttyppanel.Visibility = System.Windows.Visibility.Visible;
                    //this.window.printbtn.Visibility = System.Windows.Visibility.Visible;


                    var startDate = Helpers.SupportMethods.GetFirstDayOfMonth(seltdmnth + 1, yearlst[seltdyear]);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    LeaveInfo li = new LeaveInfo();
                    li.FromDate = startDate.ToString();
                    li.ToDate = endDate.ToString();

                    svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveReport);
                    svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveReport);


                    svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, li);


                    window.busyshow.IsBusy = false;
                }
                else
                {
                    window.reportCalcDate.Visibility = System.Windows.Visibility.Collapsed;
                    //this.window.reportgrid.Visibility = System.Windows.Visibility.Collapsed;
                    //  this.window.reportdetails.Content = "";
                    window.reporterrormsg.Content = "No Records Found for the Selected Month and Year!";
                    window.exporttyppanel.Visibility = System.Windows.Visibility.Collapsed;
                    // this.window.printbtn.Visibility = System.Windows.Visibility.Collapsed;
                    window.cnvContainer.Visibility = System.Windows.Visibility.Collapsed;

                    window.busyshow.IsBusy = false;

                    PrintTitle = "";
                }
            }
            svc.GetEmployeeAttendanceDataCompleted -= new EventHandler<GetEmployeeAttendanceDataCompletedEventArgs>(AttnReport);
        }

        public BitmapImage ToBitmapImage(byte[] btarray)
        {
            byte[] arr = null;
            System.Windows.Media.Imaging.BitmapImage bImg = null;

            try
            {
                arr = btarray;
                MemoryStream memory = new MemoryStream(arr);
                bImg = new BitmapImage();
                bImg.SetSource(memory);

                return bImg;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetAttendanceDetailsResult(object sender, GetAttendanceDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.Count != 0)
                {
                    List<HrmsReference.UserInfo> templist = new List<HrmsReference.UserInfo>();
                    templist = e.Result.ToList();
                    SearchResultList = templist;
                    search_datagrid_visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    SearchResultList = null;
                    window.search_att_datagrid.Visibility = System.Windows.Visibility.Collapsed;
                    ErrMsgSearch = "No records found!";
                }
            }

        }

        private void GetEmployeeLeaveDetailsResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();

                LeaveDetailsList = Helpers.EmpDetails.CurrEmpLeaveDetailsList;

                if (leaveDetailsList.Count() == 0)
                {
                    MyLeaveErrorMsg = "No Records found!";
                    window.applied_leave_datagrid.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);
        }

        private void GetLeaveStatisticsResult(object sender, GetLeaveStatisticsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpLeaveStatisticsList = e.Result.ToList();

                Helpers.LeaveInfo.LeaveTypes = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].LeaveTypes.ToList();
                Helpers.LeaveInfo.CasualLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].CasualLeave;
                Helpers.LeaveInfo.FestiveLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].FestiveLeave;
                Helpers.LeaveInfo.SickLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].SickLeave;

                window.leave_typ_combobx.ItemsSource = Helpers.LeaveInfo.LeaveTypes;
                LeaveBalance = 0.0;

                LeaveInfo linfo = new LeaveInfo();
                var strtDate = Helpers.SupportMethods.GetFirstDayOfMonth(DateTime.Now.Month, DateTime.Now.Year);
                linfo.FromDate = strtDate.ToString();
                linfo.ToDate = strtDate.AddMonths(1).AddDays(-1).ToString();

                sel_lv_search_month = DateTime.Now.Month - 1;
                sel_lv_search_year = Array.IndexOf(yearlst, DateTime.Now.Year);

                svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, linfo);
            }

            svc.GetLeaveStatisticsCompleted -= new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetLeaveStatisticsResult);

        }

        private void AddLeaveDetailsResult(object sender, AddLeaveDetailsCompletedEventArgs e)
        {
            CRUDStatus cs = new CRUDStatus();

            if (e.Result != null)
            {
                cs = e.Result;

                if (cs.IsSuccessful && string.IsNullOrEmpty(cs.ErrorMessage))
                {
                    window.applied_leave_datagrid.Visibility = System.Windows.Visibility.Visible;

                    // Helpers.LeaveInfo.LeaveTypes.Clear();
                    LeaveType = 0;
                    LeaveDurationType = null;
                    LeaveBalance = 0.0;
                    //LeaveDurationList.Clear();
                    FromDate = null;
                    ToDate = null;
                    Comments = "";

                    svc.GetLeaveStatisticsCompleted -= new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetLeaveStatisticsResult);
                    svc.GetLeaveStatisticsCompleted += new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetLeaveStatisticsResult);

                    svc.GetLeaveStatisticsAsync(Helpers.UserLoginStatus.currentEmpId);

                    svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);
                    svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);

                    LeaveInfo linfo = new LeaveInfo();
                    var strtDate = Helpers.SupportMethods.GetFirstDayOfMonth(DateTime.Now.Month, DateTime.Now.Year);
                    linfo.FromDate = strtDate.ToString();
                    linfo.ToDate = strtDate.AddMonths(1).AddDays(-1).ToString();

                    svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, linfo);

                    CustomMessage succmessage = new CustomMessage("Applied.", CustomMessage.MessageType.Info);
                    succmessage.Show();


                }

                else if (cs.IsSuccessful && (!string.IsNullOrEmpty(cs.ErrorMessage)))
                {
                    CustomMessage msg = new CustomMessage(cs.ErrorMessage + "\n" + "Do you want to make any changes?", CustomMessage.MessageType.Confirm);
                    msg.OKButton.Click += (obj, args) =>
                    {
                        ms.Close();
                    };
                    msg.CancelButton.Click += (obj, args) =>
                        {
                            ms.Close();

                            LeaveType = 0;
                            LeaveDurationType = null;
                            LeaveBalance = 0.0;
                            //LeaveDurationList.Clear();
                            FromDate = null;
                            ToDate = null;
                        };

                    msg.Show();

                    Helpers.LeaveInfo.LeaveTypes = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].LeaveTypes.ToList();
                    Helpers.LeaveInfo.CasualLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].CasualLeave;
                    Helpers.LeaveInfo.FestiveLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].FestiveLeave;
                    Helpers.LeaveInfo.SickLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].SickLeave;
                }
            }
            svc.AddLeaveDetailsCompleted -= new EventHandler<AddLeaveDetailsCompletedEventArgs>(AddLeaveDetailsResult);
        }

        private void GetEmpAttData(object sender, GetEmployeeAttendanceDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpAttendanceDataList = e.Result.ToList();
            }
            svc.GetEmployeeAttendanceDataCompleted -= new EventHandler<GetEmployeeAttendanceDataCompletedEventArgs>(GetEmpAttData);
        }

        private void AddAttendanceResult(object sender, AddAttendanceCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ObservableCollection<CRUDStatus> responselst = new ObservableCollection<CRUDStatus>();
                responselst = e.Result;

                if (responselst[0].IsSuccessful)
                {
                    int currEmpid = Helpers.UserLoginStatus.currentEmpId;
                    svc.GetEmployeeAttendanceDataAsync(currEmpid, false, null);
                }
            }
            svc.AddAttendanceCompleted -= new EventHandler<AddAttendanceCompletedEventArgs>(AddAttendanceResult);
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }

        private void ExecutePunchInCommand()
        {
            PunchinTime = System.DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt");

            Punchinbtnvisib = System.Windows.Visibility.Collapsed;
            Punchoutbtnvisib = System.Windows.Visibility.Visible;

            ObservableCollection<HrmsReference.UserInfo> lst = new ObservableCollection<HrmsReference.UserInfo>();
            HrmsReference.UserInfo obj = new HrmsReference.UserInfo();
            obj.EmpID = EmpID;
            obj.PunchinTime = PunchinTime;
            obj.PunchoutTime = PunchoutTime;
            obj.Pinflg = true;
            lst.Add(obj);

            svc.AddAttendanceAsync(lst);
        }

        private void ExecutePunchoutCommand()
        {
            PunchoutTime = System.DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt");

            Punchinbtnvisib = System.Windows.Visibility.Collapsed;
            Punchoutbtnvisib = System.Windows.Visibility.Collapsed;

            ObservableCollection<HrmsReference.UserInfo> lst = new ObservableCollection<HrmsReference.UserInfo>();
            HrmsReference.UserInfo obj = new HrmsReference.UserInfo();
            obj.EmpID = EmpID;
            obj.PunchinTime = PunchinTime;
            obj.PunchoutTime = PunchoutTime;
            obj.Pinflg = false;
            obj.Poutflg = true;
            lst.Add(obj);

            svc.AddAttendanceAsync(lst);
        }

        private void getAttandanceDetails()
        {
            //SelectedDate = MyDateTime.ToString().Split(' ')[0];
            DateTime SelDate = Convert.ToDateTime(MyDateTime);

            svc.GetEmployeeSinglePunchInfoCompleted -= new EventHandler<GetEmployeeSinglePunchInfoCompletedEventArgs>(GetEmpAttDataSingleResult);
            svc.GetEmployeeSinglePunchInfoCompleted += new EventHandler<GetEmployeeSinglePunchInfoCompletedEventArgs>(GetEmpAttDataSingleResult);
            svc.GetEmployeeSinglePunchInfoAsync(Helpers.UserLoginStatus.currentEmpId, SelDate);

            #region Unused
            //SelectedDate = MyDateTime.ToString().Split(' ')[0];
            //List<HRMSWEBAP.HrmsReference.UserInfo> pinList = new List<HRMSWEBAP.HrmsReference.UserInfo>();
            //HRMSWEBAP.HrmsReference.UserInfo ui = null;
            //bool flag = false;
            //foreach (var item in Helpers.EmpDetails.CurrEmpAttendanceDataList)
            //{
            //    try
            //    {
            //        if (SelectedDate.Equals(item.PunchinTime.Split(' ')[0]))
            //        {
            //            ui = new HRMSWEBAP.HrmsReference.UserInfo();
            //            ui.PunchinTime = item.PunchinTime;
            //            ui.PunchoutTime = item.PunchoutTime;
            //            ui.Duration = item.Duration;
            //            ui.SelectedDate = SelectedDate;
            //            pinList.Add(ui);
            //            flag = true;
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    { }
            //}

            //if (!flag)
            //{
            //    List.Clear();
            //    datagrid_visibility = System.Windows.Visibility.Collapsed;
            //    ErrMsg = "No Records found!";
            //}
            //else
            //{
            //    datagrid_visibility = System.Windows.Visibility.Visible;
            //    ErrMsg = string.Empty;
            //}
            //List = pinList;       
            #endregion
        }

        private void GetEmpAttDataSingleResult(object sender, GetEmployeeSinglePunchInfoCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<HrmsReference.UserInfo> pinList = new List<HrmsReference.UserInfo>();
                pinList = e.Result.ToList();
                List = pinList;
                if (pinList.Count == 0)
                {
                    List.Clear();
                    datagrid_visibility = System.Windows.Visibility.Collapsed;
                    ErrMsg = "No Records found!";
                }
                else
                {
                    datagrid_visibility = System.Windows.Visibility.Visible;
                    ErrMsg = string.Empty;
                }
            }
        }

        private void ExecuteSearchCommand()
        {
            int type = 0;
            string sdate = "";
            string edate = "";
            HrmsReference.UserInfo ui = null;

            if (StartDate == null && EndDate == null)
            {
                ErrMsgSearch = "Select startdate & enddate";
            }
            else
            {
                if (StartDate != null && EndDate == null)
                {
                    type = 1;
                    sdate = StartDate.ToString().Split(' ')[0];
                    edate = null;
                }
                else if (StartDate == null && EndDate != null)
                {
                    //type = 2;
                    //sdate = null;
                    //edate = EndDate.ToString().Split(' ')[0];
                    search_datagrid_visibility = System.Windows.Visibility.Collapsed;
                    ErrMsgSearch = "Select startdate";
                }
                else if (StartDate != null && EndDate != null)
                {
                    if ((Convert.ToDateTime(StartDate)) > (Convert.ToDateTime(EndDate)))
                    {
                        search_datagrid_visibility = System.Windows.Visibility.Collapsed;
                        ErrMsgSearch = "Startdate must be less than end date";
                    }
                    else if (Convert.ToDateTime(EndDate) > System.DateTime.Now)
                    {
                        search_datagrid_visibility = System.Windows.Visibility.Collapsed;
                        ErrMsgSearch = "End date must be less than or equal to current date";
                    }
                    else
                    {
                        type = 3;

                        sdate = StartDate.ToString();//.Split(' ')[0];
                        edate = EndDate.ToString();//.Split(' ')[0];
                    }
                }

                svc.GetAttendanceDetailsCompleted -= new EventHandler<GetAttendanceDetailsCompletedEventArgs>(GetAttendanceDetailsResult);
                svc.GetAttendanceDetailsCompleted += new EventHandler<GetAttendanceDetailsCompletedEventArgs>(GetAttendanceDetailsResult);

                switch (type)
                {
                    case 1:
                        ErrMsgSearch = "";

                        svc.GetAttendanceDetailsAsync(Helpers.UserLoginStatus.currentEmpId, sdate, edate);
                        // search_datagrid_visibility = System.Windows.Visibility.Visible;

                        break;
                    //case 2:
                    //    ErrMsg = "";                       

                    //    svc.GetAttendanceDetailsAsync(Helpers.UserLoginStatus.currentEmpId, sdate, edate);
                    //    search_datagrid_visibility = System.Windows.Visibility.Visible;
                    //    break;

                    case 3:
                        ErrMsgSearch = "";

                        svc.GetAttendanceDetailsAsync(Helpers.UserLoginStatus.currentEmpId, sdate, edate);
                        // search_datagrid_visibility = System.Windows.Visibility.Visible;
                        break;

                    default:
                        break;
                }
            }
        }

        private void ExecuteApply()
        {
            LeaveErrMsg = "";
            MyLeaveErrorMsg = "";
            if (FromDate == null || ToDate == null || LeaveType == -1 || LeaveDurationType == null)
            {
                CustomMessage errormessage = new CustomMessage("Select Mandatory Fileds", CustomMessage.MessageType.Error);

                errormessage.OKButton.Click += (obj, args) =>
                {
                    errormessage.Close();
                };
                errormessage.Show();
            }
            else if (FromDate != null && ToDate != null && LeaveType < -1 && LeaveDurationType != null)
            {
                if (Convert.ToDateTime(FromDate) > Convert.ToDateTime(ToDate))
                {
                    CustomMessage errormessage = new CustomMessage("From date should be less than or equal to To date", CustomMessage.MessageType.Error);

                    errormessage.OKButton.Click += (obj, args) =>
                    {
                        errormessage.Close();
                    };
                    errormessage.Show();
                }

                else
                {
                    DateTime from = Convert.ToDateTime(FromDate);
                    DateTime to = Convert.ToDateTime(ToDate);

                    if (LeaveDurationType == "Half Day")
                    {
                        appliedLeaves = (to - from).TotalDays;
                        appliedLeaves = 0.5 + (0.5 * appliedLeaves);
                    }
                    else if (LeaveDurationType == "Full Day")
                    {
                        appliedLeaves = (to - from).TotalDays + 1;
                    }

                    var item = Helpers.LeaveInfo.LeaveTypes.FirstOrDefault(i => i.TypeId == LeaveType);

                    svc.EnoughLeavesCompleted -= new EventHandler<HrmsReference.EnoughLeavesCompletedEventArgs>(CheckEnoughLeaves);
                    svc.EnoughLeavesCompleted += new EventHandler<HrmsReference.EnoughLeavesCompletedEventArgs>(CheckEnoughLeaves);
                    svc.EnoughLeavesAsync(Helpers.UserLoginStatus.currentEmpId, item.Name);
                }
            }
        }

        private void CheckEnoughLeaves(object sender, EnoughLeavesCompletedEventArgs e)
        {
            if (e.Result <= 0.0)
            {
                double NoLeaves = 0.0;
                NoLeaves = e.Result;
                bool flag = false;

                if (NoLeaves == 0.0 || appliedLeaves > NoLeaves)
                {
                    flag = false;
                }
                else if (appliedLeaves <= NoLeaves && NoLeaves >= 0)
                {
                    flag = true;
                }

                if (!flag)
                {
                    CustomMessage errormessage = new CustomMessage("Not Enough Leaves", CustomMessage.MessageType.Error);
                    errormessage.OKButton.Click += (obj, args) =>
                    {
                        errormessage.Close();
                    };
                    errormessage.Show();
                }
                else
                {
                    ObservableCollection<LeaveInfo> linfo = new ObservableCollection<LeaveInfo>();
                    LeaveInfo li = new LeaveInfo();

                    var item = Helpers.LeaveInfo.LeaveTypes.FirstOrDefault(i => i.TypeId == LeaveType);

                    li.EmpId = Helpers.UserLoginStatus.currentEmpId;
                    li.FromDate = FromDate;
                    li.ToDate = ToDate;
                    li.LeaveType = item.Name;
                    //li.LeavedurTypInt = LeaveType;
                    li.Leavedurationtype = LeaveDurationType;
                    li.LeaveTypeInt = LeaveType;

                    switch (item.Name)
                    {
                        case "Casual":
                            li.CasualLeave = appliedLeaves;
                            break;
                        case "Festive":
                            li.FestiveLeave = appliedLeaves;
                            break;
                        case "Sick":
                            li.SickLeave = appliedLeaves;
                            break;
                        default:
                            break;
                    }

                    if (Comments != null)
                    {
                        li.Comments = Comments;
                    }
                    else
                    {
                        li.Comments = "";
                    }

                    li.Status = false;//leave pending not yet approved.
                    li.Rejected = false;
                    li.Cancelled = false;

                    linfo.Add(li);

                    svc.AddLeaveDetailsCompleted -= new EventHandler<AddLeaveDetailsCompletedEventArgs>(AddLeaveDetailsResult);
                    svc.AddLeaveDetailsCompleted += new EventHandler<AddLeaveDetailsCompletedEventArgs>(AddLeaveDetailsResult);
                    svc.AddLeaveDetailsAsync(linfo, false);
                }
            }

            svc.EnoughLeavesCompleted -= new EventHandler<HrmsReference.EnoughLeavesCompletedEventArgs>(CheckEnoughLeaves);
        }

        private void SetLeaveDuration()
        {
            List<string> templist = new List<string>();
            templist.Add("Full Day");

            var item = Helpers.LeaveInfo.LeaveTypes.FirstOrDefault(i => i.TypeId == LeaveType);

            if (item != null && (item.Name.Equals("Casual") || item.Name.Equals("Sick")))
            {
                templist.Add("Half Day");
            }

            LeaveDurationList = templist;
        }

        private void SetLeaveCount()
        {
            var item = Helpers.LeaveInfo.LeaveTypes.FirstOrDefault(i => i.TypeId == LeaveType);
            if (item != null)
            {
                switch (item.Name)
                {
                    case "Casual":
                        LeaveBalance = Helpers.LeaveInfo.CasualLeave;
                        break;
                    case "Festive":
                        LeaveBalance = Helpers.LeaveInfo.FestiveLeave;
                        break;
                    case "Sick":
                        LeaveBalance = Helpers.LeaveInfo.SickLeave;
                        break;
                    default:
                        break;
                }
            }
        }

        #region Binding Fields

        private string _UserImageUrl;
        public string UserImageUrl
        {
            get { return _UserImageUrl; }
            set
            {
                _UserImageUrl = value;
                RaisePropertyChanged("UserImageUrl");
            }
        }

        private string _PrintTitle;
        public string PrintTitle
        {
            get { return _PrintTitle; }
            set
            {
                _PrintTitle = value; RaisePropertyChanged("PrintTitle");
            }
        }

        private List<string> _mnthlst;
        public List<string> mnthlst
        {
            get { return _mnthlst; }
            set
            {
                _mnthlst = value;
                RaisePropertyChanged("mnthlst");
            }
        }

        private int[] _yearlst;
        public int[] yearlst
        {
            get { return _yearlst; }
            set
            {
                _yearlst = value;
                RaisePropertyChanged("yearlst");
            }
        }

        private int _seltdmnth;
        public int seltdmnth
        {
            get { return _seltdmnth; }
            set
            {
                _seltdmnth = value;
                // RaisePropertyChanged("seltdmnth");
            }
        }

        private int _seltdyear;
        public int seltdyear
        {
            get { return _seltdyear; }
            set
            {
                _seltdyear = value;
                RaisePropertyChanged("seltdyear");
            }
        }

        private int _sel_lv_search_year;
        public int sel_lv_search_year
        {
            get { return _sel_lv_search_year; }
            set
            {
                _sel_lv_search_year = value;
                // RaisePropertyChanged("seltdmnth");
            }
        }

        private int _sel_lv_search_month;
        public int sel_lv_search_month
        {
            get { return _sel_lv_search_month; }
            set
            {
                _sel_lv_search_month = value;
                //RaisePropertyChanged("seltdyear");
            }
        }

        private string[] _reperttyplst;
        public string[] reperttyplst
        {
            get { return _reperttyplst; }
            set
            {
                _reperttyplst = value;
                RaisePropertyChanged("reperttyplst");
            }
        }

        private int _selttyp;
        public int selttyp
        {
            get { return _selttyp; }
            set
            {
                _selttyp = value;
                RaisePropertyChanged("selttyp");
            }
        }

        private ObservableCollection<AttendanceReportInfo> _attendanceReport;
        public ObservableCollection<AttendanceReportInfo> AttendanceReport
        {
            get
            {
                return _attendanceReport;
            }
            set
            {
                _attendanceReport = value;
                RaisePropertyChanged("AttendanceReport");
            }
        }

        private Command _punchInCommand;
        public Command PunchInCommand
        {
            get
            {
                return _punchInCommand;
            }
        }

        private Command _GenerateReportCommand;
        public Command GenerateReportCommand
        {
            get
            {
                return _GenerateReportCommand;
            }

        }

        private Command _punchoutCommand;
        public Command PunchoutCommand
        {
            get
            {
                return _punchoutCommand;
            }
        }

        private Command _goCommand;
        public Command GoCommand
        {
            get
            {
                return _goCommand;
            }
        }

        private Command _applyCommand;
        public Command ApplyCommand
        {
            get
            {
                return _applyCommand;
            }
        }

        private Command _LeaveSearchCommand;
        public Command LeaveSearchCommand
        {
            get
            {
                return _LeaveSearchCommand;
            }
        }

        private System.Windows.Visibility _punchinbtnvisib;
        public System.Windows.Visibility Punchinbtnvisib
        {
            get
            {
                return _punchinbtnvisib;
            }
            set
            {
                _punchinbtnvisib = value;
                RaisePropertyChanged("Punchinbtnvisib");
            }
        }

        private System.Windows.Visibility _punchoutbtnvisib;
        public System.Windows.Visibility Punchoutbtnvisib
        {
            get
            {
                return _punchoutbtnvisib;
            }
            set
            {
                _punchoutbtnvisib = value;
                RaisePropertyChanged("Punchoutbtnvisib");
            }
        }

        private System.Windows.Visibility _datagrid_visibility;
        public System.Windows.Visibility datagrid_visibility
        {
            get
            {
                return _datagrid_visibility;
            }
            set
            {
                _datagrid_visibility = value;
                RaisePropertyChanged("datagrid_visibility");
            }
        }

        private System.Windows.Visibility _search_datagrid_visibility;
        public System.Windows.Visibility search_datagrid_visibility
        {
            get
            {
                return _search_datagrid_visibility;
            }
            set
            {
                _search_datagrid_visibility = value;
                RaisePropertyChanged("search_datagrid_visibility");
            }
        }

        private Nullable<DateTime> myDateTime = null;
        public Nullable<DateTime> MyDateTime
        {
            get
            {
                //if (myDateTime == null)
                //{
                //    myDateTime = DateTime.Today;
                //}
                return myDateTime;
            }
            set
            {
                myDateTime = value;
                getAttandanceDetails();
                RaisePropertyChanged("MyDateTime");
            }
        }

        private double _leaveDays;
        public double LeaveDays
        {
            get
            {
                return _leaveDays;
            }
            set
            {
                _leaveDays = value;
                RaisePropertyChanged("LeaveDays");
            }
        }

        private string _myLeaveErrorMsg;
        public string MyLeaveErrorMsg
        {
            get { return _myLeaveErrorMsg; }
            set
            {
                _myLeaveErrorMsg = value;
                RaisePropertyChanged("MyLeaveErrorMsg");
            }
        }

        private List<HrmsReference.UserInfo> list;
        public List<HrmsReference.UserInfo> List
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
                RaisePropertyChanged("List");
            }
        }

        private List<HrmsReference.LeaveInfo> leaveDetailsList;
        public List<HrmsReference.LeaveInfo> LeaveDetailsList
        {
            get { return leaveDetailsList; }
            set
            {
                leaveDetailsList = value;
                RaisePropertyChanged("LeaveDetailsList");
            }

        }

        private List<HrmsReference.UserInfo> _searchResultList;
        public List<HrmsReference.UserInfo> SearchResultList
        {
            get { return _searchResultList; }
            set
            {
                _searchResultList = value;
                RaisePropertyChanged("SearchResultList");
            }
        }

        private List<string> _leaveDurationList;
        public List<string> LeaveDurationList
        {
            get { return _leaveDurationList; }
            set
            {
                _leaveDurationList = value;
                RaisePropertyChanged("LeaveDurationList");
            }
        }

        private string _leaveDurationType;
        public string LeaveDurationType
        {
            get { return _leaveDurationType; }
            set
            {
                _leaveDurationType = value;
                RaisePropertyChanged("LeaveDurationList");
            }
        }

        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                RaisePropertyChanged("StartDate");
            }
        }

        private string _endDate;
        public string EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged("EndDate");
            }
        }

        private string _fromDate;
        public string FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                RaisePropertyChanged("FromDate");
            }
        }

        private string _toDate;
        public string ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                RaisePropertyChanged("ToDate");
            }
        }

        private double _leaveBalance;
        public double LeaveBalance
        {
            get
            {
                return _leaveBalance;
            }
            set
            {
                _leaveBalance = value;
                RaisePropertyChanged("LeaveBalance");
            }
        }

        private int _leaveType;
        public int LeaveType
        {
            get
            { return _leaveType; }

            set
            {
                _leaveType = value;
                SetLeaveDuration();
                SetLeaveCount();
                RaisePropertyChanged("LeaveType");
            }
        }

        private string _comments;
        public string Comments
        {
            get
            { return _comments; }

            set
            {
                _comments = value;
                RaisePropertyChanged("Comments");
            }
        }

        private string _leaveStatus;
        public string LeaveStatus
        {
            get { return _leaveStatus; }
            set
            {
                _leaveStatus = value;
                RaisePropertyChanged("LeaveStatus");
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");

            }
        }

        private int _empId;
        public int EmpID
        {
            get
            {
                return _empId;
            }
            set
            {
                _empId = value;
                RaisePropertyChanged("EmpID");

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

        private string _cnfrmPassword;
        public string CnfrmPassword
        {
            get { return _cnfrmPassword; }
            set
            {
                _cnfrmPassword = value;
                RaisePropertyChanged("CnfrmPassword");
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }

        private string _dob;
        public string Dob
        {
            get { return _dob; }
            set
            {
                _dob = value;
                RaisePropertyChanged("Dob");
            }
        }

        private BitmapImage _imgData;
        public BitmapImage ImgData
        {
            get { return _imgData; }
            set
            {
                _imgData = value;
                RaisePropertyChanged("ImgData");
            }
        }

        private string _ErrMsg;
        public string ErrMsg
        {
            get { return _ErrMsg; }
            set
            {
                _ErrMsg = value;
                RaisePropertyChanged("ErrMsg");
            }
        }

        private string _ErrMsgSearch;
        public string ErrMsgSearch
        {
            get { return _ErrMsgSearch; }
            set
            {
                _ErrMsgSearch = value;
                RaisePropertyChanged("ErrMsgSearch");
            }
        }

        private string _LeaveErrMsg;
        public string LeaveErrMsg
        {
            get
            {
                return _LeaveErrMsg;
            }
            set
            {
                _LeaveErrMsg = value;
                RaisePropertyChanged("LeaveErrMsg");
            }
        }

        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                RaisePropertyChanged("CurrentDate");
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged("SelectedDate");
            }
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        private string _PunchinTime;
        public string PunchinTime
        {
            get { return _PunchinTime; }
            set
            {
                _PunchinTime = value;
                RaisePropertyChanged("PunchinTime");
            }
        }

        private string _punchoutTime;
        public string PunchoutTime
        {
            get { return _punchoutTime; }
            set
            {
                _punchoutTime = value;
                RaisePropertyChanged("PunchoutTime");
            }
        }

        #endregion
    }
}

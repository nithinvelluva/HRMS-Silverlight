using Apex.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HRMSWEBAP.Views;
using HRMSWEBAP.HrmsReference;

namespace HRMSWEBAP
{
    public class AdminPageViewModel : INotifyPropertyChanged//, IDataErrorInfo
    {
        HrmsServiceClient svc = new HrmsServiceClient();
        public AdminPage window;
        public event PropertyChangedEventHandler PropertyChanged;

        public AdminPageViewModel(AdminPage adminwindow)
        {
            window = adminwindow;

            svc.GetAttendanceDetailsCompleted -= new EventHandler<HrmsReference.GetAttendanceDetailsCompletedEventArgs>(GetAttendanceResult);
            svc.GetAttendanceDetailsCompleted += new EventHandler<HrmsReference.GetAttendanceDetailsCompletedEventArgs>(GetAttendanceResult);

            _viewCommand = new Command(ExecuteEView);
            _saveCommand = new Command(ExecuteSave);

            SetLeaveSource();
            GetEmpData();
        }

        public AdminPageViewModel()
        {
            // TODO: Complete member initialization
        }

        public void GetEmpData()
        {
            EmpPunchErrMsg = "";
            svc.GetDataCompleted -= new EventHandler<GetDataCompletedEventArgs>(async_searchcmpltd);
            svc.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(async_searchcmpltd);

            svc.GetDataAsync(null, false);
        }

        private void async_searchcmpltd(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<UserInfo> temp = new List<UserInfo>();

                foreach (var item in e.Result.ToList())
                {
                    if (item.Utype != 1)
                    {
                        temp.Add(item);
                    }
                }

                lst = temp;
            }

            svc.GetDataCompleted -= new EventHandler<GetDataCompletedEventArgs>(async_searchcmpltd);
        }

        private void ExecuteEView()
        {

            //rowIndex = this.window.empleavedatgridpend.SelectedIndex;
            //if (rowIndex != -1)
            //{
            //    HRMSWEBAP.HrmsReference.LeaveInfo li = new HRMSWEBAP.HrmsReference.LeaveInfo();
            //    li = this.LeaveDetailsListPending[rowIndex];

            //    LeaveRejectionComment obj = new LeaveRejectionComment(li);
            //    obj.Show();

            //}

        }

        private void GetAttendanceResult(object sender, GetAttendanceDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List = null;
                List = e.Result.ToList();

                if (List.Count() == 0)
                {
                    //List.Clear();
                    List = null;
                    Err_msg = "No Records found!";
                }
                else
                {
                    Err_msg = string.Empty;
                }
            }
            //svc.GetAttendanceDetailsCompleted -= new EventHandler<HrmsReference.GetAttendanceDetailsCompletedEventArgs>(GetAttendanceResult);
        }

        public void SetLeaveSource()
        {
            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmpLvDts);
            svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmpLvDts);
            svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, null,false);
        }

        private void GetEmpLvDts(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();

                window.errmsg_apprvdleaves.Content = "";
                window.errmsg_pendleaves.Content = "";
                window.errmsg_rejecleaves.Content = "";

                List<LeaveInfo> templistapp = new List<LeaveInfo>();
                List<LeaveInfo> templistpen = new List<LeaveInfo>();
                List<LeaveInfo> templistreje = new List<LeaveInfo>();

                foreach (var item in Helpers.EmpDetails.CurrEmpLeaveDetailsList)
                {
                    if (item.Status)
                    {
                        templistapp.Add(item);
                    }
                    else if (!item.Status && !item.Rejected)
                    {
                        templistpen.Add(item);
                    }
                    else if (item.Rejected)
                    {
                        templistreje.Add(item);
                    }
                }

                LeaveDetailsListApprov = templistapp;
                LeaveDetailsListPending = templistpen;
                LeaveDetailsListRejec = templistreje;

                if (LeaveDetailsListApprov.Count == 0)
                {
                    // this.window.empleavedatgridappov.Visibility = System.Windows.Visibility.Collapsed;
                    window.errmsg_apprvdleaves.Content = "No Records Found!";
                }
                if (LeaveDetailsListPending.Count == 0)
                {
                    // this.window.empleavedatgridpend.Visibility = System.Windows.Visibility.Collapsed;
                    window.errmsg_pendleaves.Content = "No Records Found!";
                }
                if (LeaveDetailsListRejec.Count == 0)
                {
                    //  this.window.empleavedatgridrejec.Visibility = System.Windows.Visibility.Collapsed;
                    window.errmsg_rejecleaves.Content = "No Records Found!";
                }
            }

            svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmpLvDts);
        }

        private Command _viewCommand;
        public Command ViewCommand
        {
            get
            {
                return _viewCommand;
            }
        }

        private Command _saveCommand;
        public Command SaveCommand
        {
            get
            {
                return _saveCommand;
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

        private List<LeaveInfo> leaveDetailsListApprov;
        public List<LeaveInfo> LeaveDetailsListApprov
        {
            get { return leaveDetailsListApprov; }
            set
            {
                leaveDetailsListApprov = value;
                RaisePropertyChanged("LeaveDetailsListApprov");
            }

        }

        private List<LeaveInfo> leaveDetailsListPending;
        public List<LeaveInfo> LeaveDetailsListPending
        {
            get { return leaveDetailsListPending; }
            set
            {
                leaveDetailsListPending = value;
                RaisePropertyChanged("LeaveDetailsListPending");
            }

        }

        private List<LeaveInfo> leaveDetailsListRejec;
        public List<LeaveInfo> LeaveDetailsListRejec
        {
            get { return leaveDetailsListRejec; }
            set
            {
                leaveDetailsListRejec = value;
                RaisePropertyChanged("LeaveDetailsListRejec");
            }

        }

        private List<UserInfo> _lst;
        public List<UserInfo> lst
        {
            get { return _lst; }
            set
            {
                _lst = value;
                RaisePropertyChanged("lst");
            }

        }

        private bool _status;
        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        private void ExecuteSave()
        {
            // var rows = GetDataGridRows(window.empleavedatgridpend);
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

        private string _nameSelected;
        public string NameSelected
        {
            get { return _nameSelected; }
            set
            {
                _nameSelected = value;

                if (value != null)
                {
                    //getEmpPunchDetails();
                }

                RaisePropertyChanged("NameSelected");
            }
        }

        private int _idSelected;
        public int IdSelected
        {
            get { return _idSelected; }
            set
            {
                _idSelected = value;
                getEmpPunchDetails(_idSelected);

                RaisePropertyChanged("IdSelected");
            }
        }

        private List<UserInfo> _empDetailsList;
        public List<UserInfo> EmpDetailsList
        {
            get { return _empDetailsList; }
            set
            {
                _empDetailsList = value;
                RaisePropertyChanged("EmpDetailsList");
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

        private List<UserInfo> list;
        public List<UserInfo> List
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

        private void getAttandanceDetails()
        {
            SelectedDate = MyDateTime.ToString();

            EmpID = 1;

            //svc.GetAttendanceDetailsAsync(EmpID, SelectedDate, null);
        }

        private void getEmpPunchDetails(int empId)
        {
            //EmpPunchErrMsg = "";
            //EmpDetailsList = null;

            //UserInfo ui = null;
            //List<UserInfo> tempList = new List<UserInfo>();
            //try
            //{
            //    svc.GetEmployeeAttendanceDataCompleted -= new EventHandler<GetEmployeeAttendanceDataCompletedEventArgs>(GetEmpAttDataCmpltd);
            //    svc.GetEmployeeAttendanceDataCompleted += new EventHandler<GetEmployeeAttendanceDataCompletedEventArgs>(GetEmpAttDataCmpltd);

            //    svc.GetEmployeeAttendanceDataAsync(empId, false, null);
            //}
            //catch (Exception ex)
            //{

            //}
        }

        //private void GetEmpAttDataCmpltd(object sender, GetEmployeeAttendanceDataCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        Helpers.UserLoginStatus.EmpDetailsList = e.Result.ToList();

        //        if (e.Result.Count != 0)
        //        {
        //            foreach (var item in e.Result.ToList())
        //            {
        //                item.CurrentDate = item.PunchinTime.Split(' ')[0];
        //            }
        //            window.empdetailsgrid.Visibility = System.Windows.Visibility.Visible;
        //            EmpDetailsList = e.Result.ToList();
        //        }
        //        else
        //        {
        //            EmpPunchErrMsg = "No Records Found!";
        //            // this.window.empdetailsgrid.Visibility = System.Windows.Visibility.Collapsed;
        //        }
        //    }
        //}

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

        private string _success_msg;
        public string Success_msg
        {
            get { return _success_msg; }
            set
            {
                _success_msg = value;
                RaisePropertyChanged("Success_msg");
            }
        }

        private string _EmpPunchErrMsg;
        public string EmpPunchErrMsg
        {
            get { return _EmpPunchErrMsg; }
            set
            {
                _EmpPunchErrMsg = value;
                RaisePropertyChanged("EmpPunchErrMsg");
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }

    }
}

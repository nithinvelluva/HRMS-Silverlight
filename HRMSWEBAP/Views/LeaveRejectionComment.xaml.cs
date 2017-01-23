using HRMSWEBAP.HrmsReference;
using SilverlightMessageBoxLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRMSWEBAP.Views
{
    public partial class LeaveRejectionComment : ChildWindow
    {
        HrmsServiceClient svc = new HrmsServiceClient();
        ObservableCollection<HrmsReference.LeaveInfo> temp = new ObservableCollection<HrmsReference.LeaveInfo>();
        HrmsReference.LeaveInfo linfo = new HrmsReference.LeaveInfo();
        public LeaveRejectionCommentViewModel ViewModel;

        public LeaveRejectionComment(HrmsReference.LeaveInfo lvinfo)
        {
            InitializeComponent();
            ViewModel = new LeaveRejectionCommentViewModel(this);
            DataContext = ViewModel;
            linfo = lvinfo;

            FromDatePicker.SelectedDate = Convert.ToDateTime(lvinfo.FromDate);
            ToDatePicker.SelectedDate = Convert.ToDateTime(lvinfo.ToDate);

            switch (linfo.Leavedurationtype)
            {
                case "Full Day": linfo.LeavedurTypInt = 1;
                    break;
                case "Half Day": linfo.LeavedurTypInt = 2;
                    break;
                default:
                    break;
            }

            ViewModel.LvDurTypSel = linfo.LeavedurTypInt;
            ViewModel.LvDurTypStr = linfo.Leavedurationtype;

            lvdurTypComboBx.SelectedValue = linfo.LeavedurTypInt;

            lvstatusblk.Text = lvinfo.LeaveStatus;
            switch (lvinfo.LeaveStatus)
            {
                case "APPROVED":
                    lvstatusblk.Foreground = new SolidColorBrush(Colors.Green);
                    Addcmntsbtn.Visibility = Visibility.Collapsed;
                    lv_cancel_chkBx.Visibility = Visibility.Collapsed;
                    break;
                case "PENDING":
                    lvstatusblk.Foreground = new SolidColorBrush(Colors.Orange);
                    Addcmntsbtn.Visibility = Visibility.Visible;
                    lv_cancel_chkBx.Visibility = Visibility.Visible;
                    break;
                case "REJECTED":
                    lvstatusblk.Foreground = new SolidColorBrush(Colors.Red);
                    Addcmntsbtn.Visibility = Visibility.Collapsed;
                    cntctadminbtn.Visibility = Visibility.Visible;
                    lv_cancel_chkBx.Visibility = Visibility.Collapsed;
                    break;
                case "CANCELLED":
                    lvstatusblk.Foreground = new SolidColorBrush(Colors.Red);
                    Addcmntsbtn.Visibility = Visibility.Collapsed;
                    cntctadminbtn.Visibility = Visibility.Collapsed;
                    lv_cancel_chkBx.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }

            cmntblk.Text = lvinfo.Comments;
            cmntblk.IsEnabled = false;
            Update.Visibility = Visibility.Collapsed;
        }

        private void EnableFields()
        {
            FromDatePicker.IsHitTestVisible = true;
            ToDatePicker.IsHitTestVisible = true;
            lvdurTypComboBx.IsHitTestVisible = true;
            lv_cancel_chkBx.IsHitTestVisible = true;
            cmntblk.IsEnabled = true;
        }

        private void DisableFields()
        {
            FromDatePicker.IsHitTestVisible = false;
            ToDatePicker.IsHitTestVisible = false;
            lvdurTypComboBx.IsHitTestVisible = false;
            lv_cancel_chkBx.IsHitTestVisible = false;
            cmntblk.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Addcmntsbtn_Click(object sender, RoutedEventArgs e)
        {
            EnableFields();
            Addcmntsbtn.Visibility = Visibility.Collapsed;
            Update.Visibility = Visibility.Visible;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            temp.Clear();

            if (string.IsNullOrEmpty(FromDatePicker.SelectedDate.ToString()) || string.IsNullOrEmpty(ToDatePicker.SelectedDate.ToString()) || ViewModel.LvDurTypSel <= 0)
            {
                new CustomMessage("Select Mandatory Fields!!", CustomMessage.MessageType.Error).Show();
            }
            else
            {
                if (FromDatePicker.SelectedDate > ToDatePicker.SelectedDate)
                {
                    new CustomMessage("To Date Should be greater than or equal to From Date!!", CustomMessage.MessageType.Error).Show();
                }
                else
                {
                    DateTime frmDt = Convert.ToDateTime(FromDatePicker.SelectedDate);
                    DateTime toDt = Convert.ToDateTime(ToDatePicker.SelectedDate);

                    linfo.EmpId = HRMSWEBAP.Helpers.UserLoginStatus.currentEmpId;
                    linfo.FromDate = FromDatePicker.SelectedDate.ToString();
                    linfo.ToDate = ToDatePicker.SelectedDate.ToString();

                    linfo.LeavedurTypInt = ViewModel.LvDurTypSel;

                    linfo.Leavedurationtype = (lvdurTypComboBx.SelectedItem as HRMSWEBAP.HrmsReference.LeaveInfo).Leavedurationtype;

                    double leaves = 0.0;
                    if (linfo.LeavedurTypInt == 2)
                    {
                        leaves = (toDt - frmDt).TotalDays;
                        leaves = 0.5 + (0.5 * leaves);
                    }
                    else if (linfo.LeavedurTypInt == 1)
                    {
                        leaves = (toDt - frmDt).TotalDays + 1;
                    }

                    linfo.Cancelled = lv_cancel_chkBx.IsChecked.Value;
                    if (linfo.Cancelled)
                    {
                        leaves = 0.0;
                    }

                    linfo.Days = leaves;

                    switch (linfo.LeaveType.TrimEnd())
                    {
                        case "Casual": linfo.CasualLeave = leaves;
                            break;
                        case "Festive": linfo.FestiveLeave = leaves;
                            break;
                        case "Sick": linfo.SickLeave = leaves;
                            break;
                        default:
                            break;
                    }

                    linfo.Comments = cmntblk.Text;
                    temp.Add(linfo);

                    svc.UpdateLeaveDetailsCompleted -= new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);
                    svc.UpdateLeaveDetailsCompleted += new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);

                    svc.UpdateLeaveDetailsAsync(temp, false);
                }
            }

            Addcmntsbtn.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Collapsed;

            DisableFields();
        }

        private void UpdateLeaveDetailsResult(object sender, UpdateLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                CRUDStatus cs = e.Result;

                if (cs.IsSuccessful)
                {
                    new CustomMessage("Updated.", CustomMessage.MessageType.Info).Show();
                    Addcmntsbtn.Visibility = Visibility.Visible;
                    Update.Visibility = Visibility.Collapsed;
                    cmntblk.IsEnabled = false;
                    DisableFields();

                    ReturnString = "OK";
                }
                else
                {
                    if (!string.IsNullOrEmpty(cs.ErrorMessage))
                    {
                        new CustomMessage(cs.ErrorMessage, CustomMessage.MessageType.Error).Show();
                    }
                }
                Close();
            }
        }

        private void cntctadminbtn_Click(object sender, RoutedEventArgs e)
        {
            ContactAdmin cd = new ContactAdmin();
            cd.Show();
        }

        public string ReturnString { get; set; }
    }

    public class LeaveRejectionCommentViewModel
    {
        private LeaveRejectionComment window;
        public event PropertyChangedEventHandler PropertyChanged;

        public LeaveRejectionCommentViewModel(LeaveRejectionComment leaveRejectionComment)
        {
            window = leaveRejectionComment;
            HRMSWEBAP.HrmsReference.LeaveInfo li = null;
            li = new HRMSWEBAP.HrmsReference.LeaveInfo();

            li.Leavedurationtype = "Full Day";
            li.LeavedurTypInt = 1;
            LvDurTypList = new List<HRMSWEBAP.HrmsReference.LeaveInfo>();
            LvDurTypList.Add(li);

            li = new HRMSWEBAP.HrmsReference.LeaveInfo();
            li.Leavedurationtype = "Half Day";
            li.LeavedurTypInt = 2;
            LvDurTypList.Add(li);
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private int _LvDurTypSel;
        public int LvDurTypSel
        {
            get { return _LvDurTypSel; }
            set
            {
                _LvDurTypSel = value;
                RaisePropertyChanged("LvDurTypSel");
            }
        }

        private string _LvDurTypStr;
        public string LvDurTypStr
        {
            get { return _LvDurTypStr; }
            set
            {
                _LvDurTypStr = value;
                RaisePropertyChanged("_LvDurTypStr");
            }
        }

        private List<HRMSWEBAP.HrmsReference.LeaveInfo> _lvDurTypList;
        public List<HRMSWEBAP.HrmsReference.LeaveInfo> LvDurTypList
        {
            get { return _lvDurTypList; }
            set
            {
                _lvDurTypList = value;
                RaisePropertyChanged("LvDurTypList");
            }
        }

        private bool cancelChangeFlag = false;

        public bool CancelChangeFlag
        {
            get { return cancelChangeFlag; }
            set
            {
                //Here you mimic your Toggled event calling what you want!
                if (!CancelChangeFlag)
                {
                    CustomMessage cnfrm = new CustomMessage("Are You Sure Want To Cancel ?", CustomMessage.MessageType.Confirm, null);
                    cnfrm.OKButton.Click += (obj, args) =>
                    {
                        
                    };
                    cnfrm.CancelButton.Click += (obj, args) =>
                    {
                        window.lv_cancel_chkBx.IsChecked = false;
                    };
                    cnfrm.Show();
                }
                cancelChangeFlag = value;
            }
        }

    }
}


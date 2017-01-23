using HRMSWEBAP.HrmsReference;
using SilverlightMessageBoxLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace HRMSWEBAP.Views
{
    public partial class AdminPage : Page
    {
        public AdminPageViewModel viewModel;
        List<string> nameList = new List<string>();
        int rowIndex = -1;
        //List<HrmsReference.UserInfo> temp;
        HrmsServiceClient svc = new HrmsServiceClient();

        public AdminPage()
        {
            InitializeComponent();
            viewModel = new AdminPageViewModel(this);
            DataContext = viewModel;

            svc.GetEmpPunchDetailsCompleted -= new EventHandler<HrmsReference.GetEmpPunchDetailsCompletedEventArgs>(EmpPunchResult);
            svc.GetEmpPunchDetailsCompleted += new EventHandler<HrmsReference.GetEmpPunchDetailsCompletedEventArgs>(EmpPunchResult);

            svc.GetEmpPunchDetailsAsync();
            //svc.GetDataCompleted -= new EventHandler<GetDataCompletedEventArgs>(GetEmpDataResult);
            //svc.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(GetEmpDataResult);

            //svc.GetDataAsync(null, false);            
        }

        //private void GetEmpDataResult(object sender, GetDataCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        temp =new List<UserInfo>();
        //        temp= e.Result.ToList();
        //        getName(temp);

        //        svc.GetEmpPunchDetailsAsync();
        //    }
        //}

        private void EmpPunchResult(object sender, GetEmpPunchDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.UserLoginStatus.EmpDetailsList = e.Result.ToList();                
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void getName(List<HrmsReference.UserInfo> temp)
        {
            ////var TempnameList = Helpers.UserLoginStatus.EmpDetailsList.GroupBy(item => item.Name).Select(grp => grp.First())
            ////       .ToList();          

            //foreach (var item in TempnameList)
            //{
            //    nameList.Add(item.Name.ToString());
            //}

            //nameList.Clear();
            //foreach (var item in temp)
            //{
            //    if (item.Utype != 1)
            //    {
            //        nameList.Add(item.Name.ToString());
            //    }
            //}

            //namecombobx.ItemsSource = nameList;
        }

        private void refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            //nameList.Clear();
            //svc.GetDataAsync(null, false);
        }

        private void apprvbtn_Click(object sender, RoutedEventArgs e)
        {

            rowIndex = empleavedatgridpend.SelectedIndex;

            ObservableCollection<HrmsReference.LeaveInfo> temp = new ObservableCollection<HrmsReference.LeaveInfo>();
            HrmsReference.LeaveInfo li = new HrmsReference.LeaveInfo();
            li = viewModel.LeaveDetailsListPending[rowIndex];
            
            li.Status = true;//leave approved.
            li.Rejected = false;

            temp.Add(li);           

            svc.UpdateLeaveDetailsCompleted -= new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);
            svc.UpdateLeaveDetailsCompleted += new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);

            svc.UpdateLeaveDetailsAsync(temp, true);

        }

        private void UpdateLeaveDetailsResult(object sender, UpdateLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                CRUDStatus cs = e.Result;
                if (cs.IsSuccessful)
                {
                    svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);
                    svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(GetEmployeeLeaveDetailsResult);

                    svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, null);
                }
            }
        }       

        private void GetEmployeeLeaveDetailsResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                // ObservableCollection<LeaveInfo> linfo = new ObservableCollection<LeaveInfo>();
                Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();
                viewModel.SetLeaveSource();

                CustomMessage message = new CustomMessage("Updated.", CustomMessage.MessageType.Info);

                message.OKButton.Click += (obj, args) =>
                {
                    message.Close();
                };
                message.Show();
            }
        }

        private void rejectvbtn_Click(object sender, RoutedEventArgs e)
        {
            rowIndex = empleavedatgridpend.SelectedIndex;

            ObservableCollection<HrmsReference.LeaveInfo> temp = new ObservableCollection<HrmsReference.LeaveInfo>();
            HrmsReference.LeaveInfo li = new HrmsReference.LeaveInfo();
            li = viewModel.LeaveDetailsListPending[rowIndex];
            li.Status = false;
            li.Rejected = true;            //leave rejected. 
            
            CustomMessage inputMessage = new CustomMessage("Enter Comments", CustomMessage.MessageType.TextInput);

            inputMessage.OKButton.Click += (obj, args) =>
            {
                li.Comments = inputMessage.Input;
                temp.Add(li);
                svc.UpdateLeaveDetailsCompleted -= new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);
                svc.UpdateLeaveDetailsCompleted += new EventHandler<UpdateLeaveDetailsCompletedEventArgs>(UpdateLeaveDetailsResult);

                svc.UpdateLeaveDetailsAsync(temp, true);
            };

            inputMessage.Show();
        }

        private void exportbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reviewbtn_Click(object sender, RoutedEventArgs e)
        {
            rowIndex = empleavedatgridpend.SelectedIndex;
            if (rowIndex != -1)
            {
                HRMSWEBAP.HrmsReference.LeaveInfo li = new HRMSWEBAP.HrmsReference.LeaveInfo();
                li = viewModel.LeaveDetailsListPending[rowIndex];

                LeaveRejectionComment obj = new LeaveRejectionComment(li);
                obj.Show();

            }
        }

        private void btnrprt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvdtlsbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //AddEmployee ade = new AddEmployee();
            //ade.Show();
        }

        private void updtbtn_Click(object sender, RoutedEventArgs e)
        {
            if (usrgrd.SelectedIndex > -1)
            {
                UserInfo ui = new UserInfo();
                ui = viewModel.lst[usrgrd.SelectedIndex];
                UpdateEmpData ued = new UpdateEmpData(ui);
                ued.Show();
            }
        }

        private void btndlt_Click(object sender, RoutedEventArgs e)
        {
            if (usrgrd.SelectedIndex > -1)
            {
                UserInfo ui = new UserInfo();
                ui.EmpID = viewModel.lst[usrgrd.SelectedIndex].EmpID;

                CustomMessage msg = new CustomMessage("Do you want the delete the User?", CustomMessage.MessageType.Confirm);
                msg.OKButton.Click += (obj, args) =>
                    {
                        svc.RemoveEmployeeCompleted -= new EventHandler<RemoveEmployeeCompletedEventArgs>(RemoveCmpltd);
                        svc.RemoveEmployeeCompleted += new EventHandler<RemoveEmployeeCompletedEventArgs>(RemoveCmpltd);

                        svc.RemoveEmployeeAsync(ui);
                    };
                msg.CancelButton.Click += (obj, args) =>
                    {
                        msg.Close();
                    };

                msg.Show();
            }
        }

        private void RemoveCmpltd(object sender, RemoveEmployeeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                CustomMessage msg = new CustomMessage("Successfully Removed");
                msg.Show();
                viewModel.GetEmpData();
            }
        }

        private void async_empdatacmpltd(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                viewModel.lst = null;
                viewModel.lst = e.Result.ToList();

                nameList.Clear();
                svc.GetDataAsync(null, false);
            }
        }

        private void empidtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool enteredLetter = false;
            Queue<char> text = new Queue<char>();
            foreach (var ch in empidtxt.Text)
            {
                if (char.IsDigit(ch))
                {
                    text.Enqueue(ch);
                }
                else
                {
                    enteredLetter = true;
                }
            }
            if (enteredLetter)
            {
                StringBuilder sb = new StringBuilder();
                while (text.Count > 0)
                {
                    sb.Append(text.Dequeue());
                }
                empidtxt.Text = sb.ToString();
                empidtxt.SelectionStart = empidtxt.Text.Length;
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            errortxt.Text = "";
            int id = 0;
            string eid = empidtxt.Text;
            if (eid != null && eid != "")
            {
                id = Convert.ToInt32(eid);
            }

            UserInfo ui = new UserInfo();
            ui.EmpID = id;

            svc.GetDataCompleted -= new EventHandler<GetDataCompletedEventArgs>(async_searchcmpltd);
            svc.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(async_searchcmpltd);

            svc.GetDataAsync(ui, true);

        }

        private void async_searchcmpltd(object sender, GetDataCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.Count != 0)
                {
                    ObservableCollection<UserInfo> temp = new ObservableCollection<UserInfo>();

                    temp = e.Result;
                    viewModel.lst = null;
                    viewModel.lst = temp.ToList();
                }
                else
                {
                    errortxt.Text = "No Results Found!";
                    viewModel.lst = null;
                }
            }
        }

        private void refresh_btn_PendLvs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.SetLeaveSource();
        }

        private void refresh_btn_RjcLvs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.SetLeaveSource();
        }

        private void refresh_btn_ApprLvs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.SetLeaveSource();
        }

        private void refresh_btn_EmpData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.GetEmpData();
            errortxt.Text = "";
        }
    }
}

using HRMSWEBAP.Helpers;
using HRMSWEBAP.HrmsReference;
using HRMSWEBAP.ViewModels;
using ImageTools;
using SilverlightMessageBoxLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Telerik.Windows.Media.Imaging;


namespace HRMSWEBAP.Views
{
    public partial class UserPage : Page
    {
        public UserWindowViewModel ViewModel;
        byte[] imgdata = null;
        MemoryStream ms = new MemoryStream();
        System.Windows.Media.Imaging.BitmapImage bImg = null;
        HrmsServiceClient svc = new HrmsServiceClient();
        MailReference.MailServiceClient msvc = new MailReference.MailServiceClient();
        int rowIndex = -1;
        string filePath = "";
        private Byte[] printBuffer;
        private System.Threading.SynchronizationContext printSyncContext;
        private System.IO.Stream printStream;

        public UserPage()
        {
            InitializeComponent();
            ViewModel = new UserWindowViewModel(this);
            DataContext = ViewModel;
            wlcmmsg.Content = "Welcome, " + Helpers.UserLoginStatus.currentName;

            #region Displaying dates

            /*-----------Setting range for FromDate---------------*/

            if (DateTime.Now.Month == 1)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month + 1);

                from_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(0).AddDays(-(DateTime.Now.Day - 1));
                from_date.DisplayDateEnd = System.DateTime.Now.AddMonths(1).AddDays(days - (DateTime.Now.Day));
            }
            else if (DateTime.Now.Month == 12)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                from_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(-1).AddDays(-(DateTime.Now.Day - 1));
                from_date.DisplayDateEnd = System.DateTime.Now.AddDays((days) - (DateTime.Now.Day));
            }
            else
            {
                from_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(-1).AddDays(-(DateTime.Now.Day - 1));
                from_date.DisplayDateEnd = System.DateTime.Now.AddMonths(2);
            }
            /*-------------------------------------------------------*/

            /*-----------Setting range for ToDate--------------------*/

            if (DateTime.Now.Month == 1)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month + 1);

                to_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(0).AddDays(-(DateTime.Now.Day - 1));
                to_date.DisplayDateEnd = System.DateTime.Now.AddMonths(1).AddDays(days - (DateTime.Now.Day));
            }
            else if (DateTime.Now.Month == 12)
            {
                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                to_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(-1).AddDays(-(DateTime.Now.Day - 1));
                to_date.DisplayDateEnd = System.DateTime.Now.AddDays((days) - (DateTime.Now.Day));
            }
            else
            {
                to_date.DisplayDateStart = System.DateTime.Now.AddYears(0).AddMonths(-1).AddDays(-(DateTime.Now.Day - 1));
                to_date.DisplayDateEnd = System.DateTime.Now.AddMonths(2);
            }

            /*-------------------------------------------------------*/
            #endregion
        }


        private void UpdateProfileResult(object sender, UpdateProfileCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                CRUDStatus cs = new CRUDStatus();
                cs = e.Result;
                if (cs.IsSuccessful)
                {
                    //msglabel.Content = "Updated..";
                    //msglabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));                    

                    Edit.Visibility = Visibility.Visible;

                    changeusrphoto.IsEnabled = true;
                    changeusrphoto.Visibility = Visibility.Collapsed;
                    name_textbox.IsEnabled = false;
                    pwd_reg.IsEnabled = false;
                    pwd_cfm_reg.IsEnabled = false;
                    email.IsEnabled = false;
                    phone.IsEnabled = false;

                    update.Visibility = Visibility.Collapsed;

                    CustomMessage succmessage = new CustomMessage("Profile Updated.");

                    succmessage.OKButton.Click += (obj, args) =>
                    {
                        succmessage.Close();
                    };
                    succmessage.Show();

                    pwd_cfm_reg.Password = "";
                    errtxt.Text = "";
                }
                else
                {
                    MessageBox.Show("Error!");
                }
            }
            else
            {
                MessageBox.Show("Error!");
            }
            svc.UpdateProfileCompleted -= new EventHandler<HrmsReference.UpdateProfileCompletedEventArgs>(UpdateProfileResult);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void exportbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        #region
        //private void EmpPucnchDetailsCompleted(object sender, GetEmpPunchDetailsCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        List<HRMSWEBAP.HrmsReference.UserInfo> list = e.Result.ToList();
        //        HRMSWEBAP.Models.AttendanceInfo ai = null;

        //        List<HRMSWEBAP.Models.AttendanceInfo> atList = new List<Models.AttendanceInfo>();
        //        foreach (var item in list)
        //        {
        //            if (Helpers.UserLoginStatus.currentName == item.Name)
        //            {
        //                ai = new Models.AttendanceInfo();

        //                ai.EmpID = item.EmpID;
        //                ai.EmployeeName = item.Name;
        //                ai.PunchinTime = item.PunchinTime;
        //                ai.PunchoutTime = item.PunchoutTime;
        //                ai.WorkingHours = item.Duration;
        //                ai.SelectedDate = item.SelectedDate;

        //                atList.Add(ai);

        //            }

        //        }
        //    }
        //}
        #endregion

        private void leave_info_icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            svc.GetLeaveStatisticsCompleted -= new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetEmpLeaveStatistics);
            svc.GetLeaveStatisticsCompleted += new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetEmpLeaveStatistics);

            svc.GetLeaveStatisticsAsync(Helpers.UserLoginStatus.currentEmpId);
        }

        private void GetEmpLeaveStatistics(object sender, GetLeaveStatisticsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpLeaveStatisticsList = e.Result.ToList();

                Helpers.LeaveInfo.LeaveTypes = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].LeaveTypes.ToList();
                Helpers.LeaveInfo.CasualLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].CasualLeave;
                Helpers.LeaveInfo.FestiveLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].FestiveLeave;
                Helpers.LeaveInfo.SickLeave = Helpers.EmpDetails.CurrEmpLeaveStatisticsList[0].SickLeave;

                LeaveDetails obj = new LeaveDetails();
                obj.Show();
            }

            svc.GetLeaveStatisticsCompleted -= new EventHandler<GetLeaveStatisticsCompletedEventArgs>(GetEmpLeaveStatistics);
        }

        private void to_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.FromDate != null && ViewModel.ToDate != null)
            {
                DurationErrMsg.Content = "";
            }
        }

        private void from_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.FromDate != null && ViewModel.ToDate != null)
            {
                DurationErrMsg.Content = "";
            }
        }

        private void to_date_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void end_date_pick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Edit.Visibility = Visibility.Collapsed;
            update.Visibility = Visibility.Visible;

            changeusrphoto.IsEnabled = true;
            changeusrphoto.Visibility = Visibility.Visible;
            name_textbox.IsEnabled = true;
            pwd_reg.IsEnabled = true;
            pwd_cfm_reg.IsEnabled = true;
            email.IsEnabled = true;
            phone.IsEnabled = true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Helpers.UserLoginStatus.currentName = ViewModel.Name;
            Helpers.UserLoginStatus.currentPhone = ViewModel.PhoneNumber;
            Helpers.UserLoginStatus.currentEmail = ViewModel.Email;

            errtxt.Text = "";

            if (imgdata != null && imgdata.Length > 49999)
            {
                errtxt.Text = "*Image size is more than 50kb!)";

            }
            else
            {
                if (pwd_reg.Password.Equals(pwd_cfm_reg.Password))
                {
                    Helpers.UserLoginStatus.currentPassword = pwd_reg.Password;

                    HrmsReference.UserInfo ui = new HrmsReference.UserInfo();
                    ObservableCollection<HrmsReference.UserInfo> uinfo = new ObservableCollection<HrmsReference.UserInfo>();
                    ui.EmpID = Helpers.UserLoginStatus.currentEmpId;
                    ui.Name = Helpers.UserLoginStatus.currentName;
                    ui.Number = Helpers.UserLoginStatus.currentPhone;
                    ui.Mail = Helpers.UserLoginStatus.currentEmail;

                    if (imgdata == null)
                    {
                        ui.ImgData = Helpers.UserLoginStatus.currentUserImgData;
                    }
                    else
                    {
                        ui.ImgData = imgdata;
                    }

                    ui.Pwd = Helpers.UserLoginStatus.currentPassword;

                    uinfo.Add(ui);

                    svc.UpdateProfileCompleted -= new EventHandler<HrmsReference.UpdateProfileCompletedEventArgs>(UpdateProfileResult);
                    svc.UpdateProfileCompleted += new EventHandler<HrmsReference.UpdateProfileCompletedEventArgs>(UpdateProfileResult);

                    svc.UpdateProfileAsync(uinfo);

                }
                else
                {
                    CustomMessage errormessage = new CustomMessage("Password Mismatch!", CustomMessage.MessageType.Error);
                    errormessage.Show();
                }
            }
        }

        private void changeusrphoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                bool? userClickedOK = ofd.ShowDialog();
                // Process input if the user clicked OK.
                if (userClickedOK == true)
                {
                    // Open the selected file to read.
                    System.IO.Stream fileStream = ofd.File.OpenRead();
                    FileStream fStream = ofd.File.OpenRead();
                    BinaryReader br = new BinaryReader(fStream);
                    byte[] data = br.ReadBytes((int)fStream.Length);
                    imgdata = data;
                    ms = new MemoryStream(imgdata);
                    bImg = new BitmapImage();
                    bImg.SetSource(ms);
                    usrimage.Source = bImg;
                    //  txtimg.Text = ofd.File.Name;
                }
            }
            catch (Exception ex)
            {
                CustomMessage errormessage = new CustomMessage(ex.Message, CustomMessage.MessageType.Error);
                errormessage.Show();

            }
            finally
            {

            }
        }

        private void applied_leave_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //rowIndex = applied_leave_datagrid.SelectedIndex;
            //if (rowIndex != -1)
            //{
            //    LeaveInfo li = new LeaveInfo();
            //    li = this.ViewModel.LeaveDetailsList[rowIndex];

            //    if (li.Rejected)
            //    {
            //        LeaveRejectionComment obj = new LeaveRejectionComment(li);
            //        obj.Show();
            //    }
            //}

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void duration_typ_cmbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DurationErrMsg.Content = "";
                double leaves = 0;
                if (ViewModel.FromDate != null && ViewModel.ToDate != null)
                {
                    DateTime from = Convert.ToDateTime(ViewModel.FromDate);
                    DateTime to = Convert.ToDateTime(ViewModel.ToDate);

                    if (ViewModel.LeaveDurationType == "Half Day")
                    {
                        leaves = (to - from).TotalDays;
                        leaves = 0.5 + (0.5 * leaves);
                    }
                    else if (ViewModel.LeaveDurationType == "Full Day")
                    {
                        leaves = (to - from).TotalDays + 1;
                    }

                    ViewModel.LeaveDays = leaves;
                }
                else
                {
                    DurationErrMsg.Content = "Select From Date And To Date!";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void refreshbtn_Click(object sender, RoutedEventArgs e)
        {
            // HtmlPage.Document.Submit();
        }

        private void statuslink_Click(object sender, RoutedEventArgs e)
        {
            rowIndex = applied_leave_datagrid.SelectedIndex;
            if (rowIndex != -1)
            {
                HRMSWEBAP.HrmsReference.LeaveInfo li = new HRMSWEBAP.HrmsReference.LeaveInfo();
                li = ViewModel.LeaveDetailsList[rowIndex];

                LeaveRejectionComment lrc = new LeaveRejectionComment(li);

                lrc.Closed += new EventHandler(leave_rej_window_Closed);
                lrc.Show();
            }
        }

        private void leave_rej_window_Closed(object sender, EventArgs e)
        {
            LeaveRejectionComment lw = (LeaveRejectionComment)sender;

            //if (lw.DialogResult == true && lw.ReturnString != string.Empty)
            //{
            //    this.helloTxt.Text = "Hello " + lw.nameBox.Text;
            //}
            //else if (lw.DialogResult == false)
            //{
            //    this.helloTxt.Text = "Login canceled.";
            //}

            if (!string.IsNullOrEmpty(lw.ReturnString) && lw.ReturnString.Equals("OK"))
            {
                HRMSWEBAP.HrmsReference.LeaveInfo linfo = new HRMSWEBAP.HrmsReference.LeaveInfo();

                var strtDate = Helpers.SupportMethods.GetFirstDayOfMonth(ViewModel.sel_lv_search_month + 1, ViewModel.yearlst[ViewModel.sel_lv_search_year]);
                linfo.FromDate = strtDate.ToString();
                linfo.ToDate = strtDate.AddMonths(1).AddDays(-1).ToString();

                svc.GetEmployeeLeaveDetailsCompleted -= new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveSearchResult);
                svc.GetEmployeeLeaveDetailsCompleted += new EventHandler<GetEmployeeLeaveDetailsCompletedEventArgs>(LeaveSearchResult);
                svc.GetEmployeeLeaveDetailsAsync(Helpers.UserLoginStatus.currentEmpId, Helpers.UserLoginStatus.currentUserType, linfo);
            }
        }

        private void LeaveSearchResult(object sender, GetEmployeeLeaveDetailsCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Helpers.EmpDetails.CurrEmpLeaveDetailsList = e.Result.ToList();
                applied_leave_datagrid.ItemsSource = null;
                lvDateTxtBlk.Text = "Showing Reports Of " + ViewModel.mnthlst[ViewModel.sel_lv_search_month] + " , " + ViewModel.yearlst[ViewModel.sel_lv_search_year];

                if (Helpers.EmpDetails.CurrEmpLeaveDetailsList == null || (Helpers.EmpDetails.CurrEmpLeaveDetailsList != null && Helpers.EmpDetails.CurrEmpLeaveDetailsList.Count() == 0))
                {
                    ViewModel.MyLeaveErrorMsg = "No Records found!";
                    applied_leave_datagrid.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    applied_leave_datagrid.ItemsSource = Helpers.EmpDetails.CurrEmpLeaveDetailsList;

                    ViewModel.MyLeaveErrorMsg = "";
                    applied_leave_datagrid.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void mnthcmbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mnthcmbbx.SelectedIndex > -1)
            {
                ViewModel.seltdmnth = Convert.ToInt32(mnthcmbbx.SelectedIndex.ToString());
            }
        }

        private void reprttypecmbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reprttypecmbbx.SelectedIndex > -1)
            {
                ViewModel.selttyp = Convert.ToInt32(reprttypecmbbx.SelectedIndex.ToString());
            }

        }

        private void yearcmbbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (yearcmbbx.SelectedIndex > -1)
            {
                ViewModel.seltdyear = Convert.ToInt32(yearcmbbx.SelectedIndex.ToString());
            }
        }

        private void exprtpdf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog fsd = new SaveFileDialog();
            fsd.Filter = "PDF (*.pdf)|*.pdf";

            if (fsd.ShowDialog() == true)
            {
                CreateExportDataBuffer();

                switch (fsd.FilterIndex)
                {
                    case 1:
                        HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(new Uri(Application.Current.Host.Source.ToString().Substring(0, Application.Current.Host.Source.ToString().LastIndexOf("/")) + "/Png2Pdf.ashx"));
                        hwr.Method = "POST";

                        printStream = fsd.OpenFile();
                        printSyncContext = System.Threading.SynchronizationContext.Current;
                        hwr.BeginGetRequestStream(new AsyncCallback(PrintStart), hwr);

                        break;
                    case 2:
                        //Save the PNG to local disk
                        //System.IO.Stream fs = fsd.OpenFile();
                        //fs.Write(printBuffer, 0, printBuffer.Length);
                        //fs.Close();
                        //MessageBox.Show("You PNG has been created.", "Export Complete", MessageBoxButton.OK);
                        break;
                }

            }
        }

        void CreateExportDataBuffer()
        {
            //Create WriteableBitmap object which is what is being exported.
            WriteableBitmap wBitmap = new WriteableBitmap(cnvContainer, null);
            int hgt = wBitmap.PixelHeight;
            int wdth = wBitmap.PixelWidth;

            //Create EditableImage oblect and iterrate through WriteableBitmap pixels to set EditableImage pixels
            EditableImage ei = new EditableImage(wdth, hgt);

            for (int y = 0; y < hgt; y++)
            {
                for (int x = 0; x < wdth; x++)
                {
                    int pixel = wBitmap.Pixels[((y * wdth) + x)];
                    ei.SetPixel(x, y, (byte)((pixel >> 16) & 0xff), (byte)((pixel >> 8) & 0xff), (byte)(pixel & 0xff), (byte)((pixel >> 24) & 0xff));
                }
            }

            //Get the stream from the encoder and create byte array from it
            System.IO.Stream pngStream = ei.GetStream();

            printBuffer = new Byte[pngStream.Length];
            pngStream.Read(printBuffer, 0, printBuffer.Length);

        }

        private void PrintStart(IAsyncResult asynchronousResult)
        {
            HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
            System.IO.Stream stream = (System.IO.Stream)hwr.EndGetRequestStream(asynchronousResult);

            stream.Write(printBuffer, 0, printBuffer.Length);
            stream.Close();

            hwr.BeginGetResponse(new AsyncCallback(PrintGetResponse), hwr);
        }

        private void PrintGetResponse(IAsyncResult asynchronousResult)
        {
            HttpWebRequest hwr = (HttpWebRequest)asynchronousResult.AsyncState;
            WebResponse resp = hwr.EndGetResponse(asynchronousResult);
            System.IO.Stream respStream = resp.GetResponseStream();
            Byte[] respBytes = new Byte[respStream.Length];

            respStream.Read(respBytes, 0, respBytes.Length);

            printSyncContext.Post(PrintMergeThreads, new HttpWebRequestData(hwr, respBytes));
        }

        private void PrintMergeThreads(object state)
        {
            HttpWebRequestData hwrd = (HttpWebRequestData)state;

            printStream.Write(hwrd.Data, 0, hwrd.Data.Length);
            printStream.Close();
            printStream = null;
            printSyncContext = null;

            // MessageBox.Show("Your PDF has been created.", "Export Complete", MessageBoxButton.OK);
            CustomMessage cmd = new CustomMessage("Your PDF has been created.", CustomMessage.MessageType.Info);
            cmd.Show();
        }

        private void exprtword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void exprtexcel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (reportgrid.ItemsSource != null)
            //{
            //    DataGridExtensions.ExportDataGrid(reportgrid);
            //}
        }

        private void printbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //PrintDocument pd = new PrintDocument();

            //pd.PrintPage += (s, et) =>
            //{
            //    et.PageVisual = cnvContainer;
            //};

            //pd.Print("MainPageContent");

            PrintPreview pw = new PrintPreview(ViewModel.AttendanceReport);
            pw.Show();
        }

        private void exprtpng_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //To create a out image out of an canvas just use the following code:
            ExtendedImage myImage = cnvContainer.ToImage();

            //Then use the SaveFileDialog to write the image to a file:

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Image Files (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                CreateExportDataBuffer();

                System.IO.Stream fs = dialog.OpenFile();
                fs.Write(printBuffer, 0, printBuffer.Length);
                fs.Close();

                // MessageBox.Show(, "Export Complete", MessageBoxButton.OK);
                CustomMessage cmd = new CustomMessage("You PNG has been created.", CustomMessage.MessageType.Info);
                cmd.Show();
            }
        }

        private void mailbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            busyShow.IsBusy = true;

            filePath = CreateCanvasToImage();
            if (filePath != null)
            {
                //ObservableCollection<byte[]> array = new ObservableCollection<byte[]>();
                //array.Add(imgArr);
                //if (Helpers.UserLoginStatus.currentEmpId == 47 || Helpers.UserLoginStatus.currentEmpId == 18)
                //{
                //    System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                //    myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30000); // 30000 Milliseconds 
                //    myDispatcherTimer.Tick += new EventHandler(Each_Tick);
                //    myDispatcherTimer.Start();
                //}

                //else
                //{
                msvc.SendEmailCompleted -= new EventHandler<MailReference.SendEmailCompletedEventArgs>(EmailSent);
                msvc.SendEmailCompleted += new EventHandler<MailReference.SendEmailCompletedEventArgs>(EmailSent);

                msvc.SendEmailAsync(Helpers.UserLoginStatus.currentName, Helpers.UserLoginStatus.currentEmail, null, filePath);
                //}
            }
        }

        //public void Each_Tick(object o, EventArgs sender)
        //{
        //    msvc.SendEmailCompleted -= new EventHandler<MailReference.SendEmailCompletedEventArgs>(EmailSent);
        //    msvc.SendEmailCompleted += new EventHandler<MailReference.SendEmailCompletedEventArgs>(EmailSent);

        //    msvc.SendEmailAsync(Helpers.UserLoginStatus.currentName, Helpers.UserLoginStatus.currentEmail, null, filePath);
        //}

        private void EmailSent(object sender, MailReference.SendEmailCompletedEventArgs e)
        {
            if (e.Result)
            {
                busyShow.IsBusy = false;

                CustomMessage msg = new CustomMessage("Email Sent Successfully.", CustomMessage.MessageType.Info);
                msg.Show();
            }
            else
            {
                busyShow.IsBusy = false;
                CustomMessage msgerr = new CustomMessage("Email Not Sent!", CustomMessage.MessageType.Error);
                msgerr.Show();
            }
        }

        private string CreateCanvasToImage()
        {
            try
            {
                WriteableBitmap bmp = new WriteableBitmap(cnvContainer, null);

                // Init buffer
                int w = bmp.PixelWidth;
                int h = bmp.PixelHeight;
                int[] p = bmp.Pixels;
                int len = p.Length;
                byte[] result = new byte[4 * w * h];

                // Copy pixels to buffer
                for (int i = 0, j = 0; i < len; i++, j += 4)
                {
                    int color = p[i];
                    result[j + 0] = (byte)(color >> 24); // A
                    result[j + 1] = (byte)(color >> 16); // R
                    result[j + 2] = (byte)(color >> 8);  // G
                    result[j + 3] = (byte)(color);       // B
                }


                string year = ViewModel.yearlst[ViewModel.seltdyear].ToString();
                string month = ViewModel.mnthlst[ViewModel.seltdmnth];

                string attachmentPath = Environment.CurrentDirectory + @"\" + Helpers.UserLoginStatus.currentName + "_" + month + " - " + year + ".png";

                if (File.Exists(attachmentPath))
                {
                    File.Delete(attachmentPath);

                    //string newName = attachmentPath.Split('.')[0] + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day+ ".png";
                    //File.Copy(attachmentPath, newName);
                }

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

                //save to memory stream
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                pngEncoder.Save(ms);
                ms.Close();
                System.IO.File.WriteAllBytes(attachmentPath, ms.ToArray());

                return attachmentPath;
            }

            catch (Exception ex)
            {
                busyShow.IsBusy = false;
                CustomMessage msg = new CustomMessage("An Unexpected Error Occured!", CustomMessage.MessageType.Error);
                msg.Show();

                return null;
            }
        }

        private void lv_year_cmbBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_year_cmbBx.SelectedIndex > -1)
            {
                ViewModel.sel_lv_search_year = Convert.ToInt32(lv_year_cmbBx.SelectedIndex.ToString());
            }
        }

        private void lv_month_cmbBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_month_cmbBx.SelectedIndex > -1)
            {
                ViewModel.sel_lv_search_month = Convert.ToInt32(lv_month_cmbBx.SelectedIndex.ToString());
            }
        }        
    }
}

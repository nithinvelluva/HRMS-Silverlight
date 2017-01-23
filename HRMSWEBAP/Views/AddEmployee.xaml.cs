using HRMSWEBAP.HrmsReference;
using HRMSWEBAP.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HRMSWEBAP.Views
{
    public partial class AddEmployee : ChildWindow
    {
        HrmsServiceClient svc = new HrmsServiceClient();
        ObservableCollection<UserInfo> uinfo;

        byte[] imgdata = null;
        AddEmployeeViewModel advm;
        AdminPageViewModel apvm;
        public AddEmployee()
        {
            InitializeComponent();

            advm = new AddEmployeeViewModel();
            apvm = new AdminPageViewModel();
            DataContext = advm;

            CreateuserType();
        }

        private void CreateuserType()
        {
            List<string> usrtypes = new List<string>();

            usrtypes.Add("ADMIN");
            usrtypes.Add("USER");

            usertype.ItemsSource = null;
            usertype.ItemsSource = usrtypes;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (rbfemale.IsChecked.Value)
            {
                txtsex.Text = "F";
            }
            else if (rbmale.IsChecked.Value)
            {
                txtsex.Text = "M";
            }

            if (imgdata == null)
            {
                imgdata = Icons.Resource.usravatar;
            }

            var bindingExpression = txtname.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtmail.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtmob.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtnation.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtpos.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtusr.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtpwd1.GetBindingExpression(PasswordBox.PasswordProperty);
            bindingExpression.UpdateSource();
            bindingExpression = txtpwd2.GetBindingExpression(PasswordBox.PasswordProperty);
            bindingExpression.UpdateSource();

            if (string.IsNullOrEmpty(txtname.Text) || string.IsNullOrEmpty(txtmail.Text) || string.IsNullOrEmpty(txtmob.Text)
                || string.IsNullOrEmpty(txtnation.Text) || string.IsNullOrEmpty(txtpos.Text) || string.IsNullOrEmpty(txtusr.Text) == null
                || string.IsNullOrEmpty(txtpwd1.Password) || string.IsNullOrEmpty(txtpwd2.Password))
            {
                errortxt.Text = "*Please fill the required fields!";
            }

            else if (txtpwd1.Password != txtpwd2.Password)
            {
                errortxt.Text = "*Password Mismatch!";
            }
            else if (txtpwd1.Password.Length < 6 || txtpwd1.Password.Length > 15)
            {
                errortxt.Text = "*Please check your Password!";
            }
            else if (imgdata.Length > 49999)
            {
                errortxt.Text = "*Image size is more than 50kb!)";
            }
            else
            {
                //svc.AddEmployeeCompleted -= new EventHandler<HrmsReference.AddEmployeeCompletedEventArgs>(AddEmployeeCmpltd);
                //svc.AddEmployeeCompleted += new EventHandler<HrmsReference.AddEmployeeCompletedEventArgs>(AddEmployeeCmpltd);

                UserInfo ui = new UserInfo();
                ui.Name = txtname.Text;
                ui.UserName = txtusr.Text;
                ui.Pwd = txtpwd1.Password;
                ui.Number = txtmob.Text;
                ui.Mail = txtmail.Text;
                ui.Dob = dobPicker.SelectedDate.Value.ToString().Split(' ')[0]; ;

                ui.Gender = txtsex.Text;

                if (usertype.SelectedItem.Equals("ADMIN"))
                {
                    ui.Utype = 1;
                }
                else if (usertype.SelectedItem.Equals("USER"))
                {
                    ui.Utype = 2;
                }

                ui.ImgData = imgdata;

                uinfo = new ObservableCollection<UserInfo>();
                uinfo.Add(ui);

               // svc.AddEmployeeAsync(uinfo);
            }

        }


        //private void AddEmployeeCmpltd(object sender, HrmsReference.AddEmployeeCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        if (e.Result.IsSuccessful)
        //        {
        //            CustomMessage msg = new CustomMessage("Successfully inserted details for " + "\n" + uinfo[0].Name);
        //            msg.Show();

        //            //svc.GetDataCompleted -= new EventHandler<GetDataCompletedEventArgs>(async_empdatacmpltd);
        //            //svc.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(async_empdatacmpltd);

        //            //svc.GetDataAsync(null, false);

        //        }
        //    }
        //}

        //private void async_empdatacmpltd(object sender, GetDataCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        this.apvm.lst = null;
        //        this.apvm.lst = e.Result.ToList();

        //        HtmlPage.Document.Submit();

        //        this.Close();
        //    }
        //}

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnimg_Click(object sender, RoutedEventArgs e)
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
                txtimg.Text = ofd.File.Name;
            }
        }

        private void txtmob_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}


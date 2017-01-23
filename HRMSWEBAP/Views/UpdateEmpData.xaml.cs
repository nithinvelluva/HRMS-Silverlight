using HRMSWEBAP.HrmsReference;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HRMSWEBAP.Views
{
    public partial class UpdateEmpData : ChildWindow
    {
        HrmsServiceClient svc = new HrmsServiceClient();
        UserInfo uinfo = new UserInfo();
        public UpdateEmpData(UserInfo ui)
        {
            InitializeComponent();
            uinfo = ui;
            setRoles();

            empidblk.Text = ui.EmpID.ToString();


            
        }        

        private void setRoles()
        {
            List<string> role = new List<string>();
            role.Add("USER");
            role.Add("ADMIN");
            rolecmbbx.ItemsSource = null;
            rolecmbbx.ItemsSource = role;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtmob_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool enteredLetter = false;
            Queue<char> text = new Queue<char>();
            foreach (var ch in txtmob.Text)
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
                txtmob.Text = sb.ToString();
                txtmob.SelectionStart = txtmob.Text.Length;
            }
        }
    }
}


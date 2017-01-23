using SilverlightMessageBoxLibrary;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HRMSWEBAP.Views
{
    public partial class ContactAdmin : ChildWindow
    {
        MailReference.MailServiceClient msvc = new MailReference.MailServiceClient();

        public ContactAdmin()
        {
            InitializeComponent();
        }

        private void sendquerybtn_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(subjectbx.Text) || string.IsNullOrEmpty(messagebx.Text))
            {
                //  MessageBox.Show("*Please fill mandatory fileds!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                CustomMessage msgerr = new CustomMessage("*Please fill mandatory fileds!", CustomMessage.MessageType.Error);
                msgerr.Show();
            }

            else
            {
                busyShow.IsBusy = true;

                string sentFrom = Helpers.UserLoginStatus.currentEmail;

                msvc.SendQueryMailCompleted -= new EventHandler<MailReference.SendQueryMailCompletedEventArgs>(SentQueryCmpltd);
                msvc.SendQueryMailCompleted += new EventHandler<MailReference.SendQueryMailCompletedEventArgs>(SentQueryCmpltd);

                msvc.SendQueryMailAsync(Helpers.UserLoginStatus.currentName, sentFrom, subjectbx.Text, messagebx.Text);

            }
        }

        private void SentQueryCmpltd(object sender, MailReference.SendQueryMailCompletedEventArgs e)
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
    }
}


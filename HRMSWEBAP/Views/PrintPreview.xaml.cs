using HRMSWEBAP.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Printing;

namespace HRMSWEBAP.Views
{
    public partial class PrintPreview : ChildWindow,INotifyPropertyChanged
    {
        private ObservableCollection<AttendanceReportInfo> reportinfo;
        public event PropertyChangedEventHandler PropertyChanged;

        public PrintPreview()
        {
            InitializeComponent();            
        }

        public PrintPreview(ObservableCollection<AttendanceReportInfo> rprt)
        {
            InitializeComponent();
            reportinfo = rprt;

            DataContext = this;

            nametxt.Text = Helpers.UserLoginStatus.currentName;
            yrtxt.Text = reportinfo[0].Year;
            mnthtxt.Text = reportinfo[0].Month;
            wrkdystxt.Text = reportinfo[0].WorkingDays.ToString();
            holidystxt.Text = reportinfo[0].Holidays.ToString();
            attndtxt.Text = reportinfo[0].ActiveDays.ToString();
            lvtxt.Text = reportinfo[0].LeaveDays.ToString();
            wrkhrtxt.Text = (reportinfo[0].ActiveDays * 8).ToString();

            PrintTitle = "Monthly Report - " + reportinfo[0].Month + "," + reportinfo[0].Year;
        }             

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }

        private string _PrintTitle;
        public string PrintTitle
        {
            get { return _PrintTitle; }
            set
            {
                _PrintTitle = value;
                RaisePropertyChanged("PrintTitle");
            }
        }

        private void printbtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PrintDocument pd = new PrintDocument();

            pd.PrintPage += (s, et) =>
            {
                et.PageVisual = cnvContainer;
            };

            pd.Print("MainPageContent");
        }

        private void exprtpdf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void exprtword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void exprtexcel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}


namespace HRMSWEBAP.Models
{
    public class AttendanceReportInfo
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public double TotalDays { get; set; }
        public double Holidays { get; set; }
        public double WorkingDays { get; set; }
        public double ActiveDays { get; set; }
        public double LeaveDays { get; set; }        
    }
}

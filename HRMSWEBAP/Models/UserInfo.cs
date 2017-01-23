using System;

namespace HRMSWEBAP.Models
{
    public class UserInfo
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Number { get; set; }
        public string Mail { get; set; }
        public int Utype { get; set; }
        public string fav_item { get; set; }
        public string role { get; set; }
        public DateTime dob { get; set; }
        public byte[] imgData { get; set; }
       
    }

    public class AttendanceInfo
    {
        public int EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string PunchinTime { get; set; }
        public string PunchoutTime { get; set; }
        public string WorkingHours { get; set; }
        public string SelectedDate { get; set; }
    }
}

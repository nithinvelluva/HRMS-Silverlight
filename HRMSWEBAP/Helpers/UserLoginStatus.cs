using HRMSWEBAP.Models;
using System;
using System.Collections.Generic;

namespace HRMSWEBAP.Helpers
{
    public class UserLoginStatus
    {
        public static int currentEmpId { get; set; }
        public static string currentName { get; set; }
        public static string currentUserName { get; set; }
        public static string currentPassword { get; set; }
        public static string currentGender { get; set; }
        public static string currentPhone { get; set; }
        public static string currentEmail { get; set; }
        public static int currentUserType { get; set; }
        public static DateTime currentUserDob { get; set; }
        public static byte[] currentUserImgData { get; set; }
        public static string CurrentEmpPhotoPath { get; set; }

        public static string PunchinTime { get; set; }
        public static string PunchoutTime { get; set; }

        public static bool pinFlag = false;
        public static bool poutFlag = false;

        public static List<UserInfo> userDetailsList { get; set; }

        public static List<HrmsReference.UserInfo> EmpDetailsList { get; set; }

        public static List<string> EmpNamesList { get; set; }
    }

    public class EmpDetails
    {
        public static List<HrmsReference.UserInfo> CurrEmpAttendanceDataList { get; set; }
        public static List<HrmsReference.LeaveInfo> CurrEmpLeaveStatisticsList { get; set; }
        public static List<HrmsReference.LeaveInfo> CurrEmpLeaveDetailsList { get; set; }
    }

    public class LeaveInfo
    {
        //public static int _empId { get; set; }
        public static double CasualLeave { get; set; }
        public static double FestiveLeave { get; set; }
        public static double SickLeave { get; set; }
        public static double LossOfPay { get; set; }
        public static List<HrmsReference.LeaveType> LeaveTypes { get; set; }
        public static string Fromdate { get; set; }
        public static string Todate { get; set; }
        public static string Leavedurationtype { get; set; }
        public static string LeaveType { get; set; }
        public static string Comments { get; set; }
        public static bool Status { get; set; }
        public static bool Rejected { get; set; }
        public static double Days { get; set; }
        public static byte[] LeaveStatusImg { get; set; }
     
    }
}

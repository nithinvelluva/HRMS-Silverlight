namespace HRMSWEBAP.Helpers
{
    public static class ClearTempDetails
    {
        public static void EraseTempDetails() 
        {
            UserLoginStatus.currentName = null;
            UserLoginStatus.currentUserName = null;
            UserLoginStatus.currentPassword = null;
            UserLoginStatus.currentPhone = null;
            UserLoginStatus.currentEmail = null;
            UserLoginStatus.currentUserType = 0;
        }
    }
}

using Apex.MVVM;
using HRMSWEBAP.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace HRMSWEBAP.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public bool init = false;
        public event PropertyChangedEventHandler PropertyChanged;

        bool flag = false;
        String fav = string.Empty;

        public DataSet ds = new DataSet("DS");
        public DataSet ds1 = new DataSet("DS");

        public DataTable UserDetails = null;
        public DataTable Favorites = null;

        DataRow newRow = null;
        DataRow fnewRow = null;

        public Register registerWindow;

        public System.Text.RegularExpressions.Regex userName_regex = new System.Text.RegularExpressions.Regex("^[A-Za-z0-9_]{3,8}$");
        public System.Text.RegularExpressions.Regex password_regex = new System.Text.RegularExpressions.Regex("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).})");
        public System.Text.RegularExpressions.Regex email_regex = new System.Text.RegularExpressions.Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        public System.Text.RegularExpressions.Regex number_regex = new System.Text.RegularExpressions.Regex("[^0-9]");

        public string nameError = string.Empty;
        public string userNameError = string.Empty;
        public string passwordError = string.Empty;
        public string numberError = string.Empty;
        public string emailError = string.Empty;

        public RegisterViewModel(Register regWin)
        {
            registerWindow = regWin;

            registerCommand = new Command(ExecuteRegister);
            clearCommand = new Command(ExecuteClear);

            /*This method is for initializing the datatable on page loading*/

            InitializeTable();
        }

        private String _name;
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");

            }
        }

        private String _userName;
        public String UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private String _password;
        public String Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private String _cnfrmPassword;
        public String CnfrmPassword
        {
            get { return _cnfrmPassword; }
            set
            {
                _cnfrmPassword = value;
                RaisePropertyChanged("CnfrmPassword");
            }
        }

        private String _email;
        public String Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private String _phoneNumber;
        public String PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }

        private String _error_msg;
        public String Error_Msg
        {
            get { return _error_msg; }
            set
            {
                _error_msg = value;
                RaisePropertyChanged("Error_Msg");
            }
        }

        /*Commands in the register window */

        private Command registerCommand;
        public Command RegisterCommand
        {
            get { return registerCommand; }
        }

        private Command clearCommand;
        public Command ClearCommand
        {
            get { return clearCommand; }
        }

        public string Error
        {
            get
            { throw new NotImplementedException(); }
        }

        public string this[string coloumnName]
        {

            get
            {
                if (init)
                {
                    if (coloumnName.Equals("Name"))
                    {
                        if (string.IsNullOrEmpty(Name))
                        {
                            nameError = "Please enter Name";
                            return "Please enter Name";
                        }
                        else if (Name.Length > 8)
                        {
                            nameError = "Length should not be more than 8 characters.";
                            return "Length should not be more than 8 characters.";
                        }
                        else
                        {
                            nameError = String.Empty;
                        }
                    }
                    else if (coloumnName.Equals("UserName"))
                    {
                        if (string.IsNullOrEmpty(UserName))
                        {
                            userNameError = "Please Enter UserName";
                            return "Please Enter UserName";
                        }
                        else if (UserName.Length < 3 || UserName.Length > 8)
                        {
                            userNameError = "Invalid length";
                            return "Invalid length";
                        }
                        else if (!userName_regex.IsMatch(UserName))
                        {
                            userNameError = "special characters are not allowed";
                            return "special characters are not allowed";
                        }
                        else
                        {
                            userNameError = String.Empty;
                        }

                    }
                    else if (coloumnName.Equals("Email"))
                    {
                        if (string.IsNullOrEmpty(Email))
                        {
                            emailError = "Please Enter Email Id.";
                            return "Please Enter Email Id.";
                        }
                        else if (!(email_regex.IsMatch(Email)))
                        {
                            emailError = "Enter Valid Email Id";
                            return "Enter Valid Email Id";
                        }
                        else
                        {
                            emailError = String.Empty;
                        }
                    }

                    else if (coloumnName.Equals("PhoneNumber"))
                    {
                        if (string.IsNullOrEmpty(PhoneNumber))
                        {
                            numberError = "Please Enter PhoneNumber";
                            return "Please Enter PhoneNumber";
                        }

                        else if (PhoneNumber.Length > 10 || PhoneNumber.Length < 10 || number_regex.IsMatch(PhoneNumber))
                        {
                            numberError = "Enter valid PhoneNumber";
                            return "Enter valid PhoneNumber";
                        }
                        else
                        {
                            numberError = String.Empty;
                        }
                    }
                    else if (coloumnName.Equals("Password"))
                    {
                        if (string.IsNullOrEmpty(Password))
                        {
                            passwordError = "Password cannot be empty.";
                            return "Password cannot be empty.";
                        }
                        else if (Password.Length < 4 || Password.Length > 8)
                        {
                            passwordError = "Invalid Length";
                            return "Invalid Length";
                        }
                        else if (password_regex.IsMatch(Password))
                        {
                            passwordError = "Invalid Password";
                            return "Invalid Password";
                        }
                        else
                        {
                            passwordError = String.Empty;
                        }
                    }
                    else if (coloumnName.Equals("CnfrmPassword"))
                    {
                        if (!(Password.Equals(CnfrmPassword)))
                        {
                            passwordError = "Password Mismatch";
                            return "Password Mismatch";
                        }
                        else
                        {
                            passwordError = String.Empty;
                        }
                    }
                }
                return null;
            }

        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }

        public void InitializeTable()
        {
            try
            {
                // ds.Clear();
                ds.ReadXml(@"F:\NewsReader\nmsDb\UserInfo.xml");
                ds1.ReadXml(@"F:\NewsReader\nmsDb\Favorites.xml");

            }
            catch (Exception ex)
            {
                ds.WriteXml(@"F:\NewsReader\nmsDb\UserInfo.xml");
                ds1.WriteXml(@"F:\NewsReader\nmsDb\Favorites.xml");
            }

            UserDetails = new DataTable("UserDetails");
            Favorites = new DataTable("Favorites");



            if (!(ds.Tables.Contains("UserDetails")))
            {
                ds.Tables.Add(UserDetails);

            }

            if (!(ds1.Tables.Contains("Favorites")))
            {
                ds1.Tables.Add(Favorites);
            }

            try
            {
                DataTableReader reader = ds.CreateDataReader();
                DataTableReader reader1 = ds1.CreateDataReader();

                //UserDetails Table

                DataColumn autoIncreCol = new DataColumn("UserId", typeof(System.Int32));
                UserDetails.Columns.Add(autoIncreCol);

                autoIncreCol.AutoIncrement = true;
                autoIncreCol.AutoIncrementSeed = 1;
                autoIncreCol.AutoIncrementStep = 2;
                autoIncreCol.Unique = true;
                // autoIncreCol.ReadOnly = true;


                DataColumn col2 = new DataColumn("Name", typeof(string));
                DataColumn col3 = new DataColumn("UserName", typeof(string));
                DataColumn col4 = new DataColumn("Password", typeof(string));
                DataColumn col5 = new DataColumn("Phone", typeof(string));
                DataColumn col6 = new DataColumn("Email", typeof(string));
                DataColumn col7 = new DataColumn("UserType", typeof(int));

                UserDetails.Columns.Add(col2);
                UserDetails.Columns.Add(col3);
                UserDetails.Columns.Add(col4);
                UserDetails.Columns.Add(col5);
                UserDetails.Columns.Add(col6);
                UserDetails.Columns.Add(col7);

                //Favorites Table

                DataColumn fcol = new DataColumn("ID", typeof(int));
                DataColumn fcol1 = new DataColumn("UserName", typeof(string));
                DataColumn fcol2 = new DataColumn("favorite_item", typeof(string));

                fcol.AutoIncrement = true;
                fcol.AutoIncrementSeed = 1;
                fcol.AutoIncrementStep = 2;
                fcol.Unique = true;

                Favorites.Columns.Add(fcol);
                Favorites.Columns.Add(fcol1);
                Favorites.Columns.Add(fcol2);

            }
            catch (Exception ex)
            { }
        }

        private bool pwd_Check(String pwd, String cnf)
        {
            bool flag1 = false;

            if (pwd != null && cnf != null)
            {
                if (pwd.Equals(cnf))
                {
                    flag1 = true;
                }
                else
                {
                    flag1 = false;
                }
            }
            else
            {
                flag1 = false;
            }
            return flag1;
        }

        public void ExecuteRegister()
        {
            fav = "1;2;3;4;5;6;7;8;";
            Error_Msg = string.Empty;
            init = true;

            if (!string.IsNullOrEmpty(nameError) || !string.IsNullOrEmpty(userNameError) ||
                !string.IsNullOrEmpty(numberError) || !string.IsNullOrEmpty(emailError))
            {
                //if (!(string.IsNullOrEmpty(nameError)) || !(string.IsNullOrEmpty(userNameError))
                //    || !(string.IsNullOrEmpty(passwordError)) || !(string.IsNullOrEmpty(numberError)) ||
                //!(string.IsNullOrEmpty(emailError)))
                //{

                Error_Msg = "Please fill the required fields!";

            }
            else
            {
                if (!string.IsNullOrEmpty(userNameError))
                {
                    Error_Msg = "Invalid Username";
                }
                else
                {
                    try
                    {
                        DataTableReader reader = ds.CreateDataReader();
                        DataTableReader reader1 = ds1.CreateDataReader();

                        List<string> name_List = new List<string>();
                        while (reader.Read())
                        {

                            name_List.Add(reader.GetValue(reader.GetOrdinal("UserName")).ToString());

                        }
                        reader.Close();
                        flag = false;
                        foreach (string item in name_List)
                        {
                            if (item.Equals(UserName))
                            {
                                UserName = "";
                                flag = true;
                                break;
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An Unexpected Error Occured!", "", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    if (!flag)
                    {
                        if (pwd_Check(Password, CnfrmPassword))
                        {
                            if (string.IsNullOrEmpty(emailError))
                            {
                                int userTypeNormal = 2;
                                try
                                {
                                    newRow = UserDetails.NewRow();

                                    fnewRow = Favorites.NewRow();

                                    newRow[1] = Name; newRow[2] = UserName;
                                    newRow[3] = Password; newRow[4] = PhoneNumber;
                                    newRow[5] = Email; newRow[6] = userTypeNormal;

                                    UserDetails.Rows.Add(newRow);

                                    //   UserDetails.Rows.Add(null, nam, uname, paswd, number, mail, userTypeNormal);
                                    ds.Merge(UserDetails, true, MissingSchemaAction.Ignore);
                                    ds.AcceptChanges();
                                    ds.WriteXml(@"F:\NewsReader\nmsDb\UserInfo.xml");

                                    fnewRow[1] = UserName;
                                    fnewRow[2] = fav;
                                    Favorites.Rows.Add(fnewRow);

                                    // Favorites.Rows.Add(null, uname, fav);
                                    ds1.Merge(Favorites, true, MissingSchemaAction.Ignore);
                                    ds1.AcceptChanges();
                                    ds1.WriteXml(@"F:\NewsReader\nmsDb\Favorites.xml");

                                    this.CloseWindow(registerWindow);
                                    Loginpage obj = new Loginpage();
                                    obj.Show();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An Unexpected Error Occured!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                Error_Msg = "Invalid Email!";
                            }

                        }
                        else
                        {
                            Error_Msg = "Password mismatch!";
                            // CnfrmPassword = string.Empty;
                        }

                    }
                    else
                    {
                        Error_Msg = "Username already exists!";
                    }
                }
            }

        }

        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void ExecuteClear()
        {
            init = false;

            Name = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            CnfrmPassword = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Error_Msg = string.Empty;
        }

    }
}

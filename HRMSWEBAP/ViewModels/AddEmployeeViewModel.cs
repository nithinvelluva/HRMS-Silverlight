using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace HRMSWEBAP.ViewModels
{
    public class AddEmployeeViewModel
    {
        public AddEmployeeViewModel()
        {

        }

        public static Regex RegexEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        private string _empname;
        public string Empname
        {
            get { return _empname; }
            set
            {
                if (value == _empname)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Name is required!");
                _empname = value;
                OnPropertyChanged("Empname");
            }
        }
        private string _empmob;
        public string Empmob
        {
            get { return _empmob; }
            set
            {
                if (value == _empmob)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Mobile Number is Required!");
                _empmob = value;
                OnPropertyChanged("Empmob");
            }
        }
        private string _empmail;
        public string Empmail
        {
            get { return _empmail; }
            set
            {
                if (value == _empmail)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Email is Required!");
                else if (!RegexEmail.IsMatch(value))
                    throw new ArgumentException("A valid Email address is required");
                _empmail = value;
                OnPropertyChanged("Empmail");
            }
        }
        private DateTime _empdob;
        public DateTime Empdob
        {
            get { return _empdob; }
            set
            {
                if (value == _empdob)
                    return;
                if (value == null)
                    throw new ArgumentException("Please select a Date!");
                _empdob = value;
                OnPropertyChanged("Empnation");
            }
        }

        //private string empsex;
        //public string Empsex { get; set; }

        //private string _emptype;
        //public string Emptype
        //{
        //    get { return _emptype; }
        //    set
        //    {
        //        if (value == _emptype)
        //            return;
        //        if (String.IsNullOrEmpty(value))
        //            throw new ArgumentException("User Type is Required!");
        //        _emptype = value;
        //        OnPropertyChanged("Emptype");
        //    }
        //}
        private string _empnation;
        public string Empnation
        {
            get { return _empnation; }
            set
            {
                if (value == _empnation)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Nationality is Required!");
                _empnation = value;
                OnPropertyChanged("Empnation");
            }
        }
        private string _emppos;
        public string Emppos
        {
            get { return _emppos; }
            set
            {
                if (value == _emppos)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Designation is Required!");
                _emppos = value;
                OnPropertyChanged("Emppos");
            }
        }
        private string _empusr;
        public string Empusr
        {
            get { return _empusr; }
            set
            {
                if (value == _empusr)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("UserName is required!");
                _empusr = value;
                OnPropertyChanged("Empusr");
            }
        }
        private string _emppwd1;
        public string Emppwd1
        {
            get { return _emppwd1; }
            set
            {
                if (value == _emppwd1)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Password is Required!");
                else if (value.Length < 6 || value.Length > 15)
                    throw new ArgumentException("Password must be atleast 6 characters and not more than 15 characters in length");
                _emppwd1 = value;
                OnPropertyChanged("Emppwd1");
            }
        }
        private string _emppwd2;
        public string Emppwd2
        {
            get { return _emppwd2; }
            set
            {
                if (value == _emppwd2)
                    return;
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Password is Required!");
                else if (value.Length < 6 || value.Length > 15)
                    throw new ArgumentException("Password must be atleast 6 characters and not more than 15 characters in length");
                else if (_emppwd1 != null && !_emppwd1.Equals(value))
                    throw new ArgumentException("Password doesn't match");
                _emppwd2 = value;
                OnPropertyChanged("Emppwd2");
            }
        }

        private void OnPropertyChanged(string p)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        private void PropertyChanged(AddEmployeeViewModel insertpopupViewModel, PropertyChangedEventArgs propertyChangedEventArgs)
        {

        }
    }
}

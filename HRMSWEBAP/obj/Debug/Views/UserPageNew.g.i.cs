﻿#pragma checksum "C:\Users\nithin\Documents\Visual Studio 2013\Projects\HRMSWEBAP\HRMSWEBAP\Views\UserPageNew.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "23512C9EC4A036C07067C91A0F6E2CD1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace HRMSWEBAP.Views {
    
    
    public partial class UserPageNew : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image usrimage;
        
        internal System.Windows.Controls.Button changeusrphoto;
        
        internal System.Windows.Controls.TextBlock errtxt;
        
        internal System.Windows.Controls.TextBox name_textbox;
        
        internal System.Windows.Controls.TextBox Empid_textbox;
        
        internal System.Windows.Controls.Label usname;
        
        internal System.Windows.Controls.TextBox uname_textbox;
        
        internal System.Windows.Controls.PasswordBox pwd_reg;
        
        internal System.Windows.Controls.Label contentlabel;
        
        internal System.Windows.Controls.StackPanel cnfrm_pwd_panel;
        
        internal System.Windows.Controls.PasswordBox pwd_cfm_reg;
        
        internal System.Windows.Controls.TextBlock dob;
        
        internal System.Windows.Controls.RadioButton male;
        
        internal System.Windows.Controls.RadioButton female;
        
        internal System.Windows.Controls.TextBox email;
        
        internal System.Windows.Controls.TextBox phone;
        
        internal System.Windows.Controls.Button Edit;
        
        internal System.Windows.Controls.Button update;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/HRMSWEBAP;component/Views/UserPageNew.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.usrimage = ((System.Windows.Controls.Image)(this.FindName("usrimage")));
            this.changeusrphoto = ((System.Windows.Controls.Button)(this.FindName("changeusrphoto")));
            this.errtxt = ((System.Windows.Controls.TextBlock)(this.FindName("errtxt")));
            this.name_textbox = ((System.Windows.Controls.TextBox)(this.FindName("name_textbox")));
            this.Empid_textbox = ((System.Windows.Controls.TextBox)(this.FindName("Empid_textbox")));
            this.usname = ((System.Windows.Controls.Label)(this.FindName("usname")));
            this.uname_textbox = ((System.Windows.Controls.TextBox)(this.FindName("uname_textbox")));
            this.pwd_reg = ((System.Windows.Controls.PasswordBox)(this.FindName("pwd_reg")));
            this.contentlabel = ((System.Windows.Controls.Label)(this.FindName("contentlabel")));
            this.cnfrm_pwd_panel = ((System.Windows.Controls.StackPanel)(this.FindName("cnfrm_pwd_panel")));
            this.pwd_cfm_reg = ((System.Windows.Controls.PasswordBox)(this.FindName("pwd_cfm_reg")));
            this.dob = ((System.Windows.Controls.TextBlock)(this.FindName("dob")));
            this.male = ((System.Windows.Controls.RadioButton)(this.FindName("male")));
            this.female = ((System.Windows.Controls.RadioButton)(this.FindName("female")));
            this.email = ((System.Windows.Controls.TextBox)(this.FindName("email")));
            this.phone = ((System.Windows.Controls.TextBox)(this.FindName("phone")));
            this.Edit = ((System.Windows.Controls.Button)(this.FindName("Edit")));
            this.update = ((System.Windows.Controls.Button)(this.FindName("update")));
        }
    }
}


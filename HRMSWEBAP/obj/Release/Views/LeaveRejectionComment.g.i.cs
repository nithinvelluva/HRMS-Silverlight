﻿#pragma checksum "C:\Users\nithin\Documents\Visual Studio 2013\Projects\HRMSWEBAP\HRMSWEBAP\Views\LeaveRejectionComment.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0678F16F8CA19D5CDBEA274C27A44B0F"
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
    
    
    public partial class LeaveRejectionComment : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock fromdateblk;
        
        internal System.Windows.Controls.TextBlock todateblk;
        
        internal System.Windows.Controls.TextBlock lvtypblk;
        
        internal System.Windows.Controls.TextBlock lvstatusblk;
        
        internal System.Windows.Controls.TextBox cmntblk;
        
        internal System.Windows.Controls.StackPanel userstackpnl;
        
        internal System.Windows.Controls.Button Addcmntsbtn;
        
        internal System.Windows.Controls.Button Update;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HRMSWEBAP;component/Views/LeaveRejectionComment.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.fromdateblk = ((System.Windows.Controls.TextBlock)(this.FindName("fromdateblk")));
            this.todateblk = ((System.Windows.Controls.TextBlock)(this.FindName("todateblk")));
            this.lvtypblk = ((System.Windows.Controls.TextBlock)(this.FindName("lvtypblk")));
            this.lvstatusblk = ((System.Windows.Controls.TextBlock)(this.FindName("lvstatusblk")));
            this.cmntblk = ((System.Windows.Controls.TextBox)(this.FindName("cmntblk")));
            this.userstackpnl = ((System.Windows.Controls.StackPanel)(this.FindName("userstackpnl")));
            this.Addcmntsbtn = ((System.Windows.Controls.Button)(this.FindName("Addcmntsbtn")));
            this.Update = ((System.Windows.Controls.Button)(this.FindName("Update")));
        }
    }
}


﻿#pragma checksum "C:\Users\nithin\Documents\Visual Studio 2013\Projects\HRMSWEBAP\HRMSWEBAP\Views\ContactAdmin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4B0B6F026984E447A6F645E9A944CACB"
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
    
    
    public partial class ContactAdmin : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox subjectbx;
        
        internal System.Windows.Controls.TextBox messagebx;
        
        internal System.Windows.Controls.BusyIndicator busyShow;
        
        internal System.Windows.Controls.Button sendquerybtn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HRMSWEBAP;component/Views/ContactAdmin.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.subjectbx = ((System.Windows.Controls.TextBox)(this.FindName("subjectbx")));
            this.messagebx = ((System.Windows.Controls.TextBox)(this.FindName("messagebx")));
            this.busyShow = ((System.Windows.Controls.BusyIndicator)(this.FindName("busyShow")));
            this.sendquerybtn = ((System.Windows.Controls.Button)(this.FindName("sendquerybtn")));
        }
    }
}

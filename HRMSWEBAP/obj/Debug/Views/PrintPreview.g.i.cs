﻿#pragma checksum "C:\Users\nithin\Documents\Visual Studio 2013\Projects\HRMSWEBAP\HRMSWEBAP\Views\PrintPreview.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2957FE39876753941C8F6A86EC44F852"
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
    
    
    public partial class PrintPreview : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas cnvContainer;
        
        internal System.Windows.Controls.TextBlock prnttitle;
        
        internal System.Windows.Controls.TextBlock nametxt;
        
        internal System.Windows.Controls.TextBlock yrtxt;
        
        internal System.Windows.Controls.TextBlock mnthtxt;
        
        internal System.Windows.Controls.TextBlock wrkdystxt;
        
        internal System.Windows.Controls.TextBlock holidystxt;
        
        internal System.Windows.Controls.TextBlock attndtxt;
        
        internal System.Windows.Controls.TextBlock lvtxt;
        
        internal System.Windows.Controls.TextBlock wrkhrtxt;
        
        internal System.Windows.Controls.StackPanel exporttyppanel;
        
        internal System.Windows.Controls.Image printbtn;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HRMSWEBAP;component/Views/PrintPreview.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.cnvContainer = ((System.Windows.Controls.Canvas)(this.FindName("cnvContainer")));
            this.prnttitle = ((System.Windows.Controls.TextBlock)(this.FindName("prnttitle")));
            this.nametxt = ((System.Windows.Controls.TextBlock)(this.FindName("nametxt")));
            this.yrtxt = ((System.Windows.Controls.TextBlock)(this.FindName("yrtxt")));
            this.mnthtxt = ((System.Windows.Controls.TextBlock)(this.FindName("mnthtxt")));
            this.wrkdystxt = ((System.Windows.Controls.TextBlock)(this.FindName("wrkdystxt")));
            this.holidystxt = ((System.Windows.Controls.TextBlock)(this.FindName("holidystxt")));
            this.attndtxt = ((System.Windows.Controls.TextBlock)(this.FindName("attndtxt")));
            this.lvtxt = ((System.Windows.Controls.TextBlock)(this.FindName("lvtxt")));
            this.wrkhrtxt = ((System.Windows.Controls.TextBlock)(this.FindName("wrkhrtxt")));
            this.exporttyppanel = ((System.Windows.Controls.StackPanel)(this.FindName("exporttyppanel")));
            this.printbtn = ((System.Windows.Controls.Image)(this.FindName("printbtn")));
        }
    }
}


﻿#pragma checksum "C:\Users\nithin\Documents\Visual Studio 2013\Projects\HRMSWEBAP\HRMSWEBAP\Views\PrintPreviews.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "75DEFCB929EA38FF04F0637ECA225EF9"
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
    
    
    public partial class PrintPreviews : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Canvas m_canvas;
        
        internal System.Windows.Controls.Button m_btn_print;
        
        internal System.Windows.Controls.Button m_btn_prev_page;
        
        internal System.Windows.Controls.Button m_btn_next_page;
        
        internal System.Windows.Controls.Slider m_sld_size;
        
        internal System.Windows.Controls.TextBlock m_lbl_size;
        
        internal System.Windows.Controls.Canvas m_canvas_print;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HRMSWEBAP;component/Views/PrintPreviews.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.m_canvas = ((System.Windows.Controls.Canvas)(this.FindName("m_canvas")));
            this.m_btn_print = ((System.Windows.Controls.Button)(this.FindName("m_btn_print")));
            this.m_btn_prev_page = ((System.Windows.Controls.Button)(this.FindName("m_btn_prev_page")));
            this.m_btn_next_page = ((System.Windows.Controls.Button)(this.FindName("m_btn_next_page")));
            this.m_sld_size = ((System.Windows.Controls.Slider)(this.FindName("m_sld_size")));
            this.m_lbl_size = ((System.Windows.Controls.TextBlock)(this.FindName("m_lbl_size")));
            this.m_canvas_print = ((System.Windows.Controls.Canvas)(this.FindName("m_canvas_print")));
        }
    }
}

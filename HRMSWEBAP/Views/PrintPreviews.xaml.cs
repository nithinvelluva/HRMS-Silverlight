using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Printing;
using System.Windows.Shapes;

namespace HRMSWEBAP.Views
{
    public partial class PrintPreviews : ChildWindow
    {
        private UserPage m_cavas_print;
        private DataGrid reportgrid;
        private System.Collections.IEnumerable enumerable;

        public PrintPreviews()
        {
            InitializeComponent();
        }

        public PrintPreviews(UserPage m_cavas_print)
        {            
            this.m_cavas_print = m_cavas_print;
            InitializeComponent();
        }

        public PrintPreviews(DataGrid reportgrid)
        {
            // TODO: Complete member initialization
            this.reportgrid = reportgrid;
            InitializeComponent();
        }

        public PrintPreviews(System.Collections.IEnumerable enumerable)
        {
            // TODO: Complete member initialization
            this.enumerable = enumerable;
        }       

        private void m_sld_size_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void m_btn_next_page_Click(object sender, RoutedEventArgs e)
        {

        }

        private void m_btn_prev_page_Click(object sender, RoutedEventArgs e)
        {

        }

        private void m_btn_print_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument pd = new PrintDocument();

            pd.PrintPage += (s, et) =>
            {
                et.PageVisual = reportgrid;
            };

            pd.Print("MainPageContent");
        }
    }
}


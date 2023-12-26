using CourseProjectServerApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseProjectServerApplication.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void win_KeyDown(object sender, KeyEventArgs e)
        {
            lv.BeginInit();
            if (e.Key == Key.Escape && e.IsDown)
                lv.SelectedItem = null;
            lv.EndInit();
        }

        private void search_tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchText_tblock.Visibility = search_tbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var col in (lv.View as GridView).Columns) col.Width = lv.ActualWidth/7;
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            lv.BeginInit();
            lv.SelectedItem = null;
            lv.EndInit();
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            answer_text.Text = "";
        }

        private void sendAnswer_button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void states_lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            state_btn.IsChecked = false;
        }

        private void Filter_btn_Unchecked(object sender, RoutedEventArgs e)
        {
            states_lb.SelectedIndex = 0;
            datereq_cal.SelectedDates.Clear();
            datedone_cal.SelectedDates.Clear();
        }
    }
}

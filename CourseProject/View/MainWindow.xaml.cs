using CourseProjectUserApplication.Model;
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

namespace CourseProjectUserApplication.View
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

        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            floatNewRequest_border.Visibility = Visibility.Visible;
            dark_border.Visibility = Visibility.Visible;

            var marginAnim = new ThicknessAnimation(new Thickness(0, 0, 0, 0), TimeSpan.FromSeconds(0.7));
            marginAnim.AccelerationRatio = 0;
            marginAnim.DecelerationRatio = 0;

            var opacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.5));
            
            var opacityBorderAnim = new DoubleAnimation(0.7, TimeSpan.FromSeconds(1));
           
            floatNewRequest_border.BeginAnimation(MarginProperty, marginAnim);
            floatNewRequest_border.BeginAnimation(OpacityProperty, opacityAnim);

            dark_border.BeginAnimation(OpacityProperty, opacityBorderAnim);

        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            floatNewRequest_border.Margin = new Thickness(0, 400, 0, 0);
            floatNewRequest_border.Opacity = 0;
            dark_border.Opacity = 0;

            floatNewRequest_border.Visibility = Visibility.Hidden;
            dark_border.Visibility = Visibility.Hidden;
        }

        private void title_tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            title_tblock.Visibility = title_tbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void message_tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            message_tblock.Visibility = message_tbox.Text == "" ? Visibility.Visible : Visibility.Hidden;
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            floatNewRequest_border.BeginInit();
            floatNewRequest_border.Margin = new Thickness(0, 400, 0, 0);
            floatNewRequest_border.Opacity = 0;
            floatNewRequest_border.Visibility = Visibility.Hidden;
            floatNewRequest_border.EndInit();

            dark_border.BeginInit();
            dark_border.Opacity = 0;
            dark_border.Visibility = Visibility.Hidden;
            dark_border.EndInit();
        }

        private void search_tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_tblock.Visibility = search_tbox.Text == "" ? Visibility.Visible : Visibility.Hidden;

            requests_lb.SelectedItem = null;

            requests_lb.Items.Filter = (item) =>
            {
                var x = (item as RequestModel);

                return (x.Title.ToLower().Contains(search_tbox.Text.ToLower()) ||
                        x.Message.ToLower().Contains(search_tbox.Text.ToLower()) ||
                        x.UserId.ToString().ToLower().Contains(search_tbox.Text.ToLower()) ||
                        x.RequestId.ToString().ToLower().Contains(search_tbox.Text.ToLower()));
            };
        }

        private void close_btn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

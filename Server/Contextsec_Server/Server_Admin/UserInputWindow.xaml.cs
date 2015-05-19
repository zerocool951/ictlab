using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Server_Admin {
    /// <summary>
    /// Interaction logic for UserInputWindow.xaml
    /// </summary>
    public partial class UserInputWindow : Window {
        private TypeConverter Converter;
        private object PrevValue;
        public object Value;

        public UserInputWindow(string headerText, Type type) {
            InitializeComponent();

            Lb_Header.Content = string.Format("{0} ({1})", headerText, type.Name);

            Converter = TypeDescriptor.GetConverter(type);

            Tb_Input.Focus();
            Tb_Input.TextChanged += Tb_Input_TextChanged;
        }

        private void Ready_Click(object sender, RoutedEventArgs e) {
            Ready();
        }

        private void Tb_Input_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                PrevValue = Converter.ConvertFromString(Tb_Input.Text);
                Tb_Input.Background = Brushes.LightGreen;
                Bt_Ready.IsEnabled = true;
            } catch {
                Tb_Input.Background = Brushes.PaleVioletRed;
                Bt_Ready.IsEnabled = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Ready();
            }
        }

        private void Ready() {
            Value = PrevValue;
            this.Close();
        }
    }
}
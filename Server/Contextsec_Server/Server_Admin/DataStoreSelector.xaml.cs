using Microsoft.Win32;
using Server_Data;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for DataStoreSelector.xaml
    /// </summary>
    public partial class DataStoreSelector : Window {
        private string FilePath;
        private bool CreateNew;

        public DataStoreSelector() {
            InitializeComponent();
            Pb_Key.Focus();
        }

        private void Bt_Open_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.HasValue && result == true) {
                // Open document
                FilePath = dlg.FileName;
                Lb_File.Content = new FileInfo(dlg.FileName).Name;
                Bt_Ready.IsEnabled = true;
                Bt_Ready.Content = "Open";
                CreateNew = false;
            }
        }

        private void Bt_New_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.HasValue && result == true) {
                // Open document
                FilePath = dlg.FileName;
                Lb_File.Content = new FileInfo(dlg.FileName).Name;
                Bt_Ready.IsEnabled = true;
                Bt_Ready.Content = "Create";
                CreateNew = true;
            }
        }

        private void Bt_Ready_Click(object sender, RoutedEventArgs e) {
            try {
                RuleStore store;
                if (CreateNew) {
                    if (File.Exists(FilePath)) {
                        File.Delete(FilePath);
                    }

                    store = RuleStore.Create(FilePath, Pb_Key.Password);
                } else {
                    store = RuleStore.Open(FilePath, Pb_Key.Password);
                }

                var window = new MainWindow(store);
                window.Show();
                this.Close();
            } catch (Exception ex) {
                MessageBox.Show(string.Format("Exception:\n{0}\n\n{1}", ex.Message, ex.ToString()));
            }
        }
    }
}
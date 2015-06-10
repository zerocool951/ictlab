using Server_Data;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server_Admin {
    public partial class MainWindow : Window {
        private RuleStore Store;
        private bool storeChanged = false;

        private bool StoreChanged {
            get {
                return storeChanged;
            }
            set {
                storeChanged = value;
                if (storeChanged) {
                    Bt_Save.IsEnabled = true;
                } else {
                    Bt_Save.IsEnabled = false;
                }
            }
        }

        public MainWindow(RuleStore store) {
            InitializeComponent();
            Store = store;
            Lv_RulesOverview.ItemsSource = Store.Rules;

            this.Closing += (sender, e) => {
                if (StoreChanged) {
                    if (MessageBox.Show("There are unsaved changes, save them?", "Save changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                        Store.WriteToDisc();
                    }
                }
            };
        }

        private void Bt_OpenNew_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBoxResult.OK;
            if (StoreChanged) {
                result = MessageBox.Show("There are unsaved changes, they will be discarded if you continue.", "Continue?", MessageBoxButton.OKCancel);
            }

            if (result == MessageBoxResult.OK) {
                var window = new DataStoreSelector();
                window.Show();
                this.Close();
            }
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e) {
            Store.WriteToDisc();
            StoreChanged = false;
        }

        private void Lv_RulesOverview_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UpdateLists();
        }

        private void Bt_NewRule_Click(object sender, RoutedEventArgs e) {
            Rule newRule = new Rule();

            Store.Rules.Add(newRule);
            StoreChanged = true;
            UpdateLists();
        }

        private void Bt_DelRule_Click(object sender, RoutedEventArgs e) {
            Rule rule = Lv_RulesOverview.SelectedItem as Rule;
            if (rule != null) {
                Store.Rules.Remove(rule);
                StoreChanged = true;
                UpdateLists();
            }
        }

        private void Cb_RuleTemplate_Checkchanged(object sender, RoutedEventArgs e) {
            CheckBox checkBox = sender as CheckBox;
            RuleTemplate curTemplate = RuleTemplate.GetByName(checkBox.Content as string);
            Rule selectedRule = Lv_RulesOverview.SelectedItem as Rule;

            if (curTemplate != null && selectedRule != null && checkBox.IsChecked != null) {
                bool hasTemplate = checkBox.IsChecked.Value;

                if (hasTemplate) {
                    selectedRule.Templates.Add(curTemplate);
                    selectedRule.CreateTemplateProperties();
                } else {
                    foreach (string prop in curTemplate.RequiredProperties.Keys) {
                        selectedRule.Properties.Remove(prop);
                    }
                    selectedRule.Templates.Remove(curTemplate);
                }
            }
            StoreChanged = true;
            UpdateLists();
        }

        private void UpdateLists() {
            Rule selected = Lv_RulesOverview.SelectedItem as Rule;
            if (selected != null) {
                Lv_RuleTemplates.ItemsSource = RuleTemplate.RuleTemplates.Select((rt) => {
                    var check = selected.Templates.Contains(rt);
                    var name = rt.Name;
                    return new Tuple<bool, string>(check, name);
                });
                Lv_RuleDetail.ItemsSource = selected.Properties;
            } else {
                Lv_RuleTemplates.ItemsSource = null;
                Lv_RuleDetail.ItemsSource = null;
            }
            Lv_RulesOverview.Items.Refresh();
            Lv_RuleDetail.Items.Refresh();
        }

        private void Bt_RuleValueEdit_Click(object sender, RoutedEventArgs e) {
            Button bt = sender as Button;
            Rule selected = Lv_RulesOverview.SelectedItem as Rule;
            if (selected != null && bt != null && !string.IsNullOrEmpty(bt.Tag as string)) {
                string key = (string)bt.Tag;

                var dialog = new UserInputWindow(key, selected.Templates.GetTypeByKey(key));
                if (dialog.ShowDialog() == false) {
                    object gotten = dialog.Value;
                    if (gotten != null) {
                        selected.Properties[key] = gotten;
                        StoreChanged = true;
                    }
                }
            }

            UpdateLists();
        }
    }
}
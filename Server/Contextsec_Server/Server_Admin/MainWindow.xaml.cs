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
        private bool StoreChanged = true;

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
            if (e.AddedItems.Count > 0) {
                Rule selected = e.AddedItems[0] as Rule;
                if (selected != null) {
                    Lv_RuleDetail.ItemsSource = selected.Properties;
                }
            } else {
                Lv_RuleDetail.ItemsSource = null;
            }
        }

        private void Bt_NewRule_Click(object sender, RoutedEventArgs e) {
            Rule newRule = new Rule(RuleTemplate.GetByName("Basic"));
            Rule FilledRule = UserFillEmptyProperties(newRule);

            if (FilledRule != null) {
                Store.Rules.Add(FilledRule);
                Lv_RulesOverview.Items.Refresh();
                StoreChanged = true;
            }
        }

        private void Bt_DelRule_Click(object sender, RoutedEventArgs e) {
            Rule rule = Lv_RulesOverview.SelectedItem as Rule;
            if (rule != null) {
                Store.Rules.Remove(rule);
                Lv_RulesOverview.Items.Refresh();
                StoreChanged = true;
            }
        }

        private Rule UserFillEmptyProperties(Rule rule) {
            foreach (KeyValuePair<string, Type> kvp in rule.GetTemplates().SelectMany(t => t.RequiredProperties)) {
                string key = kvp.Key;
                Type type = kvp.Value;

                var dialog = new UserInputWindow(key, type);
                if (dialog.ShowDialog() == false) {
                    object gotten = dialog.Value;
                    if (gotten != null) {
                        rule.Properties[key] = gotten;
                        continue;
                    }
                }
                return null;
            }
            return rule;
        }
    }
}
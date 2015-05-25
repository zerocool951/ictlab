using Server_Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Server_WebAPI {
    public static class DataHelper {
        private static string StorePath;
        private static string StoreKey;

        private static RuleStore Store;

        static DataHelper() {
            var configPath = ConfigurationManager.AppSettings["StorePath"].ToString();
            if (!string.IsNullOrEmpty(configPath)) {
                StorePath = configPath;
            } else {
                throw new Exception("Config value StorePath cannot be emtpy");
            }

            var configKey = ConfigurationManager.AppSettings["StoreKey"].ToString();
            if (!string.IsNullOrEmpty(configKey)) {
                StoreKey = configKey;
            }
        }

        public static RuleStore GetRuleStore() {
            if (Store == null) {
                if (!string.IsNullOrEmpty(StorePath) && !string.IsNullOrEmpty(StoreKey)) {
                    TryCreateStore();
                }
            }
            return Store;
        }

        private static bool TryCreateStore() {
            try {
                Store = RuleStore.Open(StorePath, StoreKey);
                return true;
            } catch {
                return false;
            }
        }

        public static void SetKey(string key) {
            if (Store == null) {
                StoreKey = key;
                TryCreateStore();
            }
        }
    }
}
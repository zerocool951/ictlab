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

        private const long DefaultUpdateFrequencyMs = 60000;

        private static long UpdateFrequencyMs;
        private static long LastRulestoreUpdate;

        private static RuleStore Store;

        static DataHelper() {
            var configPath = ConfigurationManager.AppSettings["StorePath"];
            if (!string.IsNullOrEmpty(configPath)) {
                StorePath = configPath;
            } else {
                throw new Exception("Config value StorePath cannot be emtpy");
            }

            var configKey = ConfigurationManager.AppSettings["StoreKey"];
            if (!string.IsNullOrEmpty(configKey)) {
                StoreKey = configKey;
            }

            var configUpdateFreq = ConfigurationManager.AppSettings["StoreUpdateFrequencyMs"];
            long freqFromConfig = DefaultUpdateFrequencyMs;
            if (!string.IsNullOrEmpty(configUpdateFreq) && long.TryParse(configUpdateFreq, out freqFromConfig)) {
                UpdateFrequencyMs = freqFromConfig;
            }
        }

        public static RuleStore GetRuleStore() {
            if (Store == null || Environment.TickCount - LastRulestoreUpdate > UpdateFrequencyMs) {
                if (!string.IsNullOrEmpty(StorePath) && !string.IsNullOrEmpty(StoreKey)) {
                    TryCreateStore();
                }
            }
            return Store;
        }

        private static bool TryCreateStore() {
            try {
                Store = RuleStore.Open(StorePath, StoreKey);
                LastRulestoreUpdate = Environment.TickCount;
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
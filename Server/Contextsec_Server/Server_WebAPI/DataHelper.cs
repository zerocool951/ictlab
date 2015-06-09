using Server_Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Server_WebAPI {
    /// <summary>
    /// Provides an easy way to interact with a stored rulestore
    /// Configure this class in Web.config
    /// </summary>
    public class DataHelper {
        private const long DefaultUpdateFrequencyMs = 60000;

        private static string StorePath;
        private static string StoreKey;

        private long UpdateFrequencyMs;
        private long LastRulestoreUpdate;
        
        private static DataHelper instance;
        /// <summary>
        /// Singleton instance of DataHelper class
        /// Will be null when for some reason unable to open/read DataStore
        /// </summary>
        public static DataHelper Instance {
            get {
                try {
                    if (instance == null) {
                        instance = new DataHelper();
                    }

                    if (instance.AllRules == null || instance.AllRules.Count() == 0) {
                        return null;
                    } else {
                        return instance;
                    }
                } catch {
                    return null;
                }
            }
        }

        private Rule[] allRules;
        /// <summary>
        /// Provides access to all the rules in configured datastore file
        /// </summary>
        public Rule[] AllRules {
            get {
                RuleStore openedRuleStore;
                if ((allRules == null || allRules.Count() == 0
                    || Environment.TickCount - LastRulestoreUpdate > UpdateFrequencyMs)
                    && TryCreateStore(out openedRuleStore)) {
                    allRules = openedRuleStore.Rules.ToArray();

                    if (allRules.Count() == 0) {
                        allRules = null;
                    }
                }

                return allRules;
            }
        }

        /// <summary>
        /// Specify the dectyption key for the datastore, key can only be set if the datastore has not yet been opened
        /// </summary>
        /// <param name="key">Dectyprion key for datastore file</param>
        public static void SetKey(string key) {
            if (Instance == null || Instance.AllRules == null || Instance.allRules.Count() == 0) {
                StoreKey = key;
            }
        }

        private DataHelper() {
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

        private bool TryCreateStore(out RuleStore ruleStore) {
            try {
                if (!string.IsNullOrEmpty(StorePath) && !string.IsNullOrEmpty(StoreKey)) {
                    ruleStore = RuleStore.Open(StorePath, StoreKey);
                    LastRulestoreUpdate = Environment.TickCount;
                    return true;
                } else {
                    ruleStore = null;
                    return false;
                }
            } catch {
                ruleStore = null;
                return false;
            }
        }
    }
}
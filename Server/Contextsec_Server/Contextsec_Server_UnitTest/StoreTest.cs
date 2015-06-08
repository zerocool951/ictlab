using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server_Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contextsec_Server_UnitTest {
    [TestClass]
    public class StoreTest {

        [TestMethod]
        public void TestRuleStore() {
            string fileLoc = "Unittest.json";
            string key = "password123";
            string templateName = "Empty";
            string saveKey = "testKey";
            string saveValue = "testVal";

            if (File.Exists(fileLoc)) {
                File.Delete(fileLoc);
            }

            RuleStore newStore = RuleStore.Create(fileLoc, key);

            newStore.Rules.Add(
                new Rule(RuleTemplate.GetByName(templateName)) {
                    Properties = new Dictionary<string, object>() {
                        { saveKey, saveValue },
                        { "key2", 1 }
                    }
                });

            newStore.WriteToDisc();

            var readStore = RuleStore.Open(fileLoc, key);

            Assert.AreEqual(saveValue, readStore.Rules.First().Properties[saveKey]);
            Assert.IsTrue(readStore.Rules.First().Templates.First().Name == templateName);

            File.Delete(fileLoc);
        }

        [TestMethod]
        public void TestRuleTemplateStore() {
            string fileLoc = "Unittest.json";
            string key = "password123";

            if (File.Exists(fileLoc)) {
                File.Delete(fileLoc);
            }

            RuleStore newStore = RuleStore.Create(fileLoc, key);
            Rule rule = new Rule(RuleTemplate.GetByName("Basic"), RuleTemplate.GetByName("Application"));

            newStore.Rules.Add(rule);
            newStore.WriteToDisc();

            var readStore = RuleStore.Open(fileLoc, key);
            Assert.IsTrue(readStore.Rules.First().Templates.Count() == 2);
            Assert.IsTrue(readStore.Rules.First().Templates.FirstOrDefault(rt => rt.Name == "Basic") != null);
            Assert.IsTrue(readStore.Rules.First().Templates.FirstOrDefault(rt => rt.Name == "Application") != null);

            File.Delete(fileLoc);
        }
    }
}
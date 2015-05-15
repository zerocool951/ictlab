using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server_Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contextsec_Server_UnitTest {
    [TestClass]
    public class UnitTest {

        [TestMethod]
        public void TestRuleStore() {
            string fileLoc = "Unittest.json";
            string key = "password123";

            if (File.Exists(fileLoc)) {
                File.Delete(fileLoc);
            }

            RuleStore newStore = RuleStore.Create(fileLoc, key);

            newStore.Rules.Add(
                new Rule() {
                    Properties = new Dictionary<string, object>() {
                        { "key1", "val1" },
                        { "key2", "val2" }
                    }
                });

            newStore.WriteToDisc();

            var readStore = RuleStore.Open(fileLoc, key);

            Assert.AreEqual("val1", readStore.Rules.First().Properties["key1"]);

            File.Delete(fileLoc);
        }
    }
}
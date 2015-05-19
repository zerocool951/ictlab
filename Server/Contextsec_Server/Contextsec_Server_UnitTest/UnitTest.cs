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
            Assert.IsTrue(readStore.Rules.First().GetTemplates().First().Name == templateName);

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
            Assert.IsTrue(readStore.Rules.First().GetTemplates().Count() == 2);
            Assert.IsTrue(readStore.Rules.First().GetTemplates().FirstOrDefault(rt => rt.Name == "Basic") != null);
            Assert.IsTrue(readStore.Rules.First().GetTemplates().FirstOrDefault(rt => rt.Name == "Application") != null);

            File.Delete(fileLoc);
        }

        [TestMethod]
        public void TestValidation() {
            RuleTemplate type = RuleTemplate.GetByName("Basic");
            Rule inValid1 = new Rule(type); //Invalid because required prop is not present
            Rule inValid2 = new Rule(type);
            inValid2.Properties.Add("Id", null); //Invalid because required prop is null

            Rule inValid3 = new Rule(type);
            inValid3.Properties.Add("Id", 1.1); //Invalid because required prop is wrong type

            Rule valid = new Rule(type);
            valid.Properties.Add("test", null);
            valid.Properties.Add("Name", "someName");
            valid.Properties.Add("Id", 1); //Valid because required prop is set and is of the right type

            Assert.IsTrue(inValid1.RuleTypeNames.Contains("Basic"));
            Assert.IsFalse(inValid1.IsValid);
            Assert.IsFalse(inValid2.IsValid);
            Assert.IsFalse(inValid3.IsValid);
            Assert.IsTrue(valid.IsValid);

            //Tests case insensitivity and duplicate detection
            Rule multiple = new Rule(RuleTemplate.GetByName("bASIC"), RuleTemplate.GetByName("Application"), RuleTemplate.GetByName("Basic")) {
                Properties = new Dictionary<string, object>() { { "Id", 1 }, { "Name", "someName" } }
            };
            Assert.IsFalse(multiple.IsValid);
            multiple.Properties.Add("Application", "testApplication");
            Assert.IsTrue(multiple.IsValid);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server_Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contextsec_Server_UnitTest {
    [TestClass]
    public class TemplateTest {

        /// <summary>
        /// Tests the static RuleTemplate.GetByName method
        /// </summary>
        [TestMethod]
        public void TestTemplateGetByName() {
            RuleTemplate foundTemplate = RuleTemplate.GetByName("bAsIc");
            Rule rule = new Rule(foundTemplate);
            Assert.IsTrue(rule.RuleTypeNames.Contains("Basic"));
        }

        /// <summary>
        /// Tests the validation of a rule which has 1 template assigned to it
        /// </summary>
        [TestMethod]
        public void TestSingleValidation() {
            RuleTemplate basicRuleTemplate = RuleTemplate.GetByName("TestTemplate");
            //#1: Invalid because required prop is not present
            Rule inValidRule1 = new Rule(basicRuleTemplate);
            //#2: Invalid because required prop is null
            Rule inValidRule2 = new Rule(basicRuleTemplate);
            inValidRule2.Properties.Add("TestKey", null);
            //#3 Invalid because required prop is wrong type
            Rule inValidRule3 = new Rule(basicRuleTemplate);
            inValidRule3.Properties.Add("TestKey", 1.1);

            //Valid because required prop is set and is of the right type
            Rule validRule = new Rule(basicRuleTemplate);
            validRule.Properties.Add("test", null);
            validRule.Properties.Add("TestKey", 1);

            Assert.IsFalse(inValidRule1.IsValid);
            Assert.IsFalse(inValidRule2.IsValid);
            Assert.IsFalse(inValidRule3.IsValid);
            Assert.IsTrue(validRule.IsValid);
        }

        /// <summary>
        /// Tests the validation of a rule which has >1 template assigned to it
        /// </summary>
        [TestMethod]
        public void TestMultipleValidation() {
            //Tests case insensitivity and duplicate template detection
            Rule multipleTemplateTest = new Rule(
                    RuleTemplate.GetByName("TestTemplate"),
                    RuleTemplate.GetByName("ApplicaTION"),
                    RuleTemplate.GetByName("tESTteMPLATE"));
            multipleTemplateTest.Properties.Add("TestKey", 1);

            //The rule is not complete and should be invalid (!IsValid)
            Assert.IsFalse(multipleTemplateTest.IsValid);

            //After this change the rule should be valid
            multipleTemplateTest.Properties.Add("Application", "testApplication");
            Assert.IsTrue(multipleTemplateTest.IsValid);
        }
    }
}
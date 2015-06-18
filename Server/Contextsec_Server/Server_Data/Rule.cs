using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    /// <summary>
    /// Context Rule, based on this rules properties a decision can be made by the ContextSec app whether or not to use a secure connection.
    /// </summary>
    public class Rule {

        /// <summary>
        /// Key-value properties of this rule.
        /// </summary>
        [JsonProperty]
        public IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// List of the templates this rule has, this indicates which keys you can expect in the Properties Dictionary.
        /// </summary>
        [JsonProperty]
        public IEnumerable<string> RuleTypeNames {
            get {
                return Templates.Select(rt => rt.Name);
            }
        }

        /// <summary>
        /// Set of Templates this Rule should adhere to.
        /// </summary>
        [JsonIgnore]
        public ISet<RuleTemplate> Templates { get; private set; }

        /// <summary>
        /// Name to display for this rule in a UI, based this rules templates.
        /// </summary>
        [JsonIgnore]
        public string DisplayName {
            get {
                foreach (RuleTemplate rt in Templates) {
                    string name = rt.GetDisplayName(this);
                    if (name != null) {
                        return name;
                    }
                }
                return "Rule";
            }
        }

        /// <summary>
        /// Rule is valid if it adheres to all the requirements of all its templates.
        /// </summary>
        [JsonIgnore]
        public bool IsValid {
            get {
                return Templates.All(rt => rt.ValidateRule(this));
            }
        }

        /// <summary>
        /// New Rule with one or more templates.
        /// </summary>
        /// <param name="templates">Template(s) to attach to this rule.</param>
        public Rule(params RuleTemplate[] templates) {
            Properties = new Dictionary<string, object>();
            if (templates == null) {
                Templates = new HashSet<RuleTemplate>();
            } else {
                Templates = new HashSet<RuleTemplate>(templates);
            }
        }

        /// <summary>
        /// Create empty properties based on the templates this Rule has, will check if property exists before creating.
        /// </summary>
        public void CreateTemplateProperties() {
            foreach (string key in Templates.SelectMany(t => t.RequiredProperties.Keys)) {
                if (!Properties.ContainsKey(key)) {
                    Properties.Add(key, null);
                }
            }
        }

        /// <summary>
        /// Creates a rule and adds teamplates based on name, ment for JSON deserialization.
        /// </summary>
        /// <param name="ruleTypeNames">List of template names</param>
        [JsonConstructor]
        private Rule(IEnumerable<string> ruleTypeNames)
            : this(GetTemplatesByName(ruleTypeNames).ToArray()) {
        }

        /// <summary>
        /// Converts a list of RuleTemplate names, into a list of RuleTemplates.
        /// Removes duplicates.
        /// </summary>
        /// <param name="templateNames">List containt the names of RuleTemplates.</param>
        private static IEnumerable<RuleTemplate> GetTemplatesByName(IEnumerable<string> templateNames) {
            var templates = new HashSet<RuleTemplate>();
            foreach (string ruleTypeName in templateNames) {
                templates.Add(RuleTemplate.GetByName(ruleTypeName));
            }

            return templates;
        }
    }
}
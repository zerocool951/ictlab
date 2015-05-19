using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    public class Rule {

        [JsonProperty]
        public Dictionary<string, object> Properties;

        [JsonProperty]
        public IEnumerable<string> RuleTypeNames {
            get {
                return Templates.Select(rt => rt.Name);
            }
        }

        [JsonIgnore]
        private HashSet<RuleTemplate> Templates;

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

        [JsonIgnore]
        public bool IsValid {
            get {
                return Templates.All(rt => rt.IsValid(Properties));
            }
        }

        public Rule(params RuleTemplate[] templates) {
            Properties = new Dictionary<string, object>();
            if (templates == null) {
                Templates = new HashSet<RuleTemplate>();
            } else {
                Templates = new HashSet<RuleTemplate>(templates);
            }
        }

        [JsonConstructor]
        private Rule(IEnumerable<string> ruleTypeNames)
            : this(GetTemplatesByName(ruleTypeNames)) {
        }

        private static RuleTemplate[] GetTemplatesByName(IEnumerable<string> templateNames) {
            var templates = new HashSet<RuleTemplate>();
            foreach (string ruleTypeName in templateNames) {
                templates.Add(RuleTemplate.GetByName(ruleTypeName));
            }

            return templates.ToArray();
        }

        public IEnumerable<RuleTemplate> GetTemplates() {
            return Templates.AsEnumerable();
        }
    }
}
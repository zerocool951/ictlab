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

        [JsonIgnore]
        public HashSet<RuleTemplate> Templates;

        [JsonProperty]
        public IEnumerable<string> RuleTypeNames {
            get {
                return Templates.Select(rt => rt.Name);
            }
        }

        public Rule(RuleTemplate template = null)
            : this((template == null) ? null : new RuleTemplate[] { template }) {
        }

        public Rule(IEnumerable<RuleTemplate> templates) {
            Properties = new Dictionary<string, object>();
            if (templates == null) {
                Templates = new HashSet<RuleTemplate>();
            } else {
                Templates = new HashSet<RuleTemplate>(templates);
            }
        }

        public bool IsValid() {
            return Templates.All(rt => rt.IsValid(Properties));
        }

        [JsonConstructor]
        private Rule(IEnumerable<string> ruleTypeNames) {
            Templates = new HashSet<RuleTemplate>();
            foreach (string ruleTypeName in ruleTypeNames) {
                Templates.Add(RuleTemplate.GetByName(ruleTypeName));
            }
        }
    }
}
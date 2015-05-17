using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    public class RuleTemplate : IEqualityComparer<RuleTemplate> {

        public static HashSet<RuleTemplate> RuleTemplates = new HashSet<RuleTemplate>() {
            new RuleTemplate("Empty", null),
            new RuleTemplate("Basic", new Dictionary<string,Type>() { { "Id", typeof(int) } }),
            new RuleTemplate("Application", new Dictionary<string,Type>() { { "Application", typeof(string) } })
        };

        /// <summary>
        /// Get a Rule template by its name
        /// </summary>
        /// <param name="name">NOT case sensitive</param>
        /// <returns>Found object, or null if not found</returns>
        public static RuleTemplate GetByName(string name) {
            return RuleTemplates.FirstOrDefault(rt => rt.Name.ToLower() == name.ToLower());
        }

        public readonly string Name;
        private readonly IReadOnlyDictionary<string, Type> RequiredProperties;

        private RuleTemplate(string name, Dictionary<string, Type> requiredProperties) {
            if (requiredProperties == null) {
                requiredProperties = new Dictionary<string, Type>();
            }
            RequiredProperties = new ReadOnlyDictionary<string, Type>(requiredProperties);

            Name = name;
        }

        public bool IsValid(Dictionary<string, object> properties) {
            foreach (KeyValuePair<string, Type> required in RequiredProperties) {
                if (properties.ContainsKey(required.Key)) {
                    object value = properties[required.Key];
                    if (value == null)
                        return false;

                    if (value.GetType() != required.Value)
                        return false;
                } else {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(RuleTemplate x, RuleTemplate y) {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(RuleTemplate obj) {
            return Name.ToLower().GetHashCode();
        }
    }
}
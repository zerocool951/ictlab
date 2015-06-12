using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    public class RuleTemplate : IEqualityComparer<RuleTemplate> {

        public static HashSet<RuleTemplate> RuleTemplates = new HashSet<RuleTemplate>() {
            new RuleTemplate("TestTemplate", new Dictionary<string,Type>() {
                { "TestKey", typeof(int) }
            }),

            new RuleTemplate("Empty", null),

            new RuleTemplate("Basic", new Dictionary<string,Type>() {
                    { "Id", typeof(long) },
                    { "Name", typeof(string)},
                    { "Group", typeof(string)}
                },
                (r) => CalcDisplayName(r, "Name")),

            new RuleTemplate("Application", new Dictionary<string,Type>() {
                    { "Application", typeof(string) }
                },
                (r) => CalcDisplayName(r, "Application")),

            new RuleTemplate("Between", new Dictionary<string,Type>() {
                { "From", typeof(long) },
                { "Until", typeof(long) }
            },
            (r) => string.Format("From: '{0}' 'till: '{1}'", CalcDisplayName(r, "From"), CalcDisplayName(r, "Until")))
        };

        private static string CalcDisplayName(Rule rule, string key) {
            object val = null;
            if (rule.Properties.TryGetValue(key, out val)) {
                if (val != null) {
                    return val.ToString();
                }
            }
            return string.Format("{0} is empty", key);
        }

        public static string[] AllTemplateNames {
            get {
                return RuleTemplates.Select(rt => rt.Name).ToArray();
            }
        }

        /// <summary>
        /// Get a Rule template by its name
        /// </summary>
        /// <param name="name">NOT case sensitive</param>
        /// <returns>Found object, or null if not found</returns>
        public static RuleTemplate GetByName(string name) {
            if (name != null) {
                return RuleTemplates.FirstOrDefault(rt => rt.Name.ToLower() == name.ToLower());
            } else {
                return null;
            }
        }

        public string Name { get; private set; }

        public readonly Func<Rule, string> GetDisplayName;
        public readonly IReadOnlyDictionary<string, Type> RequiredProperties;

        private RuleTemplate(string name, Dictionary<string, Type> requiredProperties)
            : this(name, requiredProperties, (r) => null) {
        }

        private RuleTemplate(string name, Dictionary<string, Type> requiredProperties, Func<Rule, string> getDisplayName) {
            if (requiredProperties == null) {
                requiredProperties = new Dictionary<string, Type>();
            }

            RequiredProperties = new ReadOnlyDictionary<string, Type>(requiredProperties);

            Name = name;
            GetDisplayName = getDisplayName;
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

    public static class TemplateExtensions {

        public static Type GetTypeByKey(this IEnumerable<RuleTemplate> templates, string key) {
            return templates.SelectMany(rt => rt.RequiredProperties).FirstOrDefault(kvp => kvp.Key == key).Value;
        }
    }
}
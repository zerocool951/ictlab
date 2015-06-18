using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    /// <summary>
    /// A rule template defines the different rules by what properties they have.
    /// </summary>
    public class RuleTemplate : IEqualityComparer<RuleTemplate> {

        /// <summary>
        /// Copy of the all RuleTemplates collection
        /// </summary>
        public static RuleTemplate[] RuleTemplates {
            get {
                return ruleTemplates.ToArray();
            }
        }

        private static ISet<RuleTemplate> ruleTemplates = new HashSet<RuleTemplate>() {
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
            object value = null;
            if (rule.Properties.TryGetValue(key, out value) && value != null) {
                return value.ToString();
            }
            return string.Format("{0} is empty", key);
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

        /// <summary>
        /// Name of the Rule, will be used for Rule comparison
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Function which returns the display name of a input Rule
        /// </summary>
        public readonly Func<Rule, string> GetDisplayName;

        /// <summary>
        /// Dictates what properties a rule should have and what type they should have.
        /// </summary>
        public readonly IReadOnlyDictionary<string, Type> RequiredProperties;

        private RuleTemplate(string name, IDictionary<string, Type> requiredProperties)
            : this(name, requiredProperties, (r) => null) {
        }

        private RuleTemplate(string name, IDictionary<string, Type> requiredProperties, Func<Rule, string> getDisplayName) {
            if (requiredProperties == null) {
                requiredProperties = new Dictionary<string, Type>();
            }

            RequiredProperties = new ReadOnlyDictionary<string, Type>(requiredProperties);

            Name = name;
            GetDisplayName = getDisplayName;
        }

        /// <summary>
        /// Checks if a rule follows this template.
        /// </summary>
        /// <param name="rule">The Rule to check for validity</param>
        /// <returns>True if the rule has all the required properties and they are of the expected Type, otherwise false.</returns>
        public bool ValidateRule(Rule rule) {
            foreach (KeyValuePair<string, Type> required in RequiredProperties) {
                if (rule.Properties.ContainsKey(required.Key)) {
                    object value = rule.Properties[required.Key];

                    if (value == null || value.GetType() != required.Value)
                        return false;
                } else {
                    return false;
                }
            }
            return true;
        }

        #region IEqualityComparer

        public bool Equals(RuleTemplate x, RuleTemplate y) {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(RuleTemplate obj) {
            return Name.ToLower().GetHashCode();
        }

        #endregion IEqualityComparer
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    /// <summary>
    /// Holds extension methods for RuleTemplate and collections of RuleTemplates
    /// </summary>
    public static class TemplateExtensions {

        /// <summary>
        /// Get the required Type of a Rule property by its name.
        /// </summary>
        /// <param name="key">Name of the property</param>
        /// <returns>Found Type</returns>
        public static Type GetTypeByKey(this IEnumerable<RuleTemplate> templates, string key) {
            return templates.SelectMany(rt => rt.RequiredProperties).FirstOrDefault(kvp => kvp.Key == key).Value;
        }
    }
}
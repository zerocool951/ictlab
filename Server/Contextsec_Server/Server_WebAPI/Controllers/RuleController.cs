using Server_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server_WebAPI.Controllers {
    public class RuleController : ApiController {

        /// <summary>
        /// Get all rules.
        /// </summary>
        /// <returns>Collection of all rules</returns>
        /// <exception cref="WebException">Thrown when there is no datastore connected. (Non-existing file, Invalid key, JSON read error, )</exception>
        public IEnumerable<Rule> Get() {
            var gotten = DataHelper.Instance;
            if (gotten != null) {
                return gotten.AllRules;
            } else {
                throw new WebException("No working datastore connected");
            }
        }

        /// <summary>
        /// Get a specific rule by Id.
        /// </summary>
        /// <param name="id">Rule Id.</param>
        /// <returns>Found rule, or null if rule is not found.</returns>
        /// <exception cref="WebException">Thrown when there is no datastore connected. (Non-existing file, Invalid key, JSON read error, )</exception>
        public Rule Get(long id) {
            var gotten = DataHelper.Instance;
            if (gotten != null) {
                return gotten.AllRules.FirstOrDefault(r => r.Properties.ContainsKey("Id") && r.Properties["Id"].Equals(id));
            } else {
                throw new WebException("No working datastore connected");
            }
        }
    }
}
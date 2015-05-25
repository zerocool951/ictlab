using Server_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Server_WebAPI.Controllers {
    public class RuleController : ApiController {

        // GET api/values
        public IEnumerable<Rule> Get() {
            var gotten = DataHelper.GetRuleStore();
            if (gotten != null) {
                return gotten.Rules;
            } else {
                throw new WebException("No working datastore connected");
            }
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }
    }
}
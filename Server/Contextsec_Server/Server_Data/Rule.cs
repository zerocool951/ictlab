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

        public Rule() {
            Properties = new Dictionary<string, object>();
        }
    }
}
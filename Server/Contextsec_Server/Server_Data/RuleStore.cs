using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Data {
    public class RuleStore {
        private string Key;
        private string DataFilePath;
        public List<Rule> Rules;
        private static JsonSerializerSettings JSonSettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };

        private RuleStore(string dataFilePath, string key, IEnumerable<Rule> rules) {
            DataFilePath = dataFilePath;
            Key = key;
            Rules = new List<Rule>(rules);
        }

        public void WriteToDisc() {
            using (StreamWriter sw = new StreamWriter(new FileStream(DataFilePath, FileMode.Create))) {
                string content = JsonConvert.SerializeObject(Rules, JSonSettings);
                string encryptedContent = EncryptionHelper.Encrypt(content, Key);
                sw.WriteAsync(encryptedContent);
            }
        }

        public static RuleStore Open(string dataFilePath, string key) {
            if (File.Exists(dataFilePath)) {
                string fileContent = File.ReadAllText(dataFilePath);
                string decryptedContent = EncryptionHelper.Decrypt(fileContent, key);

                try {
                    var rules = JsonConvert.DeserializeObject<IEnumerable<Rule>>(decryptedContent, JSonSettings);

                    return new RuleStore(dataFilePath, key, rules);
                } catch (JsonReaderException jre) {
                    throw new Exception(string.Format("Failed to read '{0}', make sure key is correct", dataFilePath), jre);
                }
            } else {
                throw new FileNotFoundException("Data File not found", dataFilePath);
            }
        }

        public static RuleStore Create(string dataFilePath, string key) {
            if (!File.Exists(dataFilePath)) {
                var newStore = new RuleStore(dataFilePath, key, new Rule[0]);
                newStore.WriteToDisc();
                return newStore;
            } else {
                throw new Exception("File already exists");
            }
        }
    }
}
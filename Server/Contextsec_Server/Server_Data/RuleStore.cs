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

        /// <summary>
        /// Private constructor, for internal use only
        /// </summary>
        /// <param name="dataFilePath">Path of the file holding the encrypted JSon rules</param>
        /// <param name="key">Key to (de/en)crypt the file</param>
        private RuleStore(string dataFilePath, string key, IEnumerable<Rule> rules = null) {
            DataFilePath = dataFilePath;
            Key = key;

            if (rules != null) {
                Rules = new List<Rule>(rules);
            } else {
                Rules = new List<Rule>();
            }
        }

        /// <summary>
        /// Write any changes made to this object to the filesystem.
        /// File content is encrypted using the same key which was used to create or read this instance.
        /// </summary>
        public async void WriteToDisc() {
            using (StreamWriter sw = new StreamWriter(new FileStream(DataFilePath, FileMode.Create))) {
                string content = JsonConvert.SerializeObject(Rules, JSonSettings);
                string encryptedContent = EncryptionHelper.Encrypt(content, Key);
                await sw.WriteAsync(encryptedContent);
            }
        }

        /// <summary>
        /// Decrypt and deserialize rulestore from disc
        /// </summary>
        /// <param name="dataFilePath">Path of the file holding the encrypted JSon rules</param>
        /// <param name="key">Key to decrypt the file</param>
        /// <exception cref="System.IO.FileNotFoundException">File does not exist</exception>
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

        /// <summary>
        /// Create a new empty RuleStore which is then written to disc.
        /// </summary>
        /// <param name="dataFilePath">Path of the file holding the encrypted JSon rules</param>
        /// <param name="key">Key to decrypt the file</param>
        /// <exception cref="System.IO.IOException">File already exists</exception>
        public static RuleStore Create(string dataFilePath, string key) {
            if (!File.Exists(dataFilePath)) {
                var newStore = new RuleStore(dataFilePath, key, new Rule[0]);
                newStore.WriteToDisc();
                return newStore;
            } else {
                throw new IOException("File already exists");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContactBook_ApiTests
{
    public class Contact
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("firstName")]  
        public string firstName { get; set; }
        [JsonPropertyName("lastName")]
        public string lastName { get; set; }
        [JsonPropertyName("email")]
        public string email { get; set; }
        [JsonPropertyName("phone")]
        public string phone { get; set; }
    }
}

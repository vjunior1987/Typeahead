using Newtonsoft.Json;

namespace Typeahead.Models
{
    public class EnvironmentSettings
    {
        [JsonProperty(PropertyName = Constants.Port)]
        public string Port { get; set; }

        [JsonProperty(PropertyName = Constants.SuggestionNumber)]
        public int SuggestionNumber { get; set; }

        [JsonProperty(PropertyName = Constants.Host)]
        public string Host { get; set;}
    }
}

using rm.Trie;
using System.Text.Json;

namespace Typeahead.DAL
{
    public class NameRepository : INameRepository
    {
        public TrieMap<Name> Names { get; set; }

        public NameRepository()
        {
            Names = new TrieMap<Name>();
            SeedData();
        }

        public void Update(Name name)
        {
            Names.Remove(name.Value.ToLowerInvariant());
            Names.Add(name.Value.ToLowerInvariant(), name);
        }

        private void SeedData()
        {
            if (!Names.KeyValuePairs().Any())
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, long>>(File.ReadAllText(@"./names.json"));
                foreach (var item in dict)
                {
                    Names.Add(item.Key.ToLowerInvariant(), new Name { Value = item.Key, Times = item.Value });
                }
            }
        }
    }
}

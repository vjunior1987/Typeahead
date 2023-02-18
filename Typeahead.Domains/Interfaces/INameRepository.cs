using rm.Trie;

namespace Typeahead.DAL
{
    public interface INameRepository
    {
        TrieMap<Name> Names { get; set; }
        void Update(Name name);
    }
}

using System.ComponentModel.DataAnnotations;

namespace Typeahead.DAL
{
    public class Name
    {
        [Key]
        public string Value { get; set; }
        public long Times { get; set; }
    }
}

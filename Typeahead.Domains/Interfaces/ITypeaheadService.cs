namespace Typeahead.Service
{
    public interface ITypeaheadService
    {
        Task<IEnumerable<NameDTO>> GetNamesByPrefix(string prefix);
        Task<NameDTO> IncreaseNamePopularity(NameDTO name);
    }
}

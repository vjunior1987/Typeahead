using AutoMapper;
using Microsoft.Extensions.Options;
using Typeahead.DAL;
using Typeahead.Models;

namespace Typeahead.Service
{
    public class TypeaheadService : ITypeaheadService
    {
        private readonly INameRepository _repository;
        private readonly IMapper _mapper;
        private readonly EnvironmentSettings _settings;

        public TypeaheadService(IMapper mapper, INameRepository repository, IOptions<EnvironmentSettings> options)
        {
            _repository = repository;
            _mapper = mapper;
            _settings = options.Value;
        }

        public async Task<IEnumerable<NameDTO>> GetNamesByPrefix(string prefix)
        {
            var task = Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(prefix))
                {
                    return _mapper.Map<IEnumerable<NameDTO>>(null);
                }

                var names = _repository.Names.ValuesBy(prefix.ToLower()).OrderByDescending(n => n.Times).ThenBy(n => n.Value).Take(_settings.SuggestionNumber);

                return _mapper.Map<IEnumerable<NameDTO>>(names);
            });

            return await task;
        }

        public async Task<NameDTO> IncreaseNamePopularity(NameDTO nameDTO)
        {
            var task = Task.Run(() =>
            {
                var name = _repository.Names.ValuesBy(nameDTO.Name.ToLowerInvariant()).FirstOrDefault();
                if (name != null)
                {
                    name.Times++;
                    _repository.Update(name);
                    return _mapper.Map<NameDTO>(name);
                }

                throw new KeyNotFoundException("Name not found");
            });
            return await task;
        }
    }
}
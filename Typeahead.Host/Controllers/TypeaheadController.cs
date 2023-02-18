using Microsoft.AspNetCore.Mvc;
using Typeahead.Service;

namespace Typeahead.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeaheadController : ControllerBase
    {
        private readonly ITypeaheadService _service;

        public TypeaheadController(ITypeaheadService service)
        {
            _service = service;
        }

        [HttpGet("{prefix}")]
        public async Task<IActionResult> GetNamesByPrefix([FromRoute] string prefix)
        {
            return Ok(await _service.GetNamesByPrefix(prefix));
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseNamePopularity(NameDTO name)
        {
            try
            {
                return Created(nameof(GetNamesByPrefix), await _service.IncreaseNamePopularity(name));
            }
            catch (KeyNotFoundException ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Typeahead.Host.Controllers;
using Typeahead.Service;
using Xunit;

namespace Typeahead.Controllers
{
    public class TypeaheadController_Tests
    {
        // Arrange
        private readonly ITypeaheadService _service;
        private TypeaheadController _controller;
        private const string testInput = "Jo";

        public TypeaheadController_Tests()
        {
            _service = Substitute.For<ITypeaheadService>();
            _controller = new TypeaheadController(_service);
        }

        [Fact, Description("when successfully requesting a list of suggestions for the prefix, the api should return a 200 result")]
        public async Task TypeaheadController_GetNamesByPrefix_Should_Return_OkResult_With_Names()
        {
            // Arrange
            _service.GetNamesByPrefix(Arg.Any<string>()).Returns(GetNames());

            // Act
            var result = await _controller.GetNamesByPrefix(testInput);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact, Description("when successfully requesting a list of suggestions for the prefix, the api should return 200 result containing a list of names")]
        public async Task TypeaheadController_GetNamesByPrefix_Should_Return_OkResult()
        {
            // Arrange
            _service.GetNamesByPrefix(Arg.Any<string>()).Returns(GetNames());

            // Act
            var result = await _controller.GetNamesByPrefix(testInput) as ObjectResult;

            // Assert
            Assert.NotEmpty(result.Value as IEnumerable<NameDTO>);
        }

        [Fact, Description("when increasing the popularity rate of a name, should return 201 created result")]
        public async Task TypeaheadController_IncreaseNamePopularity_Should_Return_OkResult()
        {
            // Arrange
            _service.IncreaseNamePopularity(Arg.Any<NameDTO>()).Returns(GetNames().FirstOrDefault());

            // Act
            var result = await _controller.IncreaseNamePopularity(GetNames().FirstOrDefault());

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact, Description("when increasing the popularity rate of a name, should return a 200 result containing the name and new popularity rate")]
        public async Task TypeaheadController_IncreaseNamePopularity_Should_Return_OkResult_With_Name()
        {
            // Arrange
            _service.IncreaseNamePopularity(Arg.Any<NameDTO>()).Returns(GetNames().FirstOrDefault());

            // Act
            var result = await _controller.IncreaseNamePopularity(GetNames().FirstOrDefault()) as ObjectResult;

            // Assert
            Assert.NotNull(result.Value as NameDTO);
        }

        [Fact, Description("when increasing the popularity rate of a name, should return 400 Bad request result when inputting a nonexistent name")]
        public async Task TypeaheadController_IncreaseNamePopularity_Should_Return_BadRequestResult_if_name_does_not_exist()
        {
            // Arrange
            _service.IncreaseNamePopularity(Arg.Any<NameDTO>()).Returns(_ => Task.FromException<NameDTO>(new KeyNotFoundException()));

            // Act
            var result = await _controller.IncreaseNamePopularity(GetNames().FirstOrDefault()) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private IEnumerable<NameDTO> GetNames()
        {
            return new List<NameDTO>
            {
                new NameDTO
                {
                    Name = "Johanna",
                    Times = 2
                },
                new NameDTO
                {
                    Name = "Jonathan",
                    Times = 2
                },
                new NameDTO
                {
                    Name = "Jonh",
                    Times = 1
                },
            };
        }
    }
}

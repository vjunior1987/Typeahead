using AutoMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Typeahead.DAL;
using Typeahead.Models;
using Typeahead.Service;
using Xunit;

namespace Typeahead.Services
{
    public class TypeaheadService_Tests
    {
        // Arrange
        private readonly ITypeaheadService _service;
        private readonly IMapper _mapper;
        const string testInput = "Jo";
        const int maxResults = 5;
        const int testTimes = 361;

        public TypeaheadService_Tests()
        {
            // Arrange
            IOptions<EnvironmentSettings> settings = Substitute.For<IOptions<EnvironmentSettings>>();
            settings.Value.Returns(new EnvironmentSettings { SuggestionNumber = maxResults });
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(typeof(NameProfile))));
            _service = new TypeaheadService(_mapper, new NameRepository(), settings);
        }

        // Arrange
        [Theory, Description("When searching for names with a prefix, the api should return a list of matches regardless of letter case")]
        [InlineData("jO")]
        [InlineData("JO")]
        [InlineData("jo")]
        public async Task Should_Return_matches_with_different_cases(string prefix)
        {
            // Act
            var result = await _service.GetNamesByPrefix(prefix);

            // Assert
            Assert.NotEmpty(result);

        }

        [Fact, Description("When searching for names with no matching results, the API should return an empty list")]
        public async Task Should_Return_empty_list_if_no_matching_results()
        {
            // Act
            var result = await _service.GetNamesByPrefix("foo");

            // Arrange
            Assert.Empty(result);
        }

        [Fact, Description("When searching for names with empty input, the API should return an empty list")]
        public async Task Should_Return_empty_list_if_input_is_empty()
        {
            // Act
            var result = await _service.GetNamesByPrefix("");

            // Arrange
            Assert.Empty(result);
        }

        [Fact, Description("When searching for names with no matching results, the API should restrict the number of suggestions by configuration")]
        public async Task Should_Return_list_limited_by_configuration()
        {
            // Act
            var result = await _service.GetNamesByPrefix(testInput);

            // Arrange
            Assert.Equal(maxResults, result.ToList().Count);
        }

        [Fact, Description("When searching for anmes with no matching results, the API should return a list ordered by the most popular names")]
        public async Task Should_Return_list_Ordered_by_popularity()
        {
            // Act
            var result = await _service.GetNamesByPrefix(testInput);

            // Arrange
            result.ToList().ShouldBeEquivalentTo(result.OrderByDescending(x => x.Times).ToList());
        }

        [Fact, Description("When searching for anmes with no matching results, any names with the same popularity rating should be ordered by alphabetical order")]
        public async Task Should_Return_list_Ordered_by_popularity_then_name()
        {
            // Arrange
            const string input = "A";
            // Act
            var result = await _service.GetNamesByPrefix(input);

            // Arrange
            result = result.Where(x => x.Times == testTimes);
            result.ToList().ShouldBeEquivalentTo(result.OrderBy(x => x.Name).ToList());
        }

        // Arrange
        [Theory, Description("When increasing a name popularity rate, the API should increment the rating by one each time it is requested")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task Should_increment_times_by_one_each_time(int iterations)
        {
            // Arrange
            var input = (await _service.GetNamesByPrefix("Aaren")).FirstOrDefault();
            NameDTO result = new NameDTO();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                result = await _service.IncreaseNamePopularity(input);
            }

            // Assert
            Assert.Equal(input.Times + iterations, result.Times);
        }

        [Fact, Description("When increasing a name popularity rate, the API should not create a new name if it doesn't exist")]
        public void Should_Not_create_a_new_name_if_it_does_not_exist()
        {
            // Arrange
            var input = new NameDTO { Name = "Foo" };

            // Act
            async Task result () => await _service.IncreaseNamePopularity(input);

            // Arrange
            Assert.ThrowsAsync<KeyNotFoundException>(result);
        }
    }
}
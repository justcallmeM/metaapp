namespace metaapp.Tests
{
    using Moq;
    using Infrastructure.Clients.WeatherAPI.Interfaces;
    using DataAccess.Behaviour.Interfaces;
    using Application.Common.Models.Requests;
    using DataAccess.Models;
    using Infrastructure.Services.WeatherRetrieval;
    using metaapp.Tests.Data;
    using AutoFixture;

    public class WeatherRetrievalServiceTests
    {
        private readonly Fixture fixture;

        private readonly Mock<IWeatherApiWeatherClient> _weatherApiWeatherClient;
        private readonly Mock<IWeatherDataBehaviour> _weatherDataBehaviour;

        private readonly WeatherRetrievalService _weatherRetrievalService;

        public WeatherRetrievalServiceTests()
        {
            fixture = new Fixture();

            _weatherApiWeatherClient = new Mock<IWeatherApiWeatherClient>();
            _weatherDataBehaviour = new Mock<IWeatherDataBehaviour>();

            _weatherRetrievalService = new WeatherRetrievalService(
                _weatherApiWeatherClient.Object, 
                _weatherDataBehaviour.Object);
        }

        [Fact]
        public async void GetAndSaveWeatherAsync_Given_CollectionOfRequests_When_RequestsAreValid_Then_UpsertWeatherDataAsyncIsCalledOnce()
        {
            // Arrange
            _weatherApiWeatherClient
                .Setup(x => x.GetWeatherAsync(It.IsAny<List<WeatherRequest>>()))
                .ReturnsAsync(SharedTestData.GetWeatherResponseCollectionWithRequiredData(fixture));

            _weatherDataBehaviour
                .Setup(x => x.UpsertWeatherDataAsync(It.IsAny<WeatherData>()))
                .Verifiable();

            // Act
            await _weatherRetrievalService.GetAndSaveWeatherAsync(SharedTestData.GetWeatherRequestCollectioNWithRequiredData(fixture));

            // Assert
            _weatherDataBehaviour.Verify(x => x.UpsertWeatherDataAsync(It.IsAny<WeatherData>()), Times.Once);
        }

        [Theory]
        [InlineData("--city Vilnius")]
        [InlineData("--city Vilnius, Riga")]
        public void CreateWeatherRequests_Given_Input_When_InputIsValid_Then_CollectionOfRequestsIsCreated(
            string input)
        {
            // Act
            var requests = _weatherRetrievalService.CreateWeatherRequests(input);

            // Assert
            Assert.NotEmpty(requests);
            Assert.Equal("vilnius", requests.First().CityName);
        }

        [Theory]
        [InlineData("--country Lithuania, Ukraine")]
        public void CreateWeatherRequests_Given_Input_When_InputIsInValid_Then_ExceptionIsThrown(
            string input)
        {
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => _weatherRetrievalService.CreateWeatherRequests(input));
            Assert.Equal("This argument isn't supported", exception.Message);
        }
    }
}
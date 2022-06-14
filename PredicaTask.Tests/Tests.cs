using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PredicaTask.API.Controllers;
using PredicaTask.Application.Dtos.WeatherDtos;
using PredicaTask.Application.Profiles.WeatherProfile;
using PredicaTask.Application.Services;
using PredicaTask.Data;
using PredicaTask.DataManipulation;
using PredicaTask.Domain;

namespace PredicaTask.Tests
{
    [TestFixture]
    public class Test
    {
        private readonly List<Weather> _weathers;
        private static PredicaTaskDbContext _dbContext;
        private static IMapper _mapper;

        public Test()
        {
            _weathers = new List<Weather>();
            var id = new List<int>();
            id.Add(1);
            id.Add(2);
            id.Add(3);
            id.Add(4);
            id.Add(5);
            foreach (var i in id)
                _weathers.Add(new Weather
                {
                    Id = i,
                    Summary = $"Weather {i}",
                    TemperatureC = Convert.ToInt32($"{i}"),
                    Date = Convert.ToDateTime("01-01-2022"),
                    City = $"City {i}"
                });
        }
        
        [SetUp]
        public async Task Init()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new WeatherProfile());
                });
                var mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            
            var options = new DbContextOptionsBuilder<PredicaTaskDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging()
                .Options;
            
            _dbContext = new PredicaTaskDbContext(options);
            await _dbContext.Weathers.AddRangeAsync(_weathers);
            await _dbContext.SaveChangesAsync();
        }

        [TearDown]
        public async Task Dispose()
        {
            await _dbContext.DisposeAsync();
        }
        
        [Test, Order(1)]
        public async Task GetAllWeathers()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_dbContext);
            var weatherController = new WeatherController(new WeathersService(unitOfWork, _mapper));
            
            // Act
            var actionResult = await weatherController.GetAllWeathers();

            // Assert
            var result = actionResult as OkObjectResult;
            var weatherDtos = result.Value as List<WeatherDto>;
            Assert.IsNotNull(result);
            Assert.That(weatherDtos, Is.InstanceOf<List<WeatherDto>>());
            Assert.That((result.Value as List<WeatherDto>).Count(), Is.EqualTo(5));

            foreach (var weatherDto in result.Value as List<WeatherDto>)
            {
                Assert.AreEqual(1, _dbContext.Weathers.Count(x => x.Id == weatherDto.Id));
            }
        }
        
        [Test, Order(2)]
        public async Task GetWeatherById()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_dbContext);
            var weatherController = new WeatherController(new WeathersService(unitOfWork, _mapper));
            var weatherToGet = _weathers[2];
        
            // Act
            var actionResult = await weatherController.GetWeather(weatherToGet.Id);
        
            // Assert
            var result = actionResult as OkObjectResult;
            var weatherDto = result.Value as WeatherDto;
            Assert.IsNotNull(weatherDto);
            Assert.AreEqual(weatherToGet.Id, weatherDto.Id);
        }
        
        [Test, Order(3)]
        public async Task CreateWeather()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_dbContext);
            var weatherController = new WeatherController(new WeathersService(unitOfWork, _mapper));
            var createWeatherDto = new CreateWeatherDto()
            {
                Date = Convert.ToDateTime("01-01-2022"),
                TemperatureC = 32,
                Summary = "Test",
                City = "Krakow",
            };
        
            // Act
            var actionResult = await weatherController.CreateNewWeather(createWeatherDto);
        
            // Assert
            var result = actionResult as CreatedAtRouteResult;
            var weatherDto = result.Value as WeatherDto;
            Assert.AreEqual(1, _dbContext.Weathers.Count(x => x.Summary == createWeatherDto.Summary 
                                                               && x.City == createWeatherDto.City));
        }
        
        [Test, Order(4)]
        public async Task DeleteWeatherById()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_dbContext);
            var weatherController = new WeatherController(new WeathersService(unitOfWork, _mapper));
            var weatherToDelete = _weathers[4];
        
            // Act
            var actionResult = await weatherController.DeleteWeather(weatherToDelete.Id);
        
            // Assert
            var result = actionResult as NoContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, _dbContext.Weathers.AsNoTracking().Count(x => x.Id == weatherToDelete.Id));
        }
        
        [Test, Order(5)]
        public async Task UpdateWeather()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_dbContext);
            var weatherController = new WeatherController(new WeathersService(unitOfWork, _mapper));
            var weather = _weathers[1];
            var newDate = Convert.ToDateTime("02-02-2022");
            var weatherToUpdate = new UpdateWeatherDto() 
            {
                TemperatureC = 32,
                Summary = "Test",
                Date = Convert.ToDateTime("02-02-2022")
            };
            
            // Act
            _dbContext.Entry(weather).State = EntityState.Detached;
            var actionResult = await weatherController.UpdateWeather(weather.Id, weatherToUpdate);
            
            // Assert
            var result = actionResult as NoContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(newDate, _dbContext.Weathers.FirstOrDefault(x => x.Id == weather.Id).Date);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PredicaTask.Application.Dtos.WeatherDtos;
using PredicaTask.Application.Exceptions;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Domain;

namespace PredicaTask.Application.Services
{
    public class WeathersService : IWeathersService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public WeathersService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<WeatherDto> CreateWeather(CreateWeatherDto createWeather)
        {
            var weatherDto = new WeatherDto()
            {
                City = createWeather.City,
                Date = createWeather.Date,
                Summary = createWeather.Summary,
                TemperatureC = createWeather.TemperatureC
            };

            var weather = _mapper.Map<Weather>(weatherDto);
            await _uow.Weathers.Add(weather);
            await _uow.Weathers.Save();

            return weatherDto;
        }

        public async Task<WeatherDto> GetWeather(int id)
        {
            var weather = await ReturnObjectIfTakenDataFromDbIsNotNull(id);
            var weatherDto = _mapper.Map<WeatherDto>(weather);
            return weatherDto;
        }

        public async Task<IReadOnlyCollection<WeatherDto>> GetWeathers()
        {
            var weathers = await _uow.Weathers.GetAll();
            var weathersDto = _mapper.Map<List<WeatherDto>>(weathers);

            return weathersDto;
        }

        public async Task UpdateWeather(int id, UpdateWeatherDto updateWeatherDto)
        {
            var weather = await ReturnObjectIfTakenDataFromDbIsNotNull(id);
            weather.Date = DateTime.Now;

            _mapper.Map(updateWeatherDto, weather);
            await _uow.Weathers.Modify(weather);
            await _uow.Weathers.Save();
        }

        public async Task DeleteWeather(int id)
        {
            await ReturnObjectIfTakenDataFromDbIsNotNull(id);
            await _uow.Weathers.Remove(id);
            await _uow.Weathers.Save();
        }

        private async Task<Weather> ReturnObjectIfTakenDataFromDbIsNotNull(int id)
        {    
            var weather = await _uow.Weathers.Get(x => x.Id == id);
            
            if (weather == null)
            {
                throw new NotFoundException("Weather not found");
            }

            return weather;
        }
        
    }
}
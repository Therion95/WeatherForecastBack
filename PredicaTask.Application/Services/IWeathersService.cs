using System.Collections.Generic;
using System.Threading.Tasks;
using PredicaTask.Application.Dtos.WeatherDtos;

namespace PredicaTask.Application.Services
{
    public interface IWeathersService
    {
        Task<WeatherDto> CreateWeather(CreateWeatherDto createWeather);
        Task<WeatherDto> GetWeather(int id);
        Task<IReadOnlyCollection<WeatherDto>> GetWeathers();
        Task UpdateWeather(int id, UpdateWeatherDto updateWeatherDto);
        Task DeleteWeather(int id);
        
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PredicaTask.Application.Dtos.WeatherDtos;
using PredicaTask.Application.Services;

namespace PredicaTask.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeatherController : Controller
    {
        private readonly IWeathersService _service;
        public WeatherController(IWeathersService service)
        {
            _service = service;
        }
        
        [HttpGet("{id:int}", Name = "GetWeather")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWeather(int id)
        {
            var weather = await _service.GetWeather(id);
            return Ok(weather);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllWeathers()
        {
            var weathers = await _service.GetWeathers();
            return Ok(weathers);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNewWeather([FromBody] CreateWeatherDto createWeatherDto)
        {
            var weatherDto = await _service.CreateWeather(createWeatherDto);

            return CreatedAtRoute("GetWeather", new { id = weatherDto.Id }, weatherDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateWeather(int id, [FromBody] UpdateWeatherDto updateWeatherDto)
        {
            await _service.UpdateWeather(id, updateWeatherDto);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteWeather(int id)
        {
            await _service.DeleteWeather(id);
            return NoContent();
        }
        
    }
}
using AssecorAssessment.Models;
using AssecorAssessment.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssecorAssessment.Controllers
{
    /// <summary>  
    /// REST-API Controller for managing persons.  
    /// </summary>  

    // Explenation for: Why are there no validation methods for GetAll and GetById?
    // -> GetAll does not require validation as it has no parameters.
    // -> GetById only checks if the ID is greater than 0, which is a simple validation handled directly in the controller.

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>  
        /// GET /persons - Returns all persons.  
        /// </summary>  
        /// <returns>List of all persons.</returns>  
        [HttpGet]
        public ActionResult<List<Person>> GetAll()
        {
            var persons = _personService.GetAll();

            if (!persons.Any())
                return NotFound(new { errors = new List<string> { $"No persons found." } });

            return Ok(persons);
        }

        /// <summary>  
        /// GET /persons/{id} - Returns a specific person.  
        /// </summary>  
        /// <param name="id">The ID of the person.</param>  
        /// <returns>The person if found.</returns>  
        [HttpGet("{id}")]
        public ActionResult<Person> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { errors = new List<string> { "Invalid ID. The ID must be bigger than 0." } });

            var person = _personService.GetById(id);

            if (person == null)
                return NotFound(new { errors = new List<string> { $"Person with ID {id} not found." } });

            return Ok(person);
        }

        /// <summary>  
        /// GET /persons/color/{color} - Filters persons by favorite color.  
        /// </summary>  
        /// <param name="color">The color (e.g., "blue", "red").</param>  
        /// <returns>List of persons with the specified color.</returns>  
        [HttpGet("color/{color}")]
        public ActionResult<List<Person>> GetByColor(string color)
        {
            var result = _personService.GetByColor(color);
            if (!result.Successful)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = result.ErrorMessages
                });
            }

            return Ok(result.Data);
        }

        /// <summary>  
        /// POST /persons - Adds a new person / Method from the bonus features.  
        /// </summary>  
        /// <param name="person">The new person.</param>  
        /// <returns>The created person with ID.</returns>  
        [HttpPost]
        public ActionResult<Person> Create([FromBody] Person person)
        {
            var result = _personService.Add(person);

            if (!result.Successful)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = result.ErrorMessages
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }
    }
}
using AssecorAssessment.Models;

namespace AssecorAssessment.Services
{
    /// <summary>
    /// Service layer interface for business logic.
    /// Separates controller logic from data access.
    /// </summary>
    public interface IPersonService
    {
        /// <summary>  
        /// Retrieves all persons from the repository.  
        /// </summary>  
        /// <returns>A collection of all persons.</returns>
        List<Person> GetAll();

        /// <summary>  
        /// Retrieves a person by their unique ID.  
        /// </summary>  
        /// <param name="id">The unique ID of the person.</param>  
        /// <returns>The person if found, otherwise null.</returns>  
        Person? GetById(int id);

        /// <summary>  
        /// Retrieves persons filtered by their favorite color.  
        /// </summary>  
        /// <param name="color">The favorite color to filter by (case-insensitive).</param>  
        /// <returns>A collection of persons matching the color.</returns>  
        ServiceResult<List<Person>> GetByColor(string color);

        /// <summary>  
        /// Adds a new person to the repository.  
        /// </summary>  
        /// <param name="person">The person to add.</param>  
        /// <returns>The added person with an assigned ID and a service result.</returns>  
        ServiceResult<Person> Add(Person person);
    }
}
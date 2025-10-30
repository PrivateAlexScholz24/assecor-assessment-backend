using AssecorAssessment.Models;

namespace AssecorAssessment.Services
{
    public interface IPersonValidator
    {
        /// <summary>
        /// Validates a person object before adding it to the data store.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Returs a validation result.</returns>
        ValidationResult ValidateAdd(Person person);

        /// <summary>
        /// Validates the color parameter for filtering persons by color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Returns a validation result.</returns>
        ValidationResult ValidateGetByColor(string color);
    }
}
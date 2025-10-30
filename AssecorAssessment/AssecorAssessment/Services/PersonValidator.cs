using AssecorAssessment.Helpers;
using AssecorAssessment.Models;

namespace AssecorAssessment.Services
{
    /// <summary>  
    /// Validator that contains methods for validating manipulations on a person.  
    /// </summary>  
    /// <param name="person">The person to manipulate.</param>  
    /// <returns>Returns a validatiorn result container the sucess status and error messages.</returns>  

    // My thoughts on this:
    // - ValidateAdd checks multiple fields of the Person object to ensure they meet specific criteria before adding them to the data store.
    // - ValidateGetByColor focuses on validating the color parameter used for filtering persons by their favorite color.

    // What would i improve on this class for a strong architecture?
    // -> I would seperate private validation methods into a seperate class to follow the single responsibility principle.
    // Wich advantages would this bring?
    // -> This would make the code more modular, easier to test, and maintain.
    public class PersonValidator : IPersonValidator
    {
        public ValidationResult ValidateAdd(Person person)
        {
            var result = new ValidationResult();

            var validateColorResult = ValidateColor(person.Color);
            if (!validateColorResult.Successful)
                result.ErrorMessages.AddRange(validateColorResult.ErrorMessages);

            var validateZipCodeResult = ValidateZipCode(person.Zipcode);
            if (!validateZipCodeResult.Successful)
                result.ErrorMessages.AddRange(validateZipCodeResult.ErrorMessages);

            // This is just a example if the legnth of db field as capped at 50 chars
            var validateNameLengthResult = ValidateString(person.Name, 50);
            if (!validateNameLengthResult.Successful)
                result.ErrorMessages.AddRange(validateNameLengthResult.ErrorMessages);

            // This is just a example if the legnth of db field as capped at 50 chars
            var validateLastnameLengthResult = ValidateString(person.Lastname, 50);
            if (!validateLastnameLengthResult.Successful)
                result.ErrorMessages.AddRange(validateLastnameLengthResult.ErrorMessages);

            // This is just a example if the legnth of db field as capped at 50 chars
            var validateCityLengthResult = ValidateString(person.City, 50);
            if (!validateCityLengthResult.Successful)
                result.ErrorMessages.AddRange(validateCityLengthResult.ErrorMessages);

            if (result.ErrorMessages.Any())
                return result;

            result.Successful = true;
            return result;
        }

        public ValidationResult ValidateGetByColor(string color)
        {
            return ValidateColor(color);
        }

        private ValidationResult ValidateString(string value, int maxLength)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value))
            {
                result.ErrorMessages.Add("String can not be empty");
                return result;
            }

            if (value.Length > maxLength)
            {
                result.ErrorMessages.Add($"String exceeds maximum length of {maxLength}");
                return result;
            }

            result.Successful = true;
            return result;
        }

        private ValidationResult ValidateZipCode(string zipCode)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length != 5 || !zipCode.All(char.IsDigit))
            {
                result.ErrorMessages.Add("Invalid zip code");
                return result;
            }

            result.Successful = true;
            return result;
        }

        private ValidationResult ValidateColor(string color)
        {
            var result = new ValidationResult();

            if (!ColorMapper.IsValidColor(color))
            {
                result.ErrorMessages.Add("Invalid color");
                return result;
            }

            result.Successful = true;
            return result;
        }
    }
}
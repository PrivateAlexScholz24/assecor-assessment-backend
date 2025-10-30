using AssecorAssessment.Models;
using AssecorAssessment.Repositories;

namespace AssecorAssessment.Services
{
    /// <summary>  
    /// Service implementation for person-related business logic.  
    /// </summary>  
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IPersonValidator _personValidator;

        public PersonService(IPersonRepository repository, IPersonValidator personValidator)
        {
            _repository = repository;
            _personValidator = personValidator;
        }

        /// <inheritdoc />
        public List<Person> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        /// <inheritdoc />
        public Person? GetById(int id)
        {
            return _repository.GetById(id);
        }

        /// <inheritdoc />
        public ServiceResult<List<Person>> GetByColor(string color)
        {
            var validationResult = _personValidator.ValidateGetByColor(color);
            if (!validationResult.Successful)
                return ServiceResult<List<Person>>.Fail(validationResult.ErrorMessages);

            var persons = _repository.GetByColor(color).ToList();

            return ServiceResult<List<Person>>.Ok(persons);
        }

        /// <inheritdoc />
        public ServiceResult<Person> Add(Person person)
        {
            var validationResult = _personValidator.ValidateAdd(person);
            if (!validationResult.Successful && validationResult.ErrorMessages.Any())
                return ServiceResult<Person>.Fail(validationResult.ErrorMessages);

            var addedPerson = _repository.Add(person);
            return ServiceResult<Person>.Ok(addedPerson);
        }
    }
}
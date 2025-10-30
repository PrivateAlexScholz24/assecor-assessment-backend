using AssecorAssessment.Controllers;
using AssecorAssessment.Models;
using AssecorAssessment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AssecorAssessment.Tests
{
    [TestClass]
    public class PersonsControllerTest
    {
        private Mock<IPersonService> _personServiceMock = new();

        private List<Person> _persons = new();

        [TestInitialize]
        public void TestInitialize()
        {
            _persons = new List<Person>
           {
               new Person { Id = 1, Name = "Hans", Lastname = "Müller", Zipcode = "67742", City = "Lauterecken", Color = "grün" },
               new Person { Id = 2, Name = "Peter", Lastname = "Petersen", Zipcode = "18439", City = "Stralsund", Color = "blau" },
               new Person { Id = 3, Name = "Johnny", Lastname = "Johnson", Zipcode = "88888", City = "made up", Color = "rot" },
               new Person { Id = 4, Name = "Milly", Lastname = "Millenium", Zipcode = "77777", City = "made up too", Color = "gelb" },
               new Person { Id = 5, Name = "Jonas", Lastname = "Müller", Zipcode = "32323", City = "Hansstadt", Color = "orange" },
               new Person { Id = 6, Name = "Tastatur", Lastname = "Fujitsu", Zipcode = "42342", City = "Japan", Color = "violett" },
               new Person { Id = 7, Name = "Anders", Lastname = "Andersson", Zipcode = "32132", City = "Schweden - ☀", Color = "blau" },
               new Person { Id = 8, Name = "Bertram", Lastname = "Bart", Zipcode = "12313", City = "Wasweißich", Color = "grün" },
               new Person { Id = 9, Name = "Gerda", Lastname = "Gerber", Zipcode = "76535", City = "Woanders", Color = "rot" },
               new Person { Id = 10, Name = "Klaus", Lastname = "Klaussen", Zipcode = "43246", City = "Hierach", Color = "blau" }
           };
        }

        [TestMethod]
        public void ShouldReturnAllPersonsForGetAll()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetAll();
            var okResult = result.Result as OkObjectResult;
            var persons = okResult?.Value as IEnumerable<Person>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(persons);

            var resultCount = persons.Count();
            Assert.AreEqual(10, resultCount);
            for (int i = 0; i < resultCount; i++)
                AssertPerson(_persons[i], persons.ElementAt(i));
        }

        [TestMethod]
        public void ShouldReturnNotFoundForGetAll()
        {
            // Arrange
            var controller = GetController();
            _persons.Clear();

            // Act
            var result = controller.GetAll();
            var notFoundResult = result.Result as NotFoundObjectResult;
            var persons = notFoundResult?.Value as IEnumerable<Person>;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.IsNull(persons);

            dynamic? response = notFoundResult?.Value;
            Assert.IsTrue(((List<string>)response.errors).Contains("No persons found."));
        }

        [TestMethod]
        public void ShouldReturnPersonIfGetByIdWithExistingId()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetById(1);
            var okResult = result.Result as OkObjectResult;
            var person = okResult?.Value as Person;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(person);

            AssertPerson(_persons[0], person);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfGetByIdWithInvalidId()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetById(0);
            var badRequest = result.Result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            dynamic? response = badRequest?.Value;
            Assert.IsTrue(((List<string>)response.errors).Any(e => e.Contains("Invalid ID. The ID must be bigger than 0.")));
        }

        [TestMethod]
        public void ShouldReturnNotFoundIfGetByIdWithNonExistingId()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetById(99);
            var notFound = result.Result as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(notFound);
            dynamic? response = notFound?.Value;
            Assert.IsTrue(((List<string>)response.errors).First().Contains("Person with ID 99 not found."));
        }

        [TestMethod]
        public void ShouldReturnFilteredPersonsForGetByColor()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetByColor("blau");
            var okResult = result.Result as OkObjectResult;
            var persons = okResult?.Value as IEnumerable<Person>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(persons);
            Assert.AreEqual(3, new List<Person>(persons).Count);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfGetByColorIsInvalid()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.GetByColor("pink");
            var badRequest = result.Result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);
            dynamic? response = badRequest?.Value;
            Assert.IsFalse((bool)response.success);
            Assert.IsTrue(((List<string>)response.errors).Any());
        }

        [TestMethod]
        public void ShouldReturnCreatedIfCreateIsCalledWithValidPerson()
        {
            // Arrange
            var controller = GetController();

            var newPerson = new Person
            {
                Name = "Max",
                Lastname = "Mustermann",
                Zipcode = "11111",
                City = "Köln",
                Color = "green"
            };

            // Act
            var result = controller.Create(newPerson);
            var createdResult = result.Result as CreatedAtActionResult;
            var person = createdResult?.Value as Person;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(person);

            Assert.AreEqual(11, person.Id);
            Assert.AreEqual("Max", person.Name);
            Assert.AreEqual("Mustermann", person.Lastname);
            Assert.AreEqual("11111", person.Zipcode);
            Assert.AreEqual("Köln", person.City);
            Assert.AreEqual("green", person.Color);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfCreateIsCalledWithInvalidPerson()
        {
            // Arrange
            var controller = GetController();
            var invalidPerson = new Person
            {
                Name = "",
                Lastname = "NoName",
                Zipcode = "00000",
                City = "Nowhere",
                Color = "pink"
            };

            // Act
            var result = controller.Create(invalidPerson);
            var badRequest = result.Result as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(badRequest);

            dynamic? response = badRequest?.Value;
            Assert.IsFalse((bool)response.success);
            var errors = (List<string>)response.errors;
            Assert.IsTrue(errors.Any());
            Assert.IsTrue(errors.Any(e => e.Contains("String can not be empty")));
        }

        private void AssertPerson(Person expected, Person actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Lastname, actual.Lastname);
            Assert.AreEqual(expected.Zipcode, actual.Zipcode);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.Color, actual.Color);
        }

        private PersonsController GetController()
        {
            _personServiceMock = new Mock<IPersonService>();
            _personServiceMock
                .Setup(s => s.GetAll())
                .Returns(_persons);

            _personServiceMock
                .Setup(s => s.GetById(1))
                .Returns(_persons[0]);

            _personServiceMock
                .Setup(s => s.GetById(99))
                .Returns((Person?)null);

            _personServiceMock
                 .Setup(s => s.GetByColor(It.IsAny<string>()))
                 .Returns((string color) =>
                 {
                     if (color == "pink")
                         return ServiceResult<List<Person>>.Fail(new List<string> { "Invalid color" });

                     var filteredPersons = _persons.Where(p => p.Color == "blau").ToList();
                     return ServiceResult<List<Person>>.Ok(filteredPersons);
                 });

            _personServiceMock
                .Setup(s => s.Add(It.IsAny<Person>()))
                .Returns((Person p) => ServiceResult<Person>.Ok(new Person
                {
                    Id = 11,
                    Name = p.Name,
                    Lastname = p.Lastname,
                    Zipcode = p.Zipcode,
                    City = p.City,
                    Color = p.Color
                }));

            _personServiceMock
                .Setup(s => s.Add(It.Is<Person>(p => string.IsNullOrWhiteSpace(p.Name))))
                .Returns((Person p) => ServiceResult<Person>.Fail(new List<string> { "String can not be empty" }));

            return new PersonsController(_personServiceMock.Object);
        }
    }
}
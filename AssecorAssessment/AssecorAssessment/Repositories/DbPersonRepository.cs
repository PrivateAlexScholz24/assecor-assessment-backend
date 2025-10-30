using AssecorAssessment.Data;
using AssecorAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace AssecorAssessment.Repositories
{
    public class DbPersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public DbPersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Person> GetAll()
        {
            return _context.Persons.AsNoTracking().ToList();
        }

        public Person? GetById(int id)
        {
            return _context.Persons.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Person> GetByColor(string color)
        {
            return _context.Persons
                .AsNoTracking()
                .Where(p => p.Color.ToLower() == color.ToLower())
                .ToList();
        }

        public Person Add(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return person;
        }
    }
}
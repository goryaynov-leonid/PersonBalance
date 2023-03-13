using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PersonBalance.Web.Exceptions;
using PersonBalance.Web.Models;
using PersonBalance.Web.Models.Data;

namespace PersonBalance.Web.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _context;

        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateNewPerson(PersonDto dto)
        {
            var id = Guid.NewGuid();
            _context.Persons.Add(new Person
            {
                Id = id,
                Name = dto.Name,
                Surname = dto.Surname,
                Patronymic = dto.Patronymic,
                BirthDate = dto.BirthDate
            });

            await _context.SaveChangesAsync();
            return id;
        }

        private async Task<Person> GetPerson(Guid id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);

            if (person is null)
            {
                throw new KeyNotFoundException();
            }
            return person;
        }

        public async Task<decimal> CheckBalance(Guid id)
        {
            var person = await GetPerson(id);

            return person.Balance;
        }

        public async Task ChangeBalance(Guid id, decimal balance)
        {
            if (await CheckBalance(id) + balance < 0)
            {
                throw new InvalidOperationException();
            }

            var person = await GetPerson(id);

            person.Balance += balance;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new EntityHasChangedException("Person's balance has edited", e);
            }
        }

        public async Task<byte[]> GetPersonVersion(Guid id)
        {
            return (await GetPerson(id)).RowVersion;
        }
    }
}
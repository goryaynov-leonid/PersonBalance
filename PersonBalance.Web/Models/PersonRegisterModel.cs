using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonBalance.Web.Models
{
    public class PersonRegisterModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }

        public PersonDto ToDto()
        {
            return new PersonDto
            {
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                BirthDate = BirthDate
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate.Date >= DateTime.Now.Date)
            {
                yield return new ValidationResult("Birth date can't be in future", new[] {"BirthDate"});
            }
        }
    }
}
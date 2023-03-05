using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonBalance.Web.Models.Data
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Balance { get; set; }
    }
}
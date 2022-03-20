using Microsoft.EntityFrameworkCore;

namespace Advanced.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(DataContext _context)
        {
           _context.Database.Migrate();
            if(_context.People.Count() == 0 && _context.Departments.Count() == 0 && _context.Locations.Count() == 0)
            {
                Department d1 = new Department { Name = "Sales" };
                Department d2 = new Department { Name = "Development" };
                Department d3 = new Department { Name = "Support" };
                Department d4 = new Department { Name = "Facilities" };

                _context.AddRange(d1, d2, d3, d4);
                _context.SaveChanges();

                Location l1 = new Location { City = "Oakland", State = "CA" };
                Location l2 = new Location { City = "San Jose", State = "CA" };
                Location l3 = new Location { City = "New York", State = "NY" };

                _context.AddRange(l1 , l2, l3);
                _context.SaveChanges();

                _context.People.AddRange(
                    new Person
                    {
                        FirstName = "Francesa", LastName = "Jacobs",
                        Department = d2, Location = l1
                    },
                    new Person
                    {
                        FirstName = "Charles",
                        LastName = "Fuenres",
                        Department = d2,
                        Location = l3
                    },
                    new Person
                    {
                        FirstName = "Bright",
                        LastName = "Becker",
                        Department = d4,
                        Location = l1
                    },
                    new Person
                    {
                        FirstName = "Lara",
                        LastName = "Murphy",
                        Department = d1,
                        Location = l3
                    },
                    new Person
                    {
                        FirstName = "Beasley",
                        LastName = "Hoffman",
                        Department = d4,
                        Location = l3
                    },
                    new Person
                    {
                        FirstName = "Mark",
                        LastName = "Hays",
                        Department = d4,
                        Location = l1
                    },
                    new Person
                    {
                        FirstName = "Underwood",
                        LastName = "Trujillo",
                        Department = d2,
                        Location = l1
                    },
                    new Person
                    {
                        FirstName = "Randall",
                        LastName = "Lloyd",
                        Department = d3,
                        Location = l2
                    },
                    new Person
                    {
                        FirstName = "Guzman",
                        LastName = "Case",
                        Department = d2,
                        Location = l2
                    }
                );
                _context.SaveChanges();
            }
        }
    }
}

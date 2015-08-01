using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SauShelter.Models
{
    public class ShelterModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Born { get; set; }
        public int Telephone { get; set; }
        public string Email { get; set; }
    }
    public class OwnerDBContext : DbContext
    {
        public DbSet<ShelterModel> Owners { get; set; }
    }
}
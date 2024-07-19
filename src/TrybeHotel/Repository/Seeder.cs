using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Repository.Interfaces;
using TrybeHotel.Context;
using TrybeHotel.Context.Interfaces;

namespace TrybeHotel.Repository
{
    public class Seeder
    {
        public static void SeedUserAdmin(ITrybeHotelContext _context)
        {
            try
            {
                var usersCount = _context.Users.Where(u => u.UserType == "admin").Count();
                if (usersCount == 0)
                {
                    _context.Users.Add(new User { Name = "admin", Email = "admin@admin.com", Password = "admin", UserType = "admin" });
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.ToString());
            }
        }

        internal static void SeedUserAdmin(TrybeHotelContext trybeHotelContext)
        {
            try
            {
                var usersCount = trybeHotelContext.Users.Where(u => u.UserType == "admin").Count();
                if (usersCount == 0)
                {
                    trybeHotelContext.Users.Add(new User { Name = "admin", Email = "admin@admin.com", Password = "admin", UserType = "admin" });
                    trybeHotelContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.ToString());
            }
        }
    }
}
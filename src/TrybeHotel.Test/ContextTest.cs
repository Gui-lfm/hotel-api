using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;
using Microsoft.Extensions.DependencyInjection;
using TrybeHotel.Context.Interfaces;

namespace TrybeHotel.Test;

public class ContextTest : DbContext, ITrybeHotelContext
{
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Hotel> Hotels { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public ContextTest(DbContextOptions<ContextTest> options) : base(options)
    { }

}
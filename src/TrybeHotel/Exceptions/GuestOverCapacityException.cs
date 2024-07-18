namespace TrybeHotel.Exceptions;

public class GuestOverCapacityException : Exception
{
  
  public GuestOverCapacityException(string message) : base(message) { }
}
namespace TrybeHotel.Exceptions;

public class EmailAlreadyRegisteredException : Exception
{
  
  public EmailAlreadyRegisteredException(string message) : base(message) { }
}
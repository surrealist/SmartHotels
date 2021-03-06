namespace GreatFriends.SmartHoltel.Services.Exceptions
{
  public class ReservationException : AppException
  {
    public ReservationException(string customerName, int roomId, string reason)
      : this("guest", customerName, roomId, reason)
    {
      // 
    }
    public ReservationException(string userName, string customerName, int roomId, string reason)
    {
      CustomerName = customerName;
      RoomId = roomId;
      Reason = reason;
    }

    public string CustomerName { get; }
    public int RoomId { get; }
    public string Reason { get; }

    public override string Message =>
      $"Reservation Failed: {CustomerName} cannot reserve room {RoomId} due to {Reason}";
  }
}

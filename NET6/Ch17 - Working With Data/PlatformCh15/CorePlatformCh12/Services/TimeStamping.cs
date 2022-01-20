namespace DICh14.Services
{
    public interface ITimeStapmer
    {
        string TimeStamp { get; }
    }

    public class DefaultTimeStamper : ITimeStapmer
    {
        public string TimeStamp => DateTime.Now.ToShortTimeString();
    }
}

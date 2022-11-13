namespace Taak.Libraries.Constants
{
    public class Constants
    {
        public static Dictionary<string, string> TimeFrames = new Dictionary<string, string>()
        {
            {"Morning","before  10am" },
            {"Midday","10am - 2pm" },
            {"Afternoon","2pm - 6pm" },
            {"Evening","after 6pm" }
        };

        public static Dictionary<string, string> TimeFramesIcons = new Dictionary<string, string>()
        {
            {"Morning","<i class=\"bi bi-sunrise\"></i>" },
            {"Midday","<i class=\"bi bi-sun\"></i>" },
            {"Afternoon","<i class=\"bi bi-sunset-fill\"></i>" },
            {"Evening","<i class=\"bi bi-cloud-moon-fill\"></i>" }
        };
    }
}

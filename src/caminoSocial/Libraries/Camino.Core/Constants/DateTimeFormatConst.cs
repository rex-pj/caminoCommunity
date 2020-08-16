namespace Camino.Core.Constants
{
    public class DateTimeFormatConst
    {
        public const string DateHourMinusFormat = "dd/MM/yyyy hh:mm";
        public const string DateFormat = "dd/MM/yyyy";

        public static readonly string[] ParseableFormats = new string[] 
        {
            "dd/MM/yyyy HH:mm", 
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy hh:mm tt",
            "dd/MM/yyyy hh:mm:ss tt",
            "MM/dd/yyyy HH:mm",
            "MM/dd/yyyy HH:mm:ss",
            "MM/dd/yyyy hh:mm tt",
            "MM/dd/yyyy hh:mm:ss tt"
        };
    }
}

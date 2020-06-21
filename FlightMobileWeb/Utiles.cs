namespace FlightMobileWeb
{
    public class Utiles
    {
        public static bool Between(double start, double end, double current)
        {
            bool retval = false;
            if (current >= start && current <= end)
            {
                retval = true;
            }
            return retval;
        }
    }
}

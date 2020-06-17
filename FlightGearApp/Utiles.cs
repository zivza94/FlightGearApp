namespace FlightGearApp
{
    public class Utiles
    {
        public static bool Between(double start, double end, double current)
        {
            bool retval = false;
            if (current <= end && current >= start)
            {
                retval = true;
            }
            return retval;
        }
    }
}

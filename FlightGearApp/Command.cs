using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightGearApp
{
    public class Command
    {
        private readonly Dictionary<string, KeyValuePair<double, double>> _valuesBoundry = new Dictionary<string, KeyValuePair<double, double>>();
        public Command()
        {
            Rudder = -10;
            Elevator = -10;
            Aileron = -10;
            Throttle = -10;
            _valuesBoundry.Add("Rudder", new KeyValuePair<double, double>(-1, 1));
            _valuesBoundry.Add("Elevator", new KeyValuePair<double, double>(-1, 1));
            _valuesBoundry.Add("Aileron", new KeyValuePair<double, double>(-1, 1));
            _valuesBoundry.Add("Throttle", new KeyValuePair<double, double>(0, 1));

        }
        [JsonPropertyName("rudder")]
        public double Rudder { get; set; }

        [JsonPropertyName("elevator")]
        public double Elevator { get; set; }
        
        [JsonPropertyName("aileron")]
        public double Aileron { get; set; }

        [JsonPropertyName("throttle")]
        public double Throttle { get; set; }

        public string Valid()
        {
            KeyValuePair<double, double> bound = _valuesBoundry["Rudder"];
            if (!Utiles.Between(bound.Key, bound.Value, Rudder))
            {
                return "Invalid Rudder, actual: "+Rudder+" expected: "+ bound.Key+" to "+bound.Value;
            }
            bound = _valuesBoundry["Elevator"];
            if (!Utiles.Between(bound.Key, bound.Value, Elevator))
            {
                return "Invalid Elevator, actual: " + Elevator + " expected: " + bound.Key + " to " + bound.Value;
            }
            bound = _valuesBoundry["Aileron"];
            if (!Utiles.Between(bound.Key, bound.Value, Aileron))
            {
                return "Invalid Aileron, actual: " + Aileron + " expected: " + bound.Key + " to " + bound.Value;
            }
            bound = _valuesBoundry["Throttle"];
            if (!Utiles.Between(bound.Key, bound.Value, Throttle))
            {
                return "Invalid Throttle, actual: " + Throttle + " expected: " + bound.Key + " to " + bound.Value;
            }
            return "OK";
        }
    }
}

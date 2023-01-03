using PMT_Prototype.Client.DataModels;
using System.ComponentModel.DataAnnotations;

namespace PMT_Prototype.Client.Models
{

    public class WeatherAlertModel
    {
        [Required]
        public DateTimeOffset EffectiveTime { get; set; }

        [Required]
        public DateTimeOffset Expires { get; set; }

        [Required]
        public string? Severity { get; set; }

        [Required]
        public string? Event { get; set; }

        [Required]
        public string? Sender { get; set; }

        [Required]
        public string? AdditionalInfo { get; set; }

        public bool EventIsHeat => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Heat", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Hot", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsFire => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Fire", StringComparison.InvariantCultureIgnoreCase);
        public bool EventIsThunderstorm => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Storm", StringComparison.InvariantCultureIgnoreCase);
        public bool EventIsTornado => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Torna", StringComparison.InvariantCultureIgnoreCase);
        public bool EventIsFlood => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Flood", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Hydrologic", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsSnow => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Snow", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Ice", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Freez", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Winter", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsWind => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Wind", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Gale", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsRipCurrent => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Current", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Craft", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Surf", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Sea", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsDenseFog => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Fog", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Air", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsHurricane => !string.IsNullOrWhiteSpace(Event) && (Event.Contains("Hurricane", StringComparison.InvariantCultureIgnoreCase) || Event.Contains("Tropical", StringComparison.InvariantCultureIgnoreCase));
        public bool EventIsEarthquake => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Earth", StringComparison.InvariantCultureIgnoreCase);
        public bool EventIsSpecial => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Special", StringComparison.InvariantCultureIgnoreCase);
        public bool EventIsAvalanche => !string.IsNullOrWhiteSpace(Event) && Event.Contains("Avalanche", StringComparison.InvariantCultureIgnoreCase);

        public string IconClass => EventIsAvalanche ? "fa-solid fa-hill-avalanche" :
                                    EventIsEarthquake ? "fa-solid fa-house-chimney-crack" :
                                    EventIsHurricane ? "fa-solid fa-hurricane" : 
                                    EventIsDenseFog ? "fa-solid fa-smog" :
                                    EventIsRipCurrent ? "fa-solid fa-ship" :
                                    EventIsWind ? "fa-solid fa-wind" :
                                    EventIsSnow ? "fa-solid fa-snowflake" :
                                    EventIsFlood ? "fa-solid fa-cloud-showers-water" :
                                    EventIsTornado ? "fa-solid fa-tornado" :
                                    EventIsThunderstorm ? "fa-solid fa-cloud-bolt" : 
                                    EventIsFire ? "fa-solid fa-fire" :
                                    EventIsHeat ? "fa-solid fa-temperature-arrow-up" : "fa-solid fa-meteor";

        public void MapFromDataModel(Properties dm)
        {
            EffectiveTime = dm?.Effective ?? DateTimeOffset.MinValue;
            Expires = dm?.Expires ?? DateTimeOffset.MinValue;
            Severity = dm?.Severity ?? string.Empty;
            Event = dm?.Event ?? string.Empty;
            Sender = dm?.SenderName?.Replace("NWS", "").Trim() ?? string.Empty;
            AdditionalInfo = $@"{dm?.Headline ?? string.Empty}
                                        
                                {dm?.Description ?? string.Empty}";
        }

    }

    

}

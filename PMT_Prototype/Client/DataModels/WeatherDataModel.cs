namespace PMT_Prototype.Client.DataModels
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using R = Newtonsoft.Json.Required;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class WeatherDataModel
    {
        [J("@context")] public ContextElement[] Context { get; set; }
        [J("type")] public string Type { get; set; }
        [J("features")] public Feature[] Features { get; set; }
        [J("title")] public string Title { get; set; }
        [J("updated")] public DateTimeOffset Updated { get; set; }
    }

    public partial class ContextClass
    {
        [J("@version")] public string Version { get; set; }
        [J("wx")] public Uri Wx { get; set; }
        [J("@vocab")] public Uri Vocab { get; set; }
    }

    public partial class Feature
    {
        [J("id")] public Uri Id { get; set; }
        [J("type")] public string Type { get; set; }
        [J("geometry")] public Geometry Geometry { get; set; }
        [J("properties")] public Properties Properties { get; set; }
    }

    public partial class Geometry
    {
        [J("type")] public string Type { get; set; }
        [J("coordinates")] public double[][][] Coordinates { get; set; }
    }

    public partial class Properties
    {
        [J("@id")] public Uri Id { get; set; }
        [J("@type")] public string Type { get; set; }
        [J("id")] public string PropertiesId { get; set; }
        [J("areaDesc")] public string AreaDesc { get; set; }
        [J("geocode")] public Geocode Geocode { get; set; }
        [J("affectedZones")] public Uri[] AffectedZones { get; set; }
        [J("references")] public Reference[] References { get; set; }
        [J("sent")] public DateTimeOffset Sent { get; set; }
        [J("effective")] public DateTimeOffset Effective { get; set; }
        [J("onset")] public DateTimeOffset Onset { get; set; }
        [J("expires")] public DateTimeOffset Expires { get; set; }
        [J("ends")] public DateTimeOffset? Ends { get; set; }
        [J("status")] public string Status { get; set; }
        [J("messageType")] public string MessageType { get; set; }
        [J("category")] public string Category { get; set; }
        [J("severity")] public string Severity { get; set; }
        [J("certainty")] public string Certainty { get; set; }
        [J("urgency")] public string Urgency { get; set; }
        [J("event")] public string Event { get; set; }
        [J("sender")] public string Sender { get; set; }
        [J("senderName")] public string SenderName { get; set; }
        [J("headline")] public string Headline { get; set; }
        [J("description")] public string Description { get; set; }
        [J("instruction")] public string Instruction { get; set; }
        [J("response")] public string Response { get; set; }
        [J("parameters")] public Parameters Parameters { get; set; }
    }

    public partial class Geocode
    {
        [J("SAME")] public string[] Same { get; set; }
        [J("UGC")] public string[] Ugc { get; set; }
    }

    public partial class Parameters
    {
        [J("AWIPSidentifier")] public string[] AwipSidentifier { get; set; }
        [J("WMOidentifier")] public string[] WmOidentifier { get; set; }
        [J("NWSheadline", NullValueHandling = N.Ignore)] public string[] NwSheadline { get; set; }
        [J("BLOCKCHANNEL")] public string[] Blockchannel { get; set; }
        [J("VTEC", NullValueHandling = N.Ignore)] public string[] Vtec { get; set; }
        [J("eventEndingTime", NullValueHandling = N.Ignore)] public DateTimeOffset[] EventEndingTime { get; set; }
        [J("expiredReferences", NullValueHandling = N.Ignore)] public string[] ExpiredReferences { get; set; }
        [J("EAS-ORG", NullValueHandling = N.Ignore)] public string[] EasOrg { get; set; }
    }

    public partial class Reference
    {
        [J("@id")] public Uri Id { get; set; }
        [J("identifier")] public string Identifier { get; set; }
        [J("sender")] public string Sender { get; set; }
        [J("sent")] public DateTimeOffset Sent { get; set; }
    }

    public partial struct ContextElement
    {
        public ContextClass ContextClass;
        public Uri PurpleUri;

        public static implicit operator ContextElement(ContextClass ContextClass) => new ContextElement { ContextClass = ContextClass };
        public static implicit operator ContextElement(Uri PurpleUri) => new ContextElement { PurpleUri = PurpleUri };
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ContextElementConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ContextElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContextElement) || t == typeof(ContextElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    try
                    {
                        var uri = new Uri(stringValue);
                        return new ContextElement { PurpleUri = uri };
                    }
                    catch (UriFormatException) { }
                    break;
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<ContextClass>(reader);
                    return new ContextElement { ContextClass = objectValue };
            }
            throw new Exception("Cannot unmarshal type ContextElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (ContextElement)untypedValue;
            if (value.PurpleUri != null)
            {
                serializer.Serialize(writer, value.PurpleUri.ToString());
                return;
            }
            if (value.ContextClass != null)
            {
                serializer.Serialize(writer, value.ContextClass);
                return;
            }
            throw new Exception("Cannot marshal type ContextElement");
        }

        public static readonly ContextElementConverter Singleton = new ContextElementConverter();
    }
}

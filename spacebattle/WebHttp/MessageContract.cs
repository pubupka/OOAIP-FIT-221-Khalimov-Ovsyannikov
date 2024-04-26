using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace WebHttp
{
    [DataContract(Name = "ExampleContract", Namespace = "http://example.com")]
    [ExcludeFromCodeCoverage]
    public class MessageContract
    {
        [DataMember(Name = "Type of command", Order = 1)]
        [OpenApiProperty(Description = "Type of command")]
        public string TypeOfCommand { get; set; }

        [DataMember(Name = "Game Id", Order = 2)]
        [OpenApiProperty(Description = "Game Id")]
        public int GameId { get; set; }

        [DataMember(Name = "Item id", Order = 3)]
        [OpenApiProperty(Description = "Item id")]
        public int ItemId { get; set; }

        [DataMember(Name = "Properties", Order = 4)]
        [OpenApiProperty(Description = "Properties")]
        public IDictionary<string, object> Properties { get; set; }
    }
}

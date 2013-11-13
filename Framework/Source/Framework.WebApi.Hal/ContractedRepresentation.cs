using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Framework.WebApi.Hal
{
    [DataContract]
    public class ContractedRepresentation : Representation
    {
        [DataMember]
        public override List<Link> Links
        {
            get
            {
                return base.Links;
            }
        }
    }
}

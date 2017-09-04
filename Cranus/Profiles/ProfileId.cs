using Elders.Cronus.DomainModeling;
using System.Runtime.Serialization;
using System;

namespace Cranus.Profiles
{
    [DataContract(Name = "eb341625-0c39-4537-8811-acae689890f2")]
    public class ProfileId : GuidId
    {
        protected ProfileId() { }

        public ProfileId(Guid idBase, string aggregateRootName) : base(idBase, aggregateRootName) { }

        public ProfileId(GuidId idBase, string aggregateRootName) : base(idBase, aggregateRootName) { }
    }
}

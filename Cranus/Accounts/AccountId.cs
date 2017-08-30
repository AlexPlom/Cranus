﻿using Cranus.Accounts.Commands;
using Elders.Cronus.DomainModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Cranus.Accounts
{
    [DataContract(Name = "57aa5ddf-1a13-4f68-a553-400aeb6c04eb")]
    public class AccountId : StringTenantId
    {
        protected AccountId() { }
        public AccountId(string id, string tenant) : base(id, "account", tenant) { }
        public AccountId(IUrn urn) : base(urn, "account") { }
    }
}

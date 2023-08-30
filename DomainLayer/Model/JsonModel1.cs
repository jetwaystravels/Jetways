using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class JsonModel1
    {
        public List<_value> value { get; set; }
        public _count count { get; set; }


    }
        public class _value
        { 
         public string id { get; set; }
         public string authorizationSource { get; set; }
         public string[] managedByTenants { get; set; }
            public _tags tags { get; set; }
            public _subscriptionPolicies subscriptionPolicies { get; set; }
            public string subscriptionId { get; set; }
            public string tenantId { get; set; }

            public string displayName { get; set; }
            public string state { get; set; }
        }
        public class managedByTenants
        {
           
        }

        public class _tags
        {
            public string Department { get; set; }  
            public string Account { get; set; } 
            public string Division { get; set; }
            public string CostCenter { get; set; }
            public string test { get; set; }
            public string  Dept { get; set; }
        }

        public class _subscriptionPolicies
        {
            public string locationPlacementId { get; set; }
            public string quotaId { get; set; }
            public string spendingLimit { get; set; }

        }
        public class _count
        {
            public string type { get; set; }
            public string value { get; set; }

        }
    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class JsonModel
    {
        public  _class1 class1 { get; set; }
        public  _class2 class2 { get; set; }

    }

    public class _class1
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public _awesomeobject awesomeobject { get; set; }
        public List<_user>user { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}



    }

    public class _awesomeobject
    {
        public int SomeProps1 { get; set; }
        public string SomeProps2 { get; set; }
    }

    public class _user
    {
        public int id { get; set; }
        public string name { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string email { get; set; }
    }

    public class _class2
    {

        public string SomePropertyOfClass2 { get; set; }
    }
        


}

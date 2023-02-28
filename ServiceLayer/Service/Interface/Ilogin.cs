using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;

namespace ServiceLayer.Service.Interface
{
    public interface Ilogin
    {
        List<Login> GetAllLoginRepo();
    }
}

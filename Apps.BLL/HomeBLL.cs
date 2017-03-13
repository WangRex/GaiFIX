using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Models.Sys;
using Apps.BLL.Core;
using Apps.DAL;
namespace Apps.BLL
{

    public class HomeBLL
    {
        public HomeRepository _HomeRepository;
        public HomeBLL()
        {
            _HomeRepository = new HomeRepository();
        }
    }
}

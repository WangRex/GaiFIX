using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.BLL.Core;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using Apps.DAL;
using Apps.Models.Sys;
namespace Apps.BLL
{
    public class AccountBLL
    {
        public AccountRepository accountRepository;
        public AccountBLL()
        {
            accountRepository = new AccountRepository();
        }
        public SysUser Login(string username, string pwd)
        {
            return accountRepository.Find(a => a.UserName == username && a.Password == pwd);
        }
    }
}

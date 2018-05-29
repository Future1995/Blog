using HwaTec.Blog.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.Service
{
    public interface IUserInfoService
    {
        UserInfo Login(string userCode, string password);
    }
}

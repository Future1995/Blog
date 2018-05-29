using HwaTec.Blog.Model;
using HwaTec.Blog.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HwaTec.Blog.Service
{
    public class UserInfoService : IUserInfoService
    {
        private IRepository<UserInfo> _userInfoRepository;
        public UserInfoService(IRepository<UserInfo> userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        public UserInfo Login(string userCode, string password)
        {
            return _userInfoRepository.LoadEntities(u => u.UserCode == userCode && u.Password == password).FirstOrDefault();
        }
    }
}

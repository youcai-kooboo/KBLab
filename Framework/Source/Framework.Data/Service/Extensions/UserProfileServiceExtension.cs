using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Framework.Data.Model;

namespace Framework.Data.Service
{
    public partial interface IUserProfileService
    {
        UserProfile GetUserByUserName(string userName);
        void CreateUserProfile(UserProfile user);
        IEnumerable<UserProfile> GetAllUsers();
        IEnumerable<ValidationResult> CanAddUser(UserProfile user);
        void CommitChanges();
    }

    public partial class UserProfileService : IUserProfileService
    {
        public UserProfile GetUserByUserName(string userName)
        {
            return _userProfileRepository.Get(a => a.UserName.ToLower() == userName.ToLower());
        }

        public void CreateUserProfile(UserProfile user)
        {
            try
            {
                _userProfileRepository.Add(user);
                CommitChanges();
            }
            catch (Exception ex)
            {
                //ExceptionHandler.LogException(ex);
            }
        }


        public IEnumerable<ValidationResult> CanAddUser(UserProfile user)
        {
            throw new NotImplementedException();
        }

        public void CommitChanges()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<UserProfile> GetAllUsers()
        {
            return _userProfileRepository.GetAll();
        }
    }
}

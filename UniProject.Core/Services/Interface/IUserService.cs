using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.DTOs;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.UserPanel;
using UniProject.DataLayer.Entity;

namespace UniProject.Core.Services.Interface
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activeCode);
        User GetUserByUsername(string username);
        User GetUserById(int userId);
        int GetUserIdByUsername(string username);
        string GetUsernameByUserId(int userId);
        void UpdateUser(User user);


        #region UserPanel
        InformationUserViewModel GetUserInfoByUsername(string username);
        SideBarUserProfileViewModel GetUserProfileViewModel(string username);
        EditUserProfileViewModel GetUserDataByUsername(string username);
        void EditProfile(string username, EditUserProfileViewModel editUser);
        bool CheckOldPassword(string username, string oldPassword);
        void ChangePassword(string username, ChangePasswordViewModel changePassword);

        #endregion

        #region Wallet
        int RemainderWallet(string username);
        List<WalletViewModel> GetAllTransaction(string username);
        int ChargeWallet(string username, int amount, string description, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region AdminPanel
        ShowUsersForAdminViewModel GetUsers(int pageId, string username = "", string email = "");
        ShowUsersForAdminViewModel GetDeletedUsers(int pageId, string username = "", string email = "");
        int AddUserFromAdmin(CreateUserViewModel createUser);
        EditUsersByAdminViewModel ShowUserForEditMode(int userId);
        void EditUsersByAdmin(EditUsersByAdminViewModel editUsers);
        DeleteUserByAdminViewModel GetUserInfoById(int userId);
        void DeleteUserById(int userId);
        void AddAdress(string userName, AddressViewModel address);
        EditAddressViewModel GetAddressByAddressId(int addressId);
        void UpdateAddress(EditAddressViewModel address);
        List<Address> GetAllAdresses(string userName);
        #endregion
    }
}

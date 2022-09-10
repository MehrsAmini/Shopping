using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.Convertor;
using UniProject.Core.DTOs;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.UserPanel;
using UniProject.Core.Security;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Context;
using UniProject.DataLayer.Entity;

namespace UniProject.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UniProjectContext _context;
        public UserService(UniProjectContext context)
        {
            _context = context;
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = Generator.NameGenerator.GenerateUniqCode();

            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Add(user);
            user.IsDelete = false;
            _context.SaveChanges();
            return user.UserId;
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User LoginUser(LoginViewModel login)
        {
            var hashPass = EncodePassword.EncodePasswordMd5(login.Password);
            var email = Convertor.FixedText.FixEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPass);
        }

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public InformationUserViewModel GetUserInfoByUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.ReminderWallet = user.RemainderWallet;
            return information;
        }

        public SideBarUserProfileViewModel GetUserProfileViewModel(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new SideBarUserProfileViewModel
            {
                UserName = u.UserName,
                RegisterDate = u.RegisterDate,
                Email = u.Email
            }).Single();
        }

        public EditUserProfileViewModel GetUserDataByUsername(string username)
        {
            //return _context.Users.Where(u => u.UserName == username).Select(u => new EditUserProfileViewModel()
            //{
            //    Email = u.Email,
            //    UserName = u.UserName,
            //    OldPassword=u.Password
            //}).Single();
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            var editUser = new EditUserProfileViewModel()
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return editUser;
        }

        public void EditProfile(string username, EditUserProfileViewModel editUser)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            user.UserName = editUser.UserName;
            user.Email = FixedText.FixEmail(editUser.Email);

            UpdateUser(user);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }

        public bool CheckOldPassword(string username, string oldPassword)
        {
            var pass = EncodePassword.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == username && u.Password == pass);
        }

        public void ChangePassword(string username, ChangePasswordViewModel changePassword)
        {
            var user = GetUserByUsername(username);
            user.Password = EncodePassword.EncodePasswordMd5(changePassword.NewPassword);

            UpdateUser(user);
        }

        public int RemainderWallet(string username)
        {
            User user = GetUserByUsername(username);

            var increase = _context.Wallets
                .Where(w => w.UserId == user.UserId && w.TypeId == 1 && w.IsPay)
                .Select(w => w.Amount).ToList();

            var decrease = _context.Wallets
                .Where(w => w.UserId == user.UserId && w.TypeId == 2)
                .Select(w => w.Amount).ToList();

            var remainder = increase.Sum() - decrease.Sum();
            user.RemainderWallet = remainder;
            UpdateUser(user);

            return (remainder);
        }

        public int GetUserIdByUsername(string username)
        {
            return _context.Users.Single(u => u.UserName == username).UserId;
        }

        public List<WalletViewModel> GetAllTransaction(string username)
        {
            int userId = GetUserIdByUsername(username);

            return _context.Wallets
                .Where(w => w.UserId == userId && w.IsPay)
                .Select(w => new WalletViewModel()
                {
                    Amount = w.Amount,
                    CreateTime = w.CreateDate,
                    Description = w.Description,
                    Type = w.TypeId
                }).ToList();
        }

        public int ChargeWallet(string username, int amount, string description, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                TypeId = 1,
                UserId = GetUserIdByUsername(username),
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = isPay
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            RemainderWallet(GetUsernameByUserId(wallet.UserId));
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.SingleOrDefault(w => w.WalletId == walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Entry(wallet).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public ShowUsersForAdminViewModel GetUsers(int pageId, string username = "", string email = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(username))
                result = result.Where(u => u.UserName.Contains(username));

            if (!string.IsNullOrEmpty(email))
                result = result.Where(u => u.Email.Contains(email));

            // Show Item In Page
            int take = 10;
            int skip = (pageId - 1) * take;


            ShowUsersForAdminViewModel list = new ShowUsersForAdminViewModel();

            //PageCount
            int pageCount = result.Count() % take;
            list.PageCount = result.Count() / take;
            if (pageCount > 1)
                list.PageCount++;

            list.CurrentPage = pageId;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel createUser)
        {
            User user = new User()
            {
                UserName = createUser.UserName,
                Email = FixedText.FixEmail(createUser.Email),
                Password = EncodePassword.EncodePasswordMd5(createUser.Password),
                ActiveCode = Generator.NameGenerator.GenerateUniqCode(),
                Avatar = "man.png",
                IsActive = true,
                RegisterDate = DateTime.Now,
                RemainderWallet=0
            };
            return AddUser(user);
        }

        public EditUsersByAdminViewModel ShowUserForEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUsersByAdminViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                }).Single();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId);
        }

        public void EditUsersByAdmin(EditUsersByAdminViewModel editUsers)
        {
            var user = GetUserById(editUsers.UserId);
            user.Email = FixedText.FixEmail(editUsers.Email);
            if (!string.IsNullOrEmpty(editUsers.Password))
                user.Password = EncodePassword.EncodePasswordMd5(editUsers.Password);
            user.IsActive = editUsers.IsActive;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUserById(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            user.IsActive = false;
            UpdateUser(user);
        }

        public ShowUsersForAdminViewModel GetDeletedUsers(int pageId, string username = "", string email = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters()
                .Where(u=> u.IsDelete==true);

            if (!string.IsNullOrEmpty(username))
                result = result.Where(u => u.UserName.Contains(username));

            if (!string.IsNullOrEmpty(email))
                result = result.Where(u => u.Email.Contains(email));

            // Show Item In Page
            int take = 10;
            int skip = (pageId - 1) * take;


            ShowUsersForAdminViewModel list = new ShowUsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() % take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public DeleteUserByAdminViewModel GetUserInfoById(int userId)
        {
            DeleteUserByAdminViewModel user = _context.Users.Where(u => u.UserId == userId)
                .Select(u => new DeleteUserByAdminViewModel()
                {
                    UserId=u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    RegisterDate=u.RegisterDate,
                    IsActive = u.IsActive,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                }).Single();

            string username = GetUsernameByUserId(userId);
            user.Wallet = RemainderWallet(username);

            return (user);
        }

        public string GetUsernameByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == userId).UserName;
        }

        public void AddAdress(string userName, AddressViewModel address)
        {
            int userId = GetUserIdByUsername(userName);
            Address address1 = new Address()
            {
                UserId = userId,
                FullName = address.FullName,
                MobileNumber = address.MobileNumber,
                City = address.City,
                Province = address.Province,
                PostCode = address.PostCode,
                AddressDetails = address.AddressDetails
            };
            _context.Addresses.Add(address1);
            _context.SaveChanges();
        }

        public List<Address> GetAllAdresses(string userName)
        {
            int userId = GetUserIdByUsername(userName);
            return _context.Addresses.Where(a => a.UserId == userId).ToList();
        }

        public EditAddressViewModel GetAddressByAddressId(int addressId)
        {
            return _context.Addresses.Where(a => a.AddressID == addressId)
                .Select(a => new EditAddressViewModel
                {
                    AddressId = a.AddressID,
                    FullName = a.FullName,
                    MobileNumber = a.MobileNumber,
                    City = a.City,
                    Province = a.Province,
                    PostCode = a.PostCode,
                    AddressDetails = a.AddressDetails
                }).Single();
        }

        public void UpdateAddress(EditAddressViewModel address)
        {
            Address address1 = _context.Addresses.SingleOrDefault(a => a.AddressID == address.AddressId);
            address1.FullName = address.FullName;
            address1.MobileNumber = address.MobileNumber;
            address1.PostCode = address.PostCode;
            address1.Province = address.Province;
            address1.City = address1.City;
            address1.AddressDetails = address.AddressDetails;

            _context.Addresses.Update(address1);
            _context.SaveChanges();
        }
    }
}

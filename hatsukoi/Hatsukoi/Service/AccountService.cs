using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository;
using Hatsukoi.Service.Interface;
using static Hatsukoi.Common.HatsukoiEnum;
using System.Security.Claims;
using Hatsukoi.Models.ViewModel;
using System.Text;
using Hatsukoi.Common;
using Hatsukoi.Models;
using Hatsukoi.Models.Dtos;
using Google.Apis.Auth;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Repository.DBRepository;

namespace Hatsukoi.Service
{
    public class AccountService : IAccountService
    {
        private readonly IRepository efRepository;
        private readonly IHttpContextAccessor _iHttps;
        public AccountService(IRepository loginRepository, IHttpContextAccessor iHttps)
        {
            efRepository = loginRepository;
            _iHttps = iHttps;
        }
        public UserSimpleModel GetUserLoginModel(LoginViewModel loginModel)
        {
            var inputPassword = HasherExtension.ToSHA256(loginModel.Password);
            //var id = efRepository.GetAll<User>().FirstOrDefault(x => x.Account == loginModel.Account && x.Password == inputPassword);
            var user = efRepository.FirstOrDefault<User>(x => x.Account == loginModel.Account && x.Password == inputPassword);
            UserSimpleModel output;
            if (user != null)
            {
                output = new UserSimpleModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    StoreStatus = (Common.HatsukoiEnum.StoreStatus)user.StoreStatus,
                    Image = user.Photo,
                    MemberLevel = (Common.HatsukoiEnum.MemberLevel)user.MemberLevel,
                    IsEmailCertified = user.IsEmailCertified
                };
                return output;
            }
            return new UserSimpleModel
            {
                Id = 0,
                Name = ""
            };
        }
        public UserSimpleModel GetUser()
        {
            if (_iHttps.HttpContext == null || _iHttps.HttpContext.User.Identity == null)
            {
                return null;
            }
            else if (_iHttps.HttpContext.User.Identity.IsAuthenticated == false)
            {
                return null;
            }

            int userId = 0;
            string userEmail = "";
            string userName = "";
            string userIsSeller = "0";
            string userImage = "";
            string userMemberLevel = "0";

            var idClaim = _iHttps.HttpContext.User.FindFirst("Id");
            if (idClaim != null)
            {
                userId = int.Parse(idClaim.Value);
                // 使用用户的ID进行一些操作
            }
            var emailClaim = _iHttps.HttpContext.User.FindFirst(ClaimTypes.Email);
            if (emailClaim != null)
            {
                userEmail = emailClaim.Value;
                // 使用用户的电子邮件地址进行一些操作
            }
            var NameClaim = _iHttps.HttpContext.User.FindFirst(ClaimTypes.Name);
            if (NameClaim != null)
            {
                userName = NameClaim.Value;
                // 使用用户的姓名进行一些操作
            }
            var IsSellerClaim = _iHttps.HttpContext.User.FindFirst(ClaimTypes.Role);
            if (IsSellerClaim != null)
            {
                userIsSeller = IsSellerClaim.Value;
                // 使用用户的是否為賣家进行一些操作
            }
            var ImageClaim = _iHttps.HttpContext.User.FindFirst("Image");
            if (ImageClaim != null)
            {
                userImage = ImageClaim.Value;
                // 使用用户的Image进行一些操作
            }
            var MemberLevelClaim = _iHttps.HttpContext.User.FindFirst("MemberLevel");
            if (MemberLevelClaim != null)
            {
                userMemberLevel = MemberLevelClaim.Value;
                // 使用用户的會員等級进行一些操作
            }

            return new UserSimpleModel
            {
                Id = userId,
                Name = userName,
                Email = userEmail,
                StoreStatus = (StoreStatus)(int.Parse(userIsSeller)),
                MemberLevel = (MemberLevel)(int.Parse(userMemberLevel)),
                Image = userImage
            };
        }
        public void CreateUser(CreateUserViewModel user)
        {
            var storePassword = HasherExtension.ToSHA256(user.Password);
            var birth = new DateTime(user.Year, user.Month, user.Day);
            var target = new User
            {
                Name = user.Account,
                Email = user.Email,
                Account = user.Account,
                Password = storePassword,
                //因為我們資料庫有許多其他的不可為Null的欄位，因此在這邊直接給定那些欄位預設值
                CreateDate = DateTime.UtcNow,
                Gender = user.Gender,
                BirthDate = birth,
                Photo = "https://ppt.cc/filyKx@.jpg",
                IsEmailCertified = false,
                MemberLevel = 0,
                StoreStatus = 0,
                ModifiDate = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow,
                IsEmailOrder = false,
                IsEmailHatsukoi = false,
                IsEmailActivity = false,
                IsEmailPersonal = false,
                IsEmailWeek = false,
                IsEmailMember = false,
                IsEmailFollow = false,
                IsEmailFocus = false
            };
            efRepository.Create(target);
        }
        public async Task<CheckEmailDTO> CheckEmailAndAccount(CheckEmailViewModel check)
        {
            var emailCheck = await efRepository.AnyAsync<User>(x => x.Email == check.Email);
            var accountCheck = await efRepository.AnyAsync<User>(x => x.Account == check.Account);
            return new CheckEmailDTO
            {
                CheckAccount = accountCheck,
                CheckEmail = emailCheck
            };
        }
        public int GetUserIdByEmail(string Email)
        {
            if (efRepository.Any<User>(x => x.Email == Email))
            {
                var user =  efRepository.FirstOrDefault<User>(x => x.Email == Email);
                return user.Id;
            }
            return 0;
        }
        public void CertifiedEmail(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            user.IsEmailCertified = true;
            efRepository.Update(user);
        }
        public string GetUserAccountById(int id)
        {
            if (efRepository.Any<User>(x => x.Id == id))
            {
                return efRepository.FirstOrDefault<User>(x => x.Id == id).Account;
            }
            return "";
        }
        public void ResetPwd(ResetPasswordDto dto)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == dto.id);
            user.Password = HasherExtension.ToSHA256(dto.Password);
            //關閉重設密碼網址
            user.IsEmailPersonal = false;
            efRepository.Update(user);
        }
        public string CertifiedAccount(string userAccount, ResetPasswordDto dto)
        {
            if (userAccount == "")
            {
                return "沒有此帳號";
            }
            else if (dto.Account != userAccount)
            {
                return "帳號輸入錯誤!請重新輸入";
            }
            ResetPwd(dto);
            return "";
        }
        public void OpenResetPwdAddress(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            user.IsEmailPersonal = true;
            efRepository.Update(user);
        }
        //取得User是否開啟重設密碼
        public bool GetUserIsEmailPersonalById(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            return user.IsEmailPersonal;
        }
        public void UpdateUserSendEmailTimeByUserId(int id, EmailType emailType)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            if (emailType == EmailType.CheckEmail)
            {
                user.CertifiedTime = DateTime.UtcNow;
            }
            else if (emailType == EmailType.ResetPassword)
            {
                user.ResetPasswordTime = DateTime.UtcNow;
            }
            efRepository.Update(user);
        }
        public DateTime GetUserSendEmailTimeByUserId(int id, EmailType emailType)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            if (emailType == EmailType.CheckEmail)
            {
                return (DateTime)user.CertifiedTime;
            }
            else if (emailType == EmailType.ResetPassword)
            {
                return (DateTime)user.ResetPasswordTime;
            }
            else if (emailType == EmailType.BindEmail)
            {
                return (DateTime)user.EmailRigisterTime;
            }
            return DateTime.UtcNow;
        }
        public bool IsTimeMoreThan10min(DateTime time)
        {
            var now = DateTime.UtcNow;
            var diff = now - time;
            var min = (int)diff.TotalMinutes;
            if (min > 10)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
                return payload;
            }
            catch
            {
                return null;
            }
        }
        public bool IsExternalIdExist(string googleId, HatsukoiEnum.ExternalLoginType loginType)
        {
            bool result = false;
            switch (loginType)
            {
                case ExternalLoginType.Google:
                    result = efRepository.Any<User>(x => x.Identifier == googleId);
                    break;
                case ExternalLoginType.Facebook:
                    break;
                case ExternalLoginType.Line:
                    break;
                default:
                    break;
            }
            return result;
        }
        public User GetUserInfoById(int id)
        {
            return efRepository.FirstOrDefault<User>(x => x.Id == id);
        }
        public bool CheckEmail(string email)
        {
            return efRepository.Any<User>(x => x.Email == email);
        }
        public void CreateGoogle(GoogleJsonWebSignature.Payload payload)
        {
            var target = new User
            {
                Name = payload.Name,
                Email = payload.Email,
                CreateDate = DateTime.UtcNow,
                Photo = payload.Picture,
                IsEmailCertified = true,
                MemberLevel = 1,
                StoreStatus = 0,
                Identifier = payload.Subject,
                ModifiDate = DateTime.UtcNow,
                LastLoginTime = DateTime.UtcNow,
                IsEmailOrder = false,
                IsEmailHatsukoi = false,
                IsEmailActivity = false,
                IsEmailPersonal = false,
                IsEmailWeek = false,
                IsEmailMember = false,
                IsEmailFollow = false,
                IsEmailFocus = false
            };
            efRepository.Create(target);
        }
        public bool IsGoogleExist(string email)
        {
            var id = GetUserIdByEmail(email);
            var user = GetUserInfoById(id);
            return user.Identifier != null;
        }
        public bool IsFbExist(string email)
        {
            var id = GetUserIdByEmail(email);
            var user = GetUserInfoById(id);
            return user.IdentifierFb != null;
        }
        public bool IsLineExist(string email)
        {
            var id = GetUserIdByEmail(email);
            var user = GetUserInfoById(id);
            return user.IdentifierLine != null;
        }
        public void BindGoogle(string email, string googleId)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Email == email);
            user.Identifier = googleId;
            efRepository.Update(user);
        }
        public bool IsAccountExist(string account)
        {
            return efRepository.Any<User>(x => x.Account == account);
        }
        public void UserSetAccountAndPassword(RegisterHatsukoiDto dto)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Email == dto.Email);
            user.Account = dto.Account;
            var pw = HasherExtension.ToSHA256(dto.Password);
            user.Password = pw;
            efRepository.Update(user);
        }
        public string GetUserEmailById(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            return user.Email;
        }
        public void OpenEmailBind(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            user.IsEmailRigister = true;
            user.EmailRigisterTime = DateTime.UtcNow;
            efRepository.Update(user);
        }
        public void CloseEmailBund(int id)
        {
            var user = efRepository.FirstOrDefault<User>(x => x.Id == id);
            user.IsEmailRigister = false;
            efRepository.Update(user);
        }
        public  User GetUserById(int id)
        {
            return efRepository.FirstOrDefault<User>(x => x.Id == id);
        }
    }
}

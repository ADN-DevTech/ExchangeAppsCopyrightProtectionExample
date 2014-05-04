using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DummyService.Models
{
    public class OAuthResult
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string oauth_user_name { get; set; }
        public string oauth_user_guid { get; set; }
        public string scope { get; set; }
        public string oauth_problem { get; set; }
        public string oauth_error_message { get; set; }
        public string sessionHandle { get; set; }

    }


    public class EntitlementResult
    {
        public string UserId { get; set; }
        public string AppId { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }

    }


    public enum LoginType
    {
        AutodeskId,
        CustomerUserId
    }

    public class UserInfo
    {
        public LoginType UserType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
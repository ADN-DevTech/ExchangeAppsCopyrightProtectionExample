////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved 
// Written by Daniel Du 2014 - ADN/Developer Technical Services
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////////////////


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
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace AcadPluginApp
{
    /// <summary>
    /// Interaction logic for myAppWin.xaml
    /// </summary>
    public partial class myAppWin : Window
    {

        // Hard coded consumer and secret keys and base URL.
        // In real world Apps, these values need to secured.
        // One approach is to encrypt and/or obfuscate these values
        private const string m_ConsumerKey = "your consumer key";
        private const string m_ConsumerSecret = "your consumer secert";
        private const string m_baseURL = "https://accounts.autodesk.com";
        private const string AUTODESK_EXCHANGE_URL = "https://apps.exchange.autodesk.com";


        private const string CHECK_ENTITLEMENT_ENDPOINT = "webservices/checkentitlement";

        private static RestClient m_Client;
        private string m_oAuthReqToken;
        private string m_oAuthReqTokenSecret;
        private string m_strPIN;

        private string m_oAuthAccessToken;
        private string m_oAuthAccessTokenSecret;
        private string m_sessionHandle;


        public myAppWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string internalUserId = GetUserIdWithOAuth();
            if (internalUserId == string.Empty)
            {
                logOutput.Text = "You are not entitled to use this app."
                    +"please buy it from Autodesk Exchange.";
                return;
            }

            //Since you probably do not know this AppId until you publish it, 
            //so let's save the AppId in config file so that it can be edit latter
            //without changing the source codd
            //read this appId from config file
            string thisAppId = ConfigReader.ReadConfig("thisAppId");

            bool hasEntitilement = CheckEntitlement(internalUserId, thisAppId);
            if (hasEntitilement)
            {
                logOutput.Text = "You are entitled to use this app. Thank you for purchasing!";
                btnDoMyWork.IsEnabled = true;
            }
            else
            {
                logOutput.Text = "You are not entitled to use this app."
                    +"please buy it from Autodesk Exchange.";
            }
        }

        private string GetUserIdWithOAuth()
        {
            // Instantiate the RestSharp library object RestClient. RestSharp is free and makes it
            // very easy to build apps that use the OAuth and OpenID protocols with a provider supporting
            // these protocols
            m_Client = new RestClient(m_baseURL);

            m_Client.Authenticator =
          OAuth1Authenticator.ForRequestToken(m_ConsumerKey, m_ConsumerSecret);

            // Build the HTTP request for a Request token and execute it against the OAuth provider
            var request = new RestRequest("OAuth/RequestToken", Method.POST);
            var response = m_Client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                m_Client = null;
                logOutput.Text = "couldn't request token from Autodesk oxygen provider";
                return string.Empty;
            }

            // The HTTP request succeeded. Get the request token and associated parameters.
            var qs = HttpUtility.ParseQueryString(response.Content);
            m_oAuthReqToken = qs["oauth_token"];
            m_oAuthReqTokenSecret = qs["oauth_token_secret"];
            var oauth_callback_confirmed = qs["oauth_callback_confirmed"];
            var x_oauth_client_identifier = qs["x_oauth_client_identifier"];
            var xoauth_problem = qs["xoauth_problem"];
            var oauth_error_message = qs["oauth_error_message"];

            // For in band authorization build URL for Authorization HTTP request
            RestRequest authorizeRequest = new RestRequest
            {
                Resource = "OAuth/Authorize",
            };

            authorizeRequest.AddParameter("viewmode", "desktop");
            authorizeRequest.AddParameter("oauth_token", m_oAuthReqToken);
            Uri authorizeUri = m_Client.BuildUri(authorizeRequest);

            // Launch another window with browser control and navigate to the Authorization URL
            BrowserAuthenticate frm = new BrowserAuthenticate();
            frm.Uri = authorizeUri;
            if (frm.ShowDialog() != true)
            {
                m_Client = null;
                logOutput.Text = "In band Authorization failed";

                return string.Empty;
            }

            // Build the HTTP request for an access token
            request = new RestRequest("OAuth/AccessToken", Method.POST);
            m_Client.Authenticator = OAuth1Authenticator.ForAccessToken(
           m_ConsumerKey, m_ConsumerSecret, m_oAuthReqToken, m_oAuthReqTokenSecret
           );
            // Execute the access token request
            response = m_Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                m_Client = null;
                logOutput.Text = "couldn't get access token from your Autodesk account";
                return string.Empty;
            }


            // The request for access token is successful. Parse the response and store token,token secret and session handle
            qs = HttpUtility.ParseQueryString(response.Content);
            m_oAuthAccessToken = qs["oauth_token"];
            m_oAuthAccessTokenSecret = qs["oauth_token_secret"];
            var x_oauth_user_name = qs["x_oauth_user_name"];
            var x_oauth_user_guid = qs["x_oauth_user_guid"];

 
            return x_oauth_user_guid;
        }

        /// <summary>
        /// check whether a user has entitlement to use an app
        /// </summary>
        /// <param name="internalUserId"></param>
        /// <param name="thisAppId"></param>
        /// <returns></returns>
        private bool CheckEntitlement(string internalUserId, string thisAppId)
        {
            RestClient client = new RestClient(AUTODESK_EXCHANGE_URL);
            RestRequest req = new RestRequest(CHECK_ENTITLEMENT_ENDPOINT);
            req.Method = Method.GET;
            req.AddParameter("userid", internalUserId);
            req.AddParameter("appid", thisAppId);


            ServicePointManager.ServerCertificateValidationCallback 
                += (sender, certificate, chain, sslPolicyErrors) => true;

            IRestResponse<EntitlementResult> resp = client.Execute<EntitlementResult>(req);

            if (resp.Data != null && resp.Data.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}

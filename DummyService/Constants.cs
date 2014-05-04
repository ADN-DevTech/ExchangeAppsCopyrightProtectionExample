using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DummyService
{
    public class Constants
    {

        // Hard coded consumer and secret keys and base URL.
        // In real world Apps, these values need to secured and
        // preferably not hardcoded.
        public const string CONSUMER_KEY = "your consumer key";
        public const string CONSUMER_SECRET = "your consumer secret";
        public const string OAUTH_BASE_URL = "https://accounts.autodesk.com";


        public const string AUTODESK_EXCHANGE_URL = "https://apps.exchange.autodesk.com";
        public const string CHECK_ENTITLEMENT_ENDPOINT = "webservices/checkentitlement";


    }
}
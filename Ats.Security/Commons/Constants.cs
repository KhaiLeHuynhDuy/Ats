using System.Configuration;

namespace Ats.Security.Commons
{
    public static class Constants
    {
        public const string ENTITY_FRAMEWORK_CONNECTION_STRING = "AtsDbContext";
        public static string COOKIE_PATH = "";
        public static string CONFIGURATION_AUDIENCE_ID = ConfigurationManager.AppSettings["as:AudienceId"];
        public static string CONFIGURATION_AUDIENCE_SECRET = ConfigurationManager.AppSettings["as:AudienceSecret"];
        public static string CONFIGURATION_ISSUER = ConfigurationManager.AppSettings["as:issuer"];
        public static string CONFIGURATION_WEBROOT = ConfigurationManager.AppSettings["webRoot"];
        public static string CONFIGURATION_TOKEN_ENDPOINT = ConfigurationManager.AppSettings["as:TokenEndPoint"];
        public static int ACCESSTOKEN_EXPIRE_TIMESPAN_MINUTES = 60;

        public static string SESSION_EXPIRE = ConfigurationManager.AppSettings["SESSION_EXPIRE"];
        public static string SLIDING_EXPIRATION = ConfigurationManager.AppSettings["SlidingExpiration"];

        public static int TOKEN_LIFESPAN_MINUTES = 24;
        public static int PASSWORD_MIN_LENGHT = 8;

        public const string CLAIM_TYPE_ORGS = "orgid";

        public const string APP_ERROR = "Fail, something went wrong !";
        public const string LOGIN_INVALID_USERNAME_PASSWORD = "Username or Password is incorrect.";
        public const string CHANGE_PASSWORD_CONFIRM_FAIL = "Password does not match the confirm password.";
        public const string CHANGE_PASSWORD_INVALID_NEWPASSWORD = "Password must contain: minimum 8 characters, upper case letter and numberic value.";
        public const string CHANGE_PASSWORD_TOKEN_EXPIRED = "User does not exist or Access token has expired.";
        public const string CHANGE_PASSWORD_INVALID_OLDPASSWORD = "The old password entered was invalid";

    }
}
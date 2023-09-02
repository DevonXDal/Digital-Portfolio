namespace Portfolio
{
    /// <summary>
    /// This class provides strong typing for all environment variables.
    /// If an environment variable is missing, this class leaves the value blank.
    /// 
    /// The name is shortened from Environment in order to reduce horizontal waste and keep code clean.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Env
    {
        private IConfiguration _config;

        /// <summary>
        /// Points to the database connection string.
        /// </summary>
        public string MainDb => _config.GetRequiredSection("ConnectionStrings")["MainDb"];

        /// <summary>
        /// The email to use for the Super User admin account and for email sending as a no-reply
        /// </summary>
        public string NoReplyAdminEmail => _config["NoReplyAdminEmail"];

        /// <summary>
        /// The password to use for the Super User admin account and for email sending as a no-reply
        /// </summary>
        public string NoReplyAdminPass => _config["NoReplyAdminPass"];

        public string FileStoragePath => _config["FileStoragePath"];

        public string Port => _config.GetRequiredSection("Auth0")["Port"];

        public string ClientOriginUrl => _config.GetRequiredSection("Auth0")["ClientOriginUrl"];

        public string Auth0Domain => _config.GetRequiredSection("Auth0")["Domain"];

        public string Auth0Audience => _config.GetRequiredSection("Auth0")["Audience"];

        public Env(IConfiguration config)
        {
            _config = config;
        }
    }
}

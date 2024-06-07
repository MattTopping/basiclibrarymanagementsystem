using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml.Linq;

namespace CSNetLibraryManagement
{
    /// <summary>
    ///     Helper class containing function and values that are repeated through the application.
    ///     Singleton Pattern.
    /// </summary>
    public sealed class Utilities
    {
        private static Utilities Instance = null;
        private string dbConectionString;

        /// <summary>
        ///     Function for settng the single instance
        /// </summary>
        public static Utilities GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Utilities();
            }
            return Instance;
        }

        /// <summary>
        ///     Map all app config values to vars
        /// </summary>
        public Utilities()
        {
            dbConectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        }

        /// <summary>
        ///     Get methods for dbConnectionString
        /// </summary>
        public string DbConectionString   // property
        {
            get { return dbConectionString; }   // get method
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotronix.TestApp
{
    public class MEMBERS
    {
        /// <summary>
        /// Get or set all properties.
        /// </summary>
        public struct SQLReturnValue
        {
            public string Outval { get; set; }
            public string OUTMESSAGE { get; set; }
        }

        private static string _UserID = "00000000-0000-0000-0000-000000000000";

        public static string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }
}

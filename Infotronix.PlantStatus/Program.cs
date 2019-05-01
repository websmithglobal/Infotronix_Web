using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotronix.PlantStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Status Checking ....");
            CheckStatus objStatus = new CheckStatus();
            objStatus.ReadStatus();
            Environment.Exit(0);
        }
    }
}

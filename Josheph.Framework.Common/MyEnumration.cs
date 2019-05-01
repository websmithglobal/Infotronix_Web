using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    public class MyEnumration
    {
        public enum MyStatus { Active = 1, DeActive = 2, Suspend = 3 }
        public enum EntryMode { ADD, EDIT, DELETE, GET }
        public enum Operation { WHERE, LIKE, AND, OR, NONE }

        public enum MasterType
        {
            PlantMaster = 1,
            MainDeviceMaster=2,
            SubDeviceMaster = 3
        }
    }
}

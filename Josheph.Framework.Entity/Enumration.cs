using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Entity
{
    public class Enumration
    {
        public enum InverterStatusScheider
        {
            NoIrradiation=1,
            Ongrid =4,
            IrradiationDetecting=3,
            ShutdownAbnormalGridVoltage= 72406
        }
    }
}

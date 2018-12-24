using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tbrpg.SaveLoad
{
    public enum SaveType
    {
        /// <summary>
        /// Specifies that the save location is on the local computer.
        /// </summary>
        Local,
        /// <summary>
        /// Specifies that the save location is in the cloud.
        /// </summary>
        Cloud
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public static class UnityDebugExtensions
    {
        public static void EnsureInitialized(this MonoBehaviour source,MonoBehaviour obj,string objName)
        {
            if (obj == null)
            {
                throw new Exception($"{objName} has not been initialized in {source}");
            }
        }
    }


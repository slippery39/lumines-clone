using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    /**
     * Any components created that need to sync in some way to the music can call this interface and will have it automatically registered witht he UI to recieve
     * music updates.
     */
    interface ISyncToConductor
    {
        void ConductorUpdate(ConductorInfo info);
    }


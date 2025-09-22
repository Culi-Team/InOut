using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow.FlagDefinitions
{
    public enum EUnloadTransferProcessInput
    {
        AF_CLEAN_REQ_UNLOAD,
        AF_CLEAN_UNLOAD_DONE_RECEIVED,

        UNLOAD_ALIGN_READY,

        UNLOAD_ALIGN_PLACE_DONE_RECEIVED,

        UNLOAD_TRANSFER_UNLOADING
    }
}

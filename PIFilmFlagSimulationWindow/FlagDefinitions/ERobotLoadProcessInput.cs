using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow.FlagDefinitions
{
    public enum ERobotLoadProcessInput
    {
        IN_CST_READY,
        IN_CST_PICK_DONE_RECEIVED,
        VINYL_CLEAN_REQ_LOAD,
        VINYL_CLEAN_RECEIVE_LOAD_DONE,

        OUT_CST_READY,
        OUT_CST_PLACE_DONE_RECEIVED,
        VINYL_CLEAN_REQ_UNLOAD,
        VINYL_CLEAN_RECEIVE_UNLOAD_DONE,

        FIXTURE_ALIGN_REQ_LOAD,
        FIXTURE_ALIGN_LOAD_DONE_RECEIVED,
        REMOVE_FILM_REQ_UNLOAD
    }
}

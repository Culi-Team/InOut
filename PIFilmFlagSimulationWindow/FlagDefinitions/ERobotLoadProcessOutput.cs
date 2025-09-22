using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow.FlagDefinitions
{
    public enum ERobotLoadProcessOutput
    {
        ROBOT_PICK_IN_CST_DONE,
        VINYL_CLEAN_LOAD_DONE,

        VINYL_CLEAN_UNLOAD_DONE,

        FIXTURE_ALIGN_LOAD_DONE,
        REMOVE_FILM_UNLOAD_DONE,
        ROBOT_PLACE_OUT_CST_DONE,
    }
}

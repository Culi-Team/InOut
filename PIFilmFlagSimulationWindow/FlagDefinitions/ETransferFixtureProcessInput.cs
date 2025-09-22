using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmFlagSimulationWindow.FlagDefinitions
{
    public enum ETransferFixtureProcessInput
    {
        FIXTURE_ALIGN_DONE,
        DETACH_DONE,
        REMOVE_FILM_DONE,
        DETACH_ORIGIN_DONE,

        ALIGN_TRANSFER_FIXTURE_DONE_RECEIVED,
        DETACH_TRANSFER_FIXTURE_DONE_RECEIVED,
        REMOVE_FILM_TRANSFER_FIXTURE_DONE_RECEIVED,
    }
}

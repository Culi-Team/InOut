using EQX.Core.InOut;

namespace EQX.InOut
{
    public static class VacuumHelpers
    {
        public static IVacuum SetIdentity(this IVacuum vacuum, int id, string name)
        {
            if (vacuum is VacuumBase vacuumBase)
            {
                vacuumBase.Id = id;
                vacuumBase.Name = name;
            }

            return vacuum;
        }
    }
}

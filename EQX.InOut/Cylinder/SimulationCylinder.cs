using EQX.Core.InOut;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace EQX.InOut
{
    public static class SimulationInputSetter
    {
        public static void SetSimInput(IDInput? input, bool value)
        {
            if (input == null) return;

            using (MemoryMappedViewStream stream = _mmf.CreateViewStream(input.Id, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if (value)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);
                }
            }
        }

        static MemoryMappedFile _mmf = MemoryMappedFile.OpenExisting("SimInputData");
    }

    public class SimulationCylinder : CylinderBase
    {
        #region Constructors
        public SimulationCylinder(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
            : base(inForward, inBackward, outForward, outBackward)
        {
            _mmf = MemoryMappedFile.OpenExisting("SimInputData");
        }
        #endregion

        #region Override methods
        protected override void ForwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutForward!.Value = true;
                OutBackward!.Value = false;

                SetSimInput(InForward, true);
                SetSimInput(InBackward, false);
            }
            else if (OutForward == null & OutBackward == null)
            {
                // Both input is null
                return;
            }
            else if (OutBackward != null)
            {
                // Only backward is not null
                OutBackward!.Value = false;

                SetSimInput(InBackward, false);
                SetSimInput(InForward, true);
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = true;

                SetSimInput(InForward, true);
                SetSimInput(InBackward, false);
            }
        }

        protected override void BackwardAction()
        {
            if (OutForward != null & OutBackward != null)
            {
                // Both input not null
                OutBackward!.Value = true;
                OutForward!.Value = false;

                SetSimInput(InBackward, true);
                SetSimInput(InForward, false);
            }
            else if (OutForward == null & OutBackward == null)
            {
                // Both input is null
                return;
            }
            else if (OutBackward != null)
            {
                // Only backward is not null
                OutBackward!.Value = true;
                SetSimInput(InBackward, true);
                SetSimInput(InForward, false);
            }
            else
            {
                // Only forward is not null
                OutForward!.Value = false;
                SetSimInput(InForward, false);
                SetSimInput(InBackward, true);
            }
        }
        #endregion

        #region Private methods
        private void SetSimInput(IDInput? input, bool value)
        {
            if (input == null) return;

            using (MemoryMappedViewStream stream = _mmf.CreateViewStream(input.Id, 0))
            {
                BinaryWriter writer = new BinaryWriter(stream);
                if (value)
                {
                    writer.Write((char)1);
                }
                else
                {
                    writer.Write((char)0);
                }
            }
        }
        #endregion

        #region Privates
        MemoryMappedFile _mmf;
        #endregion
    }
}

using EQX.Core.InOut;
using System.IO.MemoryMappedFiles;
using System.Reflection;

namespace EQX.InOut
{
    public class SimulationCylinderFactory : ICylinderFactory
    {
        public ICylinder Create(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            return new SimulationCylinder(inForward, inBackward, outForward, outBackward);
        }
    }

    public class SimulationCylinder : ICylinder
    {
        public bool IsForward
        {
            get
            {
                if (_inForward != null & _inBackward != null)
                {
                    // Both input not null
                    return _inForward!.Value & !_inBackward!.Value;
                }
                else if (_inForward == null & _inBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (_inBackward != null)
                {
                    // Only backward is not null
                    return !_inBackward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return _inForward!.Value;
                }
            }
        }

        public bool IsBackward
        {
            get
            {
                if (_inForward != null & _inBackward != null)
                {
                    // Both input not null
                    return !_inForward!.Value & _inBackward!.Value;
                }
                else if (_inForward == null & _inBackward == null)
                {
                    // Both input is null
                    return false;
                }
                else if (_inBackward != null)
                {
                    // Only backward is not null
                    return _inBackward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return !_inForward!.Value;
                }
            }
        }

        public SimulationCylinder(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            _inForward = inForward;
            _inBackward = inBackward;
            _outForward = outForward;
            _outBackward = outBackward;

            _mmf = MemoryMappedFile.OpenExisting("SimInputData");
        }

        public void Forward()
        {
            if (_outForward != null & _outBackward != null)
            {
                // Both input not null
                _outForward!.Value = true;
                _outBackward!.Value = false;

                SetSimInput(_inForward, true);
                SetSimInput(_inBackward, false);
            }
            else if (_outForward == null & _outBackward == null)
            {
                // Both input is null
                return;
            }
            else if (_outBackward != null)
            {
                // Only backward is not null
                _outBackward!.Value = false;

                SetSimInput(_inBackward, false);
            }
            else
            {
                // Only forward is not null
                _outForward!.Value = true;

                SetSimInput(_inForward, true);
            }
        }

        public void Backward()
        {
            if (_outForward != null & _outBackward != null)
            {
                // Both input not null
                _outBackward!.Value = true;
                _outForward!.Value = false;

                SetSimInput(_inBackward, true);
                SetSimInput(_inForward, false);
            }
            else if (_outForward == null & _outBackward == null)
            {
                // Both input is null
                return;
            }
            else if (_outBackward != null)
            {
                // Only backward is not null
                _outBackward!.Value = true;
                SetSimInput(_inBackward, true);
            }
            else
            {
                // Only forward is not null
                _outForward!.Value = false;
                SetSimInput(_inForward, false);
            }
        }

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

        #region Privates
        MemoryMappedFile _mmf;

        private IDOutput? _outForward { get; }
        private IDOutput? _outBackward { get; }
        private IDInput? _inForward { get; }
        private IDInput? _inBackward { get; }
        #endregion
    }
}

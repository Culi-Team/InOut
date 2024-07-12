using EQX.Core.InOut;

namespace EQX.InOut
{
    public class Cylinder : ICylinder
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
                    return _inForward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return !_inBackward!.Value;
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
                    return !_inForward!.Value;
                }
                else
                {
                    // Only forward is not null
                    return _inBackward!.Value;
                }
            }
        }

        public Cylinder(IDInput? inForward, IDInput? inBackward, IDOutput? outForward, IDOutput? outBackward)
        {
            _inForward = inForward;
            _inBackward = inBackward;
            _outForward = outForward;
            _outBackward = outBackward;
        }

        public void MoveForward()
        {
            if (_outForward != null & _outBackward != null)
            {
                // Both input not null
                _outForward!.Value = true;
                _outBackward!.Value = false;
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
            }
            else
            {
                // Only forward is not null
                _outForward!.Value = true;
            }
        }

        public void MoveBackward()
        {
            if (_outForward != null & _outBackward != null)
            {
                // Both input not null
                _outBackward!.Value = true;
                _outForward!.Value = false;
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
            }
            else
            {
                // Only forward is not null
                _outForward!.Value = false;
            }
        }

        #region Privates
        private readonly IDOutput? _outForward;
        private readonly IDOutput? _outBackward;
        private readonly IDInput? _inForward;
        private readonly IDInput? _inBackward;
        #endregion
    }
}

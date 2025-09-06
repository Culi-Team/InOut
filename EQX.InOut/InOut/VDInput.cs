using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut.InOut
{
    public class VDInput : ObservableObject, IDInput
    {
        public event EventHandler? ValueUpdated;
        public event EventHandler? ValueChanged;
        public int Id { get; init; }
        public string Name { get; init; }
        public bool Value
        {
            get
            {
                if (dOutput == null)
                {
                    throw new InvalidOperationException("Value input is not mapped");
                }

                return dOutput.Value;
            }
        }

        public VDInput(int id, string name, IDInputDevice dInputDevice)
        {
            Id = id;
            Name = name;

            _dInputDevice = dInputDevice;
        }

        public void Mapping(IDOutput dOutput)
        {
            if (this.dOutput != null)
            {
                throw new InvalidOperationException("Value input is mapped");
            }

            this.dOutput = dOutput;
        }

        public void RaiseValueUpdated()
        {
            OnPropertyChanged(nameof(Value));
            ValueUpdated?.Invoke(this, EventArgs.Empty);

            _oldValue = _currentValue;
            _currentValue = Value;

            if (_oldValue != _currentValue)
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _oldValue;
        private bool _currentValue;
        private readonly IDInputDevice _dInputDevice;

        private IDOutput dOutput;
    }
}

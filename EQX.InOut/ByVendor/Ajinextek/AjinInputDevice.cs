using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut
{
    public class AjinInputDevice : IDInputDevice
    {
        #region Properties
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsConnected => CAXL.AxlIsOpened() == (int)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        #endregion

        public bool this[int index] => GetInput(index);

        #region Public methods
        public bool Connect()
        {
            if (IsConnected) return true;

            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public bool Disconnect()
        {
            CAXL.AxlClose();

            return true;
        }
        #endregion

        #region Privates
        private bool GetInput(int index)
        {
            CAXD.AxdiReadInport(index, ref oldValue);

            return oldValue == 1;
        }

        private uint oldValue = 0;
        #endregion
    }

    public class AjinOutputDevice : IDOutputDevice
    {
        #region Properties
        public int Id { get; init; }
        public string Name { get; init; }
        public bool IsConnected => CAXL.AxlIsOpened() == (int)AXT_FUNC_RESULT.AXT_RT_SUCCESS;
        #endregion

        public bool this[int index]
        {
            get
            {
                return GetOutput(index);
            }
            set
            {
                SetOutput(index, value);
            }
        }

        #region Public methods
        public bool Connect()
        {
            if (IsConnected) return true;

            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public bool Disconnect()
        {
            CAXL.AxlClose();

            return true;
        }
        #endregion

        #region Privates
        private bool GetOutput(int index)
        {
            CAXD.AxdoReadOutport(index, ref oldValue);

            return oldValue == 1;
        }

        private void SetOutput(int index, bool value)
        {
            CAXD.AxdoWriteOutport(index, value ? 1u : 0u);
        }

        private uint oldValue = 0;
        #endregion
    }
}

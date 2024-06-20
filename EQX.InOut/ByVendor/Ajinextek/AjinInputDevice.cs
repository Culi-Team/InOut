using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut
{
    public class AjinInputDevice : InputDeviceBase
    {
        #region Properties
        public override bool IsConnected => CAXL.AxlIsOpened() == 0x01;
        #endregion

        #region Constructor(s)
        public AjinInputDevice(int id, string name, List<string> inputs)
            : base(id, name, inputs)
        {
        }
        #endregion

        #region Public methods
        public override bool Connect()
        {
            if (IsConnected) return true;

            if (CAXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public override bool Disconnect()
        {
            CAXL.AxlClose();

            return true;
        }
        #endregion

        #region Private methods
        protected override bool GetInput(int index)
        {
            CAXD.AxdiReadInport(index, ref oldValue);

            return oldValue == 1;
        }
        #endregion

        #region Privates
        private uint oldValue = 0;
        #endregion
    }
}

using EQX.Core.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQX.InOut
{
    public class AjinInputDevice<TEnum> : InputDeviceBase<TEnum>
    {
        #region Properties
        public override bool IsConnected => AXL.AxlIsOpened() == 0x01;
        #endregion

        #region Constructor(s)
        public AjinInputDevice(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Public methods
        public override bool Connect()
        {
            if (IsConnected) return true;

            if (AXL.AxlOpen(7) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) return false;

            return true;
        }

        public override bool Disconnect()
        {
            AXL.AxlClose();

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

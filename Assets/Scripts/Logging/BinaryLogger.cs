using System;
using System.Collections.Generic;
using System.Text;

namespace NeanderthalTools.Logging
{
    public class BinaryLogger : BaseLogger
    {
        #region Overrides

        protected override void WriteLog(IReadOnlyList<object> values)
        {
            foreach (var value in values)
            {
                byte[] bytes = null;
                if (value is float floatValue)
                {
                    bytes = BitConverter.GetBytes(floatValue);
                } else if (value is string stringValue)
                {
                    bytes = Encoding.UTF8.GetBytes(stringValue);
                }

                if (bytes != null)
                {
                    Write(bytes);
                }
            }
        }

        #endregion
    }
}

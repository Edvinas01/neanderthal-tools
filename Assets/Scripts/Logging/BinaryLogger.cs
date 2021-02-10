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
                var bytes = value switch
                {
                    float floatValue => BitConverter.GetBytes(floatValue),
                    string stringValue => Encoding.UTF8.GetBytes(stringValue),
                    _ => null
                };

                if (bytes != null)
                {
                    Write(bytes);
                }
            }
        }

        #endregion
    }
}

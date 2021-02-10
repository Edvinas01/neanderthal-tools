using System.Collections.Generic;
using System.Text;

namespace NeanderthalTools.Logging
{
    public class CsvLogger : BaseLogger
    {
        #region Overrides

        protected override void WriteLog(IReadOnlyList<object> values)
        {
            for (var i = 0; i < values.Count; i++)
            {
                WriteStr(values[i].ToString());
                if (i + 1 < values.Count)
                {
                    WriteStr(",");
                }
            }

            WriteStr("\n");
        }

        private void WriteStr(string str)
        {
            Write(Encoding.UTF8.GetBytes(str));
        }

        #endregion
    }
}

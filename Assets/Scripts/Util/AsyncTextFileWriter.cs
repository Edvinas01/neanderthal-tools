using System.Text;

namespace NeanderthalTools.Util
{
    public class AsyncTextFileWriter : AsyncFileWriter<string>
    {
        #region Methods

        protected override byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        #endregion
    }
}

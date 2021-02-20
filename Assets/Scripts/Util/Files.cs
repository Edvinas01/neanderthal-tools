using System;

namespace NeanderthalTools.Util
{
    public static class Files
    {
        /// <returns>
        /// File and folder friendly date-name.
        /// </returns>
        public static string DateName()
        {
            return $"{DateTime.UtcNow:yyyy-MM-dd'_'hh-mm-ss}";
        }
    }
}

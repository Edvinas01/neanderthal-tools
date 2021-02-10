using System;

namespace NeanderthalTools.Util
{
    public static class Files
    {
        public static string FileDateName()
        {
            return $"{DateTime.UtcNow:yyyy-MM-dd'_'hh-mm-ss}";
        }
    }
}

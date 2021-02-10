namespace NeanderthalTools.Logging
{
    public interface IMetaLogger
    {
        /// <summary>
        /// Log meta-data, such as field name.
        /// </summary>
        void LogMeta(object value);
    }
}

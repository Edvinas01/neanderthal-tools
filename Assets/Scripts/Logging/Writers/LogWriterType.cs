using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    public enum LogWriterType
    {
        None,

        [InspectorName("CSV")]
        Csv,
        Binary
    }
}

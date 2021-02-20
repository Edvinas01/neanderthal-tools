using System;
using System.Text;
using NeanderthalTools.Util;

namespace NeanderthalTools.Logging.Writers
{
    public class BinaryLogWriter : BaseLogWriter
    {
        public BinaryLogWriter(AsyncFileWriter writer, float sampleIntervalSeconds)
            : base(writer, sampleIntervalSeconds)
        {
        }

        protected override void WriteValues(params object[] values)
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
    }
}

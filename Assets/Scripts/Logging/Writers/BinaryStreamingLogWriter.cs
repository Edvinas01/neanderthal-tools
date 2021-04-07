using System;
using System.Text;
using NeanderthalTools.Util;

namespace NeanderthalTools.Logging.Writers
{
    public class BinaryStreamingLogWriter : BaseStreamingLogWriter
    {
        public BinaryStreamingLogWriter(AsyncFileWriter writer) : base(writer)
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LG.Utility {
    /// <summary>
    /// 流拓展
    /// </summary>
    public static class StreamExtend {
        /// <summary>
        /// 把另一个流里的数据写到当前流的当前位置后,不会去打开/关闭当前流,也不会去打开/关闭另一个流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sourceStream"></param>
        public static void WriteStream(this Stream stream, Stream sourceStream) {
            const int buffLen = 2048;
            byte[] buff = new byte[buffLen];
            int buffReadLen = 0;
            while ((buffReadLen = sourceStream.Read(buff, 0, buffLen)) > 0) {
                stream.Write(buff, 0, buffReadLen);
            }
        }
    }
}

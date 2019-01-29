using DotNetty.Buffers;
using DotNetty.Codecs;
using System.Collections.Generic;
using JTNE.Protocol;
using DotNetty.Transport.Channels;

namespace JTNE.DotNetty.Core.Codecs
{
    public class JTNETcpDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            byte[] buffer = new byte[input.Capacity];
            input.ReadBytes(buffer, 0, input.Capacity);
            output.Add(buffer);
        }
    }
}

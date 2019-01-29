# 了解JTNE协议处理tcp粘包/拆包

## 使用DotNetty的LengthFieldBasedFrameDecoder定长解码器

参数说明:

maxFrameLength：解码的帧的最大长度
lengthFieldOffset：长度字段的偏差(长度属性的起始位（偏移位），包中存放有整个大数据包长度的字节，这段字节的其实位置)
lengthFieldLength：长度字段占的字节数(即存放整个大数据包长度的字节所占的长度)
lengthAdjustmen：添加到长度字段的补偿值(长度调节值，在总长被定义为包含包头长度时，修正信息长度)。
initialBytesToStrip：从解码帧中第一次去除的字节数(跳过的字节数，根据需要我们跳过lengthFieldLength个字节，以便接收端直接接受到不含“长度属性”的内容)
failFast ：为true，当frame长度超过maxFrameLength时立即报TooLongFrameException异常，为false，读取完整个帧再报异常

### JTNE协议处理

22 JTNEPackage数据体长度
2  JTNEPackage数据体长度占两个字节
1  JTNEPackage校验位

``` netty

channel.Pipeline.AddLast("jtneTcpDecoder", new LengthFieldBasedFrameDecoder(int.MaxValue, 22, 2, 1, 0));
```

## 使用SuperSocket的FixedHeaderReceiveFilter固定头部解码器

``` supersocket

 public class JTNEReceiveFilter: FixedHeaderReceiveFilter<JTNERequestInfo>
{
    // 24 JTNEPackage头部固定长度
    public NEReceiveFilter()
        : base(24)
    {

    }

    protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
    {
        // +1 JTNEPackage数据长度加上一个字节的校验位
        return header.toIntH2L(22 + offset, 2) + 1;
    }

    protected override JTNERequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
    {
        var reInfo = new JTNERequestInfo();
        reInfo.OriginalPackage = new byte[header.Count + length];
        Array.Copy(header.Array, 0, reInfo.OriginalPackage, 0, header.Count);
        Array.Copy(bodyBuffer, offset, reInfo.OriginalPackage, header.Count, length);
        return reInfo;
    }
}

```

## 使用DotNetty测试粘包

1.一条实时位置上报的测试数据

``` data
2323020131323334353637383900000000000000000100D001040507003A00001A0A00640063030602007B02030202010201004100370300EC00640203020042023605085800650308AE006F0C9600030102030D1B221A0A560D086502040100CB006605010031AD030012D1CB061115007B0709000832124211320607110000159D03000003E8000003E9000003EA03000007D0000007D1000007D20300000BB800000BB900000BBA0300000FA000000FA100000FA20802010002007B0037006F03006F00DE014D03000504D2004200DE0301BC022B029A0902010004010203040200040506070867
```

2.启动JTNE服务端程序

3.使用tcp客户端工具

创建两个tcp客户端，如图所示:

![tcp_test1](https://github.com/SmallChi/JTNewEnergyDotNetty/blob/master/doc/img/tcp_test1.png)

4.客户端分别发送消息

每个客户端分别连续发送1w个包进行测试，如图所示:

服务端接收并解析到包的数量等于客户端所发送的数量，那么处理就算正确。

![tcp_test2](https://github.com/SmallChi/JTNewEnergyDotNetty/blob/master/doc/img/tcp_test2.png)

5.解决了服务端协议的解码问题对于上层业务处理来说就是搬砖的是了
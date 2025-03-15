using System.Text;
using Google.Protobuf;

namespace Common.Grpc;

public static class GrpcConverters
{
    public static ByteString ToByteString(this string value)
    {
        return UnsafeByteOperations.UnsafeWrap(Encoding.UTF8.GetBytes(value));
    }
}
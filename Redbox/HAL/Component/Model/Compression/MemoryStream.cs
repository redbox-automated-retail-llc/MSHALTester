using System.IO;


namespace Redbox.HAL.Component.Model.Compression
{
    internal sealed class MemoryStream : StreamBase
    {
        internal byte[] GetBuffer()
        {
            return this.Stream != null ? ((System.IO.MemoryStream)this.Stream).GetBuffer() : (byte[])null;
        }

        internal byte[] ToArray()
        {
            return this.Stream != null ? ((System.IO.MemoryStream)this.Stream).ToArray() : (byte[])null;
        }

        internal MemoryStream() => this.Stream = (Stream)new System.IO.MemoryStream();

        internal MemoryStream(byte[] buffer) => this.Stream = (Stream)new System.IO.MemoryStream(buffer);
    }
}

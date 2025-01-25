using SevenZip.Compression.LZMA;


namespace Redbox.HAL.Component.Model.Compression
{
    internal sealed class LzmaCompressionAlgorithm : ICompressionAlgorithm
    {
        public byte[] Compress(byte[] source) => SevenZipHelper.Compress(source);

        public byte[] Decompress(byte[] source) => SevenZipHelper.Decompress(source);
    }
}

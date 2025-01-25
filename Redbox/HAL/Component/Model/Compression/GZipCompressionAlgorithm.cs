using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;


namespace Redbox.HAL.Component.Model.Compression
{
    internal sealed class GZipCompressionAlgorithm : ICompressionAlgorithm
    {
        public byte[] Compress(byte[] source)
        {
            byte[] destinationArray;
            using (MemoryStream input = StreamBase.NewOn(source))
            {
                BinaryReader binaryReader = new BinaryReader((Stream)(StreamBase)input);
                using (MemoryStream baseOutputStream = StreamBase.New())
                {
                    GZipOutputStream gzipOutputStream = new GZipOutputStream((Stream)(StreamBase)baseOutputStream);
                    gzipOutputStream.Write(binaryReader.ReadBytes((int)input.Length), 0, (int)input.Length);
                    gzipOutputStream.Finish();
                    gzipOutputStream.Flush();
                    destinationArray = new byte[baseOutputStream.Length];
                    Array.Copy((Array)baseOutputStream.GetBuffer(), 0L, (Array)destinationArray, 0L, baseOutputStream.Length);
                }
                binaryReader.Close();
            }
            return destinationArray;
        }

        public byte[] Decompress(byte[] source)
        {
            byte[] destinationArray;
            using (MemoryStream output = StreamBase.New())
            {
                BinaryWriter binaryWriter = new BinaryWriter((Stream)(StreamBase)output);
                using (MemoryStream baseInputStream = StreamBase.NewOn(source))
                {
                    byte[] buffer = new byte[4096];
                    GZipInputStream gzipInputStream = new GZipInputStream((Stream)(StreamBase)baseInputStream, 4096);
                    while (true)
                    {
                        int count = gzipInputStream.Read(buffer, 0, 4096);
                        if (count != 0)
                            binaryWriter.Write(buffer, 0, count);
                        else
                            break;
                    }
                }
                destinationArray = new byte[output.Length];
                Array.Copy((Array)output.GetBuffer(), 0L, (Array)destinationArray, 0L, output.Length);
                binaryWriter.Close();
            }
            return destinationArray;
        }
    }
}

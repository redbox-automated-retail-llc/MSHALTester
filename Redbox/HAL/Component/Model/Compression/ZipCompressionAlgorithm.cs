using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;


namespace Redbox.HAL.Component.Model.Compression
{
    internal sealed class ZipCompressionAlgorithm : ICompressionAlgorithm
    {
        public byte[] Compress(byte[] source)
        {
            byte[] destinationArray;
            using (MemoryStream input = StreamBase.NewOn(source))
            {
                BinaryReader binaryReader = new BinaryReader((Stream)(StreamBase)input);
                using (MemoryStream baseOutputStream = StreamBase.New())
                {
                    ZipOutputStream zipOutputStream = new ZipOutputStream((Stream)(StreamBase)baseOutputStream);
                    zipOutputStream.Write(binaryReader.ReadBytes((int)input.Length), 0, (int)input.Length);
                    zipOutputStream.Finish();
                    zipOutputStream.Flush();
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
                    ZipInputStream zipInputStream = new ZipInputStream((Stream)(StreamBase)baseInputStream);
                    while (true)
                    {
                        int count = zipInputStream.Read(buffer, 0, 4096);
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

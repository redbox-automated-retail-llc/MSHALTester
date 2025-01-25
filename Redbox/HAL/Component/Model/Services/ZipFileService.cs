using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;


namespace Redbox.HAL.Component.Model.Services
{
    public sealed class ZipFileService : IZipFileService
    {
        private readonly IRuntimeService RuntimeService;

        public ZipResult Zip(IEnumerable<string> files, string zipPath)
        {
            ZipResult zipResult = new ZipResult()
            {
                ZipFile = zipPath
            };
            try
            {
                using (ZipOutputStream zipOutputStream = new ZipOutputStream((Stream)File.Create(zipPath)))
                {
                    zipOutputStream.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    int num = 0;
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        zipOutputStream.PutNextEntry(new ZipEntry(fileInfo.Name)
                        {
                            DateTime = fileInfo.CreationTime
                        });
                        using (FileStream fileStream = File.OpenRead(file))
                        {
                            int count;
                            do
                            {
                                count = fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, count);
                            }
                            while (count > 0);
                        }
                        ++num;
                    }
                    zipOutputStream.Finish();
                    zipOutputStream.Close();
                    zipResult.EntryCount = num;
                    zipResult.Success = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("!!OnZipImages: exception!!", ex);
                zipResult.Success = false;
            }
            return zipResult;
        }

        public UnzipResult Unzip(string zipFile)
        {
            string unzipPath = this.RuntimeService.RuntimePath(Path.GetFileNameWithoutExtension(new FileInfo(zipFile).Name));
            return this.Unzip(zipFile, unzipPath);
        }

        public UnzipResult Unzip(string zipFile, string unzipPath)
        {
            UnzipResult unzipResult = new UnzipResult()
            {
                SourceZip = zipFile,
                OutputPath = unzipPath,
                Success = false
            };
            if (!this.SafeCreateDirectory(unzipPath))
                return unzipResult;
            try
            {
                using (ZipInputStream zipInputStream = new ZipInputStream((Stream)File.OpenRead(zipFile)))
                {
                    ZipEntry nextEntry;
                    while ((nextEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        if (Path.GetFileName(nextEntry.Name) != string.Empty)
                        {
                            string str = Path.Combine(unzipPath, nextEntry.Name);
                            using (FileStream fileStream = File.Create(str))
                            {
                                byte[] buffer = new byte[2048];
                                while (true)
                                {
                                    int count = zipInputStream.Read(buffer, 0, buffer.Length);
                                    if (count > 0)
                                        fileStream.Write(buffer, 0, count);
                                    else
                                        break;
                                }
                            }
                            new FileInfo(str).CreationTime = nextEntry.DateTime;
                        }
                    }
                }
                unzipResult.Success = true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WithContext("ZipService UnZip failed with an exception", (object)ex);
                unzipResult.Success = false;
            }
            return unzipResult;
        }

        public ZipFileService(IRuntimeService rts) => this.RuntimeService = rts;

        private UnzipResult OnUnzip(string zipFile, string outputPath)
        {
            UnzipResult unzipResult = new UnzipResult()
            {
                OutputPath = outputPath,
                SourceZip = zipFile,
                Success = false
            };
            if (!this.SafeCreateDirectory(outputPath))
                return unzipResult;
            int num = 0;
            using (ZipFile zipFile1 = new ZipFile(zipFile))
            {
                foreach (ZipEntry entry in zipFile1)
                {
                    ++num;
                    if (entry.IsDirectory)
                    {
                        if (!this.SafeCreateDirectory(entry.Name))
                            return unzipResult;
                    }
                    else
                    {
                        byte[] buffer = new byte[4096];
                        Stream inputStream = zipFile1.GetInputStream(entry);
                        string path = Path.Combine(outputPath, entry.Name);
                        string directoryName = Path.GetDirectoryName(path);
                        if (directoryName.Length > 0 && !this.SafeCreateDirectory(directoryName))
                            return unzipResult;
                        using (FileStream destination = File.Create(path))
                            StreamUtils.Copy(inputStream, (Stream)destination, buffer);
                    }
                }
                unzipResult.EntryCount = num;
                unzipResult.Success = (long)num == zipFile1.Count;
                return unzipResult;
            }
        }

        private bool SafeCreateDirectory(string directory)
        {
            if (Directory.Exists(directory))
                return true;
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

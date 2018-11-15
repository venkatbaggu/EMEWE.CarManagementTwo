using System;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class ZIPPER
    {
        /// <summary>
        /// Gzip压缩
        /// </summary>
        /// <param name="sourceFile">待压缩文件</param>
        /// <param name="destinationFile">指定生成压缩后的文件</param>
        public void GzipFile(string sourceFile, string destinationFile)
        {
            if (File.Exists(sourceFile) == false) throw new FileNotFoundException();
            byte[] buffer = null;
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream compressedStream = null;

            try
            {
                sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[sourceStream.Length];
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);
                if (checkCounter != buffer.Length)
                {
                    throw new ApplicationException();
                }
                destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);
                compressedStream = new GZipStream(destinationStream, CompressionMode.Compress, true);
                compressedStream.Write(buffer, 0, buffer.Length);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            finally
            {
                if (sourceStream != null)
                    sourceStream.Close();

                if (compressedStream != null)
                    compressedStream.Close();

                if (destinationStream != null)
                    destinationStream.Close();
            }


        }

        /// <summary>
        /// Gzip压缩
        /// </summary>
        /// <param name="fInfo">待压缩文件,生成同名后缀".gz"的压缩文件</param>
        public void GzipFile(FileInfo fInfo)
        {
            if (File.Exists(fInfo.ToString() + ".gz"))
            {
                File.Delete(fInfo.ToString() + ".gz");
            }
            if (fInfo.Exists == false) throw new FileNotFoundException();
            try
            {
                using (FileStream inFile = fInfo.OpenRead())
                {
                    if ((File.GetAttributes(fInfo.FullName) & FileAttributes.Hidden)
                        != FileAttributes.Hidden & fInfo.Extension != ".gz")
                    {
                        using (FileStream outFile = File.Create(fInfo.FullName + ".gz"))
                        {
                            using (GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress))
                            {
                                byte[] tempInFile = new byte[inFile.Length];
                                inFile.Read(tempInFile, 0, (int)inFile.Length);
                                Compress.Write(tempInFile, 0, tempInFile.Length);
                                outFile.Flush();
                            }
                            outFile.Close();
                        }
                    }

                }
            }
            catch 
            {

               
            }
            if (fInfo.Exists)
            {
                fInfo.Delete();
            }

        }

        /// <summary>
        /// Gzip解压
        /// </summary>
        /// <param name="sourceFile">待解压文件</param>
        /// <param name="destinationFile">指定生成解压后的文件</param>
        public void UnGzipFile(string sourceFile, string destinationFile)
        {

            if (File.Exists(sourceFile) == false) throw new FileNotFoundException();
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream decompressedStream = null;
            byte[] quartetBuffer = null;
            try
            {
                sourceStream = new FileStream(sourceFile, FileMode.Open);
                decompressedStream = new GZipStream(sourceStream, CompressionMode.Decompress, true);
                quartetBuffer = new byte[4];
                int position = (int)sourceStream.Length - 4;
                sourceStream.Position = position;
                sourceStream.Read(quartetBuffer, 0, 4);
                sourceStream.Position = 0;
                int checkLength = BitConverter.ToInt32(quartetBuffer, 0);
                byte[] buffer = new byte[checkLength + 100];
                int offset = 0;
                int total = 0;
                while (true)
                {
                    int bytesRead = decompressedStream.Read(buffer, offset, 100);
                    if (bytesRead == 0)
                        break;
                    offset += bytesRead;
                    total += bytesRead;
                }
                destinationStream = new FileStream(destinationFile, FileMode.Create);
                destinationStream.Write(buffer, 0, total);
                destinationStream.Flush();
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            finally
            {
                if (sourceStream != null)
                    sourceStream.Close();
                if (decompressedStream != null)
                    decompressedStream.Close();
                if (destinationStream != null)
                    destinationStream.Close();
            }


        }

        /// <summary>
        /// Gzip解压
        /// </summary>
        /// <param name="fInfo">待解压文件，生成同名去掉后缀".gz"的解压文件</param>
        public void UnGZipFile(FileInfo fInfo)
        {
            if (fInfo.Exists == false) throw new FileNotFoundException();
            try
            {
                using (FileStream inFile = fInfo.OpenRead())
                {
                    string curFile = fInfo.FullName;
                    string origName = curFile.Remove(curFile.Length - fInfo.Extension.Length);
                    using (FileStream outFile = File.Create(origName))
                    {
                        using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))
                        {
                            byte[] tempFile = new byte[100];
                            while (true)
                            {
                                int size = Decompress.Read(tempFile, 0, 100);
                                if (size > 0)
                                    outFile.Write(tempFile, 0, size);
                                else
                                    break;
                            }
                            outFile.Flush();
                        }
                        outFile.Close();
                    }
                }
            }
            catch 
            {
                
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace LG.Utility {
    /// <summary>
    /// Zip压缩与解压缩 
    /// </summary>
    public class ZipHelper {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize) {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(fileToZip)) {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile)) {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile)) {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        ZipStream.PutNextEntry(ZipEntry);

                        ZipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try {
                            do {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        } catch (System.Exception ex) {
                            throw ex;
                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile) {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip)) {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip)) {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream ZipFile = File.Create(zipedFile)) {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile)) {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile) {
            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile)) {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile)) {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath) {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar) {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
        {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                } else // 否则直接压缩文件
            {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file)) {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite) {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile))) {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null) {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "") {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName))) {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName)) {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true) {
                                    size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();
            }
        }


        /// <summary>
        /// 目录压缩
        /// </summary>
        /// <param name="compressedFilePath">压缩包文件输出路径</param>
        /// <param name="dirPath">目录路径</param>
        /// <param name="includeDir">是否包含传入的目录一起压缩,false则只压缩目录里的内容</param>
        /// <param name="appendCompressed">是否追加压缩,需要压缩包文件已经存在</param>
        /// <returns></returns>
        /// <exception cref="Exception">要压缩的文件不存在</exception>
        /// <exception cref="Exception">压缩异常</exception>
        public static void CommpressDir(string compressedFilePath, string dirPath, bool includeDir, bool appendCompressed) {
            if (!Directory.Exists(dirPath)) {
                throw new Exception("要打包的目录不存在");
            }
            if (dirPath.EndsWith(@"\")) {
                dirPath = dirPath.Remove(dirPath.Length - 1);
            }

            //如果要追加,但是压缩包文件不存在,就改成不追加(新增)
            if (appendCompressed && !File.Exists(compressedFilePath)) appendCompressed = false;
            //获取要打包的文件列表
            string[] packFiles = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            //父级目录(用来确定压缩包里的相对路径)
            string parentDirPath = dirPath;
            if (includeDir) {
                //如果要包含当前目录,那么父级目录需要向上一级
                parentDirPath = Path.GetDirectoryName(dirPath);
            }
            //尾部加上"\",方便使用
            if (!parentDirPath.EndsWith(@"\")) parentDirPath += @"\";

            string compressedDirPath = Path.GetDirectoryName(compressedFilePath);
            if (!Directory.Exists(compressedDirPath)) Directory.CreateDirectory(compressedDirPath);


            using (ZipOutputStream s = new ZipOutputStream(new FileStream(compressedFilePath, appendCompressed ? FileMode.Append : FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                s.SetLevel(9);
                //循环所有文件,写到压缩包里
                foreach (string packFilePath in packFiles) {
                    //在压缩包里的路径
                    string inPackPath = packFilePath.Substring(parentDirPath.Length);
                    //添加这个文件
                    s.PutNextEntry(new ZipEntry(inPackPath));
                    //将这个文件写到压缩包里
                    using (FileStream fileStream = new FileStream(packFilePath, FileMode.Open, FileAccess.Read)) {
                        s.WriteStream(fileStream);
                        fileStream.Close();
                    }
                }
                s.Finish();
                s.Close();
            }
        }

        /// <summary>
        /// 目录压缩
        /// </summary>
        /// <param name="compressedFilePath">压缩包文件输出路径</param>
        /// <param name="dirPath">目录路径</param>
        /// <param name="includeDir">是否包含传入的目录一起压缩,false则只压缩目录里的内容</param>
        /// <param name="appendCompressed">是否追加压缩,需要压缩包文件已经存在</param>
        /// <returns></returns>
        /// <exception cref="Exception">要压缩的文件不存在</exception>
        /// <exception cref="Exception">压缩异常</exception>
        public static void CommpressDirByFiles(string compressedFilePath,string[] filePaths, bool includeDir, bool appendCompressed) {
            //如果要追加,但是压缩包文件不存在,就改成不追加(新增)
            if (appendCompressed && !File.Exists(compressedFilePath)) appendCompressed = false;
            if (filePaths.Count() <= 0) throw new Exception("压缩文件不能为空！");
            //获取要打包的文件列表
            string[] packFiles = filePaths;
            string compressedDirPath = Path.GetDirectoryName(compressedFilePath);
            if (!Directory.Exists(compressedDirPath)) Directory.CreateDirectory(compressedDirPath);
            using (ZipOutputStream s = new ZipOutputStream(new FileStream(compressedFilePath, appendCompressed ? FileMode.Append : FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                s.SetLevel(9);
                //循环所有文件,写到压缩包里
                foreach (string packFilePath in packFiles) {
                    //在压缩包里的路径
                    string inPackPath = Path.GetFileName(packFilePath);
                    //添加这个文件
                    s.PutNextEntry(new ZipEntry(inPackPath));
                    //将这个文件写到压缩包里
                    using (FileStream fileStream = new FileStream(packFilePath, FileMode.Open, FileAccess.Read)) {
                        s.WriteStream(fileStream);
                        fileStream.Close();
                    }
                }
                s.Finish();
                s.Close();
            }
        }
    }
}
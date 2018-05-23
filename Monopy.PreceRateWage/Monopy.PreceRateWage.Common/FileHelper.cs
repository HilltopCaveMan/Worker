/*说明
 * 作   者:黄昊
 * 文件名:FileHelper.cs
 * 功   能:一个简单的文件(文件夹)操作类(BS/CS通用).
 * 类    1:FileHelper(文件)
 * 类    2:DirectoryHelper(文件夹)
 * 版   本:V 1.0
 * 框   架:.NET 2.0
 */

using System;
using System.IO;

namespace Monopy.PreceRateWage.Common
{
    #region 操作文件

    /// <summary>
    /// FileHelper(文件)
    /// </summary>
    public sealed class FileHelper
    {
        /// <summary>
        /// 删除文件(返回bool判断是否成功删除文件)
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns>返回bool判断是否成功删除文件</returns>
        public static bool Delete(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
            {
                File.SetAttributes(FileFullPath, FileAttributes.Normal);
                File.Delete(FileFullPath);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 复制文件(目标文件存在会强制替代)
        /// </summary>
        /// <param name="SourceFileFullPath">源文件</param>
        /// <param name="DestFileFullPath">目标文件</param>
        /// <returns></returns>
        public static bool Copy(string SourceFileFullPath, string DestFileFullPath)
        {
            if (File.Exists(SourceFileFullPath))
            {
                File.Copy(SourceFileFullPath, DestFileFullPath, true);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 取文件名包含扩展名(过滤路径)
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns>返回文件名(含扩展名)</returns>
        public static string GetName(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
                return new FileInfo(FileFullPath).Name;
            else
                return null;
        }

        /// <summary>
        /// 取文件名,根据参数返回是否包含扩展名
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <param name="IsIncludeExtension">是否包含扩展名</param>
        /// <returns>返回文件名(根据参数选择扩展名)</returns>
        public static string GetName(string FileFullPath, bool IsIncludeExtension)
        {
            if (File.Exists(FileFullPath))
            {
                FileInfo F = new FileInfo(FileFullPath);
                if (IsIncludeExtension)
                    return F.Name;
                else
                    return F.Name.Replace(F.Extension, "");
            }
            return null;
        }

        /// <summary>
        /// 取扩展名
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns>返回文件扩展名</returns>
        public static string GetExtension(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
                return new FileInfo(FileFullPath).Extension;
            else
                return null;
        }

        /// <summary>
        /// 调研系统默认打开程序打开文件
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns></returns>
        public static bool Open(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
            {
                System.Diagnostics.Process.Start(FileFullPath);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 得到文件大小(用GB,MB,KB表示)
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns></returns>
        public static string GetSize(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
            {
                long FL = new FileInfo(FileFullPath).Length;
                if (FL > 1024 * 1024 * 1024)
                    return Convert.ToString(Math.Round((FL + 0.00) / (1024 * 1024 * 1024), 2)) + " GB";
                else if (FL > 1024 * 1024)
                    return Convert.ToString(Math.Round((FL + 0.00) / (1024 * 1024), 2)) + " MB";
                else
                    return Convert.ToString(Math.Round((FL + 0.00) / (1024), 2)) + " KB";
            }
            else
                return null;
        }

        /// <summary>
        /// FileToByte流
        /// </summary>
        /// <param name="FileFullPath">文件全路径</param>
        /// <returns></returns>
        public static byte[] FileToStreamByte(string FileFullPath)
        {
            byte[] Date = null;
            if (File.Exists(FileFullPath))
            {
                FileStream FS = new FileStream(FileFullPath, FileMode.Open);
                Date = new byte[FS.Length];
                FS.Read(Date, 0, Date.Length);
                FS.Close();
                return Date;
            }
            else
                return null;
        }

        /// <summary>
        /// Byte流ToFile
        /// </summary>
        /// <param name="CreateFileFullPath">生成文件的全路径</param>
        /// <param name="StreamByte">Byte流</param>
        /// <returns></returns>
        public static bool StreamByteToFile(string CreateFileFullPath, byte[] StreamByte)
        {
            try
            {
                if (File.Exists(CreateFileFullPath))
                    Delete(CreateFileFullPath);
                FileStream FS = File.Create(CreateFileFullPath);
                FS.Write(StreamByte, 0, StreamByte.Length);
                FS.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    #endregion 操作文件

    #region 操作文件夹

    /// <summary>
    /// DirectoryHelper(文件夹)
    /// </summary>
    public sealed class DirectoryHelper
    {
        public enum DirectoryOption
        {
            /// <summary>
            /// 存在删除再创建
            /// </summary>
            ExistDelete,

            /// <summary>
            /// 存在直接返回
            /// </summary>
            ExistDoNothing
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="DirOperateOption">选项</param>
        /// <returns></returns>
        public static bool CreateDir(string DirFullPath, DirectoryOption Option)
        {
            try
            {
                if (Directory.Exists(DirFullPath))
                {
                    if (Option == DirectoryOption.ExistDelete)
                    {
                        Directory.Delete(DirFullPath, true);
                    }
                    else
                    {
                        return false;
                    }
                }
                Directory.CreateDirectory(DirFullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <returns></returns>
        public static bool DeleteDir(string DirFullPath)
        {
            try
            {
                if (Directory.Exists(DirFullPath))
                {
                    Directory.Delete(DirFullPath, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件夹内的文件数组（文件不存在数组为null）
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <returns></returns>
        public static string[] GetDirFiles(string DirFullPath)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, "*.*", SearchOption.TopDirectoryOnly);
            }
            return FileList;
        }

        /// <summary>
        /// 获取文件夹内的文件数组（文件不存在数组为null）
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SO">是否包含子目录</param>
        /// <returns></returns>
        public static string[] GetDirFiles(string DirFullPath, SearchOption SO)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, "*.*", SO);
            }
            return FileList;
        }

        /// <summary>
        /// 获取文件夹内的文件数组（文件不存在数组为null）
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SearchPattern">文件扩展名例如(*.txt)</param>
        /// <returns></returns>
        public static string[] GetDirFiles(string DirFullPath, string SearchPattern)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, SearchPattern);
            }
            return FileList;
        }

        /// <summary>
        /// 获取文件夹内的文件数组（文件不存在数组为null）
        /// </summary>
        /// <param name="DirFullPath">文件夹全路径</param>
        /// <param name="SearchPattern">文件扩展名例如(*.txt)</param>
        /// <param name="SO">是否包含子目录</param>
        /// <returns></returns>
        public static string[] GetDirFiles(string DirFullPath, string SearchPattern, SearchOption SO)
        {
            string[] FileList = null;
            if (Directory.Exists(DirFullPath) == true)
            {
                FileList = Directory.GetFiles(DirFullPath, SearchPattern, SO);
            }
            return FileList;
        }
    }

    #endregion 操作文件夹
}
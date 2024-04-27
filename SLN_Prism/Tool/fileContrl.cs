using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SLN_Prism.Tool
{
    /*名称:文件控制
     *开发时间:2024.4.27
     *修改时间:2024.4.27    
     *说明:
     *1.读取所有文件方法 List<programmeFile> readAllFileMessage(string path)
     *2.创建文件(路径) createFile(string path)
     *3.删除文件(路径) deleteFile(string path)
     *4.复制文件(源文件路径,目标文件路径,若重命名是否覆盖) copyFile(string sourcePath,string destPath,bool overWrite)
     *5.移动文件(源文件路径,目标文件路径) moveToFile(string sourcePath, string destPath)
     *6.流文件写入(行数组,文件路径) fileWrite(string[] lines,string path)
     *7.流文件读取(文件路径) string[] readAllFile(string path)
     *8.创建文件夹(路径) createDirectory(string path)
     *9.移动文件夹(源文件路径,目标文件路径) moveToDirectory(string sourcePath, string destPath)
     *10.删除文件夹(路径) deleteDirectory(string path)
     *11.文件夹存在判断(路径) bool existFileInfo(string path) 
     *12.流文件写入单行(行字符串,文件路径) fileLineWrite(string line, string path)
     */
  public  class fileContrl
    {
        private const double sizeUnit = 1024;//每1KB=1024字节
        private DirectoryInfo theFolder;//声明文件操作类   
        public fileContrl()
        {

        }
        /// <summary>
        /// 文件类型结构体
        /// </summary>
        public struct programmeFile
        {
            /// <summary>
            /// 文件名称
            /// </summary>
            public string fileName;
            /// <summary>
            /// 文件类型
            /// </summary>
            public string fileType;
            /// <summary>
            /// 文件大小
            /// </summary>
            public string fileSize;
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime createTime;
            /// <summary>
            /// 最近修改时间
            /// </summary>
            public DateTime lastModifiedTime;
            /// <summary>
            /// 文档信息
            /// </summary>
            public string fileMessage;

        }

        /// <summary>
        /// 读取所有文件方法
        /// </summary>
        /// <returns>返回泛型文件结构体</returns>
        public List<programmeFile> readAllFileMessage(string path)
        {
            theFolder = new DirectoryInfo(path);//定义需要操作的文件夹路径

            List<programmeFile> _structProgramme = new List<programmeFile>();//声明泛型文件结构体用于返回

            foreach (FileInfo newFile in theFolder.GetFiles()) //历遍当前路径所有文件夹
            {
                programmeFile newProgramme = new programmeFile();//声明一个文件类型结构体

                string[] _split = newFile.Name.Split('.');//将文件名称拆分

                newProgramme.fileName = _split[0]; //文件名称
                newProgramme.fileType = "." + _split[1]; //文件后缀
                newProgramme.fileSize = (Convert.ToDouble(newFile.Length) / sizeUnit).ToString("F3") + "KB"; //文件大小
                newProgramme.createTime = newFile.CreationTime; //文件创建时间
                newProgramme.lastModifiedTime = newFile.LastWriteTime; //文件最后修改时间

                _structProgramme.Add(newProgramme); //添加到泛型结构体重
            }

            return _structProgramme; //返回泛型结构体
        }


        /// <summary>
        /// 创建文件(路径)
        /// </summary>
        /// <param name="path">路径</param>
        public void createFile(string path)
        {
            FileStream fs;//声明一文件指针


            //path = path.Substring;

            if (!System.IO.File.Exists(path)) //判断是否存在文件
            {
                fs = System.IO.File.Create(path); //存在则生成文件并获取当前打开文件指针
                fs.Close();//关闭文件
            }
        }

        /// <summary>
        /// 删除文件(路径)
        /// </summary>
        /// <param name="path">路径</param>
        public void deleteFile(string path)
        {
            if (System.IO.File.Exists(path))//判断是否存在文件
                System.IO.File.Delete(path);//若存在则删除文件
        }

        /// <summary>
        /// 复制文件(源文件路径,目标文件路径,若重命名是否覆盖)
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">目标文件路径</param>
        /// <param name="overWrite">若重命名是否覆盖</param>
        public void copyFile(string sourcePath, string destPath, bool overWrite)
        {


            string destFileName = destPath.Substring(destPath.LastIndexOf("\\") + 1, destPath.Length - destPath.LastIndexOf("\\") - 1);//目标路径
            string destPathName = destPath.Substring(0, destPath.LastIndexOf("\\"));//目标路径
            theFolder = new DirectoryInfo(destPathName);//定义文件夹控制路径
            FileInfo file = new FileInfo(sourcePath);//创建文件控制类
            if (file.Exists)
            {
                if (overWrite == false) //若不能允许覆盖写入则历遍当前文件查看是否有重复
                {
                    foreach (FileInfo newFile in theFolder.GetFiles())//历遍当前文件
                    {
                        if (newFile.Name == destFileName)
                        {
                            throw new Exception("当前已有此文件存在!");

                        }
                    }
                }

                file.CopyTo(destPath, overWrite);//复制文件

            }
            else
            {
                throw new Exception("当前不存在源文件!");
            }
        }


        /// <summary>
        /// 移动文件(源文件路径,目标文件路径)
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">目标文件路径</param>
        public void moveToFile(string sourcePath, string destPath)
        {

            string destFileName = destPath.Substring(destPath.LastIndexOf("\\") + 1, destPath.Length - destPath.LastIndexOf("\\") - 1);//目标路径
            string destFile = destPath.Substring(0, destPath.LastIndexOf("\\"));
            //string destFile = destPath;

            theFolder = new DirectoryInfo(destFile);//定义文件夹控制路径

            FileInfo file = new FileInfo(sourcePath);//创建文件控制类
            if (file.Exists) //若文件存在
            {
                bool destExists = false;
                foreach (FileInfo newFile in theFolder.GetFiles())//历遍当前文件判断是否存在
                {
                    if (newFile.Name == destFileName)
                    {
                        destExists = true;
                    }
                }

                if (!destExists)
                    file.MoveTo(destPath);//复制文件
            }
            else
            {
                throw new Exception("当前不存在文件!");
            }
        }

        /// <summary>
        /// 流文件全部写入(行数组,文件路径,字符编码)
        /// </summary>
        /// <param name="lines">行数组</param>
        /// <param name="path">文件路径</param>
        public void FileAllWrite(string[] lines, string path, Encoding ed)
        {
            //System.IO.File.WriteAllLines(path, lines, Encoding.Unicode);

            FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, ed);

            foreach (string line in lines)
            {
                sw.WriteLine(line);
            }

            sw.Close();
            fs.Close();

        }

        /// <summary>
        /// 流文件全部写入(行数组,文件路径,字符编码)
        /// </summary>
        /// <param name="lines">行数组</param>
        /// <param name="path">文件路径</param>
        public void FileAllWrite(string data, string path, Encoding ed)
        {
            //System.IO.File.WriteAllLines(path, lines, Encoding.Unicode);

            FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, ed);
            sw.Write("");
            sw.Write(data);

            sw.Close();
            fs.Close();

        }


        /// <summary>
        /// 流文件全部读取(文件路径)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string[] ReadAllLinesFile(string path, Encoding encoding)
        {
            List<string> linesTolist = new List<string>();
            string[] lines = null;
            //try
            //{
            //    lines = System.IO.File.ReadAllLines(path, Encoding.UTF8);
            //}
            //catch
            //{
            //    lines=null;

            //}
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encoding);
            string tem = "";
            while (true)
            {
                tem = sr.ReadLine();
                if (tem == null) break;
                linesTolist.Add(tem);
            }

            sr.Close();
            fs.Close();

            lines = new string[linesTolist.Count];
            for (int i = 0; i < linesTolist.Count; i++)
            {
                lines[i] = linesTolist[i];
            }


            return lines;
        }

        /// <summary>
        /// 创建文件夹(路径)
        /// </summary>
        /// <param name="path">路径</param>
        public void createDirectory(string path)
        {
            DirectoryInfo fs;//声明一文件指针类
            if (!System.IO.Directory.Exists(path)) //判断是否存在文件
            {
                fs = System.IO.Directory.CreateDirectory(path); //存在则生成文件并获取当前打开文件指针

            }
        }


        /// <summary>
        /// 流文件全部读取(文件路径)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string ReadToEndFile(string path, Encoding encoding)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, encoding);


            string readEnd = sr.ReadToEnd();


            sr.Close();
            fs.Close();

            return readEnd;
        }




        /// <summary>
        /// 移动文件夹(源文件路径,目标文件路径)
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">目标文件路径</param>
        public void moveToDirectory(string sourcePath, string destPath)
        {
            theFolder = new DirectoryInfo(destPath);//定义文件夹控制路径

            string destFileName = destPath.Substring(destPath.LastIndexOf("\\") + 1, destPath.Length - destPath.LastIndexOf("\\") - 1);

            DirectoryInfo dir = new DirectoryInfo(sourcePath);//创建文件控制类
            if (dir.Exists) //若文件存在
            {
                bool destExists = false;
                foreach (DirectoryInfo newDir in theFolder.GetDirectories())//历遍当前文件判断是否存在
                {
                    if (newDir.Name == destFileName)
                    {
                        destExists = true;
                    }
                }

                if (!destExists)
                    dir.MoveTo(destPath);//复制文件
            }
            else
            {
                throw new Exception("当前不存在文件夹!");
            }
        }

        /// <summary>
        /// 删除文件夹(路径)
        /// </summary>
        /// <param name="path">路径</param>
        public void deleteDirectory(string path)
        {
            string thisPath = path;//获取文件完整路径
            theFolder = new DirectoryInfo(path);//定义需要操作的文件夹路径
            if (System.IO.Directory.Exists(thisPath))//判断是否存在文件
            {
                foreach (FileInfo file in theFolder.GetFiles())
                {
                    file.Delete();
                }

                System.IO.Directory.Delete(thisPath);//若存在则删除文件
            }

        }

        /// <summary>
        /// 初始化文件夹
        /// </summary>
        /// <param name="path"></param>
        public void InitDirectory(string path)
        {
            //如果存在文件夹
            if (existFileInfo(path))
            {
                //删除文件夹
                deleteDirectory(path);
            }

            //重新创建文件
            createDirectory(path);
        }

        /// <summary>
        /// 文件夹存在判断(路径)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public bool existFileInfo(string path)
        {
            FileInfo file = null;
            bool isExist = false;
            try
            {
                file = new FileInfo(path);//创建文件控制类
                isExist = file.Exists;
            }
            catch
            {
                isExist = false;
            }

            return isExist;
        }



        /// <summary>
        /// 流文件写入单行(行字符串,文件路径)
        /// </summary>
        /// <param name="line">行字符串</param>
        /// <param name="path">文件路径</param>
        public void fileLineWrite(string line, string path)
        {
            //System.IO.File.WriteAllLines(path, lines, Encoding.Unicode);

            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);

            //StreamWriter sw = System.IO.File.AppendText(path);

            sw.WriteLine(line);

            sw.Close();
            fs.Close();

        }
    }
}

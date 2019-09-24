using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.DirectoryServices;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.DirectoryServices.ActiveDirectory;
using System.Management;

/// <summary>
/// 更新日誌
/// ********************************************************
/// --------------------------------------------------------
/// 日期：2016-09-13
/// 作者：肖宏亮
/// 內容：1、更改服務器判斷文件
///       2、更改log格式
/// --------------------------------------------------------
/// 日期：2016-09-12
/// 作者：肖宏亮
/// 內容：1、將工程目標框架更改為.NET 2.0
///       2、刪除多餘Service.cs文件，代碼整合進Program.cs
///       3、為程式添加圖標
///       4、刪除獲取Domain代碼
///       5、寫入日誌，不拋出異常
/// -------------------------------------------------------- 
/// 日期：2016-11-04
/// 作者：蔣寒
/// 內容：1、修改獲取流覽器歷史記錄時間問題
///       2、增加運行后將filestr1字符串定義為空
///       3、增加運行后將history字符串定義為空
///日期：2016-11-23
///內容：1、修改部份代碼
///      2、修改歷史記錄時間，visited ->updateed
/// --------------------------------------------------------/// 
/// </summary>

namespace GetTree
{
    public partial class Program
    {
        #region 聲明定義變量

        string filepath = string.Empty;
        string filepath0 = string.Empty;
        string filepath1 = string.Empty;
        string filestr = string.Empty;
        string filestr1 = string.Empty;

        string localuser = string.Empty;
        string name = string.Empty;
        string processName = "ccSvcHst";
        static string s = string.Empty;
        static string times = string.Empty;
		
        string checkpath = "\\\\10.244.170.205\\TreeLog$\\GetTree.bmp";       
        //string checkpath = "d:\\ProgramData\\AgentData\\C3407241CD02.txt";//文件存放地址
        string[] pathstr = { "c:\\Users", "c:\\Documents and Settings", "d:\\", "e:\\", "f:\\" };
        #endregion

        #region Main函數
        public static void Main()
        {
            //判断是否有进程在执行，限定只能执行一个实例
            Process instance = RunningInstance();
            if (instance != null)
            {
                Thread.Sleep(1000);
                System.Environment.Exit(1);
            }
            Program s = new Program();
            while (s.CheckDo())
            {               
                s.sleepTime();                
                s.DoAll();                
            }                
        }
        #endregion

        #region 获取正在运行的实例，没有运行的实例返回null
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "//") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
        #endregion

        #region 定時執行任務  程序在10：00到11：00之間運行
        private void sleepTime()
        {
            DateTime  nowtime = DateTime.Now;
            if(nowtime.Hour>=11){
                Thread.Sleep((24 - nowtime.Hour+10) * 3600 * 1000);
            }
           else
            {
                Thread.Sleep((10-nowtime.Hour)*3600*1000);
             }            
        }
        #region DoAll
        private void DoAll()
        {
            filepath0 = "\\\\10.244.170.205\\TreeLog$\\";
            //filepath0 = "d:\\ProgramData\\AgentData\\";
            filepath1 = Environment.MachineName;
            filepath = filepath0 + filepath1 + ".txt";
            ScanFile();
            string baseinfo = GetBaseinfo();
            string ieinfo = GetIEHistory();
            string sharefolder = GetSharedFolders();
           
            string s =baseinfo+ieinfo+filestr1+sharefolder;
            WriteLog(s);
        }
        #endregion

        #region 判斷服務器文件是否存在，決定是否執行操作
        private Boolean CheckDo()
        {   

            if (File.Exists(checkpath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 獲取基本信息
        private String GetBaseinfo()
        {
            StringBuilder filestr0 = new StringBuilder();
            string ip = GetIP();
            string usb = GetUSB();
            string syma = GetSymantec();
            DateTime d = DateTime.Now;
            times = d.Hour.ToString() + ":" + d.Minute.ToString() + ":" + d.Second.ToString();
            filestr0.Append("HostName" + ":" + Environment.MachineName + "\r\n");
            filestr0.Append("UserName" + ":" + Environment.UserName + "\r\n");
            filestr0.Append("IPAddr" + ":" + ip + "\r\n");
            filestr0.Append("LogDate" + ":" + DateTime.Now.GetDateTimeFormats('D')[1].ToString() + "\r\n");
            filestr0.Append("USBState" + ":" + usb + "\r\n");
            filestr0.Append("SymantecState" + ":" + syma + "\r\n");
            filestr0.Append("RunTime" + ":" + times + "\r\n");
            return filestr0.ToString();
        }
        #endregion

        #region 获取IP
        private string GetIP()
        {
            string hostName = Dns.GetHostName();//本机名
            string ipname = "";
            try
            {
                System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;
                ipname =  addressList.GetValue(0).ToString();
            }
            catch (Exception)
            {
                //throw;
            }
            return ipname;
        }
        #endregion

        #region 獲取USB註冊表值
        private string GetUSB()
        {
            string usbvalue = "";
            try
            {
                string regvalue = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\USBSTOR").GetValue("Start").ToString();

                if (regvalue == "3")
                {
                    usbvalue = "Opened";
                }
                else if (regvalue == "4")
                {
                    usbvalue = "Closed";
                }
            }
            catch (Exception)
            {
                //throw;
            }
            return usbvalue;
        }
        #endregion

        #region 獲取Symantec狀態
        private String GetSymantec()
        {
            if (GetPidByProcessName(processName))
            {
                return "open";
            }
            else
            {
                return  "closed";
            }
        }
        #region 判斷進程是否運行
        public static Boolean GetPidByProcessName(string processname)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processname);
            foreach (Process p in arrayProcess)
            {

                return true;
            }
            return false;
        }
        #endregion
   
        #region 扫描文件
        private void ScanFile()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Directory.Exists(pathstr[i]))
                {
                    getAllFiles(pathstr[i]);
                }
            }
        }
        #endregion

        #region 获取指定的目录中的所有文件（包括文件夹）
        public void getAllFiles(string directory)
        {
            getFiles(directory);
            getDirectory(directory);
        }
        #endregion

        #region 获取指定的目录中的所有文件（不包括文件夹）
        public void getFiles(string directory)
        {
            try
            {
                string[] path = System.IO.Directory.GetFiles(directory);
                string g = "";
                for (int i = 0; i < path.Length; i++)
                {
                    string last = path[i];

                    if (last.EndsWith(".exe"))
                    {
                        int j = last.LastIndexOf("\\");
                        string k = last.Substring(j + 1);

                        g += "*" + k;
                    }
                }
                if (g != "")
                {
                    filestr1 += directory + g + "\r\n";
                }
            }
            catch (Exception)
            {
                //throw u;
            }
        }
        #endregion

        #region 获取指定的目录中的所有目录（文件夹）
        public void getDirectory(string directory)
        {
            try
            {
                string[] directorys = System.IO.Directory.GetDirectories(directory);
                if (directorys.Length <= 0) //如果该目录总没有其他文件夹
                    return;
                else
                {
                    for (int i = 0; i < directorys.Length; i++)//排除部份安全文件
                    {
                        if (directorys[i] == "c:\\Users\\All Users")
                        { continue; }
                        else
                        { getAllFiles(directorys[i]); }
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        #endregion
        #endregion

        #region  獲取共享資料夾
        private string GetSharedFolders()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select  *  from  win32_share");//察看共享目录  
            // 标准SQL的select+WMI的扩展=>只读的查询语言         
            string sharepath = "";
            foreach (ManagementObject share in searcher.Get())
            {
                try
                {                      
                    string name = share["Name"].ToString();
                    string path = share["Path"].ToString();
                    string sharepath0= path+">"+ name +"\r\n";
                    if (!sharepath0.Contains("$"))
                    {
                        sharepath += sharepath0;                      
                    }
                } 
                catch (Exception e)
                {
                    return null;
                }
            }
            return sharepath;
        }

        #endregion
        #region 写入日志
        private void WriteLog(string filestr)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                //filestr = filestr0 + filestr1;
                FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);//Create：创建一个新的文件。如果文件已存在，则删除旧文件，然后创建新文件。
                //Write：允许随后打开文件写入
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                //sw.Write(filestr);
                sw.Write(filestr);
                sw.Dispose();
                fs.Dispose();
                filestr1 ="";
            }
            catch (Exception)
            {
                //throw;
            }
        }
        #endregion
       
        #region GetHistory
        private String GetIEHistory()
        {
            IUrlHistoryStg2 vUrlHistoryStg2 = (IUrlHistoryStg2)new UrlHistory();
            IEnumSTATURL vEnumSTATURL = vUrlHistoryStg2.EnumUrls();
            STATURL vSTATURL;
            StringBuilder history = new StringBuilder();
            uint vFectched;

            while (vEnumSTATURL.Next(1, out vSTATURL, out vFectched) == 0)
            {

                FILETIME filetime;
                filetime.dwLowDateTime = vSTATURL.ftLastUpdated.dwLowDateTime; //ftLastUpdated
                filetime.dwHighDateTime = vSTATURL.ftLastUpdated.dwHighDateTime;
                DateTime urltime = DateTime.Parse(FILETIMEtoDataTime(filetime)).AddHours(8);               
                TimeSpan difftime = DateTime.Now - urltime;
                if (difftime.Days <1)
                {
                    string s = urltime.Year.ToString() + "-" + urltime.Month.ToString() + "-" + urltime.Day.ToString() + " " + urltime.Hour.ToString() + ":" + urltime.Minute.ToString() + ":" + urltime.Second.ToString();
                    history.Append(s + " " + vSTATURL.pwcsUrl + "\r\n");
                }
            }
            return history.ToString();
        }
        #endregion

        #region
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct INTERNET_CACHE_ENTRY_INFO
        {
            public int dwStructSize;
            public IntPtr lpszSourceUrlName;
            public IntPtr lpszLocalFileName;
            public int CacheEntryType;
            public int dwUseCount;
            public int dwHitRate;
            public int dwSizeLow;
            public int dwSizeHigh;
            public FILETIME LastModifiedTime;
            public FILETIME ExpireTime;
            public FILETIME LastAccessTime;
            public FILETIME LastSyncTime;
            public IntPtr lpHeaderInfo;
            public int dwHeaderInfoSize;
            public IntPtr lpszFileExtension;
            public int dwExemptDelta;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int FileTimeToSystemTime(
        IntPtr lpFileTime,
        IntPtr lpSystemTime);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindFirstUrlCacheEntry(
        [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlSearchPattern,
        IntPtr lpFirstCacheEntryInfo,
        ref int lpdwFirstCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool FindNextUrlCacheEntry(
        IntPtr hEnumHandle,
        IntPtr lpNextCacheEntryInfo,
        ref int lpdwNextCacheEntryInfoBufferSize);

        [DllImport("wininet.dll")]
        public static extern bool FindCloseUrlCache(
        IntPtr hEnumHandle);

        const int ERROR_NO_MORE_ITEMS = 259;
        #endregion

        #region FILETIMEtoDataTime

        private string FILETIMEtoDataTime(FILETIME time)
        {
            IntPtr filetime = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FILETIME)));
            IntPtr systime = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SYSTEMTIME)));
            Marshal.StructureToPtr(time, filetime, true);
            FileTimeToSystemTime(filetime, systime);
            SYSTEMTIME st = (SYSTEMTIME)Marshal.PtrToStructure(systime, typeof(SYSTEMTIME));
         
            string Time = st.wYear.ToString() + "-" + st.wMonth.ToString() + "-" + st.wDay.ToString() + " " + st.wHour.ToString() + ":" + st.wMinute.ToString() + ":" + st.wSecond.ToString();
            return Time;
        }
        #endregion
    }

    #region COM接口实现获取IE历史记录
    #region
    //自定义结构 IUrlHistory
    public struct STATURL
    {
        public static uint SIZEOF_STATURL =
            (uint)Marshal.SizeOf(typeof(STATURL));
        public uint cbSize;                    //网页大小
        [MarshalAs(UnmanagedType.LPWStr)]      //网页Url
        public string pwcsUrl;
        [MarshalAs(UnmanagedType.LPWStr)]      //网页标题
        public string pwcsTitle;
        public System.Runtime.InteropServices.ComTypes.FILETIME
            ftLastVisited,                     //网页最近访问时间
            ftLastUpdated,                     //网页最近更新时间
            ftExpires;
        public uint dwFlags;
    }
    #endregion
    //ComImport属性通过guid调用com组件
    [ComImport, Guid("3C374A42-BAE4-11CF-BF7D-00AA006946EE"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]

    interface IEnumSTATURL
    {
        [PreserveSig]
        //搜索IE历史记录匹配的搜索模式并复制到指定缓冲区
        uint Next(uint celt, out STATURL rgelt, out uint pceltFetched);
        void Skip(uint celt);
        void Reset();
        void Clone(out IEnumSTATURL ppenum);
        void SetFilter(
            [MarshalAs(UnmanagedType.LPWStr)] string poszFilter,
            uint dwFlags);
    }

    [ComImport, Guid("AFA0DC11-C313-11d0-831A-00C04FD5AE38"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
   
    #region IUrlHistoryStg methods 
    interface IUrlHistoryStg2
    {
        void AddUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
            [MarshalAs(UnmanagedType.LPWStr)] string pocsTitle,
            uint dwFlags);

        void DeleteUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
            uint dwFlags);

        void QueryUrl(
            [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
            uint dwFlags,
            ref STATURL lpSTATURL);

        void BindToObject(
            [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
            ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvOut);

        IEnumSTATURL EnumUrls();
       
        void AddUrlAndNotify(
            [MarshalAs(UnmanagedType.LPWStr)] string pocsUrl,
            [MarshalAs(UnmanagedType.LPWStr)] string pocsTitle,
            uint dwFlags,
            [MarshalAs(UnmanagedType.Bool)] bool fWriteHistory,
            [MarshalAs(UnmanagedType.IUnknown)] object    /*IOleCommandTarget*/
            poctNotify,
            [MarshalAs(UnmanagedType.IUnknown)] object punkISFolder);

        void ClearHistory();       //清除历史记录
    }
 #endregion

    [ComImport, Guid("3C374A40-BAE4-11CF-BF7D-00AA006946EE")]
    class UrlHistory /* : IUrlHistoryStg[2] */ { }
}
    #endregion
    #endregion
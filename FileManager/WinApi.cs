using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Reflection;


namespace FileManager
{

    class Manager
    {
        static void Main()
        {
            PhysicalDrive pd = new PhysicalDrive( @"\\.\PhysicalDrive0" );

            for (int i = 0; i < pd.Length; i++ )
            {
                Console.WriteLine(pd[i].ToString());
            }


            
            
            Console.ReadKey();
        }


    }

    static class WinApi
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
        [MarshalAs(UnmanagedType.LPWStr)] String filename,
        [MarshalAs(UnmanagedType.U4)] FileAccess access,
        [MarshalAs(UnmanagedType.U4)] FileShare share,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        IntPtr templateFile);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true) ]
        public static extern bool ReadFile(SafeFileHandle hFile, byte[] lpBuffer,
        uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped); 
      
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint SetFilePointer(
            SafeFileHandle hFile,        // дескриптор файла
            uint lDistanceToMove,        // байты перемещения указателя
            out int lpDistanceToMoveHigh,  // байты перемещения указателя
            uint dwMoveMethod);           // точка отсчета)

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetLogicalDriveStrings(uint bufferLength, [Out] char[] buffer);

        [DllImport("kernel32.dll")]
        public static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string lpRootPathName);

 
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        extern public static bool GetVolumeInformation(
        char[] RootPathName, StringBuilder VolumeNameBuffer,
        int VolumeNameSize, out uint VolumeSerialNumber,
        uint MaximumComponentLength, int FileSystemFlags,
        StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetVolumeNameForVolumeMountPoint(string
           lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName,
           uint cchBufferLength);

        public static string GetVolumeName(string MountPoint)
        {
            const int MaxVolumeNameLength = 100;
            StringBuilder sb = new StringBuilder(MaxVolumeNameLength);
            if (!GetVolumeNameForVolumeMountPoint(MountPoint, sb, (uint)MaxVolumeNameLength))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return sb.ToString();
        }


        public static uint FILE_CURRENT = 1;
        
         



    }

    class LARGE_INTEGER
    {
        private UInt64 Qpart;
        public int Hpart;//

        public UInt64 QuadPart
        {
            get { return Qpart; }
            set { Qpart = value; }
        }
  
        public uint LowPart
        {
            get { return (uint)(Qpart & 0x00000000FFFFFFFF); }
            set { Qpart |= (UInt64)value; }
        }
        
        
        public int HightPart
        {
            get { return (int)(Qpart  >> 32); }
            set 
            {  
                Qpart |= ((UInt64)value) << 32;
            }
        }

        public LARGE_INTEGER()
        {
            Qpart = 0;
        }        

        public LARGE_INTEGER(UInt64 value)
        {
            QuadPart = value; 
        }


    }//LARGE_INTEGER

   


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewProjectInstallationTool
{
    public class SymbolicLinkCreator
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public static void Create(string source, string target, SymbolicLink dwFlags)
        {
            if( !CreateSymbolicLink(source, target, dwFlags) )
            {
                //non-zero result
                int lastWin32Code = Marshal.GetLastWin32Error();
                uint lastErrorCode = GetLastError();

                if( lastErrorCode == 1314 )
                {
                    throw new Exception($"Failed to create symbolic link, Check if Application is running under Administrator!. error codes: win32 {lastWin32Code}, last {lastErrorCode}");
                }
                else
                {
                    throw new Exception($"Failed to create symbolic link, error codes: win32 {lastWin32Code}, last {lastErrorCode}");
                }
            }
        }
    }
}

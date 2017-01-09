using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace WPF_Windows_Spotlight.Models.FileSystem
{
    public class Everything
    {
        const int EVERYTHING_OK = 0;
        const int EVERYTHING_ERROR_MEMORY = 1;
        const int EVERYTHING_ERROR_IPC = 2;
        const int EVERYTHING_ERROR_REGISTERCLASSEX = 3;
        const int EVERYTHING_ERROR_CREATEWINDOW = 4;
        const int EVERYTHING_ERROR_CREATETHREAD = 5;
        const int EVERYTHING_ERROR_INVALIDINDEX = 6;
        const int EVERYTHING_ERROR_INVALIDCALL = 7;

        [DllImport("Everything32.dll", CharSet = CharSet.Unicode)]
        public static extern int Everything_SetSearchW(string lpSearchString);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetMatchPath(bool bEnable);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetMatchCase(bool bEnable);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetMatchWholeWord(bool bEnable);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetRegex(bool bEnable);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetMax(int dwMax);
        [DllImport("Everything32.dll")]
        public static extern void Everything_SetOffset(int dwOffset);

        [DllImport("Everything32.dll")]
        public static extern bool Everything_GetMatchPath();
        [DllImport("Everything32.dll")]
        public static extern bool Everything_GetMatchCase();
        [DllImport("Everything32.dll")]
        public static extern bool Everything_GetMatchWholeWord();
        [DllImport("Everything32.dll")]
        public static extern bool Everything_GetRegex();
        [DllImport("Everything32.dll")]
        public static extern UInt32 Everything_GetMax();
        [DllImport("Everything32.dll")]
        public static extern UInt32 Everything_GetOffset();
        [DllImport("Everything32.dll")]
        public static extern string Everything_GetSearchW();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetLastError();

        [DllImport("Everything32.dll")]
        public static extern bool Everything_QueryW(bool bWait);

        [DllImport("Everything32.dll")]
        public static extern void Everything_SortResultsByPath();

        [DllImport("Everything32.dll")]
        public static extern int Everything_GetNumFileResults();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetNumFolderResults();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetNumResults();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetTotFileResults();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetTotFolderResults();
        [DllImport("Everything32.dll")]
        public static extern int Everything_GetTotResults();
        [DllImport("Everything32.dll")]
        public static extern bool Everything_IsVolumeResult(int nIndex);
        [DllImport("Everything32.dll")]
        public static extern bool Everything_IsFolderResult(int nIndex);
        [DllImport("Everything32.dll")]
        public static extern bool Everything_IsFileResult(int nIndex);
        [DllImport("Everything32.dll", CharSet = CharSet.Unicode)]
        public static extern void Everything_GetResultFullPathNameW(int nIndex, StringBuilder lpString, int nMaxCount);
        [DllImport("Everything32.dll")]
        public static extern void Everything_Reset();

        public static List<string> Search(string keyword, int count = 10)
        {
            List<string> results = new List<string>();
            Everything_SetSearchW(keyword);
            Everything_SetMax(count);
            Everything_QueryW(true);
            //Everything_SortResultsByPath();

            const int bufsize = 260;
            StringBuilder buf = new StringBuilder(bufsize);
            for (var i = 0; i < Everything_GetNumResults(); i++)
            {
                Everything_GetResultFullPathNameW(i, buf, bufsize);
                var pathName = buf.ToString();
                results.Add(pathName);
            }

            return results;
        }
    }
}

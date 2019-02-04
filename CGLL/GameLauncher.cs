using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Game launcher class
    /// </summary>
    public static class GameLauncher
    {
        /// <summary>
        /// Inject plugin
        /// </summary>
        /// <param name="pluginPath">Plugin path</param>
        /// <param name="processHandle">Process handle</param>
        /// <param name="loadLibraryW">LoadLibraryW function pointer</param>
        private static void InjectPlugin(string pluginPath, IntPtr processHandle, IntPtr loadLibraryW)
        {
            if (File.Exists(pluginPath))
            {
                IntPtr ptr = Kernel32.VirtualAllocEx(processHandle, IntPtr.Zero, (uint)(pluginPath.Length + 1) * 2U, Kernel32.AllocationType.Reserve | Kernel32.AllocationType.Commit, Kernel32.MemoryProtection.ReadWrite);
                if (ptr != IntPtr.Zero)
                {
                    int nobw = 0;
                    byte[] p = Encoding.Unicode.GetBytes(pluginPath);
                    byte[] nt = Encoding.Unicode.GetBytes("\0");
                    if (Kernel32.WriteProcessMemory(processHandle, ptr, p, (uint)(p.Length), out nobw) && Kernel32.WriteProcessMemory(processHandle, new IntPtr(ptr.ToInt64() + p.LongLength), nt, (uint)(nt.Length), out nobw))
                    {
                        uint tid = 0U;
                        IntPtr rt = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0U, loadLibraryW, ptr, /* CREATE_SUSPENDED */ 0x4, out tid);
                        if (rt != IntPtr.Zero)
                        {
                            Kernel32.ResumeThread(rt);
                            unchecked
                            {
                                Kernel32.WaitForSingleObject(rt, (uint)(Timeout.Infinite));
                            }
                        }
                    }
                    Kernel32.VirtualFreeEx(processHandle, ptr, 0, Kernel32.AllocationType.Release);
                }
            }
        }

        /// <summary>
        /// Is game running
        /// </summary>
        private static bool IsGameRunning(string path)
        {
            Process[] processes = ((path == null) ? null : Process.GetProcessesByName(path));
            return ((processes == null) ? false : (processes.Length > 0));
        }

        /// <summary>
        /// aunh game
        /// </summary>
        /// <typeparam name="T">Session log user data type</typeparam>
        /// <param name="gameLaunchOptions">Game launch options</param>
        /// <param name="userData">User data</param>
        /// <returns>Game</returns>
        public static Game<T> LaunchGame<T>(GameLaunchOptionsDataContract gameLaunchOptions, T userData)
        {
            Game<T> ret = null;
            if (gameLaunchOptions != null)
            {
                IntPtr mh = Kernel32.GetModuleHandle("kernel32.dll");
                if (mh != IntPtr.Zero)
                {
                    IntPtr load_library_w = Kernel32.GetProcAddress(mh, "LoadLibraryW");
                    if (load_library_w != IntPtr.Zero)
                    {
                        Kernel32.PROCESS_INFORMATION process_info;
                        Kernel32.STARTUPINFO startup_info = new Kernel32.STARTUPINFO();
                        if (IsGameRunning(Path.GetFileNameWithoutExtension(gameLaunchOptions.GamePath)))
                        {
                            ResourcesState last_resource_state;
                            SessionLogDataContract<T> last_session_log_data;
                            if (gameLaunchOptions.CreateSessionLog)
                            {
                                last_resource_state = new ResourcesState(gameLaunchOptions.SessionLogResourcePaths);
                                last_session_log_data = new SessionLogDataContract<T>(DateTime.Now, TimeSpan.Zero, userData);
                            }
                            if (Kernel32.CreateProcess(gameLaunchOptions.GamePath, gameLaunchOptions.LaunchParameters, IntPtr.Zero, IntPtr.Zero, false, /* DETACHED_PROCESS */ 0x8 | /* CREATE_SUSPENDED */ 0x4, IntPtr.Zero, gameLaunchOptions.WorkingDirectory, ref startup_info, out process_info))
                            {
                                foreach (string plugin_path in gameLaunchOptions.Plugins)
                                {
                                    if (plugin_path != null)
                                    {
                                        InjectPlugin(plugin_path, process_info.hProcess, load_library_w);
                                    }
                                }
                                Kernel32.ResumeThread(process_info.hThread);
                                Kernel32.CloseHandle(process_info.hProcess);
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RGSS_Extractor
{
	internal static class Program
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool AttachConsole(int dwProcessId);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		[STAThread]
		private static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				IntPtr foregroundWindow = Program.GetForegroundWindow();
				int processId;
				Program.GetWindowThreadProcessId(foregroundWindow, out processId);
				Process processById = Process.GetProcessById(processId);
				if (processById.ProcessName == "cmd")
				{
					Program.AttachConsole(processById.Id);
				}
				else
				{
					Program.AllocConsole();
				}
				Main_Parser main_Parser = new Main_Parser();
				main_Parser.parse_file(args[0]);
				main_Parser.export_archive();
				Program.FreeConsole();
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}

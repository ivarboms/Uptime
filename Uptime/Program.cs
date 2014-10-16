using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Uptime
{
	// From http://stackoverflow.com/a/4423615
	public static class TimeSpanExtensions
	{
		public static string ToReadableAgeString(this TimeSpan span)
		{
			return string.Format("{0:0}", span.Days / 365.25);
		}

		public static string ToReadableString(this TimeSpan span)
		{
			string formatted = string.Format("{0}{1}{2}{3}",
				span.Days > 0 ? string.Format("{0:0} days, ", span.Days) : string.Empty,
				span.Hours > 0 ? string.Format("{0:0} hours, ", span.Hours) : string.Empty,
				span.Minutes > 0 ? string.Format("{0:0} minutes, ", span.Minutes) : string.Empty,
				span.Seconds > 0 ? string.Format("{0:0} seconds", span.Seconds) : string.Empty);

			if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

			return formatted;
		}
	}

	class Program
	{
		public static DateTime GetLastBootTime()
		{
			ManagementObject mo = new ManagementObject(@"\\.\root\cimv2:Win32_OperatingSystem=@");
			DateTime lastBootUp = ManagementDateTimeConverter.ToDateTime(mo["LastBootUpTime"].ToString());
			return lastBootUp;
		}

		static void Main(string[] args)
		{
			DateTime lastBoot = GetLastBootTime();
			
			while (true)
			{
				TimeSpan uptime = DateTime.Now - lastBoot;
				Console.Clear();
				Console.WriteLine("Boot time: {0:HH:mm:ss}", lastBoot);
				Console.WriteLine("Uptime: {0}", uptime.ToReadableString());

				Thread.Sleep(1000);
			}
		}
	}
}

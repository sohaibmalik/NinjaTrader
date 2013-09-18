using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace AlsiUtils
{
    public static class Utilities
    {
        public static List<string> GetMacAddress()
        {
        
            const int MIN_MAC_ADDR_LENGTH = 12;
            List<string> macAddress = new List<string>();
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.WriteLine(("Found MAC Address: " + nic.GetPhysicalAddress().ToString() + " Type: " + nic.NetworkInterfaceType));
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !String.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Debug.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress.Add(tempMac);
                }
            }
            return macAddress;
        }

        public static string GetUsername()
        {
            string user = SystemInformation.UserName.ToString();
            return user;

        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName,
                                                string pszSubIdList);



        public static string WrapWords(string actualText, int m_maxWidth)
        {
            const char space = (char)32;
            string[] words = actualText.Split(space);

            string ret = string.Empty;
            int lineCounter = 1;

            foreach (string word in words)
            {
                if (ret.Length + word.Length > m_maxWidth * lineCounter)
                {
                    ret = string.Concat(ret, "\n", word);
                    lineCounter++;
                }
                else
                {
                    ret = string.Concat(ret, space, word);
                }
            }

            return ret.Trim();
        }


        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public static void FindWindowAndShow(string WindowName)
        {
            IntPtr hwnd = FindWindow(null,WindowName);
           //see HOD-Utils for how to send messages
        }



				public static void PrintAllProperties(object obj)
				{
					Debug.WriteLine("===========Print new object====================");
					foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
					{
						string name = descriptor.Name;
						object value = descriptor.GetValue(obj);
						Debug.WriteLine("{0}={1}", name, value);
					}
					Debug.WriteLine("===============================================");
				}


				
        
    }
}

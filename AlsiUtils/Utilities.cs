using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AlsiUtils
{
    public static class Utilities
    {
        public static string GetMacAddress()
        {
           return "test";
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = "";
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.WriteLine(("Found MAC Address: " + nic.GetPhysicalAddress().ToString() + " Type: " + nic.NetworkInterfaceType));
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !String.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Debug.WriteLine("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
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

    }
}

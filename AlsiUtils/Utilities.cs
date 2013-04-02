using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AlsiUtils
{
    public static class Utilities
    {
        public static string GetMacAddress()
        {
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
    }
}

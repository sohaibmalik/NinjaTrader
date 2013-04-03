using System;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using AlsiUtils;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace NotifierClientApp
{
    public class Admin
    {
        AlsiTMDataContext dc;
        string mac = "";
        private bool _IsAdmin;
        private bool Loaded;
        public long UserID;
        double version;
        public Admin()
        {


            var versionstring = "100";

            version = double.Parse(versionstring);
          //  MessageBox.Show(versionstring);
            dc = new AlsiTMDataContext();
            mac = Utilities.GetMacAddress();
            UserID = dc.tblUsers.Where(z => z.USER_MACADRESS == mac).Select(z => z.ID).First();
            CreateNewUserIfNotExist();
            var user = dc.tblUsers.Where(z => z.USER_MACADRESS == mac).First();
            if (GetNewVersionNumber(user) > version)
            {
                var u = new UpdateForm("https://www.dropbox.com/s/rcnhl5eab7f2ywp/WebNotify.zip");
                u.ShowDialog();
            }

            var log = new tblLog
            {
                LOG_TS = DateTime.UtcNow.AddHours(2),
                LOG_USER_ID = user.ID,
                LOG_VERSION = version.ToString(),
            };
            dc.tblLogs.InsertOnSubmit(log);
            dc.SubmitChanges();
        }

        private double GetNewVersionNumber(tblUser user)
        {
            try
            {
                List<double> versions = new List<double>();

                foreach (var q in dc.tblLogs.Select(z => z.LOG_VERSION))
                {
                    double ver = 0;
                    double.TryParse(q, out ver);
                    versions.Add(ver);
                }
                return versions.Max();
            }
            catch { return 0; }
        }

        public bool IsAdmin
        {
            get
            {
                _IsAdmin = CheckISAdmin();
                return _IsAdmin;
            }

        }

        private bool CheckISAdmin()
        {
            if (Loaded) return _IsAdmin;
            else
                Loaded = true;
            dc = new AlsiTMDataContext();

            _IsAdmin = dc.tblUsers.Any(z => z.USER_MACADRESS == AlsiUtils.Utilities.GetMacAddress() && z.USER_ADMIN == true);
            return _IsAdmin;
        }

        private void CreateNewUserIfNotExist()
        {

            if (!dc.tblUsers.Any(z => z.USER_MACADRESS == mac))
            {
                var user = new tblUser
                {
                    USER_NAME = Utilities.GetUsername(),
                    USER_MACADRESS = mac,
                    USER_ADMIN = false,

                };
                dc.tblUsers.InsertOnSubmit(user);
                dc.SubmitChanges();
            }
        }

        public Table<tblUser> GetAllUsers()
        {
            dc = new AlsiTMDataContext();
            return dc.tblUsers;
        }

        public void ReportLiveStatus(bool Live)
        {
            var user = dc.tblUsers.Where(z => z.USER_MACADRESS == mac).First();
            user.USER_LIVE = Live;
            dc.SubmitChanges();
        }
    }
}

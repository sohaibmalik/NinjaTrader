using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiUtils;

namespace NotifierClientApp
{
    public partial class AdminForm : Form
    {
        Admin _admin;
       

        public AdminForm(Admin admin)
        {
            InitializeComponent();
            _admin = admin;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            Utilities.SetWindowTheme(userListView.Handle, "Explorer", null);
            PopulateListView();
             var tt = new ToolTip();
             tt.SetToolTip(userListView, "Double click to toggle admin");
        }


        private void PopulateListView()
        {
            userListView.Items.Clear();
            foreach (var u in _admin.GetAllUsers())
            {
                ListViewItem lvi = new ListViewItem(u.USER_NAME);
                lvi.SubItems.Add(u.USER_ADMIN.ToString());
                lvi.ImageIndex = (bool)u.USER_LIVE ? 1 : 0;
                lvi.Tag = u;
                userListView.Items.Add(lvi);
            }
        }

        private void userListView_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            
            if(userListView.SelectedItems.Count==0)return;
            var user = (tblUser)userListView.SelectedItems[0].Tag;
            var dc = new AlsiTMDataContext();
            var dbu = dc.tblUsers.Where(z => z.USER_MACADRESS == user.USER_MACADRESS).First();
            dbu.USER_ADMIN = !dbu.USER_ADMIN;
            dc.SubmitChanges();

            PopulateListView();
            Cursor = Cursors.Default;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            ResfreshStatus();
        }

        private void ResfreshStatus()
        {
            Cursor = Cursors.WaitCursor;
            PopulateListView();
            Cursor = Cursors.Default;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResfreshStatus();
        }



       



        }
}

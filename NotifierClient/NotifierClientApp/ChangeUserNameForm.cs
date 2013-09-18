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
    public partial class ChangeUserNameForm : Form
    {
        AlsiTMDataContext dc;
        Admin _admin;
        tblUser User;
        public ChangeUserNameForm(Admin admin)
        {
            InitializeComponent();
            dc = new AlsiTMDataContext();
            _admin = admin;
        }

        private void ChangeUserNameForm_Load(object sender, EventArgs e)
        {

           User = dc.tblUsers.Where(z => z.ID == _admin.UserID).First();
           usernameInputTextbox.Text = User.USER_NAME;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            User.USER_NAME = usernameInputTextbox.Text;
            dc.SubmitChanges();
            Close();
        }

        private void usernameInputTextbox_TextChanged(object sender, EventArgs e)
        {
            OKButton.Enabled = (usernameInputTextbox.TextLength != 0);
        }
    }
}

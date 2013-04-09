using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiUtils;

namespace NotifierClientApp
{
    public partial class Notify : Form
    {
        AlsiWebService.AlsiNotifyService service = new AlsiWebService.AlsiNotifyService();
        AlsiWebService.xlTradeOrder lastOrder;
        private bool _updateApp1;
        private DateTime _app1LastUpdate = DateTime.Now.AddDays(-1);
        private int _OrderUpdate, _StatusUpdate, _StatusDelay;
        private delegate void StatusFail(AlsiWebService.Boodskap boodskap);
        private StatusFail onStatusFail;
        private NotifyIcon tradeNotify = new NotifyIcon();
        private NotifyIcon newMsgNotify = new NotifyIcon();
        private Admin admin = new Admin();
        public Notify()
        {
            InitializeComponent();
            tradeNotify.BalloonTipClicked += new EventHandler(tradeNotify_BalloonTipClicked);
            newMsgNotify.BalloonTipClicked += new EventHandler(newMsgNotify_BalloonTipClicked);
        }

       



        private void updateFromWeb(AlsiWebService.xlTradeOrder order)
        {
            Debug.WriteLine("-------------------------------------------------------");
            Debug.WriteLine(order.Timestamp + "  " + order.Price + "  " + order.Status);
            Debug.WriteLine("-------------------------------------------------------");
            ListViewItem lvi = new ListViewItem(order.Timestamp.ToLongTimeString());
            lvi.Tag = order;
            lvi.SubItems.Add(order.Contract);
            lvi.SubItems.Add(order.BS.ToString());
            lvi.SubItems.Add(order.Volume.ToString());
            lvi.SubItems.Add(order.Price.ToString());
            lvi.SubItems.Add(order.Status.ToString());
            updateListView(lvi, order);
            ColorStatus();
        }

        private void updateListView(ListViewItem lvi, AlsiWebService.xlTradeOrder order)
        {

            var ind = ordersListView.Items.Count;
            if (ind == 0)
            {
                ordersListView.Items.Add(lvi);
                lastOrder = (AlsiWebService.xlTradeOrder)lvi.Tag;
                return;
            }

            //Status Update

            if (lastOrder.Price == order.Price
                && lastOrder.Volume == order.Volume
                && lastOrder.BS == order.BS
                && lastOrder.Status != order.Status
                 )
            {
                lastOrder = order;
                ordersListView.Items[ind - 1].Tag = order;
                ordersListView.Items[ind - 1].SubItems[5].Text = order.Status.ToString();
                return;
            }


            bool oldorder = (lastOrder.Price == order.Price
                 && lastOrder.Volume == order.Volume
                 && lastOrder.BS == order.BS
                 && lastOrder.Status == order.Status);

            //New Order
            if (!oldorder)
            {
                lastOrder = order;
                lvi.Tag = order;
                ordersListView.Items.Add(lvi);
            }



        }


        private DateTime _alertAcknowledged;
        private void ColorStatus()
        {
            foreach (ListViewItem i in ordersListView.Items)
            {
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Ready)
                {
                    var ordertime = ((AlsiWebService.xlTradeOrder)i.Tag).Timestamp;
                    i.BackColor = Color.DarkOrange;
                    if (_alertAcknowledged <= ordertime) balloonNotify(App.AlsiTrade,"Alsi Trade", "New Order!",tradeNotify);

                }
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Completed)
                {
                    var ordertime = ((AlsiWebService.xlTradeOrder)i.Tag).Timestamp;
                    i.BackColor = Color.LightGreen;
                    if (_alertAcknowledged <= ordertime) balloonNotify(App.AlsiTrade,"Alsi Trade", "Order Matched!", tradeNotify);

                }
            }

        }

        private void getAllorders()
        {
            try
            {
                var allOrders = service.getAllOrders();
                ordersListView.Items.Clear();
                foreach (var v in allOrders)
                    if (allOrders.Count() > 0)
                        foreach (var o in allOrders) updateFromWeb(o);

            }
            catch (Exception ex)
            {

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _StatusUpdate = Properties.Settings.Default.StatusUpdateInt;
            _OrderUpdate = Properties.Settings.Default.OrderUpdateInt;
            _StatusDelay = Properties.Settings.Default.StatusDelayInt;

            OrderUpdateToolStripTextBox.Text = _OrderUpdate.ToString();
            StatusUpdateToolStripTextBox.Text = _StatusUpdate.ToString();
            DelayUpdateToolStripTextBox.Text = _StatusDelay.ToString();

            CheckForIllegalCrossThreadCalls = false;
            OrderUpdateTimer.Enabled = true;
            OrderUpdateTimer.Interval = _OrderUpdate;
            OrderUpdateTimer.Start();

            StatusUpdateTimer.Enabled = true;
            StatusUpdateTimer.Interval = _StatusUpdate;
            StatusUpdateTimer.Start();
            statusLabel1.Text = "Trade Bot Online";

            SetAdminButtons(admin.IsAdmin);
            admin.ReportLiveStatus(true);
            AlsiUtils.WebSettings.GetSettings();

            orderUpdateIntervalMsToolStripMenuItem.Visible = admin.IsAdmin;
            statusUpdateDelaySecToolStripMenuItem.Visible = admin.IsAdmin;
            statusUpdateIntervalMsToolStripMenuItem.Visible = admin.IsAdmin;

            LoadChat();
        }

        private void SetAdminButtons(bool vis)
        {
            adminToolStripMenuItem.Visible = vis;
            clearHistoryToolStripMenuItem.Enabled = vis;

        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateBW.RunWorkerAsync();
            }
            catch (Exception ee)
            { };
        }

        private void updateBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var o = service.getLastOrder();
                if (o != null) updateFromWeb(o);

            }
            catch (Exception ex)
            {

            }
        }

        private int alsiTradeFailedCount = 0;

        void statusFailed(App app, string Mesg)
        {

            if (app == App.AlsiTrade)
            {
                alsiTradeFailedCount++;
                Debug.WriteLine(app + " Failed  " + alsiTradeFailedCount);
            }

            if (alsiTradeFailedCount > 10 && admin.IsAdmin) balloonNotify(App.AlsiTrade,"Alsi Trade", "Failed to update !", tradeNotify);


        }



        private void balloonNotify(App app,string Title, string Msg,NotifyIcon ni)
        {
                             
            if (!admin.IsAdmin && Msg.Contains("Matched")) return;
            // (new SoundPlayer(Properties.Resources.alert3)).Play();
            ni.Visible = true;
            ni.Icon = Properties.Resources.alert;
            ni.ShowBalloonTip(1000, Title, Msg, ToolTipIcon.Info);

        }

       

        void newMsgNotify_BalloonTipClicked(object sender, EventArgs e)
        {
            newMsgNotify.Visible = false;
        }

        void tradeNotify_BalloonTipClicked(object sender, EventArgs e)
        {
            _alertAcknowledged = DateTime.UtcNow.AddHours(2);
            alsiTradeFailedCount = 0;
            tradeNotify.Visible = false; 
        }


        private void getAllOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderUpdateTimer.Stop();
            getAllorders();
            OrderUpdateTimer.Start();
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            service.clearLists();
            ordersListView.Items.Clear();
        }

        enum App
        {
            AlsiTrade = 1,
            NewMessage = 2,

        }

        private void app1Statustimer_Tick(object sender, EventArgs e)
        {
            try
            {
                appStatusBW.RunWorkerAsync();
                AppUpdateColors();
            }
            catch (Exception ex)
            {

            }
        }

        private void AppUpdateColors()
        {
            if (!_updateApp1)
            {
                statusFailed(App.AlsiTrade, "Failed");
                statusLabel1.BackColor = Color.Red;
            }
            else
            {
                statusLabel1.BackColor = Color.LightGreen;
                alsiTradeFailedCount = 0;
            }

        }



        private void appStatusBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                getAppUpdate();
                UpdateChat(false);
                ordersListView.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                ordersListView.BackColor = Color.Red;
                _updateApp1 = false;
                service = new AlsiWebService.AlsiNotifyService();
            }
        }

        private void getAppUpdate()
        {
            var n = DateTime.UtcNow.AddHours(2);
            var b = service.getLastMessage();
            if (b.Message == AlsiWebService.Messages.isAlive) _app1LastUpdate = b.TimeStamp;
            Debug.WriteLine("Appupdate " + b.TimeStamp + "  " + b.Message);
            var check1 = _app1LastUpdate.AddSeconds(Properties.Settings.Default.StatusDelayInt);

            if (check1 > n)
                _updateApp1 = true;
            else
                _updateApp1 = false;

        }

        private void OrderUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(OrderUpdateToolStripTextBox.Text, out _OrderUpdate);
            Properties.Settings.Default.OrderUpdateInt = _OrderUpdate;
            OrderUpdateTimer.Interval = _OrderUpdate;
            Properties.Settings.Default.Save();
        }

        private void StatusUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(StatusUpdateToolStripTextBox.Text, out _StatusUpdate);
            Properties.Settings.Default.StatusUpdateInt = _StatusUpdate;
            StatusUpdateTimer.Interval = _StatusUpdate;
            Properties.Settings.Default.Save();
        }

        private void DelayUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(DelayUpdateToolStripTextBox.Text, out _StatusDelay);
            Properties.Settings.Default.StatusDelayInt = _StatusDelay;
            Properties.Settings.Default.Save();

        }

        private void tradeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!Communicator.Internet.CheckConnection())
            {
                MessageBox.Show("No Internet Connection", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TradeLogForm ts = new TradeLogForm(admin);
            ts.Show();
            Cursor = Cursors.Default;
        }



        private void GetAlsiPrices()
        {
            Cursor = Cursors.WaitCursor;
            if (!Communicator.Internet.CheckConnection())
            {
                MessageBox.Show("No Internet Connection", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var alsi = AlsiTrade_Backend.HiSat.HistData.GetHistoricalTICK_FromWEB(DateTime.Now, DateTime.Now, AlsiUtils.WebSettings.General.HISAT_INST);
            if (alsi.Count == 0)
            {
                pricesStatusLabel.Text = "Market Closed";
                return;
            }
            pricesStatusLabel.Text = alsi.Last().Close.ToString();
            Cursor = Cursors.Default;
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AdminForm(admin);
            a.Show();
        }

        private void Notify_FormClosing(object sender, FormClosingEventArgs e)
        {
            admin.ReportLiveStatus(false);

        }

        private void pricesStatusLabel_Click(object sender, EventArgs e)
        {
            GetAlsiPrices();
        }

        private void changeUserNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var b = new ChangeUserNameForm(admin);
            b.ShowDialog();
            admin = new Admin();
            PopulateUserListView();

        }

        #region CHAT
        private Chat t;
        private List<Chat> ChatList;
        private List<Chat> ReadList = new List<Chat>();
        private List<int> SendToList = new List<int>();
        private BackgroundWorker userUpdateBW = new BackgroundWorker();
        private tblUser _SelectedUser;

        public tblUser SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                UpdateChat(true);
            }
        }

        private void LoadChat()
        {
            t = new Chat();
            t.NewMessage += new Chat.OnNewMessage(t_NewMessage);
            Utilities.SetWindowTheme(userListView.Handle, "Explorer", null);
            Utilities.SetWindowTheme(chatHistoryListView.Handle, "Explorer", null);
            chatSendButton.BackgroundImage = Properties.Resources.Messages_DisAbled_icon;
            chatSendButton.Enabled = false;
            userUpdateBW.DoWork += new DoWorkEventHandler(userUpdateBW_DoWork);
            ToolTip userInfoTootltip = new ToolTip();
            userInfoTootltip.SetToolTip(userListView, "Check to select users to send message to.\nHighlight a user to view chat.");
            PopulateUserListView();
            Text = Text + "  Logged in as " + admin.UserList.Where(x => x.ID == admin.UserID).First().USER_NAME;

            try
            {
                userListView.Items[0].Selected = true;
            }
            catch { }

        }

        void t_NewMessage(object sender, Chat.NewMessageEventArgs e)
        {
            if (_SelectedUser.ID != e.FromUserID)
            {
                var msg = AlsiUtils.Utilities.WrapWords(e.Message, 30);
                balloonNotify(App.NewMessage, e.FromUser + " :", msg, newMsgNotify);
            }
        }



        void userUpdateBW_DoWork(object sender, DoWorkEventArgs e)
        {
            RefreshUsers();
        }

        private void userStatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            userUpdateBW.RunWorkerAsync();

        }

        public void RefreshUsers()
        {
            var updated = admin.GetAllUsers();
            foreach (ListViewItem lvi in userListView.Items)
            {
                var user = (tblUser)lvi.Tag;
                user.USER_LIVE = updated.Where(z => z.ID == user.ID).Select(x => x.USER_LIVE).First();
                lvi.ImageIndex = (bool)user.USER_LIVE ? 1 : 0;
                //var cl = ChatList.Where(z => z.FromUserID == user.ID);
                //var cll = cl.Any(z => z.Viewed == false);
                //if (cll) lvi.BackColor = Color.Yellow;
            }
            userListView.Refresh();
        }

        private void PopulateUserListView()
        {
            userListView.BeginUpdate();
            userListView.Items.Clear();
            foreach (var u in admin.GetAllUsers().Where(z => z.ID != admin.UserID))
            {
                ListViewItem lvi = new ListViewItem(u.USER_NAME);
                lvi.SubItems.Add(u.USER_ADMIN.ToString());
                lvi.ImageIndex = (bool)u.USER_LIVE ? 1 : 0;
                lvi.Tag = u;
                userListView.Items.Add(lvi);
            }
            userListView.EndUpdate();
        }

        private int TryUpdate = 0;
        private void UpdateChat(bool switched)
        {
            if (switched) chatHistoryListView.Items.Clear();
            if (TryUpdate == 3) return;
            try
            {

                ChatList = t.GetChatMessages(admin.UserID, SelectedUser.ID);
                ReadList.Clear();

                if (chatHistoryListView.Items.Count != 0)
                    foreach (ListViewItem r in chatHistoryListView.Items) ReadList.Add((Chat)r.Tag);

                //compare lists
                var temp = ReadList.ToLookup(x => x.MessageID);
                var newList = ChatList.Where(x => (!temp.Contains(x.MessageID)));
                foreach (var N in newList)
                {
                    ListViewItem lvi = new ListViewItem(admin.UserList.Where(x => x.ID == N.FromUserID).First().USER_NAME + " :    " + N.Message);
                    lvi.Tag = N;
                    chatHistoryListView.Items.Add(lvi);
                }


                TryUpdate = 0;
            }
            catch
            {
                TryUpdate++;
                UpdateChat(false);
            }


        }


        private void DisplayUserInfo(tblUser user)
        {
            nameSelectedLabel.Text = "Send :";

        }


        private void userListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var user = (tblUser)e.Item.Tag;
            if (e.Item.Checked) SendToList.Add(user.ID);
            else
                SendToList.Remove(user.ID);

            VerifyInput();
            DisplayUserInfo(user);
        }


        private void chatInputTextBox_TextChanged(object sender, EventArgs e)
        {
            VerifyInput();
        }

        private void VerifyInput()
        {
            var x = (chatInputTextBox.TextLength != 0 && SendToList.Count != 0);
            chatSendButton.Enabled = (x);
            chatSendButton.BackgroundImage = x ? Properties.Resources.Messages_icon : Properties.Resources.Messages_DisAbled_icon;
        }

        private void chatSendButton_Click(object sender, EventArgs e)
        {
            var msg = new StringBuilder();
            msg.Append(chatInputTextBox.Text);

            int index = 0;
            DateTime time = new DateTime();
            foreach (var tu in SendToList)
            {
                var c = new Chat();
                c.FromUserID = admin.UserID;
                c.ToUserID = tu;
                if (index == 0) time = service.GetChatTime();
                c.Time = time;
                c.Message = msg.ToString();
                if (index == 0) t.InsertChatMessage(c);
                t.InsertChatUserMessage(c);
                index++;
            }

            chatInputTextBox.Clear();

        }

        private ToolTip MsgTooltip = new ToolTip();
        private void chatHistoryListView_MouseHover(object sender, EventArgs e)
        {
            //Point localPoint = chatHistoryListView.PointToClient(Cursor.Position);
            //ListViewItem x = chatHistoryListView.GetItemAt(localPoint.X, localPoint.Y);
            //if (x == null) return;
            //var msg = (Chat)x.Tag;
            ////MsgTooltip.SetToolTip(userListView, msg.Message);
            //MsgTooltip.Show(msg.Message, userListView, 5000);
        }

        private void chatHistoryListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var msg = (Chat)e.Item.Tag;
            // if (!msg.Viewed && msg.ToUserID == admin.UserID) admin.SetDocViewed(true, msg);
            //  e.Item.ForeColor = Color.Black;            
            var MSG = new StringBuilder(admin.UserList.Where(z => z.ID == msg.FromUserID).First().USER_NAME + ":\n");
            MSG.Append(msg.Time.ToShortTimeString() + "\n");
            MSG.Append(msg.Message);
            MsgTooltip.Show(Utilities.WrapWords(MSG.ToString(), 100), userListView, 3000);

        }


        private void userListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (userListView.SelectedItems.Count == 0) return;
            Cursor = Cursors.WaitCursor;
            SelectedUser = (tblUser)userListView.SelectedItems[0].Tag;
            foreach (ListViewItem lvi in userListView.Items) lvi.Checked = false;
            Cursor = Cursors.Default;
        }



        #endregion




        private void SendTEST()
        {
            bool Matched = true;
            var t = new AlsiUtils.Trade()
            {
                TimeStamp = DateTime.Now,
                BuyorSell = Trade.BuySell.Buy,
                InstrumentName = "Test",
                TradedPrice = 13333,
                TradeVolume = 1,

            };


            var o = new AlsiWebService.xlTradeOrder();
            if (t.BuyorSell == Trade.BuySell.Buy) o.BS = AlsiWebService.BuySell.Buy;
            if (t.BuyorSell == Trade.BuySell.Sell) o.BS = AlsiWebService.BuySell.Sell;
            o.Timestamp = t.TimeStamp;
            o.Volume = t.TradeVolume;
            o.Price = t.TradedPrice;
            o.Contract = t.InstrumentName;
            o.Status = Matched ? AlsiWebService.orderStatus.Completed : AlsiWebService.orderStatus.Ready;
            service.InsertNewOrder(o);
            var s = new AlsiWebService.AlsiNotifyService();
            s.InsertNewOrder(o);

            if (t.Reason == Trade.Trigger.CloseLong || t.Reason == Trade.Trigger.CloseLong) WebSettings.General.MANUAL_CLOSE_TRIGGER = true;
        }










    }





}

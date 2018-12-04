using BruteForce.Sistem.Models;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BruteForce.Sistem
{
    public partial class frmBilgiler : KryptonForm
    {
        
        public frmBilgiler()
        {
            InitializeComponent();
        }
        private BruteForceNavigate model = new BruteForceNavigate();
        private List<BruteForceUsers> users { get; set; } = null;
        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            frmAna.usersList = users;
            frmAna.navigate = model;
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            OpenFileDialog ac = new OpenFileDialog();
            ac.Filter = "Metin Belgesi |*.txt";
            //ac.ShowDialog();
            if (ac.ShowDialog()==DialogResult.OK)
            {
                path = ac.FileName;
                usernamTxt.Text = path;
            }
            if(!string.IsNullOrEmpty(path))
            {
                string text =
                    File.ReadAllText(path, Encoding.UTF8);
                text = text.Replace("\r\n", "#");
                foreach (string item in text.Split('#'))
                {
                    if (users == null) users = new List<BruteForceUsers>();
                    users.Add(new BruteForceUsers() { User = item.ToString() });
                }
            }
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            OpenFileDialog ac = new OpenFileDialog();
            ac.Filter = "Metin Belgesi |*.txt";
            //ac.ShowDialog();
            if (ac.ShowDialog() == DialogResult.OK)
            {
                path = ac.FileName;
                sifreTxt.Text = path;
            }
            if (!string.IsNullOrEmpty(path))
            {
                string text =
                    File.ReadAllText(path, Encoding.UTF8);
                text = text.Replace("\r\n", "#");
                foreach (string item in text.Split('#'))
                {
                    if (users == null) users = new List<BruteForceUsers>();
                    var add = users.Where(sa => sa.Password == null).First();
                    users.Remove(add);
                    add.Password = item;
                    users.Add(add);
                }
            }
            var res = users;
        }

        private void urlTxt_TextChanged(object sender, EventArgs e)
        {
            model.Url = urlTxt.Text;
        }

        private void kryptonRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (kryptonRadioButton1.Checked)
            {
                model.useUserAgent = true;
            }
        }

        private void kryptonRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (kryptonRadioButton2.Checked)
            {
                model.useUserAgent = false;
            }
        }
    }
}

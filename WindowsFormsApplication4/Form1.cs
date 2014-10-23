using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security;


namespace KishanKotadiya
{
    public partial class Form1 : Form
    {
       
        Class1 ob = new Class1();
        String ip;
        public delegate void ShowMessage(string message);
        public ShowMessage myDelegate;

        Int32 port;
        UdpClient udpClient;
        Thread thread;
        String status;
        static int count =0;
        String sourceIP;
        
        private void ReceiveMessage()
        {
            try
            {
                udpClient = new UdpClient(port);
            }
            catch(Exception)
            {
                MessageBox.Show("Port is already in use please restart the application and use any unsued port for communication.");
               
            }
            while (true)
            {
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] content = udpClient.Receive(ref remoteIPEndPoint);
                sourceIP =  remoteIPEndPoint.Address.ToString();
                
                    string message = Encoding.ASCII.GetString(content);
                    Console.WriteLine(message);
                    this.Invoke(myDelegate, new object[] { message });
                   
            }
        }


        private void ShowMessageMethod(string message)
        {
           
                count++;
                richTextBox1.AppendText(sourceIP + ":" + " " + message + "\n");
                richTextBox1.ScrollToCaret();
                label2.Text = "Connetcted to " + sourceIP + " on port " + port + ".";
                if (count != 0)
                {
                    panel2.Visible = false;
                    txtmsg.Enabled = true;
                    picoffline.Visible = false;
                    richTextBox1.Enabled = true;
                    timer1.Start();
                    txtmsg.Focus();
                    piconline.Visible = true;
                    txtip.Text = sourceIP;
                    ip = sourceIP;
                    
                    label2.Visible = true;
                }
            
        }
        public Form1()
        {
            InitializeComponent();
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            
        }
        private void checkbtn_Click(object sender, EventArgs e)
        {
            ip = txtip.Text;
            online();
            
           
               
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            online();

        }

        public void online()
        {


            status = ob.ping(ip);
            if (status == "Success")
            {
                piconline.Visible = true;
                
                txtmsg.Enabled = true;
                picoffline.Visible = false;
                richTextBox1.Enabled = true;
                timer1.Start();
                label2.Text = "";
                if(count==0)
                txtmsg.Focus();
            }
            else
            {
                label2.Text = "Disconnected.";
                label2.Visible = true;
                piconline.Visible = false;
                picoffline.Visible = true;
                txtmsg.Enabled = false;
                richTextBox1.Enabled = false;
               
               // MessageBox.Show("Your desire node is not in network.");    
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
            picoffline.Visible = true;
            piconline.Visible = false;
            txtmsg.Enabled = false;
            label2.Text = "";
            richTextBox1.Enabled = false;
            txtip.Enabled = false;
            checkbtn.Enabled = false;
           
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Lan-Talk";
            notifyIcon1.BalloonTipText = "Program is minimized to tray.";

            if (FormWindowState.Minimized == this.WindowState)
            {
               notifyIcon1.Visible = true;
               notifyIcon1.ShowBalloonTip(500);
               this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
            notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

       

        private void txtmsg_KeyPress(object sender, KeyPressEventArgs e)
        {      
            if(e.KeyChar == (char)13)
            {
                String msg;

                msg = ip + ": " + txtmsg.Text;
                if(txtmsg.Text =="")
                {
                
                }
                else{
                
                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText(msg + "\n");
                ob.SendBroadcast(ip, port, txtmsg.Text);
                
                txtmsg.Text = "";
                
                }              
            }
        }

       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
         }

        private void button1_Click(object sender, EventArgs e)
        {
            port = Convert.ToInt32(this.txtport.Text);
            myDelegate = new ShowMessage(ShowMessageMethod);
            thread = new Thread(new ThreadStart(ReceiveMessage));
            thread.IsBackground = true;
            thread.Start();
            picoffline.Visible = true;
            piconline.Visible = false;
            txtmsg.Enabled = false;
            label2.Text = "";
            richTextBox1.Enabled = false;
            txtip.Enabled = true;
            checkbtn.Enabled = true;            
            panel2.Visible = false;
            txtip.Focus();
        }
       

        private void txtport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtport.Text != "" && Convert.ToInt32(this.txtport.Text) > 0 && Convert.ToInt32(this.txtport.Text) < 65536)
                {
                    button1.PerformClick();
                }
                else
                {
                    MessageBox.Show("You have entered invalid port.");
                }
            }
        }

      

        private void txtip_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                ip = txtip.Text;
                online();
            }
        }

          
     }
}
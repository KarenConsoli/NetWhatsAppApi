using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT_Client
{
    public partial class Form1 : Form
    {
        private delegate void myUICallBack(string myStr, TextBox ctl);
        


        private void myUI(string myStr, TextBox ctl)
        {
            if (this.InvokeRequired)
            {
                myUICallBack myUpdate = new myUICallBack(myUI);
                this.Invoke(myUpdate, myStr, ctl);
            }
            else
            {
                ctl.AppendText(myStr + Environment.NewLine);
            }
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            myUI(System.Text.Encoding.UTF8.GetString(e.Message), MessageTextBox);
        }


      public void connect()
        {
            MqttClient client;
            try
            {
                client = new MqttClient(HostTextBox.Text, 1883, false, null);
                client.Connect(Guid.NewGuid().ToString());
                client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
            }
            catch
            {
                label4.Text = "Can't connect to server";
            }

            client.Subscribe(new string[] { "wpmsg" }, new byte[] { (byte)0 });
        }
              
        

      
    }
}

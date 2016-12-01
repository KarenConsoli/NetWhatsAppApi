using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppApi;
using WhatsAppApi.Account;
using uPLibrary.Networking.M2Mqtt;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Security.Cryptography;
using System.IO;

namespace WhatsTest
{


    internal class Program
    {

        static MqttClient client;
        private delegate void myUICallBack(string myStr);
        public byte[] Clave = Encoding.ASCII.GetBytes("Vision Studio, video analitico inteligente. La cantidad correcta, en el lugar preciso, en el momento adecuado. Guia automatico inteligente del Cabezal Cosechadora");
        #region hola
        // DEMO STORE SHOULD BE DATABASE OR PERMANENT MEDIA IN REAL CASE
        static IDictionary<string, axolotl_identities_object> axolotl_identities = new Dictionary<string, axolotl_identities_object>();
        static IDictionary<uint, axolotl_prekeys_object> axolotl_prekeys = new Dictionary<uint, axolotl_prekeys_object>();
        static IDictionary<uint, axolotl_sender_keys_object> axolotl_sender_keys = new Dictionary<uint, axolotl_sender_keys_object>();
        static IDictionary<string, axolotl_sessions_object> axolotl_sessions = new Dictionary<string, axolotl_sessions_object>();
        static IDictionary<uint, axolotl_signed_prekeys_object> axolotl_signed_prekeys = new Dictionary<uint, axolotl_signed_prekeys_object>();

        static WhatsApp wa = null;



        [STAThread]
        private static void Main(string[] args)
        {

            var tmpEncoding = Encoding.UTF8;
            System.Console.OutputEncoding = Encoding.Default;
            System.Console.InputEncoding = Encoding.Default;

           

            /*string sender = "5491160330589"; // Mobile number with country code (but without + or 00)
            string password = "";//v2 password
            string target = "5491137583898";// Mobile number to send the message to*/
            Program pro = new Program();
            /* string code = "821984";
             string msg = "Estoy Probando WhatsApp Api en C#";*/
            pro.connect();
            //pro.messageRequest(sender);
            //pro.codeRequest(sender, code);
            //pro.res(target, msg);
            // create client instance 


            //Console.ReadLine();

        }
        private void myUI(string myStr)
        {
            Console.WriteLine(myStr);
            
            Program pro = new Program();

            #region Encriptacion
            string code= pro.desencripta(myStr);
            Console.WriteLine(code);
            #endregion


            Char delimiter = ',';
            String[] validate = code.Split(delimiter);

            if (validate[0] == "validate")
            {
                pro.messageRequest(validate[1]);

            }


            if (validate[0] == "config")
            {
                pro.codeRequest(validate[2],validate[1]);

            }

            validate = code.Split(delimiter);
            if (validate[0] == "msg")
            {
                pro.res(validate[1], validate[2]);

            }

            if (validate[0] == "msgimg")
            {
          //      pro.resImage(validate[1], Convert.ToByte(validate[2]));

            }

        }


        public string desencripta(string cadena)
        {
          string key = "Vision Studio, video analitico inteligente. La cantidad correcta, en el lugar preciso, en el momento adecuado. Guia automatico inteligente del Cabezal Cosechadora";
         

        
            string text = cadena;
    var result = new StringBuilder();

    for (int c = 0; c < text.Length; c++)
        result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

    return result.ToString();

        }

   

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            myUI(System.Text.Encoding.UTF8.GetString(e.Message));
        }


        public void connect()
        {

            try
            {
                client = new MqttClient("192.168.0.115", 1883, false, null);
                client.Connect(Guid.NewGuid().ToString());
                Console.WriteLine("Connected!!");
                
                client.Subscribe(new string[] { "wpmsg" }, new byte[] { (byte)0 });
                Console.WriteLine("Suscribed!!");
                 client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(client_MqttMsgPublishReceived);
            }
            catch
            {
               Console.WriteLine( "Can't connect to server");
            }

     

        }

        public void messageRequest(string sender)
        {
            #region MessageRequest
            string password = "";
            if (!String.IsNullOrEmpty(sender))
            {
                string method = "sms";
                //method = "voice";

                WhatsAppApi.Register.WhatsRegisterV2.RequestCode(sender, out password, method);
                Console.WriteLine("Code has been Sent!");
                Program pro = new Program();

                #region Encriptacion
                string h = pro.desencripta("Code has been Sent!");
                Console.WriteLine(h);
                #endregion
                client.Publish("wpmsg", Encoding.UTF8.GetBytes(h), (byte)2, false); //password received


            }
            #endregion


        }



        public void codeRequest(string sender, string code)
        {

            #region codeRequest
            string password = "";
            password = WhatsAppApi.Register.WhatsRegisterV2.RegisterCode(sender, code);
            if (!String.IsNullOrEmpty(password))
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                #region Encriptacion
                Program pro = new Program();
                string h = pro.desencripta("Mobile Registered!");
                Console.WriteLine(h);
                #endregion
                client.Publish("wpmsg", Encoding.UTF8.GetBytes(h), (byte)2, false);
                Console.WriteLine("User:");
                Console.WriteLine(sender);
                Console.WriteLine("Password:");
                string pass = String.Format("{0}", password);
                Console.WriteLine(pass);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                // Example #1: Write an array of strings to a file.
                // Create a string array that consists of three lines.
                string[] lines = { pass };
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().
                System.IO.File.WriteAllLines(@"C:\Api\pass.txt", lines);


                // Example #1: Write an array of strings to a file.
                // Create a string array that consists of three lines.
                string[] line = { sender };
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().
                System.IO.File.WriteAllLines(@"C:\Api\num.txt", line);

            }


            #endregion

        }
        public void res(string target, string msg)
        {
            #region res
            Program pro = new Program();
            string password = "";
            string sender = "";


            string line;
            // Read the file and display it line by line.
            System.IO.StreamReader files = new System.IO.StreamReader(@"..\Api\num.txt");
            while ((line = files.ReadLine()) != null)
            {
                sender = line;
                break;
            }
            Console.WriteLine("User: " + sender);
            files.Close();



            /*******/

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\Api\pass.txt");
            while ((line = file.ReadLine()) != null)
            {
                password = line;
                break;
            }
            Console.WriteLine("Password: " + password);
            file.Close();


            if (!pro.CheckLogin(sender, password))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Login failed");
                #region Encriptacion
               
                string h = pro.desencripta("NO");
                Console.WriteLine(h);
                #endregion
                client.Publish("wpmsg", Encoding.UTF8.GetBytes(h), (byte)2, false);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Login Sucessfull :D !!!");
                #region Encriptacion
                
                string h = pro.desencripta("Yes");
                Console.WriteLine(h);
                #endregion
                client.Publish("wpmsg", Encoding.UTF8.GetBytes(h), (byte)2, false);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }



            WhatSocket.Instance.SendMessage(target, msg);
            Console.WriteLine("Send Message to:");
            Console.WriteLine(target);
            Console.WriteLine("Message:");
            Console.WriteLine(msg);

            #endregion

        }

        public void resImage(string target, byte [] img)
        {
            #region res
            Program pro = new Program();
            string password = "";
            string sender = "";


            string line;
            // Read the file and display it line by line.
            System.IO.StreamReader files = new System.IO.StreamReader(@"..\Api\num.txt");
            while ((line = files.ReadLine()) != null)
            {
                sender = line;
                break;
            }
            Console.WriteLine("User: " + sender);
            files.Close();



            /*******/

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\Api\pass.txt");
            while ((line = file.ReadLine()) != null)
            {
                password = line;
                break;
            }
            Console.WriteLine("Password: " + password);
            file.Close();


            if (!pro.CheckLogin(sender, password))
            {
                Console.WriteLine("Login failed");

            }
            else
            {
                Console.WriteLine("Login Sucessfull :D !!!");

            }

          

            WhatSocket.Instance.SendMessageImage(target, img, WhatsAppApi.ApiBase.ImageType.PNG);
            Console.WriteLine("Send Message to:");
            Console.WriteLine(target);
           

            #endregion

        }
       
        

        private bool CheckLogin(string user, string pass)
        {
            string nickname = "Vision Studio SA";
            try
            {
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                    return false;

                WhatSocket.Create(user, pass, nickname, true);
                WhatSocket.Instance.Connect();
                WhatSocket.Instance.Login();
                //check login status
                if (WhatSocket.Instance.ConnectionStatus == WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN)
                {
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }
   
        #endregion

    }

    public class axolotl_identities_object
    {
        public string recipient_id { get; set; }
        public string registration_id { get; set; }
        public byte[] public_key { get; set; }
        public byte[] private_key { get; set; }
    }
    public class axolotl_prekeys_object
    {
        public string prekey_id { get; set; }
        public byte[] record { get; set; }

    }
    public class axolotl_sender_keys_object
    {
        public uint sender_key_id { get; set; }
        public byte[] record { get; set; }
    }
    public class axolotl_sessions_object
    {
        public string recipient_id { get; set; }
        public uint device_id { get; set; }
        public byte[] record { get; set; }
    }
    public class axolotl_signed_prekeys_object
    {
        public uint prekey_id { get; set; }
        public byte[] record { get; set; }
    }
    public class WhatSocket
    {
        private static WhatsAppApi.WhatsApp _instance;

        public static WhatsAppApi.WhatsApp Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    throw new Exception("Instance is not set");
                }
            }
        }

        public static void Create(string username, string password, string nickname, bool debug = false)
        {
            _instance = new WhatsAppApi.WhatsApp(username, password, nickname, debug);
        }
    }

    public class User
    {
        public string PhoneNumber { get; private set; }
        public string UserName { get; private set; }
        public WhatsUser WhatsUser { get; private set; }

        public User(string phone, string name)
        {
            this.PhoneNumber = phone;
            this.UserName = name;
        }

        public static User UserExists(string phoneNum, string nickName)
        {
            WhatsUserManager man = new WhatsUserManager();
            var whatsUser = man.CreateUser(phoneNum, phoneNum);
            var tmpUser = new User(phoneNum, nickName);
            tmpUser.SetUser(whatsUser);
            return tmpUser;
        }

        public void SetUser(WhatsUser user)
        {
            if (this.WhatsUser != null)
                return;

            this.WhatsUser = user;
        }

    }




   

}


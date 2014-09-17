    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using System.Security.Cryptography;

namespace LicGenUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static byte[] _LicBytes = ASCIIEncoding.ASCII.GetBytes("license1");
        static String _timeFormat = "dd-MM-yyyy hh:mm:ss tt";
        private const String STR_ONLINE = "OnLine";
        private const String STR_OFFLINE = "OffLine";
        private const String STR_SEP = "adsk";
        private String LicVersion = "";


        //Decrypt provide the code
        private void button1_Decrypt_Click(object sender, EventArgs e)
        {
            //
            String Code = Decrypt(textBox1_lockCode.Text, _LicBytes);
            String[] stringSeparators = new String[] {STR_SEP};
            String[] result = Code.Split(stringSeparators, StringSplitOptions.None);

            if (result.Length != 5)
                return;


            //version
            LicVersion = result[0];

            //ignore the time, second value


            //machine code
            textBox1_shortMacCode.Text = result[2];

            //app name
            label4_appname.Text = result[3];

            //type of Lic
            label4_licType.Text = result[4];
        }

        //generate the Lic
        private void Lic_Generate_button_Click(object sender, EventArgs e)
        {
            String enCryptDate = "";
            if (comboBox1_Lic.SelectedIndex == 0)
            {
                //online
                DateTime time = new DateTime();
                String sExpiryTimeStamp = time.ToString(_timeFormat, CultureInfo.InvariantCulture);

                enCryptDate = Encrypt(LicVersion + STR_SEP + 
                    label4_appname.Text + STR_SEP + 
                    textBox1_shortMacCode.Text + STR_SEP + 
                    sExpiryTimeStamp + STR_SEP +
                    STR_ONLINE, _LicBytes);
            }
            else
            {
                //offline

                String sExpiryTimeStamp = dateTimePicker1_expire.Value.ToString(_timeFormat, 
                                        CultureInfo.InvariantCulture);

                enCryptDate = Encrypt(LicVersion + STR_SEP + 
                    label4_appname.Text + STR_SEP +
                    textBox1_shortMacCode.Text + STR_SEP +
                    sExpiryTimeStamp + STR_SEP +
                    STR_OFFLINE, _LicBytes);
            }

            textBox1_LicCode.Text = enCryptDate;


        }

        //close
        private void button1_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1_expire.Format = DateTimePickerFormat.Custom;
            dateTimePicker1_expire.CustomFormat = "MM-dd-yyyy hh:mm:ss tt";

            comboBox1_Lic.SelectedIndex = 1;
        }

        //e
        public static String Encrypt(String originalString, byte[] bytes)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write))
                    {

                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(originalString);
                            writer.Flush();
                            cryptoStream.FlushFinalBlock();
                            writer.Flush();

                            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
        }

        public static String Decrypt(String cryptedString, byte[] bytes)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            String deCrypted = "";
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            deCrypted = reader.ReadToEnd();
                        }
                    }
                }
            }

            return deCrypted;
        }

       
        private void comboBox1_Lic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1_Lic.SelectedIndex == 0)
            {
                //online
                dateTimePicker1_expire.Enabled = false;

            }
            else
            {
                //offline
                dateTimePicker1_expire.Enabled = true;
            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            textBox1_lockCode.Text = "";

            textBox1_LicCode.Text = "";

            textBox1_shortMacCode.Text = "";

            //app name
            label4_appname.Text = "";

            //type of Lic
            label4_licType.Text = "";
        }


        //no need of these

        private void dateTimePicker1_expire_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_LicCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_appname_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_licType_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_shortMacCode_TextChanged(object sender, EventArgs e)
        {

        }

       

    }
}

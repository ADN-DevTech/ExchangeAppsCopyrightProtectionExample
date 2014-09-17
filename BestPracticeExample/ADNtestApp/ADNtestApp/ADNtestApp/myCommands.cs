// (C) Copyright 2014 by Autodesk 
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;
using System.IO;
using System.Globalization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using System.Net;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(ADNtestApp.MyCommands))]

namespace ADNtestApp
{

    // This class is instantiated by AutoCAD for each document when
    // a command is called by the user the first time in the context
    // of a given document. In other words, non static data in this class
    // is implicitly per-document!
    public class MyCommands
    {
        
        // Modal Command with localized name
        [CommandMethod("Testlicense", CommandFlags.Modal)]
        public void Testlicense() // This method can have any name
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            
            // Put your command code here
            using(ResultBuffer inResBuf = new ResultBuffer())
            {
                using(ResultBuffer outResBuf = VerifyLicMethod(inResBuf))
                {
                    TypedValue[] value = outResBuf.AsArray();

                    if (value.Length == 1)
                    {
                        short nResult = (short)value[0].Value;

                        //
                        if (nResult == SUCCESS)
                        {
                            ed.WriteMessage("SUCCESS -");
                        }
                        else if (nResult == EXPIRE_ONLINE)
                        {
                            ed.WriteMessage("sign in to Autodesk 360 and then use the App");
                            return;
                        }
                        else if (nResult == EXPIRE_OFFLINE)
                        {
                            ed.WriteMessage("license expired, contact publihser");
                            return;
                        }
                        else 
                        {
                            ed.WriteMessage("unable to get the license, contact publihser ");
                            return;
                        }

                    }
                }
            }

        }

        //this command prints the license Info
        //for tetsing only.
        [CommandMethod("PrintlicenseInfo", CommandFlags.Modal)]
        public void PrintlicenseInfo() // This method can have any name
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            _machineLockCode = GetMachineLockCode();
            String appname = Encrypt(_appname, _keyBytes);

            _licFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Autodesk\\", MakeValidFileName(appname));

            //read the file.
            String storedAppId = String.Empty;
            String storedMachineLockCode = String.Empty;
            String storedExpiryTimeStamp = String.Empty;
            String storedLastRunTimeStamp = String.Empty;
            String storedLastOnLineRunTimeStamp = String.Empty;
            String isOnLine = String.Empty;


            if (!ReadFromLicFile(_licFilePath, ref storedAppId,
                ref storedMachineLockCode, ref storedExpiryTimeStamp,
                ref storedLastRunTimeStamp, ref storedLastOnLineRunTimeStamp,
                ref isOnLine))
            {
                ed.WriteMessage("unable to read the file");
                return ;
            }

            ed.WriteMessage("license file : " + _licFilePath + "\n");
            ed.WriteMessage("license type : " + isOnLine + "\n");
            ed.WriteMessage("Last run date and time : " + storedLastRunTimeStamp + "\n");

            if (isOnLine.Equals(STR_OFFLINE))
            {
                //only reuired for offline license type
                ed.WriteMessage("Expiry date and time : " + storedExpiryTimeStamp + "\n");
            }
            else
            {
                //only reuired for online license type
                ed.WriteMessage("Autodesk 360 contacted date and time : " + storedLastOnLineRunTimeStamp + "\n");
                ed.WriteMessage("Expiry date and time : " + storedExpiryTimeStamp + "\n");
            }

        }


        //this command shows a dialog box with 
        //machine code. This code needs to be used to genarate the 
        //license using the license generation tool

        [CommandMethod("getLockcode", CommandFlags.Modal)]
        public void getLockcode()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            _machineLockCode = GetMachineLockCode();
            String appname = Encrypt(_appname, _keyBytes);
            
            _licFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Autodesk\\", MakeValidFileName(appname));

            //read and find if license type is online or offline.
            int returnVal = isOfflineLic();

            LockCode lockDlg = new LockCode();

            //add  time, so that string looks different each time
            String timenow = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);

            if (returnVal == ONLINE) //online
            {
                lockDlg.codeTxt.Text = Encrypt(_licVersion.ToString() + STR_SEP +
                                               timenow + STR_SEP + 
                                            _machineLockCode + STR_SEP +
                                          _appname +  STR_SEP +
                                            STR_ONLINE, _LicBytes);
            }
            else 
            {

                lockDlg.codeTxt.Text = Encrypt(_licVersion.ToString() + STR_SEP + 
                                            timenow + STR_SEP + 
                                            _machineLockCode + STR_SEP +
                                          _appname +  STR_SEP +
                                           STR_OFFLINE, _LicBytes);
            }

            lockDlg.bUpdateLic = false;

            //show the dialog
            Application.ShowModalDialog(lockDlg);

        }

        //this comamnd updates the local license file.
        //this command shows a dialog box, enter the code from lic tool

        [CommandMethod("updatelicense", CommandFlags.Modal)]
        public void updatelicense() // This method can have any name
        {
            //we will have 

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            LockCode lockDlg = new LockCode();
            lockDlg.codeTxt.Text = "";
            lockDlg.bUpdateLic = true;
            System.Windows.Forms.DialogResult results = Application.ShowModalDialog(lockDlg);

            if (results != System.Windows.Forms.DialogResult.OK)
                return;


            _machineLockCode = GetMachineLockCode();
            String appname = Encrypt(_appname, _keyBytes);

            _licFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Autodesk\\", MakeValidFileName(appname));

            //Decrypt using lic bytes...
            String license = Decrypt(lockDlg.codeTxt.Text, _LicBytes);

            String[] stringSeparators = new String[] { STR_SEP };
            String[] result = license.Split(stringSeparators, StringSplitOptions.None);

            if (result.Length != 5)
                return;

            //version
            String version = result[0];

            //app name
            String name = result[1];

            //machine code
            String macCode = result[2];

            //expire date
            String strDays = result[3];

            //type of Lic
            String licType = result[4];

            //check machine is same 
            if (!macCode.Equals(_machineLockCode))
            {
                ed.WriteMessage("error, not the same machine");
                return;
            }

            //check if the app name is same
            if (!name.Equals(_appname))
            {
                ed.WriteMessage("error, not the same app");
                return;
            }


            //update the license
            String sLastRunTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
            String onLineTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);

            DateTime dbExpireDate = DateTime.Now;

            if (licType.Equals(STR_ONLINE))
            {
                String sExpiryTimeStamp = DateTime.Now.AddDays(_days).ToString(_timeFormat, CultureInfo.InvariantCulture);
                Write2LicFile(_licFilePath, _appID, _machineLockCode, sExpiryTimeStamp, sLastRunTimeStamp, sLastRunTimeStamp, STR_ONLINE);
            }
            else
            {
                Write2LicFile(_licFilePath, _appID, _machineLockCode, strDays, sLastRunTimeStamp, sLastRunTimeStamp, STR_OFFLINE);
            }

        }

        

        const short RTSHORT = 5003; // int
        static String _timeFormat = "dd-MM-yyyy hh:mm:ss tt";

        static String _appID = "appstore.exchange.autodesk.com:screenshot:en"; //your app id
        static String _appname = "screenshot"; //your app name.

        //messages
        const short SUCCESS = 0;
        const short ONLINE = 1;
        const short OFFLINE = 2;
        const short FILE_CREATED = 3;
        const short UNABLE_READFILE = 4;
        const short DIFFERENT_APPID = 5;
        const short DIFFERENT_MACHINE = 6;
        const short BACK_DATE = 7;
        const short EXPIRE_ONLINE = 8;
        const short EXPIRE_OFFLINE = 9;
        const short NO_ENTITILEMENT = 10;

        private const String AUTODESK_EXCHANGE_URL = "https://apps.exchange.autodesk.com";
        private const String CHECK_ENTITLEMENT_ENDPOINT = "webservices/checkentitlement";

        private const String STR_ONLINE = "OnLine";
        private const String STR_OFFLINE = "OffLine";
        private const String STR_SEP = "adsk";

        static private String _machineLockCode = "";
        static private String _licFilePath = "";

        static private double _days = 30; // number of days by which user needs to conatct Autodesk 360

        // Secret key for Symmetric Cryptography. Can be anything, so lets keep it simple for the demo.
        static byte[] _keyBytes = ASCIIEncoding.ASCII.GetBytes("Autodesk");

        //this for license only
        static byte[] _LicBytes = ASCIIEncoding.ASCII.GetBytes("license1");

        //future use... now it is 1..
        static private short _licVersion = 1;


        [LispFunction("VerifyLic")]
        public ResultBuffer VerifyLicMethod(ResultBuffer inResBuf)
        {
            ResultBuffer resBuf = new ResultBuffer();

            _machineLockCode = GetMachineLockCode();
            String appname = Encrypt(_appname, _keyBytes);
            
            _licFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\Autodesk\\", MakeValidFileName(appname));

            //read and find if license type is online or offline.
            int returnVal = isOfflineLic();

            if (returnVal == ONLINE) //online
            {
                int nlic = verifyOnlineEntitlement();
                resBuf.Add(new TypedValue(RTSHORT, nlic));
            }
            else if (returnVal == OFFLINE) //online
            {
                int nLic = verifyofflineEntitlement(STR_OFFLINE);
                resBuf.Add(new TypedValue(RTSHORT, nLic));
            }
            else
            {
                //error
                resBuf.Add(new TypedValue(RTSHORT, returnVal));
            }

            return resBuf;
        }

        public static int isOfflineLic()
        {
            String licFile = Encrypt(_appID, _keyBytes);

            //No file, then consider online 
            if (!File.Exists(_licFilePath))
            {
                //create the file and return;
                String sExpiryTimeStamp = DateTime.Now.AddDays(_days).ToString(_timeFormat, CultureInfo.InvariantCulture);
                String sLastRunTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                String onLineTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);

                Write2LicFile(_licFilePath, _appID, _machineLockCode, sExpiryTimeStamp, sLastRunTimeStamp, sLastRunTimeStamp, STR_ONLINE);

                return ONLINE;
            }
            else
            {
                String storedAppId = String.Empty;
                String storedMachineLockCode = String.Empty;
                String storedExpiryTimeStamp = String.Empty;
                String storedLastRunTimeStamp = String.Empty;
                String storedLastOnLineRunTimeStamp = String.Empty;
                String isOnLine = String.Empty;


                if (!ReadFromLicFile(_licFilePath, ref storedAppId, 
                        ref storedMachineLockCode, ref storedExpiryTimeStamp, 
                        ref storedLastRunTimeStamp, ref storedLastOnLineRunTimeStamp, 
                        ref isOnLine))
                {
                    //error reading file, return error
                    return UNABLE_READFILE;
                }

                //check the value
                if (isOnLine.Equals(STR_OFFLINE))
                {
                    return OFFLINE;
                }

            }

            return ONLINE;
        }

        public static String GetMachineLockCode()
        {
            StringBuilder sb = new StringBuilder("");
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics != null)
            {
                foreach (NetworkInterface ni in nics)
                {
                    sb.Append(String.Format("NID:{0} NADDR:{1}", ni.Id, ni.GetPhysicalAddress().ToString()));
                }
            }
            return sb.ToString();
        }

        // Uses the DES crypto provider to encrypt a string.
        // Used to encrypt the details before they are stored in the license file
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


        public static void Write2LicFile(String licFilePath,
                                    String appId,
                                    String machineLockCode,
                                    String sExpiryTimeStamp,
                                    String sLastRunTimeStamp,
                                    String sLastonlineRunTimeStamp,
                                    String onLine)
        {
            try
            {
                if (File.Exists(licFilePath))
                    File.Delete(licFilePath);

                StreamWriter sw = new StreamWriter(licFilePath, false);
                sw.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", Encrypt(appId, _keyBytes),
                    Encrypt(machineLockCode, _keyBytes),
                    Encrypt(sExpiryTimeStamp, _keyBytes),
                    Encrypt(sLastRunTimeStamp, _keyBytes),
                    Encrypt(sLastonlineRunTimeStamp, _keyBytes),
                    Encrypt(onLine, _keyBytes)));
                sw.Close();
            }
            catch
            {
            }
        }

        // Reads the details to a license file in the following format
        // Format :
        // <AppId>\t<MachineLockCode>\t<ExpiryDateTimeStamp>\t<LastRunDateTimeStamp>
        public static bool ReadFromLicFile(String licFilePath,
                            ref String appId,
                            ref String machineLockCode,
                            ref String sExpiryTimeStamp,
                            ref String sLastRunTimeStamp,
                            ref String storedLastOnLineRunTimeStamp,
                            ref String onLine)
        {
            try
            {
                String line = String.Empty;
                using (StreamReader sr = new StreamReader(licFilePath))
                {
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        String[] items = line.Split('\t');
                        if (items.Length != 6)
                        {// Incorrect format.
                            sr.Close();
                            return false;
                        }

                        appId = Decrypt(items[0], _keyBytes);
                        machineLockCode = Decrypt(items[1], _keyBytes);
                        sExpiryTimeStamp = Decrypt(items[2], _keyBytes);
                        sLastRunTimeStamp = Decrypt(items[3], _keyBytes);
                        storedLastOnLineRunTimeStamp = Decrypt(items[4], _keyBytes);
                        onLine = Decrypt(items[5], _keyBytes);
                        break;
                    }
                    sr.Close();
                }

                return true;
            }
            catch
            {
            }
            return false;
        }

       

        public static int verifyOnlineEntitlement()
        {

            String strid = Application.GetSystemVariable("ONLINEUSERID") as String;
            int nLic = NO_ENTITILEMENT;

            try
            {

                if (strid.Equals(""))
                {
                    nLic = verifyofflineEntitlement(STR_ONLINE);
                }
                else
                {
                    //check for online entitle ment
                    RestClient client = new RestClient(AUTODESK_EXCHANGE_URL);
                    RestRequest req = new RestRequest(CHECK_ENTITLEMENT_ENDPOINT);
                    req.Method = Method.GET;

                    req.AddParameter("userid", strid);
                    req.AddParameter("appid", _appID);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse<EntitlementResult> resp = client.Execute<EntitlementResult>(req);

                    if (resp.Data != null && resp.Data.IsValid)
                    {
                        nLic = SUCCESS;

                        //update the file
                        String sExpiryTimeStamp = DateTime.Now.AddDays(_days).ToString(_timeFormat, CultureInfo.InvariantCulture);
                        String sLastRunTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                        String onLineTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                        //write the file
                        String licFile = Encrypt(_appID, _keyBytes);

                        Write2LicFile(_licFilePath, _appID, _machineLockCode, sExpiryTimeStamp,
                            sLastRunTimeStamp, onLineTimeStamp, STR_ONLINE);

                    }
                    else
                    {
                        nLic = NO_ENTITILEMENT;
                    }

                }
            }
            catch
            {
                //some error...
            }

            return nLic;

        }



        public static int verifyofflineEntitlement(String strLicType)
        {

            
            if (!File.Exists(_licFilePath))
            {
                //fine...
                return UNABLE_READFILE;
            }

            //open the license file.
            //read the conetent..
            String storedAppId = String.Empty;
            String storedMachineLockCode = String.Empty;
            String storedExpiryTimeStamp = String.Empty;
            String storedLastRunTimeStamp = String.Empty;
            String storedLastOnLineRunTimeStamp = String.Empty;
            String isOnLine = String.Empty;

           
            if (!ReadFromLicFile(_licFilePath, ref storedAppId, 
                ref storedMachineLockCode, ref storedExpiryTimeStamp, 
                ref storedLastRunTimeStamp, ref storedLastOnLineRunTimeStamp, 
                ref isOnLine))
            {
                return UNABLE_READFILE;
            }


            //check the app id
            if (!storedAppId.Equals(_appID))
            {
                return DIFFERENT_APPID;
            }
            //check the machine id..

            if (!storedMachineLockCode.Equals(_machineLockCode))
            {
                return DIFFERENT_MACHINE;
            }

            DateTime currentTimeStamp = DateTime.Now;

            DateTime dbLastRunTimeStamp = DateTime.Now;
            if (DateTime.TryParseExact(storedLastRunTimeStamp, _timeFormat, 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, 
                    out dbLastRunTimeStamp))
            {
                if (currentTimeStamp < dbLastRunTimeStamp)
                {// Back dated
                    return BACK_DATE;
                }
            }

            //check online or offline lic.
            if (isOnLine.Equals(STR_ONLINE))
            {
                //check last online date.
                DateTime dbOnlineRunTimeStamp = DateTime.Now;

                if (DateTime.TryParseExact(storedLastOnLineRunTimeStamp, 
                    _timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, 
                                out dbOnlineRunTimeStamp))
                {
                    //change the number of days if required like 15 days etc...
                    DateTime onlineDate = dbOnlineRunTimeStamp.AddDays(_days); 

                    //for testing - 
                    //DateTime onlineDate = dbOnlineRunTimeStamp.AddMinutes(2.0);

                    //dbOnlineRunTimeStamp
                    if (currentTimeStamp > onlineDate)
                    {// need to contact exchange store..
                        return EXPIRE_ONLINE;
                    }
                }
                else
                {
                    return EXPIRE_ONLINE;
                }

                String sExpiryTimeStamp = dbOnlineRunTimeStamp.AddDays(_days).ToString(_timeFormat, CultureInfo.InvariantCulture);
                String sLastRunTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                String onLineTimeStamp = dbOnlineRunTimeStamp.ToString(_timeFormat, CultureInfo.InvariantCulture);
                //write the file
                Write2LicFile(_licFilePath, _appID, _machineLockCode, sExpiryTimeStamp,
                    sLastRunTimeStamp, onLineTimeStamp, STR_ONLINE);
            }
            else
            {
                //check the expiry date
                DateTime dbStoredExpiryTimeStamp = DateTime.Now;

                if (DateTime.TryParseExact(storedExpiryTimeStamp,
                    _timeFormat, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out dbStoredExpiryTimeStamp))
                {
                    if (currentTimeStamp > dbStoredExpiryTimeStamp)
                    {
                        return EXPIRE_OFFLINE;
                    }
                }
                else
                {
                    return EXPIRE_OFFLINE;
                }

                //write the file
                String sExpiryTimeStamp = dbStoredExpiryTimeStamp.ToString(_timeFormat, CultureInfo.InvariantCulture);
                String sLastRunTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                String onLineTimeStamp = DateTime.Now.ToString(_timeFormat, CultureInfo.InvariantCulture);
                //write the file

                Write2LicFile(_licFilePath, _appID, _machineLockCode, 
                        sExpiryTimeStamp, sLastRunTimeStamp,
                            onLineTimeStamp, STR_OFFLINE);
            }

            return SUCCESS;
        }

        private static String MakeValidFileName(String name)
        {
            try
            {
                String invalidChars = System.Text.RegularExpressions.Regex.Escape(new String(System.IO.Path.GetInvalidFileNameChars()));
                String invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

                return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
            }
            catch
            {
                return name;
            }
        }
       
    }

    class EntitlementResult
    {
        public String UserId { get; set; }
        public String AppId { get; set; }
        public bool IsValid { get; set; }
        public String Message { get; set; }

    }
}

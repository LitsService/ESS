using ESS_Web_Application.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ESS_Web_Application.Helper
{
    public class clsCommon
    {
        #region Profile Image Save And Retrive

        public static byte[] GetImageFromDB(string UserID)
        {
            byte[] ImageByteArray = null;

            try
            {
                SqlParameter[] ht = new SqlParameter[]{
                new SqlParameter("@UserId",UserID)
                //new SqlParameter("@imgID",ImgId)
            };

                DataTable rd = DBContext.GetDataTable("sp_GetEmployeeImage", ht);
                foreach (DataRow RW in rd.Rows)
                {
                    ImageByteArray = (byte[])RW["ImgBody"];
                }
                return ImageByteArray;
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_GetEmployeeImage :::::" + ex.Message);
            }
        }

        public static int SaveImageToDB(byte[] ImageByteArray, string UserID)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserId", UserID);
                ht.Add("@EmpImage", ImageByteArray);

                return DBContext.ExecuteNonQuery("sp_InsertUpdateEmployeeImage", ht);
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_InsertUpdateEmployeeImage :::::" + ex.Message);
            }
        }

        #endregion

        public static bool ValidatePageSecurity(string UserName, string ActionKeyWord)
        {
            if (UserName.ToLower() == "administrator" || UserName == "")
                return true;
            else
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserName", UserName);
                ht.Add("@ActionKeyWord", ActionKeyWord);

                var result = bool.Parse(DBContext.ExecuteScalar("sp_Admin_get_ACL_IsAllowed", ht).ToString());
                return result;
            }
        }

        public enum RequestStatus
        {
            Initiated = 1,
            Edited = 2,
            Pending = 3,
            InProcess = 4,
            Approved = 5,
            Rejected = 6,
            Canceled = 7,
            Completed = 8,
            Revised = 9,
            Recalled = 10
        }
        static bool RedirectionCallback(string url)
        {
            // Return true if the URL is an HTTPS URL.
            return url.ToLower().StartsWith("https://");
        }
        public static bool SendMail(string MailBody, string EmailTo, string MailFrom, string Subject)
        {
            try
            {

                //ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                //service.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EMAIL_ACC"], ConfigurationManager.AppSettings["EMAIL_PWD"]);
                ////service.AutodiscoverUrl(MailFrom, RedirectionCallback);


                //EmailMessage message = new EmailMessage(service);
                //message.Subject = Subject;
                //message.Body = MailBody;

                //if (EmailTo.Contains(";") | EmailTo.Contains(","))
                //{
                //    char[] deli = new char[2];
                //    deli[0] = ';';
                //    deli[1] = ',';

                //    string[] emails = EmailTo.Split(deli);

                //    foreach (string toEmail in emails)
                //    {
                //        if (!string.IsNullOrEmpty(toEmail))
                //            message.ToRecipients.Add(toEmail);
                //    }
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(EmailTo))
                //        message.ToRecipients.Add(EmailTo);
                //}
                //message.From = new EmailAddress(MailFrom);
                //message.Send();

                EmailTo = /*EmailTo +*/ "saleem@lits.services";

                MailMessage msg = new MailMessage();

                EmailTo = EmailTo.Trim();

                if (EmailTo.Contains(";") | EmailTo.Contains(","))
                {
                    char[] deli = new char[2];
                    deli[0] = ';';
                    deli[1] = ',';

                    string[] emails = EmailTo.Split(deli);

                    foreach (string toEmail in emails)
                    {
                        msg.To.Add(toEmail);
                    }
                }
                else
                {
                    msg.To.Add(EmailTo);
                }

                msg.From = new MailAddress(MailFrom);
                msg.Subject = Subject;
                msg.Body = MailBody;
                msg.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTP_HOST"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]));
                smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTP_EnableSsl"].ToString());
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), ConfigurationManager.AppSettings["EMAIL_PWD"].ToString());
                smtp.Send(msg);

                msg.Dispose();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public static bool SendMail(string MailBody, string EmailTo, string MailFrom, string Subject, System.Net.Mail.Attachment[] attachments)
        {
            try
            {
                MailMessage msg = new MailMessage();

                if (EmailTo.Contains(";") | EmailTo.Contains(","))
                {
                    char[] deli = new char[2];
                    deli[0] = ';';
                    deli[1] = ',';

                    string[] emails = EmailTo.Split(deli);

                    foreach (string toEmail in emails)
                    {
                        msg.To.Add(toEmail);
                    }
                }
                else
                {
                    msg.To.Add(EmailTo);
                }

                msg.From = new MailAddress(MailFrom);
                msg.Subject = Subject;
                msg.Body = MailBody;
                msg.IsBodyHtml = true;

                foreach (System.Net.Mail.Attachment attchmnts in attachments)
                {
                    msg.Attachments.Add(attchmnts);
                }

                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTP_HOST"], int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]));
                smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTP_EnableSsl"]);
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EMAIL_ACC"], ConfigurationManager.AppSettings["EMAIL_PWD"]);
                smtp.Send(msg);
                msg.To.Clear();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public static string GetPostedFieldText(DateTime dt)
        {
            TimeSpan ts = DateTime.Now.Subtract(dt);

            if (ts.Days > 30)
                return string.Format("{0:dd-MM-yyyy}", dt);
            else if (ts.Days == 1)
                return string.Format("{0} {1}", ts.Days, "day ago");
            else if (ts.Days > 1)
                return string.Format("{0} {1}", ts.Days, "days ago");
            else if (ts.Hours == 1)
                return string.Format("{0} {1}", ts.Hours, "hour ago");
            else if (ts.Hours > 1)
                return string.Format("{0} {1}", ts.Hours, "hours ago");
            else if (ts.Minutes > 1)
                return string.Format("{0} {1}", ts.Minutes, "minutes ago");
            else
                return string.Format("{0} {1}", ts.Minutes, "minute ago");
        }

        public static string getDirection(string culture)
        {
            //check whether a culture is stored in the session
            if (!string.IsNullOrEmpty(culture) && culture == "ar-AE")
            {
                return "rtl";
            }
            else
            {
                return "ltr";
            }
        }

        public static string FloatRightIfRTL(string culture)
        {
            //check whether a culture is stored in the session
            if (!string.IsNullOrEmpty(culture) && culture == "ar-AE")
            {
                return "f-right";
            }
            else
            {
                return "";
            }
        }

        public static string FloatLeftIfRTL(string culture)
        {
            //check whether a culture is stored in the session
            if (!string.IsNullOrEmpty(culture) && culture == "ar-AE")
            {
                return "f-left";
            }
            else
            {
                return "";
            }
        }

        public static string TextLeftIfRTL(string culture)
        {
            //check whether a culture is stored in the session
            if (!string.IsNullOrEmpty(culture) && culture == "ar-AE")
            {
                return "t-left";
            }
            else
            {
                return "";
            }
        }

        public static string TextRightIfRTL(string culture)
        {
            //check whether a culture is stored in the session
            if (!string.IsNullOrEmpty(culture) && culture == "ar-AE")
            {
                return "t-right";
            }
            else
            {
                return "";
            }
        }

        //public static string GenerateReportHTML(string ReportDisplayName, string ReportPath, string prmRequestID, string CurrentUserID)
        //{
        //    string _ReportPath = "";
        //    try
        //    {
        //        // Prepare Render arguments

        //        string historyID = null;
        //        string deviceInfo = null;
        //        string format = "IMAGE";
        //        Byte[] results;
        //        string encoding = String.Empty;
        //        string mimeType = String.Empty;
        //        string extension = String.Empty;
        //        Rse2005.Warning[] warnings = null;
        //        string[] streamIDs = null;


        //        Rse2005.ReportExecutionServiceSoapClient rsExec = new Rse2005.ReportExecutionServiceSoapClient();

        //        System.Net.NetworkCredential clientCredentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["NetworkUsername"], ConfigurationManager.AppSettings["NetworkPassword"]);
        //        rsExec.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
        //        rsExec.ClientCredentials.Windows.ClientCredential = clientCredentials;

        //        rsExec.ClientCredentials.SupportInteractive = true;

        //        Rse2005.TrustedUserHeader tuh = new Rse2005.TrustedUserHeader();
        //        Rse2005.ServerInfoHeader sih = new Rse2005.ServerInfoHeader();
        //        Rse2005.ExecutionInfo ei = new Rse2005.ExecutionInfo();


        //        rsExec.LoadReport(tuh, ReportPath, historyID, out sih, out ei);

        //        Rse2005.ParameterValue[] rptParameters = new Rse2005.ParameterValue[3];

        //        rptParameters[0] = new Rse2005.ParameterValue();
        //        rptParameters[0].Name = "RequestID";
        //        rptParameters[0].Value = prmRequestID;

        //        //render the PDF
        //        Rse2005.ExecutionHeader eh = new Rse2005.ExecutionHeader();
        //        eh.ExecutionID = ei.ExecutionID;
        //        rsExec.SetExecutionParameters(eh, tuh, rptParameters, "en-us", out ei);

        //        //Rse2005.DataSourceCredentials[] dsc = new Rse2005.DataSourceCredentials[1];

        //        //dsc[0] = new Rse2005.DataSourceCredentials();
        //        //dsc[0].DataSourceName = "DataSource1";
        //        //dsc[0].UserName = ConfigurationManager.AppSettings["DBUsername"];
        //        //dsc[0].Password = ConfigurationManager.AppSettings["DBPassword"];

        //        //rsExec.SetExecutionCredentials(eh, tuh, dsc, out ei);

        //        rsExec.Render(eh, tuh, format, deviceInfo, out results, out extension, out mimeType, out encoding, out warnings, out streamIDs);

        //        try
        //        {
        //            if (!Directory.Exists(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempReportFolder"] + "/" + CurrentUserID)))
        //                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempReportFolder"] + "/" + CurrentUserID));

        //            _ReportPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["TempReportFolder"] + "/" + CurrentUserID + "/" + ReportDisplayName + "_" + prmRequestID + ".jpg");

        //            FileStream stream = File.Create(_ReportPath, results.Length);
        //            stream.Write(results, 0, results.Length);
        //            stream.Close();

        //            rsExec.Close();
        //            //return _ReportPath;
        //            _ReportPath = ConfigurationManager.AppSettings["TempReportFolder"] + "/" + CurrentUserID + "/" + ReportDisplayName + "_" + prmRequestID + ".jpg";
        //        }
        //        catch (Exception ex)
        //        {
        //            RecordException(ex.Message);
        //            // Console.Write("Exception:" + ex.Message + "\n" + ex.StackTrace);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        RecordException(ex.Message);

        //        //Console.Write("Exception:" + ex.Message + "\n" + ex.StackTrace);
        //    }

        //    return _ReportPath;
        //}

        public static void RecordException(string errorDetail)
        {

            SqlParameter[] ht = {
                                new SqlParameter("@Description", errorDetail),
                                new SqlParameter("@UserHostAddress", HttpContext.Current.Request.UserHostAddress),
                                new SqlParameter("@ID","0")
                            };
            DataTable dt = DBContext.GetDataTable("sp_App_Insert_Failure", ht);
            string id = "0";
            foreach (DataRow rw in dt.Rows)
            {
                id = rw.IsNull("ID") ? "0" : rw["ID"].ToString();
            }

            HttpContext.Current.Response.Redirect("~/Error.aspx?EXID=" + id, true);
        }

        public static Hashtable get_AppSettings(string App_Group, string App_Key)
        {
            Hashtable _AppSettings = new Hashtable();
            SqlParameter[] newValues = new SqlParameter[] { new SqlParameter("@App_Group", App_Group), new SqlParameter("@App_Key", App_Key) };
            DataTable dt = DBContext.GetDataTable("sp_App_Settings_Get", newValues);
            foreach (DataRow RW in dt.Rows)
            {
                _AppSettings.Add(RW["App_Key"], RW["App_Value"]);
            }

            return _AppSettings;
        }
    }
}
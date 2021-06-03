using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ESS_Web_Application.Helper
{
    public class UploadFiles
    {
        public static void UploadFile(HttpPostedFileBase fileUpload, string DocType, string DocNumber)
        {
            string str = DocType;
            string str1 = DocNumber;
            int contentLength = fileUpload.ContentLength;
            string fileName = fileUpload.FileName;
            int num = fileName.LastIndexOf("\\");
            fileName = fileName.Substring(num + 1);
            string contentType = fileUpload.ContentType;
            byte[] numArray = new byte[contentLength];
            fileUpload.InputStream.Read(numArray, 0, contentLength);
            SaveFile(str1, str, fileName, contentType, contentLength, numArray, "Remarks");


        }
        public string CalculateFileSize(double FileInBytes)
        {
            string str = "00";
            if (FileInBytes < 1024)
            {
                str = string.Concat(FileInBytes, " B");
            }
            else if (FileInBytes > 1024 & FileInBytes < 1048576)
            {
                str = string.Concat(Math.Round(FileInBytes / 1024, 2), " KB");
            }
            else if (!(FileInBytes > 1048576 & FileInBytes < 107341824))
            {
                str = (!(FileInBytes > 107341824 & FileInBytes < 1099511627776) ? string.Concat(Math.Round(FileInBytes / 1024 / 1024 / 1024 / 1024, 2), " TB") : string.Concat(Math.Round(FileInBytes / 1024 / 1024 / 1024, 2), " GB"));
            }
            else
            {
                str = string.Concat(Math.Round(FileInBytes / 1024 / 1024, 2), " MB");
            }
            return str;
        }

        //public string DeleteFile(string FileName)
        //{
        //    string message = "";
        //    try
        //    {
        //        int num = 0;
        //        int.TryParse(FileName, out num);
        //        DeleteFile(num);
        //    }
        //    catch (Exception exception)
        //    {
        //        message = exception.Message;
        //    }
        //    return message;
        //}

        public static DownloadDetails DownloadFile(string filePath)
        {
            DownloadDetails d = new DownloadDetails();
            if (!string.IsNullOrEmpty(filePath))
            {
                DataTable aFile = GetAFile(filePath);
                DataRow item = aFile.Rows[0];
                string str = (string)item["Name"];
                string item1 = (string)item["ContentType"];
                byte[] numArray = (byte[])item["Data"];
                d = new DownloadDetails()
                {
                    Bytes = numArray,
                    ContentType = item1,
                    Name = str
                };
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.AddHeader("Content-type", item1);
                Response.AddHeader("Content-Disposition", string.Concat("attachment; filename=", str));
                Response.BinaryWrite(numArray);
                Response.Flush();
                //Response.End();
            }
            return d;

        }

        public DataTable GetFilesInDirectory(string sourcePath)
        {
            DataTable dataTable = new DataTable();
            if (Directory.Exists(sourcePath))
            {
                dataTable.Columns.Add(new DataColumn("Name"));
                dataTable.Columns.Add(new DataColumn("Size"));
                dataTable.Columns["Size"].DataType = typeof(double);
                dataTable.Columns.Add(new DataColumn("ConvertedSize"));
                FileInfo[] files = (new DirectoryInfo(sourcePath)).GetFiles("*.*");
                for (int i = 0; i < (int)files.Length; i++)
                {
                    FileInfo fileInfo = files[i];
                    DataRow name = dataTable.NewRow();
                    name["Name"] = fileInfo.Name;
                    name["Size"] = fileInfo.Length;
                    name["ConvertedSize"] = this.CalculateFileSize((double)fileInfo.Length);
                    dataTable.Rows.Add(name);
                }
            }
            return dataTable;
        }

        [ScriptMethod]
        [WebMethod]
        public static object GetUploadStatus()
        {
            UploadDetail item = (UploadDetail)HttpContext.Current.Session["UploadDetail"];
            if (item == null || !item.IsReady)
            {
                return null;
            }
            int uploadedLength = item.UploadedLength;
            int contentLength = item.ContentLength;
            int num = (int)Math.Ceiling((double)uploadedLength / (double)contentLength * 100);
            string str = "Uploading...";
            string str1 = string.Format("{0}", item.FileName);
            string str2 = string.Format("{0} of {1} Bytes", uploadedLength, contentLength);
            return new { percentComplete = num, message = str, fileName = str1, downloadBytes = str2 };
        }

        public void LoadUploadedFiles(string DocType, string DocNumber)
        {
            string str = DocType;// Session["DocType"].ToString();
            string str1 = DocNumber;// Session["DocNumber"].ToString();
            //Session["UserId"].ToString();
            DataTable fileList = GetFileList(str, str1);
            //gv.DataSource = fileList;
            //gv.DataBind();
            if (fileList != null && fileList.Rows.Count > 0)
            {
                double num = Convert.ToDouble(fileList.Compute("SUM(Size)", ""));
                if (num > 0)
                {
                    string lblTotalSize = CalculateFileSize(num);
                }
            }
        }


        public static void DeleteFile(string id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                OpenConnection(sqlConnection);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = "DELETE FROM TBL_UploadFiles WHERE DOCID=@ID";
                sqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlCommand.Parameters.Add("@DOCID", SqlDbType.VarChar, 50);
                sqlCommand.Parameters["@DOCID"].Value = id;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
        }

        public static DataTable GetAFile(string id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                OpenConnection(sqlConnection);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = "SELECT ID, Name, ContentType, Size, Data FROM TBL_UploadFiles WHERE DOCID=@DOCID";
                sqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlCommand.Parameters.Add("@DOCID", SqlDbType.VarChar, 50);
                sqlCommand.Parameters["@DOCID"].Value = id;
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
            return dataTable;
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public static DataTable GetFileList(string DocType, string DocID)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                OpenConnection(sqlConnection);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = "SELECT ID, Name, ContentType, Size , Remarks FROM TBL_UploadFiles WHERE  DOCID = @DOCID AND DocType= @DocType";
                sqlCommand.Parameters.Add("@DOCID", SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add("@DocType", SqlDbType.VarChar, 50);
                sqlCommand.Parameters["@DOCID"].Value = DocID;
                sqlCommand.Parameters["@DocType"].Value = DocType;
                sqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
            return dataTable;
        }

        private static void OpenConnection(SqlConnection connection)
        {
            connection.ConnectionString = GetConnectionString();
            connection.Open();
        }

        public static void SaveFile(string DocID, string DocType, string name, string contentType, int size, byte[] data, string Remarks)
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                OpenConnection(sqlConnection);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = string.Concat("INSERT INTO TBL_UploadFiles VALUES(@DOCID, @DocType, @Name, @ContentType, ", "@Size, @Data, @Remarks)");
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.Add("@DOCID", SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add("@DocType", SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                sqlCommand.Parameters.Add("@ContentType", SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add("@size", SqlDbType.Int);
                sqlCommand.Parameters.Add("@Data", SqlDbType.VarBinary);
                sqlCommand.Parameters.Add("@Remarks", SqlDbType.VarChar, 50);
                sqlCommand.Parameters["@DOCID"].Value = DocID;
                sqlCommand.Parameters["@DocType"].Value = DocType;
                sqlCommand.Parameters["@Name"].Value = name;
                sqlCommand.Parameters["@ContentType"].Value = contentType;
                sqlCommand.Parameters["@size"].Value = size;
                sqlCommand.Parameters["@Data"].Value = data;
                sqlCommand.Parameters["@Remarks"].Value = Remarks;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }


    }

    public class UploadDetail
    {
        public int ContentLength
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public bool IsReady
        {
            get;
            set;
        }

        public int UploadedLength
        {
            get;
            set;
        }

        public UploadDetail()
        {
        }
    }
    public class DownloadDetails
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }
    }
}
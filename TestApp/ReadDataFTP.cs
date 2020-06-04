using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Globalization;

namespace Infotronix.TestApp
{
    public class ReadDataFTP : IDisposable
    {
        public bool ReadData()
        {
            try
            {
                DataTable dtFpt = SqlHelper.ExecuteProcedure("select SubDeviceId,FTPFolder,FTPFilename,FTPFileDateFormat,MultiplyConversation,DeviceType from SubDeviceMaster where Status = 1");
                //DataTable dtFpt = SqlHelper.ExecuteProcedure("select SubDeviceId,FTPFolder,FTPFilename,FTPFileDateFormat,MultiplyConversation,DeviceType from SubDeviceMaster where Status = 1 and SubDeviceId in (select SubDeviceId from SubDeviceMaster where PlantID = '7644080E-A37A-4F91-AD31-20E55D7535DB')");

                //string FolderName = dateTimePicker1.Value.ToString("yyyy-MM-dd"); //DateTime.Now.ToString("yyyy-MM-dd");
                DateTime dtCurrentDateTime = DateTime.Now;
                for (int i = 0; i < dtFpt.Rows.Count; i++)
                {
                    string FolderName = string.Format(dtFpt.Rows[i]["FTPFilename"].ToString(), dtCurrentDateTime.ToString(dtFpt.Rows[i]["FTPFileDateFormat"].ToString()));
                    string SubDeviceId = dtFpt.Rows[i]["SubDeviceId"].ToString(), FTPFolder = dtFpt.Rows[i]["FTPFolder"].ToString();
                    decimal multiplier = 0;

                    if (!decimal.TryParse(dtFpt.Rows[i]["MultiplyConversation"].ToString(), out multiplier))
                    {
                        multiplier = 0;
                    }

                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\downloadFile"))
                    {
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\downloadFile");
                    }

                    string DeviceType = dtFpt.Rows[i]["DeviceType"].ToString();

                    bool Added = Download(AppDomain.CurrentDomain.BaseDirectory + @"\downloadFile", FolderName, FTPFolder, dtCurrentDateTime.ToString("yyyy-MM-dd"), SubDeviceId, multiplier, DeviceType);

                    if (Added == true)
                    {
                        string[,] para = { { "SubDeviceId", SubDeviceId }, { "FileName", FolderName }, { "FTPFolder", FTPFolder } };
                        string sqlQue = "insert into FileUploadLog (FileUploadLogID,SubDeviceId,FileName,FTPFolder) values (NEWID(),@SubDeviceId,@FileName,@FTPFolder)";
                        SqlHelper.ExecuteNoneQueryWithPera(sqlQue, para);
                    }
                }
                return true;
            }
            catch (Exception ex)
            { ErrorLog(ex, "00000000-0000-0000-0000-000000000000"); return false; }
        }

        private bool Download(string filePath, string fileName, string FTPFolder, string DeviceDate, string SubDeviceId, decimal multiplier, string DeviceType)
        {
            FtpWebRequest reqFTP;
            string fileName_new = DateTime.Now.Ticks + "_" + FTPFolder + "_" + fileName;
            FileStream outputStream = new FileStream(filePath + "\\" + fileName_new, FileMode.Create);
            try
            {
                //filePath = <<The full path where the 
                //file is to be created. the>>, 
                //fileName = <<Name of the file to be createdNeed not 
                //name on FTP server. name name()>>
                //outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                DataTable dtFpt = SqlHelper.ExecuteProcedure("select top 1 ftpip,userid,password from FTPMaster where IsStatus = 1");
                string ftpServerIP = dtFpt.Rows[0]["ftpip"].ToString(), ftpUserID = dtFpt.Rows[0]["userid"].ToString(), ftpPassword = dtFpt.Rows[0]["password"].ToString();

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + FTPFolder + "/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                if (DeviceType == "1")
                {
                    SaveData(filePath + "/" + fileName_new, DeviceDate, SubDeviceId, multiplier);
                }
                else if (DeviceType == "3")
                {
                    SaveData3(filePath + "/" + fileName_new, DeviceDate, SubDeviceId, multiplier);
                }
                else
                { SaveDataNew(filePath + "/" + fileName_new, DeviceDate, SubDeviceId, multiplier); }


                File.Delete(filePath + "/" + fileName_new);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                outputStream.Close();
                ErrorLog(ex, SubDeviceId);
                return false;
            }
        }

        private void SaveDataNew(string FilePath, string DeviceDate, string SubDeviceId, decimal multiplier)
        {
            string extension = Path.GetExtension(FilePath), AccessColumn = "", SerialNo = "", DeviceTotalCount = "";
            try
            {
                DataSet tempDs = GetCSVFileToDataTableNew("", FilePath, extension, out SerialNo);

                for (int ids = 0; ids < tempDs.Tables.Count; ids++)
                {
                    DataTable tempTable = tempDs.Tables[ids];
                    string ColNM = string.Empty, DelCol = string.Empty;
                    AccessColumn = string.Empty;

                    string[] SpSrNo = tempTable.TableName.Split(' ');

                    DataTable dt = SqlHelper.ExecuteProcedure("Select A.ColumnMappingID,A.MainColumnID,D.SubDeviceId,B.ColumnName,A.CSVFileColumnsID,C.CSVFileColumnName,D.SerialNo from ColumnMapping as A  join MainColumn as B on A.MainColumnID= B.MainColumnID join CSVFileColumns as C on A.CSVFileColumnsID = C.CSVFileColumnsID join MainDeviceMaster as F on  C.DeviceID = F.DeviceID join SubDeviceMaster as D on F.DeviceID = D.DeviceID where D.SerialNo = '" + SpSrNo[1].Trim() + "'  order by ColumnName");

                    if (dt.Rows.Count > 0)
                    {
                        SubDeviceId = dt.Rows[0]["SubDeviceId"].ToString();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try { AccessColumn += dt.Rows[i]["ColumnName"].ToString() + ","; tempTable.Columns[dt.Rows[i]["CSVFileColumnName"].ToString()].ColumnName = dt.Rows[i]["ColumnName"].ToString(); }
                            catch (Exception ex) { }
                        }
                        AccessColumn = AccessColumn.Remove(AccessColumn.Length - 1, 1);

                        DataTable dtCount = SqlHelper.ExecuteProcedure("select count(DeviceDataID) as TotalCount from DeviceData where SubDeviceId = '" + SubDeviceId + "' and DeviceDate = '" + DeviceDate + "' ");
                        DeviceTotalCount = dtCount.Rows[0]["TotalCount"].ToString();

                        for (int i = 0; i < tempTable.Columns.Count; i++)
                        {
                            ColNM = tempTable.Columns[i].ColumnName;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            { if (dt.Rows[j]["ColumnName"].ToString() == ColNM) { ColNM = ""; break; } }
                            DelCol += !string.IsNullOrEmpty(ColNM) ? ColNM + "," : "";
                        }

                        foreach (string str in DelCol.Split(','))
                            if (!string.IsNullOrEmpty(str)) tempTable.Columns.Remove(str);

                        string[,] para = new string[dt.Rows.Count + 2, 2];
                        decimal Val = 0;
                        para[0, 0] = "SubDeviceId";
                        para[0, 1] = SubDeviceId;
                        para[1, 0] = "DeviceDate";
                        para[1, 1] = DeviceDate;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        CultureInfo enUS = new CultureInfo("en-US");
                        int total = (tempTable.Rows.Count - int.Parse(DeviceTotalCount));
                        for (int i = 0; i < total; i++)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                para[j + 2, 0] = dt.Rows[j]["ColumnName"].ToString();
                                if (j == 0)
                                    para[j + 2, 1] = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                if (dt.Rows[j]["ColumnName"].ToString() == "DeviceDateTime")
                                {
                                    DateTime outDate = DateTime.Now;
                                    if (!DateTime.TryParseExact(tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), "yy-MM-dd HH:mm:ss", enUS, DateTimeStyles.None, out outDate))
                                    {
                                        outDate = DateTime.Now;
                                    }
                                    para[j + 2, 1] = outDate.ToString();
                                }
                                //para[j + 2, 1] = DateTime.ParseExact(tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), "yy-MM-dd HH:mm:ss", provider).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                {
                                    //if (tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString() == "Time")
                                    //{
                                    //    string a = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                    //}
                                    //if (dt.Rows[j]["ColumnName"].ToString().ToLower() == "eac")
                                    //{
                                    //    if(Val != 0)
                                    //        Val = (Val * Convert.ToDouble(multiplier));
                                    //}
                                    try
                                    {
                                        if (Decimal.TryParse(tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), out Val))
                                        {
                                            para[j + 2, 1] = Val.ToString();
                                        }
                                        else { para[j + 2, 1] = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(); }
                                    }
                                    catch
                                    {
                                        para[j + 2, 1] = string.Empty;
                                    }

                                }
                            }
                            string sqlQue = "insert into DeviceData(SubDeviceId,DeviceDate," + AccessColumn + ") values (@SubDeviceId,@DeviceDate,@" + AccessColumn.Replace(",", ",@") + ")";
                            //sqlQue = sqlQue.Replace("@Eac", "@Eac*" + multiplier);
                            SqlHelper.ExecuteNoneQueryWithPera(sqlQue, para);
                        }
                    }
                }
            }
            catch (Exception ex)
            { ErrorLog(ex, SubDeviceId); }

        }

        private void SaveData3(string FilePath, string DeviceDate, string SubDeviceId, decimal multiplier)
        {
            string extension = Path.GetExtension(FilePath), AccessColumn = "", SerialNo = "", DeviceTotalCount = "";
            try
            {
                DataTable tempTable = GetCSVFileToDataTable3("", FilePath, extension, out SerialNo);
                if (SerialNo != "")
                {
                    string ColNM = string.Empty, DelCol = string.Empty;
                    string[] SpSrNo = SerialNo.Trim().Split(' ');

                    SpSrNo = SpSrNo[1].Trim().Split(':');

                    DataTable dt = SqlHelper.ExecuteProcedure("Select A.ColumnMappingID,A.MainColumnID,B.ColumnName,A.CSVFileColumnsID,C.CSVFileColumnName,D.SerialNo from ColumnMapping as A  join MainColumn as B on A.MainColumnID= B.MainColumnID join CSVFileColumns as C on A.CSVFileColumnsID = C.CSVFileColumnsID join MainDeviceMaster as F on  C.DeviceID = F.DeviceID join SubDeviceMaster as D on F.DeviceID = D.DeviceID where D.SerialNo = '" + SpSrNo[1].Trim() + "'  order by ColumnName");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try { AccessColumn += dt.Rows[i]["ColumnName"].ToString() + ","; tempTable.Columns[dt.Rows[i]["CSVFileColumnName"].ToString()].ColumnName = dt.Rows[i]["ColumnName"].ToString(); }
                            catch (Exception ex) { }
                        }
                        AccessColumn = AccessColumn.Remove(AccessColumn.Length - 1, 1);

                        DataTable dtCount = SqlHelper.ExecuteProcedure("select count(DeviceDataID) as TotalCount from DeviceData where SubDeviceId = '" + SubDeviceId + "' and DeviceDate = '" + DeviceDate + "' ");
                        DeviceTotalCount = dtCount.Rows[0]["TotalCount"].ToString();

                        for (int i = 0; i < tempTable.Columns.Count; i++)
                        {
                            ColNM = tempTable.Columns[i].ColumnName;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            { if (dt.Rows[j]["ColumnName"].ToString() == ColNM) { ColNM = ""; break; } }

                            DelCol += !string.IsNullOrEmpty(ColNM) ? ColNM + "," : "";
                        }

                        foreach (string str in DelCol.Split(','))
                            if (!string.IsNullOrEmpty(str)) tempTable.Columns.Remove(str);

                        string[,] para = new string[dt.Rows.Count + 2, 2];
                        decimal Val = 0;
                        para[0, 0] = "SubDeviceId";
                        para[0, 1] = SubDeviceId;
                        para[1, 0] = "DeviceDate";
                        para[1, 1] = DeviceDate;
                        for (int i = int.Parse(DeviceTotalCount); i < tempTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                para[j + 2, 0] = dt.Rows[j]["ColumnName"].ToString();
                                //if (j == 0) 
                                //para[j + 2, 1] = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                if (dt.Rows[j]["ColumnName"].ToString() == "DeviceDateTime")
                                // para[j + 2, 1] = DeviceDate + " " + tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                {
                                    string tempdate1 = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();

                                    string[] parsed = tempdate1.Split(' ');

                                    para[j + 2, 1] = DeviceDate + " " + parsed[1];
                                }
                                else
                                {
                                    try
                                    {
                                        Decimal.TryParse(tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), out Val);
                                        if (dt.Rows[j]["ColumnName"].ToString().ToLower() == "eac")
                                        {
                                            if (Val != 0)
                                                Val = (Val * Convert.ToDecimal(multiplier));
                                        }
                                        para[j + 2, 1] = Val.ToString();
                                    }
                                    catch
                                    {
                                        para[j + 2, 1] = "0";
                                    }
                                }
                            }
                            string sqlQue = "insert into DeviceData(SubDeviceId,DeviceDate," + AccessColumn + ") values (@SubDeviceId,@DeviceDate,@" + AccessColumn.Replace(",", ",@") + ")";
                            //sqlQue = sqlQue.Replace("@Eac", "@Eac*" + multiplier);
                            SqlHelper.ExecuteNoneQueryWithPera(sqlQue, para);
                        }
                    }
                    else
                    {
                        CommanClass.SendMail(null, SpSrNo[1].Trim());
                    }
                }
            }
            catch (Exception ex)
            { ErrorLog(ex, SubDeviceId); }
        }

        private void SaveData(string FilePath, string DeviceDate, string SubDeviceId, decimal multiplier)
        {
            string extension = Path.GetExtension(FilePath), AccessColumn = "", SerialNo = "", DeviceTotalCount = "";
            try
            {
                DataTable tempTable = GetCSVFileToDataTable("", FilePath, extension, out SerialNo);
                if (SerialNo != "")
                {
                    string ColNM = string.Empty, DelCol = string.Empty;
                    string[] SpSrNo = SerialNo.Trim().Split(' ');
                    DataTable dt = SqlHelper.ExecuteProcedure("Select A.ColumnMappingID,A.MainColumnID,B.ColumnName,A.CSVFileColumnsID,C.CSVFileColumnName,D.SerialNo from ColumnMapping as A  join MainColumn as B on A.MainColumnID= B.MainColumnID join CSVFileColumns as C on A.CSVFileColumnsID = C.CSVFileColumnsID join MainDeviceMaster as F on  C.DeviceID = F.DeviceID join SubDeviceMaster as D on F.DeviceID = D.DeviceID where D.SerialNo = '" + SpSrNo[3].Trim() + "'  order by ColumnName");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try { AccessColumn += dt.Rows[i]["ColumnName"].ToString() + ","; tempTable.Columns[dt.Rows[i]["CSVFileColumnName"].ToString()].ColumnName = dt.Rows[i]["ColumnName"].ToString(); }
                            catch (Exception ex) { }
                        }
                        AccessColumn = AccessColumn.Remove(AccessColumn.Length - 1, 1);

                        DataTable dtCount = SqlHelper.ExecuteProcedure("select count(DeviceDataID) as TotalCount from DeviceData where SubDeviceId = '" + SubDeviceId + "' and DeviceDate = '" + DeviceDate + "' ");
                        DeviceTotalCount = dtCount.Rows[0]["TotalCount"].ToString();

                        for (int i = 0; i < tempTable.Columns.Count; i++)
                        {
                            ColNM = tempTable.Columns[i].ColumnName;
                            for (int j = 0; j < dt.Rows.Count; j++)
                            { if (dt.Rows[j]["ColumnName"].ToString() == ColNM) { ColNM = ""; break; } }

                            DelCol += !string.IsNullOrEmpty(ColNM) ? ColNM + "," : "";
                        }

                        foreach (string str in DelCol.Split(','))
                            if (!string.IsNullOrEmpty(str)) tempTable.Columns.Remove(str);

                        string[,] para = new string[dt.Rows.Count + 2, 2];
                        decimal Val = 0;
                        para[0, 0] = "SubDeviceId";
                        para[0, 1] = SubDeviceId;
                        para[1, 0] = "DeviceDate";
                        para[1, 1] = DeviceDate;
                        for (int i = int.Parse(DeviceTotalCount); i < tempTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                para[j + 2, 0] = dt.Rows[j]["ColumnName"].ToString();
                                //if (j == 0) 
                                //para[j + 2, 1] = tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                if (dt.Rows[j]["ColumnName"].ToString() == "DeviceDateTime")
                                    //para[j + 2, 1] = DateTime.ParseExact(DeviceDate + tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), "yyyy-MM-dd mm:ss", null).ToString("yyyy-MM-dd mm:ss");
                                    para[j + 2, 1] = DeviceDate + " " + tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString();
                                else
                                {
                                    try
                                    {
                                        Decimal.TryParse(tempTable.Rows[i][dt.Rows[j]["ColumnName"].ToString()].ToString(), out Val);
                                        if (dt.Rows[j]["ColumnName"].ToString().ToLower() == "eac")
                                        {
                                            if (Val != 0)
                                                Val = (Val * Convert.ToDecimal(multiplier));
                                        }
                                        para[j + 2, 1] = Val.ToString();
                                    }
                                    catch
                                    {
                                        para[j + 2, 1] = string.Empty;
                                    }
                                }
                            }
                            string sqlQue = "insert into DeviceData(SubDeviceId,DeviceDate," + AccessColumn + ") values (@SubDeviceId,@DeviceDate,@" + AccessColumn.Replace(",", ",@") + ")";
                            //sqlQue = sqlQue.Replace("@Eac", "@Eac*" + multiplier);
                            SqlHelper.ExecuteNoneQueryWithPera(sqlQue, para);
                        }
                    }
                    else
                    {
                        CommanClass.SendMail(null, SpSrNo[3].Trim());
                    }
                }
            }
            catch (Exception ex)
            { ErrorLog(ex, SubDeviceId); }
        }

        private static DataTable GetCSVFileToDataTable(string Query, string FilePath, string extension, out string SerialNo)
        {
            SerialNo = "";
            try
            {
                var Data = File.ReadAllText(FilePath);
                DataTable oDataTable = new DataTable();
                Data = Data.Replace("#", "\r\n").Replace("\r", "").Replace("\n", "#").Replace("\r\n", "#").Replace(";", ",");
                string[] StrVal = Data.Split('#');
                for (int i = 0; i < StrVal.Length; i++)
                {
                    if (!string.IsNullOrEmpty(StrVal[i]))
                    {
                        string[] RowVal = StrVal[i].Split(',');
                        if (i == 1)
                        { SerialNo = RowVal[0]; }
                        else if (i == 2)
                            for (int j = 0; j < RowVal.Length; j++)
                                oDataTable.Columns.Add(new DataColumn(RowVal[j]));
                        else
                        {
                            DataRow dr = oDataTable.NewRow();
                            for (int j = 0; j < RowVal.Length; j++)
                                dr[oDataTable.Columns[j]] = RowVal[j];
                            oDataTable.Rows.Add(dr);
                        }
                    }
                }
                return oDataTable;
            }
            catch (Exception ex)
            { return null; }
        }

        private static DataTable GetCSVFileToDataTable3(string Query, string FilePath, string extension, out string SerialNo)
        {
            SerialNo = "";
            try
            {
                var Data = File.ReadAllText(FilePath);
                DataTable oDataTable = new DataTable();
                Data = Data.Replace("#", "\r\n").Replace("\r", "").Replace("\n", "#").Replace("\r\n", "#").Replace(";", ",");
                string[] StrVal = Data.Split('#');
                for (int i = 0; i < StrVal.Length; i++)
                {
                    if (!string.IsNullOrEmpty(StrVal[i]))
                    {
                        string[] RowVal = StrVal[i].Split(',');
                        if (i == 1)
                        { SerialNo = RowVal[0]; }
                        else if (i == 3)
                            for (int j = 0; j < RowVal.Length; j++)
                                oDataTable.Columns.Add(new DataColumn(RowVal[j]));
                        else
                        {
                            DataRow dr = oDataTable.NewRow();
                            for (int j = 0; j < RowVal.Length; j++)
                                dr[oDataTable.Columns[j]] = RowVal[j];
                            oDataTable.Rows.Add(dr);
                        }
                    }
                }
                return oDataTable;
            }
            catch (Exception ex)
            { return null; }
        }

        private static DataSet GetCSVFileToDataTableNew(string Query, string FilePath, string extension, out string SerialNo)
        {
            SerialNo = "";
            string SubDeviceID = "";
            try
            {
                var Data = File.ReadAllText(FilePath);
                DataSet ds = new DataSet();
                string TableName = "";
                DataTable oDataTable = new DataTable();
                Data = Data.Replace("#", "\r\n").Replace("\r", "").Replace("\n", "#").Replace("\r\n", "#").Replace(";", ",");
                string[] StrVal = Data.Split('#');
                int DVRowVal = 0;
                for (int i = 0; i < StrVal.Length; i++)
                {
                    if (!string.IsNullOrEmpty(StrVal[i]))
                    {
                        string[] RowVal = StrVal[i].Split(',');
                        if (i == 1)
                        { SerialNo = RowVal[0]; }
                        else
                        {
                            if (RowVal[0].StartsWith("INV"))
                            {
                                DVRowVal = 1;
                                ds.Tables.Add(RowVal[0].ToString());
                                TableName = RowVal[0].ToString();
                            }
                            else
                            {
                                if (DVRowVal == 1)
                                {
                                    for (int j = 0; j < RowVal.Length; j++)
                                        ds.Tables[TableName].Columns.Add(new DataColumn(RowVal[j]));
                                    ds.Tables[TableName].Columns.Add(new DataColumn("OtherVal"));
                                    DVRowVal = 0;
                                }
                                else
                                {
                                    DataRow dr = ds.Tables[TableName].NewRow();
                                    for (int j = 0; j < RowVal.Length; j++)
                                        dr[ds.Tables[TableName].Columns[j]] = RowVal[j];
                                    ds.Tables[TableName].Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            { return null; }
        }

        private void ErrorLog(Exception ex, string SubDeviceId)
        {
            try
            {
                string[,] para = { { "SubDeviceId", SubDeviceId }, { "ErrorMessage", ex.Message.ToString() }, { "ErrorDetails", ex.ToString() } };
                string sqlQue = "insert into DeviceErrorLog(DeviceErrorLogID,SubDeviceID,ErrorMessage,ErrorDetails) values (NEWID(),@SubDeviceID,@ErrorMessage,@ErrorDetails)";
                SqlHelper.ExecuteNoneQueryWithPera(sqlQue, para);
                CommanClass.SendMail(ex, SubDeviceId);
            }
            catch (Exception ex1)
            { }
        }

        private string[] ListFiles()
        {
            try
            {
                DataTable dtFpt = SqlHelper.ExecuteProcedure("select top 1 ftpip,userid,password from FTPMaster where IsStatus = 1");
                string ftpServerIP = dtFpt.Rows[0]["ftpip"].ToString(), ftpUserID = dtFpt.Rows[0]["userid"].ToString(), ftpPassword = dtFpt.Rows[0]["password"].ToString();

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServerIP + "/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception)
            { return null; }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReadDataFTP() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}


/*
 * =================================
 *  TIGERFORGE UniREST Client v.3.5
 * ---------------------------------
 *           Core Engine
 * =================================
 */

using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using static UnityEngine.Networking.UnityWebRequest;

namespace TigerForge
{

    /// <summary>
    /// UniREST Client v.3.5
    /// </summary>
    public class UniRESTClient
    {

        #region " PROPERTIES "

        /// <summary>
        /// The Server folder to use.
        /// </summary>
        public enum Target
        {
            UserFolder,
            GameFolder
        }

        /// <summary>
        /// The Server root folder where to look for files.
        /// </summary>
        public enum TargetRoot
        {
            /// <summary>
            /// The User's dedicated folder where the created files are saved.
            /// </summary>
            UserFilesFolder,
            /// <summary>
            /// The User's dedicated folder where the uploaded files are stored.
            /// </summary>
            UserUploadsFolder,
            /// <summary>
            /// The Game's dedicated folder where the created files are saved.
            /// </summary>
            GameFilesFolder,
            /// <summary>
            /// The Game's dedicated folder where the uploaded files are stored.
            /// </summary>
            GameUploadsFolder
        }

        public class Account
        {
            public string username = "";
            public string password = "";
            public string useWordPressUsers = "";
            public string registrationDate = "";
            public string lastLoginDate = "";
        }

        private class GenericData
        {
            public string data1 = "";
            public string data2 = "";
            public string data3 = "";
            public string data4 = "";
            public string data5 = "";
            public string data6 = "";
        }

        /// <summary>
        /// Return the current Account's data: username, password, last login date and registration date.
        /// </summary>
        public static Account userAccount = new Account();

        /// <summary>
        /// Return the error code if the Login or Registration operation went wrong.
        /// </summary>
        public static string ServerError = "";

        /// <summary>
        /// The user ID of the logged in user.
        /// </summary>
        public static int UserID = 0;

        /// <summary>
        /// Retun true if there is a login session opened.
        /// </summary>
        public static bool isLoggedIn = false;

        /// <summary>
        /// When set to true (default), UniREST Client will save the user credentials locally. This will allow the use of automatic login features. If you don't want to have the user account locally saved, set this property to false, but automatic login won't work.
        /// </summary>
        public static bool rememberMe = true;

        /// <summary>
        /// When set to true, the UniREST client will send some debug information to the Unity Console.
        /// </summary>
        public static bool debugMode = false;

        /// <summary>
        /// Store all the debug messages when the debugMode is set to true.
        /// </summary>
        public static List<string> debug = new List<string>();

        /// <summary>
        /// Return the last error message from a Database operation or an empty string if everything is ok.
        /// </summary>
        public static string DBerror = "";

        /// <summary>
        /// Return the last SQL query executed by the Database.
        /// </summary>
        public static string DBquery = "";

        /// <summary>
        /// Return an integer value representing the result of a Database operation. The meaning of the value depends on the following operations (in the other cases, the value is always 0): 
        /// <para>Write : the value is the ID of the new record created into the Database table.</para>
        /// <para>Update : the value is the number of rows that have been updated.</para>
        /// <para>Delete : the value is the number of rows that have been deleted.</para>
        /// </summary>
        public static int DBresponse = 0;

        /// <summary>
        /// [DEPRECATED] If 'true' the UniREST Server will update the Reading Token everytime a reading operation is performed on the Database.
        /// <para>Note: you can keep it set to 'false' and use the ReadTokenUpdate() method for manually updating this token.</para>
        /// <para>Deprecation note: Tokens are automatically updated nomore. They must be manually updated.</para>
        /// </summary>
        // public static bool UpdateReadToken = false;

        /// <summary>
        /// [DEPRECATED] If 'true' the UniREST Server will update the Writing Token everytime a writing operation (Write, Update or Delete) is performed on the Database. 
        /// <para>Note: you can keep it set to 'false' and use the WriteTokenUpdate() method for manually updating this token.</para>
        /// <para>Deprecation note: Tokens are automatically updated nomore. They must be manually updated.</para>
        /// </summary>
        // public static bool UpdateWriteToken = false;

        #endregion


        #region " UNITYTOTEXT / TEXTTOUNITY "

        /// <summary>
        /// Convert a Unity data-type object in a string suitable to be saved in a Database table. It recognizes: Vector2, Vector3, Vector4, Quaternion, Transform, Color, Color32, Rect.
        /// </summary>
        public static string UnityToText(object data)
        {
            string type = data.GetType().ToString();
            if (!type.StartsWith("UnityEngine")) return "";

            List<float> converted = new List<float>();

            switch (type)
            {
                case "UnityEngine.Vector2":
                    Vector2 v2Data = (Vector2)data;
                    converted.Add(v2Data.x);
                    converted.Add(v2Data.y);

                    break;

                case "UnityEngine.Vector3":
                    Vector3 v3Data = (Vector3)data;
                    converted.Add(v3Data.x);
                    converted.Add(v3Data.y);
                    converted.Add(v3Data.z);

                    break;

                case "UnityEngine.Vector4":
                    Vector4 v4Data = (Vector4)data;
                    converted.Add(v4Data.x);
                    converted.Add(v4Data.y);
                    converted.Add(v4Data.z);
                    converted.Add(v4Data.w);

                    break;

                case "UnityEngine.Quaternion":
                    Quaternion qData = (Quaternion)data;
                    converted.Add(qData.x);
                    converted.Add(qData.y);
                    converted.Add(qData.z);
                    converted.Add(qData.w);

                    break;

                case "UnityEngine.Transform":
                    Transform trData = (Transform)data;
                    converted.Add(trData.position.x);
                    converted.Add(trData.position.y);
                    converted.Add(trData.position.z);
                    converted.Add(trData.localPosition.x);
                    converted.Add(trData.localPosition.y);
                    converted.Add(trData.localPosition.z);
                    converted.Add(trData.localScale.x);
                    converted.Add(trData.localScale.y);
                    converted.Add(trData.localScale.z);
                    converted.Add(trData.lossyScale.x);
                    converted.Add(trData.lossyScale.y);
                    converted.Add(trData.lossyScale.z);
                    converted.Add(trData.rotation.x);
                    converted.Add(trData.rotation.y);
                    converted.Add(trData.rotation.z);
                    converted.Add(trData.rotation.w);
                    converted.Add(trData.localRotation.x);
                    converted.Add(trData.localRotation.y);
                    converted.Add(trData.localRotation.z);
                    converted.Add(trData.localRotation.w);
                    converted.Add(trData.eulerAngles.x);
                    converted.Add(trData.eulerAngles.y);
                    converted.Add(trData.eulerAngles.z);
                    converted.Add(trData.localEulerAngles.x);
                    converted.Add(trData.localEulerAngles.y);
                    converted.Add(trData.localEulerAngles.z);

                    break;

                case "UnityEngine.Color":
                    Color clData = (Color)data;
                    converted.Add(clData.r);
                    converted.Add(clData.g);
                    converted.Add(clData.b);
                    converted.Add(clData.a);

                    break;

                case "UnityEngine.Color32":
                    Color32 cl32Data = (Color32)data;
                    converted.Add(cl32Data.r);
                    converted.Add(cl32Data.g);
                    converted.Add(cl32Data.b);
                    converted.Add(cl32Data.a);

                    break;

                case "UnityEngine.Rect":
                    Rect reData = (Rect)data;
                    converted.Add(reData.x);
                    converted.Add(reData.y);
                    converted.Add(reData.width);
                    converted.Add(reData.height);
                    converted.Add(reData.center.x);
                    converted.Add(reData.center.y);
                    converted.Add(reData.max.x);
                    converted.Add(reData.max.y);
                    converted.Add(reData.min.x);
                    converted.Add(reData.min.y);
                    converted.Add(reData.position.x);
                    converted.Add(reData.position.y);
                    converted.Add(reData.size.x);
                    converted.Add(reData.size.y);
                    converted.Add(reData.xMax);
                    converted.Add(reData.xMin);
                    converted.Add(reData.yMax);
                    converted.Add(reData.yMin);
                    break;

                default:
                    break;
            }

            var text = string.Join("|", converted);

            return text;
        }

        /// <summary>
        /// Convert a string containing a Unity object data to a Unity object of the given type. It recognizes: Vector2, Vector3, Vector4, Quaternion, Transform, Color, Color32, Rect.
        /// </summary>
        public static T TextToUnity<T>(string data)
        {
            string type = typeof(T).ToString();
            if (!type.StartsWith("UnityEngine")) return default;

            var tmp = data.Split('|');
            var dataList = new List<float>();
            foreach (var value in tmp) dataList.Add(float.Parse(value));

            switch (type)
            {
                case "UnityEngine.Vector2":
                    Vector2 v2 = new Vector2(dataList[0], dataList[1]);
                    return (T)Convert.ChangeType(v2, typeof(T));

                case "UnityEngine.Vector3":
                    Vector3 v3 = new Vector3(dataList[0], dataList[1], dataList[2]);
                    return (T)Convert.ChangeType(v3, typeof(T));

                case "UnityEngine.Vector4":
                    Vector4 v4 = new Vector4(dataList[0], dataList[1], dataList[2], dataList[3]);
                    return (T)Convert.ChangeType(v4, typeof(T));

                case "UnityEngine.Quaternion":
                    Quaternion q = new Quaternion(dataList[0], dataList[1], dataList[2], dataList[3]);
                    return (T)Convert.ChangeType(q, typeof(T));

                case "UnityEngine.Transform":
                    var go = new GameObject();
                    go.transform.position = new Vector3(dataList[0], dataList[1], dataList[2]);
                    go.transform.localPosition = new Vector3(dataList[3], dataList[4], dataList[5]);
                    go.transform.localScale = new Vector3(dataList[6], dataList[7], dataList[8]);
                    go.transform.rotation = new Quaternion(dataList[12], dataList[13], dataList[14], dataList[15]);
                    go.transform.localRotation = new Quaternion(dataList[16], dataList[17], dataList[18], dataList[19]);
                    go.transform.eulerAngles = new Vector3(dataList[20], dataList[21], dataList[22]);
                    go.transform.localEulerAngles = new Vector3(dataList[23], dataList[24], dataList[25]);
                    return (T)Convert.ChangeType(go.transform, typeof(T));

                case "UnityEngine.Color":
                    Color c = new Color(dataList[0], dataList[1], dataList[2], dataList[3]);
                    return (T)Convert.ChangeType(c, typeof(T));

                case "UnityEngine.Color32":
                    Color32 c32 = new Color32((byte)dataList[0], (byte)dataList[1], (byte)dataList[2], (byte)dataList[3]);
                    return (T)Convert.ChangeType(c32, typeof(T));

                case "UnityEngine.Rect":
                    Rect r = new Rect(dataList[0], dataList[1], dataList[2], dataList[3]);
                    r.center = new Vector2(dataList[4], dataList[5]);
                    r.max = new Vector2(dataList[6], dataList[7]);
                    r.min = new Vector2(dataList[8], dataList[9]);
                    r.position = new Vector2(dataList[10], dataList[11]);
                    r.size = new Vector2(dataList[12], dataList[13]);
                    r.xMax = dataList[14];
                    r.xMin = dataList[15];
                    r.yMax = dataList[16];
                    r.yMin = dataList[17];
                    return (T)Convert.ChangeType(r, typeof(T));

                default:
                    break;
            }

            return default;
        }

        #endregion


        #region " DOWNLOAD / UPLOAD "

        /// <summary>
        /// Initialize a file Download instance.
        /// </summary>
        public class Download
        {

            private UnityWebRequest www;
            private List<string> filesURL = new List<string>();
            private List<string> filesTARGET = new List<string>();
            private List<int> fileSizes = new List<int>();
            private DateTime lastTimestamp;
            private bool isStopped = false;

            /// <summary>
            /// Set the callback function to call when the download is completed.
            /// </summary>
            public UnityAction<Status> onCompleteCallBack = null;

            /// <summary>
            /// Set the callback function to call during the whole downloading process. The status property will contain the download process details.
            /// </summary>
            public UnityAction<Status> onProgessCallBack = null;

            /// <summary>
            /// Set the callback function to call when an error occurs. The status.error property will contain the system error message.
            /// </summary>
            public UnityAction<Status> onErrorCallBack = null;

            /// <summary>
            /// Set the delay that controls how often the download progress related informations are collected and released. By default, it is set to 250 milliseconds, so the progress informations are elaborated 4 times per second. 
            /// </summary>
            public int progressDelaySpeed = 250;

            public class Status
            {
                public string error = "";
                public int totalBytes = 0;
                public int totalFiles = 0;
                public float totalProgress = 0;
                public int currentFileIndex = 0;
                public int currentFileSize = 0;
                public float currentFileProgress = 0;
                public string currentDestionationFile = "";
                public string currentFileURL = "";
                public float speed = 0;

                /// <summary>
                /// The list of all downloaded files as byte arrays.
                /// </summary>
                public List<byte[]> filesData = new List<byte[]>();

                /// <summary>
                /// The list of all downloaded files as local paths.
                /// </summary>
                public List<string> filesPath = new List<string>();
            }

            /// <summary>
            /// Get some information about the current download status.
            /// </summary>
            public Status status = new Status();

            /// <summary>
            /// Initialize the Download engine.
            /// </summary>
            public Download()
            {
                 filesURL = new List<string>();
                 filesTARGET = new List<string>();
            }

            /// <summary>
            /// Add a direct link to a remote file to this Download queue.
            /// </summary>
            public void FromURL(string fileURL, string localDestinationFolder)
            {
                if (fileURL == "") { Console.Error("Try to add an empty URL to the Download queue."); return; }

                filesURL.Add(fileURL);
                filesTARGET.Add(GetDestinationFilePath(fileURL, localDestinationFolder));
            }

            /// <summary>
            /// Add a link to a remote file in the Server's Game folder to this Download queue.
            /// </summary>
            public void FromGame(string fileName, string localDestinationFolder)
            {
                if (fileName == "") { Console.Error("Try to add an empty file name to the Download queue."); return; }
                if (fileName.StartsWith("/")) fileName = fileName.Substring(1);

                var fileURL = UniRESTClientConfig.ServerUrl + UniRESTClientConfig.AppID + "GAME/" + fileName;
                filesURL.Add(fileURL);
                filesTARGET.Add(GetDestinationFilePath(fileURL, localDestinationFolder));
            }

            /// <summary>
            /// Add a link to a remote file in the user's dedicated folder to this Download queue.
            /// </summary>
            public void FromUser(string fileName, string localDestinationFolder)
            {
                if (UserID == 0) { Console.Error("The Download.FromUser method must be used only with a logged-in user."); return; }
                if (fileName == "") { Console.Error("Try to add an empty file name to the Download queue."); return; }
                if (fileName.StartsWith("/")) fileName = fileName.Substring(1);

                var fileURL = UniRESTClientConfig.ServerUrl + UniRESTClientConfig.AppID + "USER/" + UserID + "/uploads/" + fileName;
                filesURL.Add(fileURL);
                filesTARGET.Add(GetDestinationFilePath(fileURL, localDestinationFolder));
            }

            /// <summary>
            /// Start the download of the collected files.
            /// </summary>
            public void Start()
            {
                isStopped = false;
                GetFilesSizes(0);
            }


            /// <summary>
            /// Stop the downloading. This may throw a generic, non-blocking, "Curl error 23" error log message.
            /// </summary>
            public void Stop()
            {
                try
                {
                    if (isStopped) return;
                    isStopped = true;
                    www.Abort();
                    www.Dispose();
                }
                catch (Exception) { }
            }

            private void GetFilesSizes(int index)
            {
                status.filesPath = new List<string>();
                www = UnityWebRequest.Head(filesURL[index]);
                www.SendWebRequest();

                GetFileSize(index);
            }

            private async void GetFileSize(int index)
            {
                await Task.Delay(250);

                if (IsError())
                {
                    status.error = www.error;
                    if (onErrorCallBack != null) onErrorCallBack.Invoke(status);
                }
                else if (www.isDone)
                {
                    string strSize = www.GetResponseHeader("Content-Length");
                    fileSizes.Add(int.Parse(strSize));
                    index++;
                    if (index < filesURL.Count) GetFilesSizes(index); else StartDownloadQueue();
                }
                else
                {
                    GetFileSize(index);
                }
            }

            private void StartDownloadQueue()
            {
                status.totalBytes = 0;
                status.totalProgress = 0;
                status.totalFiles = fileSizes.Count;
                foreach (int size in fileSizes) status.totalBytes += size;
                
                SingleFileDownload(0);
            }

            private void SingleFileDownload(int index)
            {
                status.currentDestionationFile = "";
                status.currentFileURL = filesURL[index];
                status.currentFileIndex = index + 1;
                status.currentFileSize = fileSizes[index];
                lastTimestamp = DateTime.Now;

                www = new UnityWebRequest(filesURL[index]);

                if (filesTARGET[index] == "") {
                    www.downloadHandler = new DownloadHandlerBuffer();
                } else {
                    status.currentDestionationFile = filesTARGET[index];
                    status.filesPath.Add(status.currentDestionationFile);
                    www.downloadHandler = new DownloadHandlerFile(status.currentDestionationFile);
                }

                www.SendWebRequest();
               
                RunDownload(index);
            }

            private async void RunDownload(int index)
            {
                await Task.Delay(progressDelaySpeed);

                try
                {

                    if (IsError())
                    {
                        status.error = www.error;
                        if (onErrorCallBack != null) onErrorCallBack.Invoke(status);
                    }
                    else if (www.isDone)
                    {
                        index++;
                        if (index < filesURL.Count)
                        {
                            if (filesTARGET[index] == "") status.filesData.Add(www.downloadHandler.data);
                            SingleFileDownload(index);
                        }
                        else
                        {
                            if (filesTARGET[index - 1] == "") status.filesData.Add(www.downloadHandler.data);
   
                            if (onCompleteCallBack != null) onCompleteCallBack.Invoke(status);

                            www.Dispose();
                        }
                    }
                    else
                    {
                        status.currentFileProgress = www.downloadProgress;

                        float percStep = 100 / status.totalFiles;
                        status.totalProgress = ((percStep * index) + (percStep * status.currentFileProgress)) / 100;

                        status.speed = (float)(www.downloadedBytes / (DateTime.Now - lastTimestamp).TotalSeconds);
                        if (onProgessCallBack != null) onProgessCallBack.Invoke(status);
                        RunDownload(index);
                    }

                }
                catch (Exception)
                {
                }
                            
            }

            private string GetDestinationFilePath(string fileURL, string destinationFolder)
            {
                if (destinationFolder == "") return "";

                Uri uri = new Uri(fileURL);
                string filename = Path.GetFileName(uri.LocalPath);
                return Path.Combine(destinationFolder, filename);
            }

            /// <summary>
            /// Return true if there is a communication error (Unity version dependent)
            /// </summary>
            private bool IsError()
            {
#if UNITY_2020_1_OR_NEWER
                return www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.DataProcessingError || www.result == UnityWebRequest.Result.ProtocolError;
#else
                return www.isNetworkError || www.isHttpError;
#endif
            }

        }

        /// <summary>
        /// Initialize a file Upload instance.
        /// </summary>
        public class Upload
        {

            private UnityWebRequest www;
            private List<string> filesURL = new List<string>();
            private List<string> filesTARGET = new List<string>();
            private List<int> fileSizes = new List<int>();
            private DateTime lastTimestamp;
            private bool isStopped = false;

            /// <summary>
            /// Set the callback function to call when the upload is completed.
            /// </summary>
            public UnityAction<Status> onCompleteCallBack = null;

            /// <summary>
            /// Set the callback function to call during the whole downloading process. The status property will contain the upload process details.
            /// </summary>
            public UnityAction<Status> onProgessCallBack = null;

            /// <summary>
            /// Set the callback function to call when an error occurs. The status.error property will contain the system error message.
            /// </summary>
            public UnityAction<Status> onErrorCallBack = null;

            /// <summary>
            /// Set the delay that controls how often the upload progress related informations are collected and released. By default, it is set to 250 milliseconds, so the progress informations are elaborated 4 times per second. 
            /// </summary>
            public int progressDelaySpeed = 250;

            public class Status
            {
                public string error = "";
                public int totalBytes = 0;
                public int totalFiles = 0;
                public float totalProgress = 0;
                public int currentFileIndex = 0;
                public int currentFileSize = 0;
                public float currentFileProgress = 0;
                public string currentDestionationFile = "";
                public string currentFileURL = "";
                public float speed = 0;
            }

            /// <summary>
            /// Get some information about the current upload status.
            /// </summary>
            public Status status = new Status();

            /// <summary>
            /// Initialize the Upload engine.
            /// </summary>
            /// <param name="localFiles"></param>
            public Upload()
            {
                filesURL = new List<string>();
                filesTARGET = new List<string>();
                fileSizes = new List<int>();
            }

            /// <summary>
            /// Stop the uploading. This may throw a generic, non-blocking, error log message.
            /// </summary>
            public void Stop()
            {
                try
                {
                    if (isStopped) return;
                    isStopped = true;
                    www.Abort();
                    www.Dispose();
                }
                catch (Exception) { }
            }

            /// <summary>
            /// Start the upload of the collected files.
            /// </summary>
            public void Start()
            {
                isStopped = false;
                status.totalBytes = 0;
                status.totalFiles = filesURL.Count;

                foreach (var file in filesURL)
                {
                    var size = (int)new FileInfo(file).Length;
                    fileSizes.Add(size);
                    status.totalBytes += size;
                }

                StartUploadQueue(0);
            }

            /// <summary>
            /// Add a file to send to the Server's Game folder to this Upload queue.
            /// </summary>
            public void ToGame(string localFileName, string destinationFolder = "")
            {
                if (localFileName == "") { Console.Error("Try to add an empty file name to the Upload queue."); return; }
                if (destinationFolder != "" && destinationFolder.StartsWith("/")) destinationFolder = destinationFolder.Substring(1);
                if (destinationFolder != "" && !destinationFolder.EndsWith("/")) destinationFolder += "/";

                var remoteTarget = UniRESTClientConfig.AppID + "GAME/" + destinationFolder;
                filesTARGET.Add(remoteTarget);

                filesURL.Add(localFileName);
            }

            /// <summary>
            /// Add a file to send to the user's dedicated folder to this Upload queue.
            /// </summary>
            public void ToUser(string localFileName, string destinationFolder = "")
            {
                if (UserID == 0) { Console.Error("The Upload.ToUser method must be used only with a logged-in user."); return; }
                if (localFileName == "") { Console.Error("Try to add an empty file name to the Upload queue."); return; }
                if (destinationFolder != "" && destinationFolder.StartsWith("/")) destinationFolder = destinationFolder.Substring(1);
                if (destinationFolder != "" && !destinationFolder.EndsWith("/")) destinationFolder += "/";

                var remoteTarget = UniRESTClientConfig.AppID + "USER/" + UserID + "/uploads/" + destinationFolder;
                filesTARGET.Add(remoteTarget);

                filesURL.Add(localFileName);
            }


            private void StartUploadQueue(int index)
            {
                if (index >= filesURL.Count) return;

                var data = System.IO.File.ReadAllBytes(filesURL[index]);

                WWWForm form = new WWWForm();
                form.AddField("folder", filesTARGET[index]);
                form.AddField("code", EncryptionHelper.Encrypt(UniRESTClientConfig.AppID));
                form.AddBinaryData("files[]", data, GetFileName(filesURL[index]));

                www = UnityWebRequest.Post(Rest.GetUrlToServer("unirestclientserver/fileuploader/"), form);
                www.SendWebRequest();

                lastTimestamp = DateTime.Now;
                status.currentDestionationFile = filesTARGET[index] + GetFileName(filesURL[index]);
                status.currentFileURL = filesURL[index];
                status.currentFileIndex = index + 1;
                status.currentFileSize = fileSizes[index];

                RunUpload(index);
            }

            private async void RunUpload(int index)
            {
                await Task.Delay(progressDelaySpeed);

                if (IsError())
                {
                    status.error = www.error;
                    if (onErrorCallBack != null) onErrorCallBack.Invoke(status);
                }
                else if (www.isDone)
                {
                    index++;
                    if (index < filesURL.Count)
                    {
                        StartUploadQueue(index);
                    }
                    else
                    {
                        if (onCompleteCallBack != null) onCompleteCallBack.Invoke(status);
                        www.Dispose();
                    }
                }
                else
                {
                    status.currentFileProgress = www.uploadProgress;

                    float percStep = 100 / status.totalFiles;
                    status.totalProgress = ((percStep * index) + (percStep * status.currentFileProgress)) / 100;

                    status.speed = (float)(www.uploadedBytes / (DateTime.Now - lastTimestamp).TotalSeconds);
                    if (onProgessCallBack != null) onProgessCallBack.Invoke(status);
                    RunUpload(index);
                }
            }


            private string GetFileName(string url)
            {
                Uri uri = new Uri(url);
                return Path.GetFileName(uri.LocalPath);
            }

            /// <summary>
            /// Return true if there is a communication error (Unity version dependent)
            /// </summary>
            private bool IsError()
            {
#if UNITY_2020_1_OR_NEWER
                return www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.DataProcessingError || www.result == UnityWebRequest.Result.ProtocolError;
#else
                return www.isNetworkError || www.isHttpError;
#endif
            }

        }

        #endregion


        #region " OTP "

        /// <summary>
        /// Generate an OTP (One-time password) linked to the given username. This OTP code can be successively used with ChangePasswordOTP() and WPChangePasswordOTP() methods for changing the user's password.
        /// <para>username : the user's username.</para>
        /// <para>callback (bool, string) : the callback function to call when the operation is completed. It must contain a boolean and a string parameters (the result of the operation and the generated OTP).</para>
        /// </summary>
        public async static UniTask GenerateOTP(string username, Action<bool, string> callBack)
        {
            var response = await RestAsync.Http("unirestclientuser/generateotp/", new GenericData
            {
                data1 = username,
            });

            var OTP = "";

            if (response.isArrived)
            {
                if (response.StatusIsOK())
                {
                    OTP = response.GetResponse();
                    ServerError = "";
                }
                else
                {
                    ServerError = "ERROR";
                }
            }
            else
            {
                ServerError = "NO_COMMUNICATION";
            }

            callBack?.Invoke(ServerError == "", OTP);
        }

        #endregion


        #region " ASYNC "

        /// <summary>
        /// A class for performing asynchronous operations.
        /// </summary>
        public class Async
        {
            #region " - LOGIN / REGISTRATION "

            #region " Login "

            /// <summary>
            /// [UNIREST CLIENT] This override is for internal use only.
            /// </summary>
            public async static UniTask Login(string username, string password, System.Action<bool> onComplete, bool _ut)
            {
                isLoggedIn = false;

                if (await ServerIsConnected())
                {
                    userAccount = new Account { username = username, password = password };

                    var response = await RestAsync.Http("unirestclientuser/login/", new Account
                    {
                        username = username,
                        password = EncryptionHelper.Sha256(password),
                        useWordPressUsers = "",
                        lastLoginDate = (_ut) ? "" : "_NO_TOKEN_CHANGE_"
                    });

                    if (response.isArrived && response.StatusIsOK())
                    {
                        var tokens = response.GetDecryptedResponse().Split('|');
                        if (tokens.Length >= 4)
                        {
                            RestAsync.UpdateTokens(tokens[0], tokens[1], tokens[2], tokens[3]);
                            UserID = int.Parse(tokens[3]);
                            userAccount.lastLoginDate = tokens[4];
                            userAccount.registrationDate = tokens[5];
                            ServerError = "";
                            isLoggedIn = true;
                            if (rememberMe) AccountSave();
                        }
                        else
                        {
                            userAccount = new Account();
                            ServerError = "NO_TOKENS";
                        }
                    }
                    else
                    {
                        userAccount = new Account();
                        ServerError = "NO_ACCOUNT";
                    }
                }
                else
                {
                    userAccount = new Account();
                    // ServerError already contains an error "code".
                }

                onComplete?.Invoke(ServerError == "");
                
            }

            /// <summary>
            /// Perform a Login operation with the given username and password, opening a secure connection with the UniREST Server. Once the operation is completed, the OnComplete callback function will be called.
            /// <para>username : the account's username.</para>
            /// <para>password : the account's password.</para>
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. If login succeeds, this method initializes the UserID property with the unique ID from the Database and the userAccount property with some user details. In case of error, the ServerError property contains the error string code.</para>
            /// </summary>
            public async static UniTask Login(string username, string password, System.Action<bool> onComplete)
            {
                await Login(username, password, onComplete, true);
            }

            /// <summary>
            /// Register this Unity Game or Application and automatically perform the login. Use this method if this Unity Project doesn't require users account, but the UniREST Solution functionalities only.
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. In case of error, the ServerError property contains the error string code.</para>
            /// </summary>
            /// <returns></returns>
            public async static UniTask ApplicationLogin(Action<bool> onComplete)
            {
                var appUsername = UniRESTClientConfig.AppID;
                var appPassword = UniRESTClientConfig.Key1.Substring(4, 8) + UniRESTClientConfig.Key2.Substring(4, 8);

                await Login(appUsername, appPassword, async (bool loggedIn) => 
                {
                    if (loggedIn)
                    {
                        onComplete?.Invoke(true);
                    }
                    else
                    {
                        await Registration(appUsername, appPassword, async (bool registered) =>
                        {
                            if (registered)
                            {
                                await Login(appUsername, appPassword, (bool loggedIn2) => { onComplete?.Invoke(loggedIn2); }, false);
                            }
                            else
                            {
                                onComplete?.Invoke(false);
                            }
                        });
                    }
                }, false);
            }

            /// <summary>
            /// Perform a new login attempt with the username and password from a previously successful login.
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. In case of error, the ServerError property contains the error string code.</para>
            /// </summary>
            public async static UniTask Relogin(Action<bool> onComplete)
            {
                if (userAccount.username == "" || userAccount.password == "") onComplete?.Invoke(false);

                await Login(userAccount.username, userAccount.password, (bool loggedIn) => { onComplete?.Invoke(loggedIn); });
            }

            /// <summary>
            /// Perform a user login using the locally saved username and password. In order to use this method, the rememberMe property must be set to true.
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. In case of error, the ServerError property contains the error string code.</para>
            /// <para>autoRegistration (optional) : if set to TRUE, it generates a new account (if it doesn't exist), saves it locally and finally performs a login.</para>
            /// </summary>
            /// <returns></returns>
            public async static UniTask AutoLogin(Action<bool> onComplete, bool autoRegistration = false)
            {
                var account = AccountLoad();
                if (account.Count == 0)
                {
                    if (autoRegistration)
                    {
                        userAccount = new Account { username = GetUID(), password = GetUID() };
                        await Registration(userAccount.username, userAccount.password, async (bool registered) => 
                        {
                            if (registered)
                            {
                                await Login(userAccount.username, userAccount.password, (bool loggedIn) => { onComplete?.Invoke(loggedIn); });
                            }
                            else
                            {
                                onComplete?.Invoke(false);
                            }
                        });
                    }
                    else
                    {
                        onComplete?.Invoke(false);
                    }
                }
                else
                {
                    await Login(account["username"], account["password"], (bool loggedIn) => { onComplete?.Invoke(loggedIn); });
                }
            }


            /// <summary>
            /// Perform a Login operation in WordPress with the given username and password, opening a secure connection with the UniREST Server. Once the operation is completed, the OnComplete callback function will be called.
            /// <para>username : the WordPress account's username.</para>
            /// <para>password : the WordPress account's password.</para>
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. If login succeeds, this method initializes the UserID property with the unique ID from the Database and the userAccount property with some user details. In case of error, the ServerError property contains the error string code.</para>
            /// </summary>
            public async static UniTask WPLogin(string username, string password, Action<bool> onComplete)
            {
                isLoggedIn = false;

                if (await ServerIsConnected())
                {
                    userAccount = new Account { username = username, password = password };

                    var response = await RestAsync.Http("unirestclientuser/wplogin/", new Account
                    {
                        username = username,
                        password = EncryptionHelper.Sha256(password),
                        useWordPressUsers = password
                    });

                    if (response.isArrived)
                    {
                        var tokens = response.GetDecryptedResponse().Split('|');
                        if (tokens.Length >= 4)
                        {
                            RestAsync.UpdateTokens(tokens[0], tokens[1], tokens[2], tokens[3]);
                            UserID = int.Parse(tokens[3]);
                            userAccount.lastLoginDate = tokens[4];
                            userAccount.registrationDate = tokens[5];
                            ServerError = "";
                            isLoggedIn = true;
                            if (rememberMe) AccountSave();
                        }
                        else
                        {
                            userAccount = new Account();
                            ServerError = (tokens.Length > 0) ? tokens[0] : "NO_TOKENS";
                        }
                    }
                    else
                    {
                        userAccount = new Account();
                        ServerError = "NO_ACCOUNT";
                    }
                }
                else
                {
                    userAccount = new Account();
                    // ServerError already contains an error "code".
                }

                onComplete?.Invoke(ServerError == "");
            }

            #endregion

            #region " Registration "

            /// <summary>
            /// Perform the registration of a new account in the UniREST Server, with the given username and password. If the request is valid, a new user account is added to the Database.
            /// <para>username : the account's username.</para>
            /// <para>password : the account's password.</para>
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. In case of error, the ServerError property contains the error string code.</para>
            /// </summary>
            public async static UniTask Registration(string username, string password, Action<bool> onComplete)
            {
                DBresponse = 0;

                if (await ServerIsConnected())
                {
                    var response = await RestAsync.Http("unirestclientuser/registration/", new Account
                    {
                        username = username,
                        password = EncryptionHelper.Sha256(password),
                        useWordPressUsers = ""
                    });

                    if (response.isArrived)
                    {
                        if (response.StatusIsOK())
                        {
                            DBresponse = response.GetResponseAsInt();
                            ServerError = "";
                        }
                        else
                        {
                            DBresponse = 0;
                            ServerError = "INVALID";
                        }
                    }
                    else
                    {
                        ServerError = "CONNECTION_ERROR";
                    }
                }
                else
                {
                    // ServerError already contains an error "code".
                }

                onComplete?.Invoke(ServerError == "");
            }

            /// <summary>
            /// Perform the registration of a new account both in WordPress and in the UniREST Server, with the given username and password. If the request is valid, a new user account is created and accessible in the WordPress login page and using the WPLogin() method.
            /// <para>username : the account's username.</para>
            /// <para>password : the account's password.</para>
            /// <para>OnComplete(bool) : the callback function when the operation is completed. A returned boolean value is set to true on succeed, false on error. In case of error, the ServerError property contains the error string code.</para>
            /// <para>NOTE: in WordPress, the account is registered with just the provided username and password. Other data, as email or user role, is not included. For this reason, the use of this method is discouraged.</para>
            /// </summary>
            public async static Task WPRegistration(string username, string password, Action<bool> onComplete)
            {
                DBresponse = 0;

                if (await ServerIsConnected())
                {
                    var response = await RestAsync.Http("unirestclientuser/wpregistration/", new Account
                    {
                        username = username,
                        password = EncryptionHelper.Sha256(password),
                        useWordPressUsers = password
                    });

                    if (response.isArrived)
                    {
                        ServerError = "";
                        DBresponse = response.GetResponseAsInt();
                    }
                    else
                    {
                        ServerError = response.GetResponse();
                    }
                }
                else
                {
                    // ServerError already contains an error "code".
                }

                onComplete?.Invoke(ServerError == "");
            }

            #endregion

            #region " Server Connection "

            /// <summary>
            /// Call the Server for opening a communication channel. Then, ask the Server for authorization. If everything is correct, the communication Client/Server starts.
            /// <para>The method just return TRUE (communication started) or FALSE (communication failed. ServerError: NO_AUTH, CALL_ERROR, NO_COMMUICATION).</para>
            /// </summary>
            private async static UniTask<bool> ServerIsConnected()
            {
                ServerError = "";

                var response = await RestAsync.Http("unirestclientserver/call/", new Response());
                if (response.isArrived)
                {
                    var data = response.GetDecryptedResponseAsInt() + 30;

                    response = await RestAsync.Http("unirestclientserver/connect/", new Response { data = data.ToString() });
                    ServerError = (response.isArrived && response.StatusIsOK()) ? "" : "NO_AUTH";
                }
                else
                {
                    ServerError = (response.errorType == RestAsync.HttpResponse.Error.HTTP_ERROR) ? "CALL_ERROR" : "NO_COMMUNICATION";
                }

                return ServerError == "";
            }

            #endregion

            #region " Logout "

            /// <summary>
            /// Perform the User or Application logout. The security Tokens are resetted and all the communications with the Server are interrupted.
            /// </summary>
            public static void Logout()
            {
                RestAsync.UpdateTokens("", "", "", "");
                UserID = 0;
                userAccount = new Account();
                DBerror = "";
                DBquery = "";
                DBresponse = 0;
                ServerError = "";

                isLoggedIn = false;
            }

            #endregion

            #endregion

            #region " - READONE / READALL "

            /// <summary>
            /// Perform a reading operation on the UniREST Server. If the request is valid, the method returns one single data record from the Database.
            /// </summary>
            /// <returns></returns>
            public async static UniTask ReadOne<TABLE>(string api, object tableData, Action<TABLE, bool> callBack)
            {
                var response = await RestAsync.Action(api, "READ", tableData, "");
                
                DBresponse = 0;

                try
                {
                    if (response.reply.DataIsEmptyJson())
                    {
                        Console.Info("The ReadOne() method call returned no results or the result is an array. The ReadOne() method requires that the 'One Record Only' option is checked in the API read settings.");
                        callBack?.Invoke(default, false);
                    }
                    else
                    {
                        var data = JsonUtility.FromJson<TABLE>(response.reply.data);
                        callBack?.Invoke(data, true);
                    }
                }
                catch (Exception)
                {
                    Console.Info("The ReadOne() method call returned an invalid response. The ReadOne() method requires that the 'One Record Only' option is checked in the API read settings.");
                    callBack?.Invoke(default, false);
                }
            }

            /// <summary>
            /// Perform a reading operation on the UniREST Server. If the request is valid, the method returns an array of data records from the Database.
            /// </summary>
            public async static UniTask Read<TABLE>(string api, object tableData, Action<TABLE[]> callBack)
            {
                var response = await RestAsync.Action(api, "READ", tableData, "");

                DBresponse = 0;

                try
                {
                    if (response.reply.DataIsEmptyJson())
                    {
                        Console.Info("The Read() method call returned no results. Check if the called API is correctly configured (for example, if the API's ''One record only'' option is checked, you must use the ReadOne() method).");
                        callBack?.Invoke(default);
                    }
                    else
                    {
                        TABLE[] json = JsonHelper.getJsonArray<TABLE>(response.reply.data);
                        if (json.Length == 0) Console.Info("The Read() method call returned no results.");
                        callBack?.Invoke(json);
                    }
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    Console.Info("The Read() method call returned no results. Check if the called API is correctly configured (for example, if the API's ''One record only'' option is checked, you must use the ReadOne() method).");
                    callBack?.Invoke(default);
                }
            }

            #endregion

            #region " - WRITE / UPDATE "

            /// <summary>
            /// Perform a writing operation on the UniREST Server. If the request is valid, a new data record is added to the Database and the method returns true.
            /// </summary>
            public async static UniTask Write(string api, object tableData, Action<bool> callBack)
            {
                var response = await RestAsync.Action(api, "WRITE", tableData, "");

                DBresponse = 0;

                if (response.result == "OK")
                {
                    int.TryParse(response.reply.data, out DBresponse);
                    if (DBresponse == 0) Console.Info("The Write() method call performed no operation.");
                }

                callBack?.Invoke(response.result == "OK");
            }

            /// <summary>
            /// Perform an updating operation on the UniREST Server. If the request is valid, the existing Database record is updated with the new provided data record and the method returns true.
            /// By default, all fields are updated. If you want to update just some fields, list the fields to update in the fieldsToUpdate optional parameter (field names must be separated by a space).
            /// </summary>
            public async static UniTask Update(string api, object tableData, Action<bool> callBack, string fieldsToUpdate = "")
            {
                var response = await RestAsync.Action(api, "UPDATE", tableData, fieldsToUpdate);

                DBresponse = 0;

                if (response.result == "OK")
                {
                    int.TryParse(response.reply.data, out DBresponse);
                    if (DBresponse == 0) Console.Info("The Update() method call performed no operation.");
                }

                callBack?.Invoke(response.result == "OK");
            }

            #endregion

            #region " - DELETE "

            /// <summary>
            /// Perform a deleting operation on the UniREST Server. If the request is valid, the Database records located by the API condition are deleted and the method returns true.
            /// </summary>
            public async static UniTask Delete(string api, object tableData, Action<bool> callBack)
            {
                var response = await RestAsync.Action(api, "DELETE", tableData, "");

                DBresponse = 0;

                if (response.result == "OK")
                {
                    int.TryParse(response.reply.data, out DBresponse);
                    if (DBresponse == 0) Console.Info("The Delete() method call performed no operation.");
                }

                callBack?.Invoke(response.result == "OK");
            }

            #endregion

            #region " - CALL (Custom PHP) "

            /// <summary>
            /// Call a Custom PHP script. This method expect to receive an array of data.
            /// </summary>
            public async static UniTask Call<DATA>(string api, object customData, Action<DATA[]> callBack)
            {
                var response = await RestAsync.Action(api, "", customData, "");

                DBresponse = 0;

                try
                {
                    DATA[] json = JsonHelper.getJsonArray<DATA>(response.reply.data);
                    if (json.Length == 0) Console.Info("The Call() method returned no results.");
                    callBack?.Invoke(json);
                }
                catch (Exception e)
                {
                    Console.Info("The Call() method returned no results: " + e.Message);
                    callBack?.Invoke(default);
                }
            }

            /// <summary>
            /// Call a Custom PHP script. This method expect to receive a single record of data.
            /// </summary>
            public async static UniTask CallOne<DATA>(string api, object customData, Action<DATA> callBack)
            {
                var response = await RestAsync.Action(api, "", customData, "");

                DBresponse = 0;

                try
                {
                    var data = JsonUtility.FromJson<DATA>(response.reply.data);
                    callBack?.Invoke(data);
                }
                catch (Exception e)
                {
                    Console.Info("The CallOne() method returned no results: " + e.Message);
                    callBack?.Invoke(default);
                }
            }

            #endregion

            #region " - SPECIAL OPERATIONS "

            /// <summary>
            /// Update the value (int or float) of a specific record and column using the given math formula.
            /// <para>api : the API to call.</para>
            /// <para>tabelData : the values for locating a specific record.</para>
            /// <para>fieldToUpdate : the column name to update.</para>
            /// <para>formula: a valid math formula for recalculating the column value.</para>
            /// </summary>
            public async static UniTask UpdateMath(string api, object tableData, string fieldToUpdate, string formula, Action<float, bool> callBack)
            {
                var response = await RestAsync.Action(api, "MATH", tableData, fieldToUpdate + "|" + formula);

                DBresponse = 0;
                DBerror = "";

                if (response.reply.data.Contains("ERROR"))
                {
                    var tmp = response.reply.data.Split('|');
                    DBerror = tmp[1];
                    var data = float.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    callBack?.Invoke(data, false);
                }
                else
                {
                    var data = float.Parse(response.reply.data, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    callBack?.Invoke(data, true);
                }

                
            }

            /// <summary>
            /// Update the JSON content of a specific record and column using the given Json data.
            /// <para>api : the API to call.</para>
            /// <para>tabelData : the values for locating a specific record.</para>
            /// <para>fieldToUpdate : the column name to update.</para>
            /// <para>formula: a Json data. Existing keys will be updated with the given values, whereas new keys will be added.</para>
            /// </summary>
            public async static UniTask UpdateJSON(string api, object tableData, string fieldToUpdate, Json jsonData, Action<bool> callBack)
            {
                var response = await RestAsync.Action(api, "UPDATEJSON", tableData, fieldToUpdate + "|" + jsonData.ToString());

                DBresponse = 0;

                if (response.result == "OK")
                {
                    int.TryParse(response.reply.data, out DBresponse);
                    if (DBresponse == 0) Console.Info("The Update() method call performed no operation.");
                }

                callBack?.Invoke(response.result == "OK");
            }

            /// <summary>
            /// Check if records with the given values exist in the API's table. Return true if at least one record exists.
            /// <para>api : the API to call.</para>
            /// <para>tabelData : the values to check into the table.</para>
            /// <para>fieldsToCheck : the columns where to look for the given values. Column names must be listed separated by a space.</para>
            /// </summary>
            /// <param name="api"></param>
            /// <param name="tableData"></param>
            /// <param name="fieldsToCheck"></param>
            /// <returns></returns>
            public async static UniTask Exists(string api, object tableData, string fieldsToCheck, Action<bool> callBack)
            {
                var response = await RestAsync.Action(api, "EXISTS", tableData, fieldsToCheck);

                DBresponse = 0;

                callBack?.Invoke(response.reply.data == "1");
            }

            /// <summary>
            /// Return the number of records that contain the given values.
            /// <para>api : the API to call.</para>
            /// <para>tabelData : the values to check into the table.</para>
            /// <para>fieldsToCheck : the columns where to look for the given values. Column names must be listed separated by a space.</para>
            /// </summary>
            public async static UniTask Count(string api, object tableData, string fieldsToCheck, Action<int> callBack)
            {
                var response = await RestAsync.Action(api, "COUNT", tableData, fieldsToCheck);

                DBresponse = 0;

                var data = int.Parse(response.reply.data);
                callBack?.Invoke(data);
            }

            #endregion

            #region " - CHANGE PASSWORD "

            /// <summary>
            /// Change the account password.
            /// <para>username : the username to update with the new password.</para>
            /// <para>oldPassword : the password to change.</para>
            /// <para>newPassword : the new password.</para>
            /// </summary>
            public async static UniTask ChangePassword(string username, string oldPassword, string newPassword, Action<bool> callBack)
            {
                var result = await ChangeMyPassword(username, oldPassword, newPassword, false);
                callBack?.Invoke(result);
            }

            /// <summary>
            /// Change the WordPress account password.
            /// <para>username : the username to update with the new password.</para>
            /// <para>oldPassword : the password to change.</para>
            /// <para>newPassword : the new password.</para>
            /// </summary>
            /// <returns></returns>
            public async static UniTask WPChangePassword(string username, string oldPassword, string newPassword, Action<bool> callBack)
            {
                var result = await ChangeMyPassword(username, oldPassword, newPassword, true);
                callBack?.Invoke(result);
            }

            /// <summary>
            /// Change the account password using the generated user's OTP (use the generateOTP() methods for obtaining a valid OTP code).
            /// <para>username : the username to update with the new password.</para>
            /// <para>OTP : a valid user's OTP code.</para>
            /// <para>newPassword : the new password.</para>
            /// </summary>
            public async static UniTask ChangePasswordOTP(string username, string OTP, string newPassword, Action<bool> callBack)
            {
                var result = await ChangeMyPassword(username, "[OTP]" + OTP + "[/OTP]", newPassword, false);
                callBack?.Invoke(result);
            }

            /// <summary>
            /// Change the WordPress account password using the generated user's OTP (use the generateOTP() methods for obtaining a valid OTP code).
            /// <para>username : the username to update with the new password.</para>
            /// <para>OTP : a valid user's OTP code.</para>
            /// <para>newPassword : the new password.</para>
            /// </summary>
            /// <returns></returns>
            public async static UniTask WPChangePasswordOTP(string username, string OTP, string newPassword, Action<bool> callBack)
            {
                var result = await ChangeMyPassword(username, "[OTP]" + OTP + "[/OTP]", newPassword, true);
                callBack?.Invoke(result);
            }

            private async static UniTask<bool> ChangeMyPassword(string username, string oldPassword, string newPassword, bool isWordPress)
            {
                DBresponse = 0;

                if (await ServerIsConnected())
                {
                    var response = await RestAsync.Http("unirestclientuser/changepassword/", new GenericData
                    {
                        data1 = username,
                        data2 = EncryptionHelper.Sha256(oldPassword),
                        data3 = EncryptionHelper.Sha256(newPassword),
                        data4 = oldPassword,
                        data5 = newPassword,
                        data6 = (isWordPress) ? "WP" : ""
                    });

                    if (response.isArrived)
                    {
                        if (response.StatusIsOK())
                        {
                            DBresponse = response.GetResponseAsInt();
                            ServerError = "";
                        }
                        else
                        {
                            DBresponse = 0;
                            ServerError = "ERROR";
                        }
                    }
                    else
                    {
                        ServerError = "NO_COMMUNICATION";
                    }
                }
                else
                {
                    return false;
                }

                return ServerError == "";
            }

            #endregion

            #region " - TOKEN MANAGER "

            public class TokenManager
            {
                /// <summary>
                /// Generate a new Write Token for this logged in user.
                /// <para>callBack : the callback function to call on operation completed. It must have two parameters: a boolean that will be 'true' if the operation succeded amd a string that will contain the new generated Token.</para>
                /// <para>integrateToken (optional, default 'true') : integrate the new generated Token into the UniREST Client. Set it to 'false' if you want manually manage this Token.</para>
                /// </summary>
                public async static UniTask UpdateWrite(Action<bool, string> callBack, bool integrateToken = true)
                {
                    var response = await RestAsync.Action("unirestclientserver/newwritetoken/", "", "", "");
                    if (integrateToken && response.result == "OK" && response.reply.data != "") RestAsync.HTTPTokens.tokenW = response.reply.data; 
                    callBack?.Invoke(response.result == "OK", response.reply.data);
                }

                /// <summary>
                /// Generate a new Read Token for this logged in user.
                /// <para>callBack : the callback function to call on operation completed. It must have two parameters: a boolean that will be 'true' if the operation succeded amd a string that will contain the new generated Token.</para>
                /// <para>integrateToken (optional, default 'true') : integrate the new generated Token into the UniREST Client. Set it to 'false' if you want manually manage this Token.</para>
                /// </summary>
                public async static UniTask UpdateRead(Action<bool, string> callBack, bool integrateToken = true)
                {
                    var response = await RestAsync.Action("unirestclientserver/newreadtoken/", "", "", "");
                    if (integrateToken && response.result == "OK" && response.reply.data != "") RestAsync.HTTPTokens.tokenR = response.reply.data;
                    callBack?.Invoke(response.result == "OK", response.reply.data);
                }

                /// <summary>
                /// Generate a new Login Token for this logged in user.
                /// <para>callBack : the callback function to call on operation completed. It must have two parameters: a boolean that will be 'true' if the operation succeded amd a string that will contain the new generated Token.</para>
                /// <para>integrateToken (optional, default 'true') : integrate the new generated Token into the UniREST Client. Set it to 'false' if you want manually manage this Token.</para>
                /// </summary>
                public async static UniTask UpdateLogin(Action<bool, string> callBack, bool integrateToken = true)
                {
                    var response = await RestAsync.Action("unirestclientserver/newlogintoken/", "", "", "");
                    if (integrateToken && response.result == "OK" && response.reply.data != "") RestAsync.HTTPTokens.tokenL = response.reply.data;
                    callBack?.Invoke(response.result == "OK", response.reply.data);
                }

                /// <summary>
                /// Integrate the provided Write Token into the UniREST Client.
                /// </summary>
                public static void IntegrateWrite(string writeToken)
                {
                    if (writeToken != "") RestAsync.UpdateSignleTokens("", "", writeToken);
                }

                /// <summary>
                /// Integrate the provided Read Token into the UniREST Client.
                /// </summary>
                public static void IntegrateRead(string readToken)
                {
                    if (readToken != "") RestAsync.UpdateSignleTokens("", readToken, "");
                }

                /// <summary>
                /// Integrate the provided Login Token into the UniREST Client.
                /// </summary>
                public static void IntegrateLogin(string loginToken)
                {
                    if (loginToken != "") RestAsync.UpdateSignleTokens(loginToken, "", "");
                }

                /// <summary>
                /// Check if the Write Token integrated into the UniREST Client is equal to the Write Token registered on the Database.
                /// <para>callBack : the callback function to call on operation completed. It must have a boolean parameter that will be 'true' if the Tokens match.</para>
                /// </summary>
                public async static UniTask CheckWrite(Action<bool> callBack)
                {
                    var response = await RestAsync.Action("unirestclientserver/gettokens/", "", "", "");
                    if (response.reply.data == "")
                    {
                        callBack?.Invoke(false);
                    } else
                    {
                        var tokens = response.reply.data.Split('|');
                        callBack?.Invoke(RestAsync.HTTPTokens.tokenW == tokens[2]);
                    }
                }

                /// <summary>
                /// Check if the Read Token integrated into the UniREST Client is equal to the Read Token registered on the Database.
                /// <para>callBack : the callback function to call on operation completed. It must have a boolean parameter that will be 'true' if the Tokens match.</para>
                /// </summary>
                public async static UniTask CheckRead(Action<bool> callBack)
                {
                    var response = await RestAsync.Action("unirestclientserver/gettokens/", "", "", "");
                    if (response.reply.data == "")
                    {
                        callBack?.Invoke(false);
                    }
                    else
                    {
                        var tokens = response.reply.data.Split('|');
                        callBack?.Invoke(RestAsync.HTTPTokens.tokenR == tokens[1]);
                    }
                }

                /// <summary>
                /// Check if the Login Token integrated into the UniREST Client is equal to the Login Token registered on the Database.
                /// <para>callBack : the callback function to call on operation completed. It must have a boolean parameter that will be 'true' if the Tokens match.</para>
                /// </summary>
                public async static UniTask CheckLogin(Action<bool> callBack)
                {
                    var response = await RestAsync.Action("unirestclientserver/gettokens/", "", "", "");
                    if (response.reply.data == "")
                    {
                        callBack?.Invoke(false);
                    }
                    else
                    {
                        var tokens = response.reply.data.Split('|');
                        callBack?.Invoke(RestAsync.HTTPTokens.tokenL == tokens[0]);
                    }
                }
            }

            /// <summary>
            /// [DEPRECATED - Use the TokenManager instead] Update the Reading Token and/or the Write Token of this logged in user. Be sure to use this method when you're not performing operations on the Database.
            /// </summary>
            public async static UniTask TokenUpdate(bool readToken, bool writeToken, Action<bool> callBack)
            {
                var rT = false;
                var wT = false;

                if (readToken && writeToken)
                {
                    rT = await ReadTokenUpdate();
                    wT = await WriteTokenUpdate();
                    callBack?.Invoke(rT && wT);
                }
                else if (readToken)
                {
                    rT = await ReadTokenUpdate();
                    callBack?.Invoke(rT);
                }
                else if (writeToken)
                {
                    wT = await WriteTokenUpdate();
                    callBack?.Invoke(wT);
                }

            }

            /// <summary>
            /// [DEPRECATED - Use the TokenManager instead] Update the Reading Token of this logged in user.
            /// </summary>
            /// <returns></returns>
            private async static UniTask<bool> ReadTokenUpdate()
            {
                var response = await RestAsync.Action("unirestclientserver/newreadtoken/", "", "", "");
                return response.result == "OK";
            }

            /// <summary>
            /// [DEPRECATED - Use the TokenManager instead] Update the Writing Token of this logged in user.
            /// </summary>
            /// <returns></returns>
            private async static UniTask<bool> WriteTokenUpdate()
            {
                var response = await RestAsync.Action("unirestclientserver/newwritetoken/", "", "", "");
                return response.result == "OK";
            }

            #endregion

            #region " - ACCOUNT SAVE / LOAD "

            /// <summary>
            /// Locally save username and password (from the userAccount property) for the various automatic operations (uses Unity PlayerPrefs).
            /// </summary>
            private static void AccountSave()
            {
                PlayerPrefs.SetString("__UniREST_User_Username__", userAccount.username);
                PlayerPrefs.SetString("__UniREST_User_Password__", userAccount.password);
            }

            /// <summary>
            /// Load the locally stored username and password for the various automatic operations (uses Unity PlayerPrefs). It returns a dictionary with 'username' and 'password' keys.
            /// </summary>
            private static Dictionary<string, string> AccountLoad()
            {
                var storage = new Dictionary<string, string>();
                storage.Add("username", PlayerPrefs.GetString("__UniREST_User_Username__"));
                storage.Add("password", PlayerPrefs.GetString("__UniREST_User_Password__"));
                return storage;
            }

            #endregion

            #region " - FILE MANAGER "

            /// <summary>
            /// Initialize an instance of a remote user's File that can be saved or loaded.
            /// </summary>
            public class File
            {
                /// <summary>
                /// Return the detected error when a file operation fails.
                /// </summary>
                public string FileError = "";

                string loadedData = "";
                string fileName = "";
                Target target;
                bool useCompression = true;

                /// <summary>
                /// Initialize the File instance with the given file name and target folder.
                /// <para>fileName : the file name can contains subfolders (e.g. folder1/folder2/myfile). File extension is optional and shouldn't be included.</para>
                /// <para>targetFolder : the folder to work with. It can be the Game folder or the logged-in user's dedicated folder.</para>
                /// <para>useDataCompression (optional, default true) : if true, the data will be compressed / decompressed.</para>
                /// </summary>
                public File(string fileName, Target targetFolder, bool useDataCompression = true)
                {
                    if (fileName.StartsWith("/")) fileName = fileName.Substring(1);
                    if (fileName == "") Console.Error("Trying to create a File instance with an invalid file name.");

                    target = targetFolder;
                    if (UserID == 0) { Console.Error("There's not a logged-in user."); }

                    useCompression = useDataCompression;

                    this.fileName = fileName;
                }

                /// <summary>
                /// Save the given data to a remote file, into the user folder. If the operation fails, the method returns false and an error message is in the FileError property.
                /// <para>data : the data to save in the remote file.</para>
                /// </summary>
                public async UniTask Save(object data, Action<bool> callBack)
                {
                    if (fileName == "") { FileError = "No valid file name"; callBack?.Invoke(false); }

                    var serializer = new UniRESTBinary();
                    serializer.LoadFromData(data);
                    if (useCompression) serializer.CompressLoadedBytes();
                    var dataToSend = serializer.ToString();

                    var response = await RestAsync.FileManager("unirestclientserver/filemanager/", "SAVE", dataToSend, GetFileName());

                    if (response.result == "OK")
                    {
                        FileError = "";
                        callBack?.Invoke(true);
                    }
                    else
                    {
                        FileError = response.data;
                        callBack?.Invoke(false);
                    }
                }

                /// <summary>
                /// Load data from a remote file of the user folder. If the operation fails, the method returns false and an error message is in the FileError property.
                /// </summary>
                public async UniTask Load<T>(Action<T, bool> callBack)
                {
                    if (fileName == "") { FileError = "No valid file name"; callBack?.Invoke(default, false); }

                    var response = await RestAsync.FileManager("unirestclientserver/filemanager/", "LOAD", "", GetFileName());

                    if (response.result == "OK")
                    {
                        FileError = "";
                        loadedData = response.data;
                        callBack?.Invoke(GetData<T>(), true);
                    }
                    else
                    {
                        FileError = response.data;
                        loadedData = "";
                        callBack?.Invoke(default, false);
                    }
                }

                public T GetData<T>()
                {
                    var deserializer = new UniRESTBinary();
                    deserializer.LoadFromBinaryString(loadedData);
                    if (useCompression) deserializer.DecompressLoadedBytes();
                    return deserializer.ToObject<T>();
                }

                /// <summary>
                /// [DEPRECATED] Delete a remote file (the file only) from the user folder. This operation always return true: use the FileError property to check the presence of an error.
                /// Deprecation note: use FileManager class instead.
                /// </summary>
                /// <returns></returns>
                public async UniTask Delete(Action<bool> callBack)
                {
                    if (fileName == "") { FileError = "No valid file name"; callBack?.Invoke(false); }

                    var response = await RestAsync.FileManager("unirestclientserver/filemanager/", "DELETE", "", GetFileName());

                    FileError = response.data;
                    callBack?.Invoke(true);
                }

                private string GetFileName()
                {
                    if (target == Target.UserFolder) return UserID + "|" + fileName + "|USER"; else return UserID + "|" + fileName + "|GAME";
                }

                /// <summary>
                /// [DEPRECATED] Return the list of all the files found in the given folder, including the files under subfolders.
                /// Deprecation note: use FileManager class instead.
                /// </summary>
                /// <param name="targetRootFolder"></param>
                /// <param name="subfolder"></param>
                /// <returns></returns>
                public async static UniTask List(TargetRoot targetRootFolder, Action<string[]> callBack, string subfolder = "")
                {
                    if (subfolder.StartsWith("/")) subfolder = subfolder.Substring(1);

                    var fileName = "";
                    var data = subfolder + "|";

                    switch (targetRootFolder)
                    {
                        case TargetRoot.UserFilesFolder:
                            fileName = UserID + "|{USER_FILES_FOLDER}|USER";
                            break;
                        case TargetRoot.UserUploadsFolder:
                            fileName = UserID + "|{USER_UPLOADS_FOLDER}|USER";
                            break;
                        case TargetRoot.GameFilesFolder:
                            fileName = UserID + "|{GAME_FILES_FOLDER}|GAME";
                            break;
                        case TargetRoot.GameUploadsFolder:
                            fileName = UserID + "|{GAME_UPLOADS_FOLDER}|GAME";
                            break;
                        default:
                            fileName = "";
                            break;
                    }

                    var response = await RestAsync.FileManager("unirestclientserver/filemanager/", "LIST", data, fileName);

                    if (response.result == "OK")
                    {
                        callBack?.Invoke(response.data.Split('|'));
                    }
                    else
                    {
                        callBack?.Invoke(default);
                    }
                }
            }

            /// <summary>
            /// Initialize a new FileManager instance that can manage online files.
            /// </summary>
            public class FileManager
            {
                /// <summary>
                /// Return the detected error when an operation fails.
                /// </summary>
                public string Error = "";

                /// <summary>
                /// Delete the given online file and call the given callBack function on operation completed.
                /// <para>target: where the file is located (user/game created files or uploaded files folder).</para>
                /// <para>fileName: the file to delete. If the file is under subfolders, the path must be included (e.g. myassets/enemies/dragon.fbx).</para>
                /// <para>callBack: the function to call on operation completed. It must have a boolean parameter that will be true in case of success, false otherwise.</para>
                /// <para>Note: if the operation fails, the FileManager.Error property will contain an error code.</para>
                /// </summary>
                public async UniTask Delete(TargetRoot target, string fileName, Action<bool> callBack)
                {
                    var action = UserID + "|DELETE|" + target.ToString();
                    var extra = fileName + "|";
                    var response = await RestAsync.FileManager("unirestclientserver/filemanager2/", action, "", extra);

                    if (response.result == "OK")
                    {
                        if (response.data == "")
                        {
                            Error = "";
                            callBack?.Invoke(true);
                        }
                        else
                        {
                            Error = response.data;
                            callBack?.Invoke(false);
                        }                        
                    }
                    else
                    {
                        Error = "COMMUNICATION_ERROR";
                        callBack?.Invoke(false);
                    }
                }

                /// <summary>
                /// Rename the given online file and call the given callBack function on operation completed.
                /// <para>target: where the file is located (user/game created files or uploaded files folder).</para>
                /// <para>fileName: the file to rename. If the file is under subfolders, the path must be included (e.g. myassets/enemies/dragon.fbx).</para>
                /// <para>newName: the new name for this file. If the original file is under subfolders, the path must be included (e.g. myassets/enemies/new_name.fbx).</para>
                /// <para>callBack: the function to call on operation completed. It must have a boolean parameter that will be true in case of success, false otherwise.</para>
                /// <para>Note: if the operation fails, the FileManager.Error property will contain an error code.</para>
                /// </summary>
                public async UniTask Rename(TargetRoot target, string fileName, string newName, Action<bool> callBack)
                {
                    var action = UserID + "|RENAME|" + target.ToString();
                    var extra = fileName + "|" + newName;
                    var response = await RestAsync.FileManager("unirestclientserver/filemanager2/", action, "", extra);

                    if (response.result == "OK")
                    {
                        if (response.data == "")
                        {
                            Error = "";
                            callBack?.Invoke(true);
                        }
                        else
                        {
                            Error = response.data;
                            callBack?.Invoke(false);
                        }
                    }
                    else
                    {
                        Error = "COMMUNICATION_ERROR";
                        callBack?.Invoke(false);
                    }
                }

                /// <summary>
                /// Check if the given online file exists and call the given callBack function on operation completed.
                /// <para>target: where the file is located (user/game created files or uploaded files folder).</para>
                /// <para>fileName: the file to rename. If the file is under subfolders, the path must be included (e.g. myassets/enemies/dragon.fbx).</para>
                /// <para>callBack: the function to call on operation completed. It must have a boolean parameter that will be true if the file exists, false otherwise.</para>
                /// <para>Note: if the operation fails or the file doesn't exist, the FileManager.Error property will contain an error code.</para>
                /// </summary>
                public async UniTask Exists(TargetRoot target, string fileName, Action<bool> callBack)
                {
                    var action = UserID + "|EXISTS|" + target.ToString();
                    var extra = fileName + "|";
                    var response = await RestAsync.FileManager("unirestclientserver/filemanager2/", action, "", extra);

                    if (response.result == "OK")
                    {
                        if (response.data == "")
                        {
                            Error = "";
                            callBack?.Invoke(true);
                        }
                        else
                        {
                            Error = response.data;
                            callBack?.Invoke(false);
                        }
                    }
                    else
                    {
                        Error = "COMMUNICATION_ERROR";
                        callBack?.Invoke(false);
                    }
                }

                /// <summary>
                /// Return the given online file size and call the given callBack function on operation completed.
                /// <para>target: where the file is located (user/game created files or uploaded files folder).</para>
                /// <para>fileName: the file to rename. If the file is under subfolders, the path must be included (e.g. myassets/enemies/dragon.fbx).</para>
                /// <para>callBack: the function to call on operation completed. It must have a long parameter that will be the size in bytes or 0 in case of error.</para>
                /// <para>Note: if the operation fails or the file doesn't exist, the FileManager.Error property will contain an error code.</para>
                /// </summary>
                public async UniTask Size(TargetRoot target, string fileName, Action<long> callBack)
                {
                    var action = UserID + "|SIZE|" + target.ToString();
                    var extra = fileName + "|";
                    var response = await RestAsync.FileManager("unirestclientserver/filemanager2/", action, "", extra);

                    if (response.result == "OK")
                    {
                        if (response.data == "FILE_NOT_EXISTING")
                        {
                            Error = response.data;
                            callBack?.Invoke(0);
                        }
                        else
                        {
                            Error = "";
                            callBack?.Invoke(long.Parse(response.data));
                        }
                    }
                    else
                    {
                        Error = "COMMUNICATION_ERROR";
                        callBack?.Invoke(0);
                    }
                }

                /// <summary>
                /// Return the list of all the files under the given folder (subfolders included) and call the given callBack function on operation completed.
                /// <para>target: where the file is located (user/game created files or uploaded files folder).</para>
                /// <para>path: an empty string for searching into the root or a path for searching into a subfolder (e.g. myassets/enemies/).</para>
                /// <para>callBack: the function to call on operation completed. It must have a string array parameter that will contain the files list or nothing in case of error.</para>
                /// <para>Note: if the operation fails, the FileManager.Error property will contain an error code.</para>
                /// </summary>
                public async UniTask List(TargetRoot target, string path, Action<string[]> callBack)
                {                 
                    var action = UserID + "|LIST|" + target.ToString();
                    var extra = path + "|";
                    var response = await RestAsync.FileManager("unirestclientserver/filemanager2/", action, "", extra);

                    if (response.result == "OK")
                    {
                        Error = "";
                        callBack?.Invoke(response.data.Split('|'));
                    }
                    else
                    {
                        Error = "COMMUNICATION_ERROR";
                        callBack?.Invoke(new string[] { });
                    }
                }

            }

            #endregion

            #region " - REPEATER "

            /// <summary>
            /// Initialize a special feature to execute an async callback function more times.
            /// </summary>
            public class Repeater
            {
                Func<int, UniTask> callBack;
                System.Action onComplete;
                int to;

                /// <summary>
                /// Start the execution of the given async callback and then call the onComplete function when operations are completed.
                /// <para>from : the start index.</para>
                /// <para>to : the end index (this value is excluded)</para>
                /// <para>callBack: the function to execute. It must have an int parameter as index between the 'from' value and the 'to - 1' value.</para>
                /// <para>onComplete: the function to call when operations are completed.</para>
                /// </summary>
                public void Run(int from, int to, Func<int, UniTask> callBack, System.Action onComplete)
                {
                    this.callBack = callBack;
                    this.onComplete = onComplete;
                    this.to = to;

                    _ = Loop(from);
                }

                private async UniTask Loop(int index)
                {
                    await callBack.Invoke(index);

                    index++;
                    if (index < to) _ = Loop(index); else onComplete.Invoke();
                }

            }

            #endregion

        }

        #endregion


        #region " SYNCTABLES "

        /// <summary>
        /// Initialize a new SyncTable communication system.
        /// </summary>
        public class SyncTable
        {
            class Sync
            {
                public string tableName;
                public int user_id;
                public int millisecondsDelay;
                public Action<Data> callBack;
                public bool isStopped;
                public string token;
            }

            /// <summary>
            /// The user's data slots.
            /// </summary>
            public class Data
            {
                public string s1 = null;
                public string s2 = null;
                public string s3 = null;
                public string s4 = null;
                public string s5 = null;
                public string s6 = null;
                public string s7 = null;
                public string s8 = null;
                public string s9 = null;
                public string s10 = null;
                private int id;
                public int user_id;
                public string token;
                public string tableName;
            }

            Dictionary<string, Sync> sync = new Dictionary<string, Sync>();

            /// <summary>
            /// Add a new configuration profile for connecting with a user's data.
            /// <para>id : a unique id string that represents this profile.</para>
            /// <para>tableName : the name of the SyncTable to communicate with.</para>
            /// <para>userID: id of the user.</para>
            /// <para>millisecondsDelay (optional, default: 1000) : the number of milliseconds to wait before contacting the SyncTable.</para>
            /// </summary>
            public void Add(string id, string tableName, int userID, int millisecondsDelay = 1000)
            {
                sync[id] = new Sync { tableName = "tfur_" + tableName, user_id = userID, millisecondsDelay = millisecondsDelay, callBack = null, isStopped = true, token = "" };
            }

            /// <summary>
            /// Write the given data into the user's slots.
            /// <para>id : id string of the profile.</para>
            /// <para>data : the data to save in the user's slots.</para>
            /// <para>callBack : the function to call on operations completed. The boolean parameter is true if writing has been executed, false otherwise.</para>
            /// </summary>
            public async UniTask Write(string id, Data data, Action<bool> callBack)
            {
                data.tableName = sync[id].tableName;
                data.user_id = sync[id].user_id;
                var response = await RestAsync.Http("unirestclientuser/synctablewrite/", data);

                if (response.isArrived)
                {
                    callBack?.Invoke(response.StatusIsOK());
                }
                else
                {
                    callBack?.Invoke(false);
                }
            }

            /// <summary>
            /// Define the function to call when the user's data is received.
            /// <para>id : id string of the profile.</para>
            /// <para>callBack : the function to call on operations completed. The Data parameter contains the user's slots data.</para>
            /// </summary>
            public void Read(string id, Action<Data> callBack)
            {
                sync[id].callBack = callBack;
            }

            /// <summary>
            /// Start the listening to the user's slots. If data is received the Read callBack function will be executed.
            /// <para>id : id string of the profile.</para>
            /// </summary>
            public void ListenTo(string id)
            {
                sync[id].isStopped = false;
                _ = Loop(id);
            }

            /// <summary>
            /// Start the listening of all users slots. If data is received the Read callBack function will be executed.
            /// </summary>
            public void ListenToAll()
            {
                foreach (var id in sync) ListenTo(id.Key);
            }

            private async UniTask Loop(string id)
            {
                await UniTask.Delay(sync[id].millisecondsDelay);

                var response = await RestAsync.Http("unirestclientuser/synctableread/", sync[id]);

                if (response.isArrived)
                {
                    var json = response.GetDecryptedResponse();
                    if (json != "")
                    {
                        var data = JsonUtility.FromJson<Data>(json);
                        sync[id].token = data.token;
                        var readCallBack = sync[id].callBack;
                        readCallBack?.Invoke(data);
                    }
                }
                
                if (!sync[id].isStopped) _ = Loop(id);
            }

            /// <summary>
            /// Stop the listening to a user's slots data.
            /// <para>id : id string of the profile.</para>
            /// </summary>
            public void StopListening(string id)
            {
                sync[id].isStopped = true;
            }

            /// <summary>
            /// Stop all the active listenings.
            /// </summary>
            public void StopAllListening()
            {
                foreach (var id in sync) StopListening(id.Key);
            }

        }

        #endregion


        #region " VARIOUS "

        /// <summary>
        /// Set the characted encoding type for the communications between the UniREST Client and the UniREST Server. UTF8 is by default. Be careful changing this setting.
        /// <para>textEncodingType : the encoding system to use.</para>
        /// </summary>
        public static void SetTextEncoding(Encoding textEncodingType)
        {
            EncryptionHelper.encoder = textEncodingType;
        }

        /// <summary>
        /// Return a random 30 length string of alphanumeric characters.
        /// </summary>
        private static string GetUID()
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var randomString1 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var randomString2 = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            return (randomString1 + timeStamp + randomString2).Substring(0, 30);
        }

        #endregion



        #region " DEPRECATED "

        //        #region " [DEPRECATED] SYNC "


        //        #region " READ ONE / ALL "

        //        /// <summary>
        //        /// [DEPRECATED] Perform a reading operation on the UniREST Server. If the request is valid, the method returns one single data record from the Database.
        //        /// </summary>
        //        public static TABLE ReadOne<TABLE>(string api, object tableData)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "READ",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            try
        //            {
        //                return JsonUtility.FromJson<TABLE>(response.reply.data);
        //            }
        //            catch (Exception)
        //            {
        //                Console.Info("The ReadOne() method call returned no results or the result is an array. The ReadOne() method requires that the 'One Record Only' option is checked in the API read settings.");
        //                return default;
        //            }
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform a reading operation on the UniREST Server. If the request is valid, the method returns an array of data records from the Database.
        //        /// </summary>
        //        public static TABLE[] Read<TABLE>(string api, object tableData)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "READ",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            try
        //            {
        //                TABLE[] json = JsonHelper.getJsonArray<TABLE>(response.reply.data);
        //                if (json.Length == 0) Console.Info("The Read() method call returned no results.");
        //                return json;
        //            }
        //            catch (Exception e)
        //            {
        //                var msg = e.Message;
        //                Console.Info("The Read() method call returned no results. Check if the called API is correctly configured (for example, if the API's ''One record only'' option is checked, you must use the ReadOne() method).");
        //                return default;
        //            }
        //        }

        //        #endregion


        //        #region " WRITE / UPDATE "

        //        /// <summary>
        //        /// [DEPRECATED] Perform a writing operation on the UniREST Server. If the request is valid, a new data record is added to the Database and the method returns true.
        //        /// </summary>
        //        public static bool Write(string api, object tableData)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "WRITE",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            if (response.result == "OK") {
        //                DBresponse = int.Parse(response.reply.data);
        //                if (DBresponse == 0) Console.Info("The Write() method call performed no operation.");
        //            }

        //            return response.result == "OK";
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform an updating operation on the UniREST Server. If the request is valid, the existing Database record is updated with the new provided data record and the method returns true.
        //        /// By default, all fields are updated. If you want to update just some fields, list the fields to update in the fieldsToUpdate optional parameter (field names must be separated by a space).
        //        /// </summary>
        //        public static bool Update(string api, object tableData, string fieldsToUpdate = "")
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "UPDATE",
        //                data = jsonData,
        //                extra = fieldsToUpdate
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            if (response.result == "OK")
        //            {
        //                DBresponse = int.Parse(response.reply.data);
        //                if (DBresponse == 0) Console.Info("The Update() method call performed no operation.");
        //            }

        //            return response.result == "OK";
        //        }

        //        #endregion


        //        #region " DELETE "

        //        /// <summary>
        //        /// [DEPRECATED] Perform a deleting operation on the UniREST Server. If the request is valid, the Database records located by the API condition are deleted and the method returns true.
        //        /// </summary>
        //        public static bool Delete(string api, object tableData)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "DELETE",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            if (response.result == "OK")
        //            {
        //                DBresponse = int.Parse(response.reply.data);
        //                if (DBresponse == 0) Console.Info("The Delete() method call performed no operation.");
        //            }

        //            return response.result == "OK";
        //        }

        //        #endregion


        //        #region " CALL (Custom PHP) "

        //        /// <summary>
        //        /// [DEPRECATED] Call a Custom PHP script. This method expect to receive an array of data.
        //        /// </summary>
        //        public static DATA[] Call<DATA>(string api, object customData)
        //        {
        //            var jsonData = JsonUtility.ToJson(customData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            try
        //            {
        //                DATA[] json = JsonHelper.getJsonArray<DATA>(response.reply.data);
        //                if (json.Length == 0) Console.Info("The Call() method returned no results.");
        //                return json;
        //            }
        //            catch (Exception e)
        //            {
        //                Console.Info("The Call() method returned no results: " + e.Message);
        //                return default;
        //            }
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Call a Custom PHP script. This method expect to receive a single record of data.
        //        /// </summary>
        //        public static DATA CallOne<DATA>(string api, object customData)
        //        {
        //            var jsonData = JsonUtility.ToJson(customData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "",
        //                data = jsonData,
        //                extra = ""
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            try
        //            {
        //                return JsonUtility.FromJson<DATA>(response.reply.data);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.Info("The CallOne() method returned no results: " + e.Message);
        //                return default;
        //            }
        //        }

        //        #endregion


        //        #region " SPECIAL OPERATIONS "

        //        /// <summary>
        //        /// [DEPRECATED] Update the value (int or float) of a specific record and column using the given math formula.
        //        /// <para>api : the API to call.</para>
        //        /// <para>tabelData : the values for locating a specific record.</para>
        //        /// <para>fieldToUpdate : the column name to update.</para>
        //        /// <para>formula: a valid math formula for recalculating the column value.</para>
        //        /// </summary>
        //        public static float UpdateMath(string api, object tableData, string fieldToUpdate, string formula)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "MATH",
        //                data = jsonData,
        //                extra = fieldToUpdate + "|" + formula
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;
        //            DBerror = "";

        //            if (response.reply.data.Contains("ERROR"))
        //            {
        //                var tmp = response.reply.data.Split('|');
        //                DBerror = tmp[1];
        //                return float.Parse(tmp[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        //            }
        //            else
        //            {
        //                return float.Parse(response.reply.data, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        //            }            
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Update the JSON content of a specific record and column using the given Json data.
        //        /// <para>api : the API to call.</para>
        //        /// <para>tabelData : the values for locating a specific record.</para>
        //        /// <para>fieldToUpdate : the column name to update.</para>
        //        /// <para>formula: a Json data. Existing keys will be updated with the given values, whereas new keys will be added.</para>
        //        /// </summary>
        //        public static bool UpdateJSON(string api, object tableData, string fieldToUpdate, Json jsonData)
        //        {
        //            var jData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "UPDATEJSON",
        //                data = jData,
        //                extra = fieldToUpdate + "|" + jsonData.ToString()
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            if (response.result == "OK")
        //            {
        //                DBresponse = int.Parse(response.reply.data);
        //                if (DBresponse == 0) Console.Info("The Update() method call performed no operation.");
        //            }

        //            return response.result == "OK";
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Check if records with the given values exist in the API's table. Return true if at least one record exists.
        //        /// <para>api : the API to call.</para>
        //        /// <para>tabelData : the values to check into the table.</para>
        //        /// <para>fieldsToCheck : the columns where to look for the given values. Column names must be listed separated by a space.</para>
        //        /// </summary>
        //        public static bool Exists(string api, object tableData, string fieldsToCheck)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "EXISTS",
        //                data = jsonData,
        //                extra = fieldsToCheck
        //            });
        //            result.Wait();
        //            var response = result.Result;

        //            DBresponse = 0;

        //            return response.reply.data == "1";
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Return the number of records that contain the given values.
        //        /// <para>api : the API to call.</para>
        //        /// <para>tabelData : the values to check into the table.</para>
        //        /// <para>fieldsToCheck : the columns where to look for the given values. Column names must be listed separated by a space.</para>
        //        /// </summary>
        //        public static int Count(string api, object tableData, string fieldsToCheck)
        //        {
        //            var jsonData = JsonUtility.ToJson(tableData);

        //            var result = Rest.Action(api, new RestPackage
        //            {
        //                action = "COUNT",
        //                data = jsonData,
        //                extra = fieldsToCheck
        //            });
        //            result.Wait();
        //            var response = result.Result;
        //            DBresponse = 0;

        //            return int.Parse(response.reply.data);
        //        }

        //        #endregion


        //        #region " LOGIN / REGISTRATION "

        //        /// <summary>
        //        /// [DEPRECATED] Perform a Login operation with the given username and password, opening a secure connection with the UniREST Server. If the request is valid, the method returns true and the user ID is stored in the UserID property. If an error occurs, the ServerError property will have an error code.
        //        /// </summary>
        //        public static bool Login(string username, string password, bool updateTokens = true)
        //        {
        //            isLoggedIn = false;

        //            if (ServerIsConnected())
        //            {
        //                userAccount = new Account { username = username, password = password };

        //                var result = Rest.Send("unirestclientuser/login/", new Account
        //                {
        //                    username = username,
        //                    password = EncryptionHelper.Sha256(password),
        //                    useWordPressUsers = "",
        //                    lastLoginDate = (updateTokens) ? "" : "_NO_TOKEN_CHANGE_"
        //                });
        //                result.Wait();
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    var tokens = EncryptionHelper.Decrypt(response.data).Split('|');
        //                    if (tokens.Length >= 4)
        //                    {
        //                        Rest.SetTokens(tokens[0], tokens[1], tokens[2], tokens[3]);
        //                        UserID = int.Parse(tokens[3]);
        //                        userAccount.lastLoginDate = tokens[4];
        //                        userAccount.registrationDate = tokens[5];
        //                        ServerError = "";
        //                        isLoggedIn = true;
        //                        if (rememberMe) LocalFile.Save(username, password);
        //                    }
        //                    else
        //                    {
        //                        userAccount = new Account();
        //                        ServerError = "NO_TOKENS";
        //                    }
        //                }
        //                else
        //                {
        //                    userAccount = new Account();
        //                    ServerError = "NO_ACCOUNT";
        //                }
        //            }
        //            else
        //            {
        //                userAccount = new Account();
        //                return false;
        //            }

        //            return ServerError == "";

        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform a new login attempt with the username and password from a previously successful login.
        //        /// </summary>
        //        public static bool Relogin()
        //        {
        //            if (userAccount.username == "" || userAccount.password == "") return false;

        //            return Login(userAccount.username, userAccount.password);
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform a user login using the locally saved username and password. In order to use this method, the rememberMe property must be set to true.
        //        /// <para>autoRegistration (optional) : if set to TRUE, it generates a new account (if it doesn't exist), saves it locally and finally performs a login.</para>
        //        /// </summary>
        //        public static bool AutoLogin(bool autoRegistration = false)
        //        {
        //            var account = LocalFile.Load();
        //            if (account.Count == 0)
        //            {
        //                if (autoRegistration)
        //                {
        //                   userAccount = new Account { username = GetUID(), password = GetUID() };
        //                    if (Registration(userAccount.username, userAccount.password)) return Login(userAccount.username, userAccount.password); else return false;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return Login(account["username"], account["password"]);
        //            }
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Register this Unity Game or Application and automatically perform the login. Use this method if this Unity Project doesn't require users account, but the UniREST Solution functionalities only.
        //        /// </summary>
        //        public static bool ApplicationLogin()
        //        {
        //            var appUsername = UniRESTClientConfig.AppID;
        //            var appPassword = UniRESTClientConfig.Key1.Substring(4, 8) + UniRESTClientConfig.Key2.Substring(4, 8);

        //            if (Login(appUsername, appPassword, false))
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                _ = Registration(appUsername, appPassword);
        //                return Login(appUsername, appPassword, false);
        //            }
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform the registration of a new account in the UniREST Server, with the given username and password. If the request is valid, a new user account is added to the Database, the method returns true and a login operation can be performed. If an error occurs, the ServerError property will have an error code.
        //        /// </summary>
        //        public static bool Registration(string username, string password)
        //        {
        //            DBresponse = 0;

        //            if (ServerIsConnected())
        //            {
        //                var result = Rest.Send("unirestclientuser/registration/", new Account
        //                {
        //                    username = username,
        //                    password = EncryptionHelper.Sha256(password),
        //                    useWordPressUsers = ""
        //                });

        //                result.Wait();
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    ServerError = "";
        //                    DBresponse = int.Parse(response.data);
        //                }
        //                else
        //                {
        //                    ServerError = "INVALID";
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //            return ServerError == "";
        //        }

        //        private static bool ServerIsConnected()
        //        {
        //            ServerError = "";
        //            UserID = 0;

        //            var result = Rest.Send("unirestclientserver/call/", new Response());
        //            result.Wait();
        //            var response = result.Result;

        //            if (response.result == "OK")
        //            {
        //                var decoded = int.Parse(EncryptionHelper.Decrypt(response.data)) + 30;

        //                result = Rest.Send("unirestclientserver/connect/", new Response { data = decoded.ToString() });
        //                result.Wait();
        //                response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    ServerError = "";
        //                }
        //                else
        //                {
        //                    ServerError = "NO_AUTH";
        //                }

        //            }
        //            else
        //            {
        //                ServerError = "NO_COMMUNICATION";
        //            }

        //            return ServerError == "";
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform a WordPress Login operation with the given username and password, opening a secure connection with the UniREST Server. If the request is valid, the method returns true and the user ID is stored in the UserID property. If an error occurs, the ServerError property will have an error code.
        //        /// </summary>
        //        public static bool WPLogin(string username, string password)
        //        {
        //            isLoggedIn = false;

        //            if (ServerIsConnected())
        //            {
        //                userAccount = new Account { username = username, password = password };

        //                var result = Rest.Send("unirestclientuser/wplogin/", new Account
        //                {
        //                    username = username,
        //                    password = EncryptionHelper.Sha256(password),
        //                    useWordPressUsers = password
        //                });
        //                result.Wait();
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    var tokens = EncryptionHelper.Decrypt(response.data).Split('|');
        //                    if (tokens.Length >= 4)
        //                    {
        //                        Rest.SetTokens(tokens[0], tokens[1], tokens[2], tokens[3]);
        //                        UserID = int.Parse(tokens[3]);
        //                        userAccount.lastLoginDate = tokens[4];
        //                        userAccount.registrationDate = tokens[5];
        //                        ServerError = "";
        //                        isLoggedIn = true;
        //                        if (rememberMe) LocalFile.Save(username, password);
        //                    }
        //                    else
        //                    {
        //                        userAccount = new Account();
        //                        ServerError = "NO_TOKENS";
        //                    }
        //                }
        //                else
        //                {
        //                    userAccount = new Account();
        //                    ServerError = response.data;
        //                }
        //            }
        //            else
        //            {
        //                userAccount = new Account();
        //                return false;
        //            }

        //            return ServerError == "";

        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform the registration of a new account in the UniREST Server and WordPress system, with the given username and password. If the request is valid, a new user account is added to the Database, the method returns true and a login operation can be performed. If an error occurs, the ServerError property will have an error code.
        //        /// </summary>
        //        public static bool WPRegistration(string username, string password)
        //        {
        //            DBresponse = 0;

        //            if (ServerIsConnected())
        //            {
        //                var result = Rest.Send("unirestclientuser/wpregistration/", new Account
        //                {
        //                    username = username,
        //                    password = EncryptionHelper.Sha256(password),
        //                    useWordPressUsers = password
        //                });

        //                result.Wait();
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    ServerError = "";
        //                    DBresponse = int.Parse(response.data);
        //                }
        //                else
        //                {
        //                    ServerError = response.data;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //            return ServerError == "";
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Perform the User or Application logout. The security Tokens are resetted and all the communications with the Server are interrupted.
        //        /// </summary>
        //        public static void Logout()
        //        {
        //            Rest.SetTokens("", "", "", "0");
        //            UserID = 0;
        //            userAccount = new Account();
        //            DBerror = "";
        //            DBquery = "";
        //            DBresponse = 0;
        //            ServerError = "";

        //            isLoggedIn = false;
        //        }

        //        #endregion


        //        #region " CHANGE PASSWORD "

        //        /// <summary>
        //        /// [DEPRECATED] Change the account password.
        //        /// <para>username : the username to update with the new password.</para>
        //        /// <para>oldPassword : the password to change.</para>
        //        /// <para>newPassword : the new password.</para>
        //        /// </summary>
        //        public static bool ChangePassword(string username, string oldPassword, string newPassword)
        //        {
        //            return ChangeMyPassword(username, oldPassword, newPassword, false);
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Change the WordPress account password.
        //        /// <para>username : the username to update with the new password.</para>
        //        /// <para>oldPassword : the password to change.</para>
        //        /// <para>newPassword : the new password.</para>
        //        /// </summary>
        //        public static bool WPChangePassword(string username, string oldPassword, string newPassword)
        //        {
        //            return ChangeMyPassword(username, oldPassword, newPassword, true);
        //        }

        //        private static bool ChangeMyPassword(string username, string oldPassword, string newPassword, bool isWordPress)
        //        {
        //            DBresponse = 0;

        //            if (ServerIsConnected())
        //            {
        //                var result = Rest.Send("unirestclientuser/changepassword/", new GenericData
        //                {
        //                    data1 = username,
        //                    data2 = EncryptionHelper.Sha256(oldPassword),
        //                    data3 = EncryptionHelper.Sha256(newPassword),
        //                    data4 = oldPassword,
        //                    data5 = newPassword,
        //                    data6 = (isWordPress) ? "WP" : ""
        //                });

        //                result.Wait();

        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    ServerError = "";
        //                    DBresponse = int.Parse(response.data);
        //                }
        //                else
        //                {
        //                    ServerError = response.data;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //            return ServerError == "";
        //        }

        //        #endregion


        //        #region " UPDATE TOKEN "

        //        /// <summary>
        //        /// [DEPRECATED] Update the Reading Token of this logged in user.
        //        /// </summary>
        //        public static bool ReadTokenUpdate()
        //        {
        //            var result = Rest.Action("unirestclientserver/newreadtoken/", new RestPackage { action = "", data = "", extra = "" });
        //            result.Wait();
        //            var response = result.Result;

        //            if (response.result == "OK")
        //            {
        //                return true;
        //            } else
        //            {
        //                return false;
        //            }
        //        }

        //        /// <summary>
        //        /// [DEPRECATED] Update the Writing Token of this logged in user.
        //        /// </summary>
        //        public static bool WriteTokenUpdate()
        //        {
        //            var result = Rest.Action("unirestclientserver/newwritetoken/", new RestPackage { action = "", data = "", extra = "" });
        //            result.Wait();
        //            var response = result.Result;

        //            if (response.result == "OK")
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        #endregion


        //        #region " FILE MANAGER "

        //        /// <summary>
        //        /// [DEPRECATED] Initialize an instance of a remote user's File that can be saved or loaded.
        //        /// </summary>
        //        public class File
        //        {
        //            /// <summary>
        //            /// Return the detected error when a file operation fails.
        //            /// </summary>
        //            public string FileError = "";

        //            string loadedData = "";
        //            string fileName = "";
        //            Target target;
        //            bool useCompression = true;

        //            /// <summary>
        //            /// [DEPRECATED] Initialize the File instance with the given file name and target folder.
        //            /// <para>fileName : the file name can contains subfolders (e.g. folder1/folder2/myfile). File extension is optional and shouldn't be included.</para>
        //            /// <para>targetFolder : the folder to work with. It can be the Game folder or the logged-in user's dedicated folder.</para>
        //            /// <para>useDataCompression (optional, default true) : if true, the data will be compressed / decompressed.</para>
        //            /// </summary>
        //            public File(string fileName, Target targetFolder, bool useDataCompression = true)
        //            {
        //                if (fileName.StartsWith("/")) fileName = fileName.Substring(1);
        //                if (fileName == "") Console.Error("Trying to create a File instance with an invalid file name.");

        //                target = targetFolder;
        //                if (UserID == 0) { Console.Error("There's not a logged-in user."); }

        //                useCompression = useDataCompression;

        //                this.fileName = fileName;
        //            }

        //            /// <summary>
        //            /// [DEPRECATED] Save the given data to a remote file, into the user folder. If the operation fails, the method returns false and an error message is in the FileError property.
        //            /// <para>data : the data to save in the remote file.</para>
        //            /// </summary>
        //            public bool Save(object data)
        //            {
        //                if (fileName == "") { FileError = "No valid file name"; return false; }

        //                var serializer = new UniRESTBinary();
        //                serializer.LoadFromData(data);
        //                if (useCompression) serializer.CompressLoadedBytes();
        //                var dataToSend = serializer.ToString();

        //                var result = Rest.FileManager("unirestclientserver/filemanager/", "SAVE", dataToSend, GetFileName());
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    FileError = "";
        //                    return true;
        //                }
        //                else
        //                {
        //                    FileError = response.data;
        //                    return false;
        //                }
        //            }

        //            /// <summary>
        //            /// [DEPRECATED] Load data from a remote file of the user folder. If the operation fails, the method returns false and an error message is in the FileError property.
        //            /// </summary>
        //            public bool Load()
        //            {
        //                if(fileName == "") { FileError = "No valid file name"; return false; }

        //                var result = Rest.FileManager("unirestclientserver/filemanager/", "LOAD", "", GetFileName());
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    FileError = "";
        //                    loadedData = response.data;
        //                    return true;
        //                }
        //                else
        //                {
        //                    FileError = response.data;
        //                    loadedData = "";
        //                    return false;
        //                }
        //            }

        //            public T GetData<T>()
        //            {
        //                var deserializer = new UniRESTBinary();
        //                deserializer.LoadFromBinaryString(loadedData);
        //                if (useCompression) deserializer.DecompressLoadedBytes();
        //                return deserializer.ToObject<T>();
        //            }

        //            /// <summary>
        //            /// [DEPRECATED] Delete a remote file (the file only) from the user folder. This operation always return true: use the FileError property to check the presence of an error.
        //            /// </summary>
        //            public bool Delete()
        //            {
        //                if (fileName == "") { FileError = "No valid file name";  return false; }

        //                var result = Rest.FileManager("unirestclientserver/filemanager/", "DELETE", "", GetFileName());
        //                var response = result.Result;

        //                FileError = response.data;
        //                return true;
        //            }

        //            private string GetFileName()
        //            {
        //                if (target == Target.UserFolder) return UserID + "|" + fileName + "|USER"; else return UserID + "|" + fileName + "|GAME";
        //            }

        //            /// <summary>
        //            /// [DEPRECATED] Return the list of all the files found in the given folder, including the files under subfolders.
        //            /// </summary>
        //            public static string[] List(TargetRoot targetRootFolder, string subfolder = "")
        //            {
        //                if (subfolder.StartsWith("/")) subfolder = subfolder.Substring(1);

        //                var fileName = "";
        //                var data = subfolder + "|";

        //                switch (targetRootFolder)
        //                {
        //                    case TargetRoot.UserFilesFolder:
        //                        fileName = UserID + "|{USER_FILES_FOLDER}|USER";
        //                        break;
        //                    case TargetRoot.UserUploadsFolder:
        //                        fileName = UserID + "|{USER_UPLOADS_FOLDER}|USER";
        //                        break;
        //                    case TargetRoot.GameFilesFolder:
        //                        fileName = UserID + "|{GAME_FILES_FOLDER}|GAME";
        //                        break;
        //                    case TargetRoot.GameUploadsFolder:
        //                        fileName = UserID + "|{GAME_UPLOADS_FOLDER}|GAME";
        //                        break;
        //                    default:
        //                        fileName = "";
        //                        break;
        //                }

        //                var result = Rest.FileManager("unirestclientserver/filemanager/", "LIST", data, fileName);
        //                var response = result.Result;

        //                if (response.result == "OK")
        //                {
        //                    return response.data.Split('|');
        //                }
        //                else
        //                {
        //                    return default;
        //                }
        //            }
        //        }

        //#endregion


        //        #endregion

        #endregion

    }


    #region " JSON "

    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array = null;
        }
    }

    public class Json
    {

        private Dictionary<string, string> storage = new Dictionary<string, string>();

        /// <summary>
        /// Create a new empty JSON structure.
        /// </summary>
        public Json()
        {
            storage = new Dictionary<string, string>();
        }

        /// <summary>
        /// Create a JSON structure from the given JSON string representation.
        /// </summary>
        public Json(string jsonString)
        {
            storage = Parser(jsonString);
        }

        /// <summary>
        /// Add a new value, identified by the given name, to the JSON structure.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            if (storage.ContainsKey(name)) storage[name] = value.ToString(); else storage.Add(name, value.ToString());
        }

        /// <summary>
        /// Remove the given name from the JSON structure.
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            if (storage.ContainsKey(name)) storage.Remove(name);
        }

        /// <summary>
        /// Return the JSON string representation of the collected values.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var json = "{";

            foreach (KeyValuePair<string, string> s in storage)
            {
                json += "\"" + s.Key + "\":" + "\"" + s.Value.Replace("\"", "\\\"") + "\",";
            }

            json = json.Remove(json.Length - 1);

            json += "}";

            return json;
        }

        /// <summary>
        /// Return a value, from a JSON structure, identified by the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            if (storage.ContainsKey(name))
            {
                return (T)Convert.ChangeType(storage[name], typeof(T));
            } else
            {
                try
                {
                    return (T)Convert.ChangeType("", typeof(T));
                }
                catch (Exception)
                {
                    return (T)Convert.ChangeType("0", typeof(T));
                }
            }

        }

        private Dictionary<string, string> Parser(string jsonString)
        {
            var storage = new Dictionary<string, string>();

            jsonString = jsonString.Replace("{", "").Replace("}", "");

            var substrings = jsonString.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            foreach (var s in substrings)
            {
                var elements = s.Split(new string[] { "\":\"" }, StringSplitOptions.None);

                var key = elements[0];
                if (key.StartsWith("\"")) key = key.Substring(1);

                var value = elements[1];
                if (value.EndsWith("\"") && !value.EndsWith("\\\"")) value = value.Remove(elements[1].Length - 1);

                storage.Add(key, value);

            }

            return storage;
        }

    }

    public class JsonArray
    {
        private List<Json> storage = new List<Json>();

        /// <summary>
        /// Create a new empty JSON array.
        /// </summary>
        public JsonArray()
        {
            storage = new List<Json>();
        }

        /// <summary>
        /// Create a JSON array from the given JSON string representation.
        /// </summary>
        public JsonArray(string jsonString)
        {
            storage = Parser(jsonString);
        }

        /// <summary>
        /// Add a new JSON structure to the array.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(Json jsonData)
        {
            storage.Add(jsonData);
        }

        /// <summary>
        /// Return the JSON string representation of the collected data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var json = "[";

            foreach (var s in storage)
            {
                json += s.ToString() + ",";
            }

            json = json.Remove(json.Length - 1);

            json += "]";

            return json;
        }

        private List<Json> Parser(string jsonString)
        {
            var storage = new List<Json>();

            jsonString = jsonString.Replace("[", "").Replace("]", "");

            var substrings = jsonString.Split(new string[] { "},{" }, StringSplitOptions.None);

            foreach (var s in substrings)
            {
                var json = "{" + s.Trim('{').Trim('}') + "}";

                var newJson = new Json(json);

                storage.Add(newJson);
            }

            return storage;
        }

        /// <summary>
        /// Return a value, from a JSON structure, identified by the given name in the given array index.
        /// </summary>
        public T Get<T>(int index, string name)
        {
            if (index >= storage.Count)
            {
                try
                {
                    return (T)Convert.ChangeType("", typeof(T));
                }
                catch (Exception)
                {
                    return (T)Convert.ChangeType("0", typeof(T));
                }
            }

            var json = storage[index];

            return json.Get<T>(name);
            
        }

    }

#endregion


    #region " LOCAL FILE SAVE / LOAD "

    public class LocalFile
    {
        /// <summary>
        /// Locally save username and password for automatic login features (WebGL supported).
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Save(string username, string password)
        {
            var storage = new Dictionary<string, string>
                        {
                            { "username", username },
                            { "password", password }
                        };
            var fileName = Application.persistentDataPath + "/UniRESTClientLoginData.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream saveFile = File.Create(fileName);
            bf.Serialize(saveFile, storage);
            saveFile.Close();
        }

        /// <summary>
        /// Return a locally saved account for automatic login features (WebGL supported):
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Load()
        {
            var storage = new Dictionary<string, string>();

            var fileName = Application.persistentDataPath + "/UniRESTClientLoginData.dat";
            if (!File.Exists(fileName)) return storage;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(fileName, FileMode.Open);
            storage = (Dictionary<string, string>)bf.Deserialize(loadFile);
            loadFile.Close();

            return storage;
        }

    }

#endregion


    #region " DEBUG MODE "

    static class Console
    {
        public static void Log(string message, string url = "", string jsonContent = "", string decoded = "", Action action = null, Response response = null)
        {
            if (!UniRESTClient.debugMode) return;

            if (message != "") Debug.Log(message + "\n");

            UniRESTClient.debug.Add(message);

            if (url != "")
            {

                Debug.Log("<color='orange'>[TO SERVER]</color> <color='yellow'><b>" + url + "</b></color>\n<color='cyan'>" + jsonContent.Replace("\\", "") + "</color>");

                Debug.Log("<color='orange'>[FROM SERVER]</color> <color='yellow'>" + url + "</color>\n<color='lime'>" + action.data + "</color>");

                Debug.Log("<color='orange'>[DATABASE]</color> <color='yellow'>" + url + "</color> (<color='orange'>" + action.dberror + "</color>)\n<color='silver'>" + action.dbquery + "</color>");

                UniRESTClient.debug.Add(url);
                UniRESTClient.debug.Add(jsonContent.Replace("\\", ""));
                UniRESTClient.debug.Add(action.data);

            }
        }

        public static void HttpLog(string message, string url, string error)
        {
            if (!UniRESTClient.debugMode) return;

            if (url != "")
            {

                Debug.Log("<color='orange'>[TO SERVER]</color> <color='yellow'><b>" + url + "</b></color>\n");

                Debug.Log("<color='orange'>[SERVER REPLY]</color> <color='cyan'>" + message + "</color>\n");

                if (error != null) Debug.Log("<color='red'>[ERROR]</color> <color='yellow'>" + error + "</color>\n");

            }
        }

        public static void Info(string message)
        {
            if (!UniRESTClient.debugMode) return;

            Debug.Log("<color='cyan'>[INFO]</color> " + message + "\n");
            UniRESTClient.debug.Add(message);
        }

        public static void Error(string message)
        {
            if (!UniRESTClient.debugMode) return;

            Debug.Log("<color='red'>[ERROR]</color> " + message + "\n");
            UniRESTClient.debug.Add(message);
        }

        public static void Warning(string message)
        {
            if (!UniRESTClient.debugMode) return;

            Debug.Log("<color='orange'>[WARNING]</color> " + message + "\n");
            UniRESTClient.debug.Add(message);
        }

        public static void Internal(string url, string message)
        {
            if (!UniRESTClient.debugMode) return;

            var type = "";
            if (url.Contains("login")) type = "[LOGIN]";
            if (url.Contains("registration")) type = "[REGISTATION]";
            if (url.Contains("call")) type = "[AUTHORIZATION]";
            if (url.Contains("connect")) type = "[RESPONSE]";

            Debug.Log("<color='orange'>" + type + "</color> " + message + "\n");
            UniRESTClient.debug.Add(type + " " + message);
        }
    }

#endregion

}



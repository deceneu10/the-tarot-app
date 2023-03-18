using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace TigerForge
{

    public class UniRESTDownloadUpload
    {


        #region " DOWNLOAD "

        /// <summary>
        /// Initialize a file Download instance.
        /// </summary>
        public class Download
        {

            #region " VARIABLES "

            private class Queue
            {
                public string fileURL = "";
                public string destination = "";
                public int size = 0;
            }
            private List<Queue> queue;

            private UnityWebRequest www;
            private DateTime lastTimestamp;
            private bool isStopped = false;

            public string fileURL = "";

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
            /// If true, the Download system will try to complete an interrupted download if only a part of a file has been downloaded locally.
            /// </summary>
            public bool canResume = false;

            /// <summary>
            /// Set the speed with which the download informations are collected and released.
            /// </summary>
            public int delay = 250;

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

            #endregion

            #region " DOWNLOAD + ADD LINK "

            /// <summary>
            /// Initialize the Download engine.
            /// </summary>
            /// <param name="filesURL"></param>
            public Download()
            {
                queue = new List<Queue>();
            }

            /// <summary>
            /// Add a direct link to a remote file to this Download queue. The downloaded data will be saved in a file.
            /// </summary>
            public void AddLink(string fileURL, string localDestinationFolder)
            {
                if (fileURL == "") { Console.Error("Try to add an empty URL to the Download queue."); return; }

                queue.Add(new Queue {
                    fileURL = fileURL,
                    destination = GetDestinationFilePath(fileURL, localDestinationFolder)
                });
            }

            /// <summary>
            /// Add a direct link to a remote file to this Download queue. The downloaded data will be available as a bytes array.
            /// </summary>
            public void AddLink(string fileURL)
            {
                if (fileURL == "") { Console.Error("Try to add an empty URL to the Download queue."); return; }

                queue.Add(new Queue
                {
                    fileURL = fileURL,
                    destination = ""
                });
            }

            #endregion

            #region " START / STOP "

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

            #endregion


            #region " GET FILES SIZE "

            // Get all the files size of the queue. When done, start the download.

            /// <summary>
            /// Get the files size asking to the Server.
            /// </summary>
            private void GetFilesSizes(int index)
            {
                status.filesPath = new List<string>();
                www = UnityWebRequest.Head(queue[index].fileURL);
                www.SendWebRequest();

                GetFileSize(index);
            }

            /// <summary>
            /// Wait for the Server response with the file Size.
            /// </summary>
            /// <param name="index"></param>
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
                    queue[index].size = int.Parse(strSize);
                    index++;
                    if (index < queue.Count) GetFilesSizes(index); else StartDownloadQueue();
                }
                else
                {
                    GetFileSize(index);
                }
            }

            #endregion

            #region " START DOWNLOADING FILES "

            private void StartDownloadQueue()
            {
                status.totalBytes = 0;
                status.totalProgress = 0;
                status.totalFiles = queue.Count;
                foreach (var q in queue) status.totalBytes += q.size;

                SingleFileDownload(0);
            }

            private void SingleFileDownload(int index)
            {
                status.currentDestionationFile = "";
                status.currentFileURL = queue[index].fileURL;
                status.currentFileIndex = index + 1;
                status.currentFileSize = queue[index].size;
                lastTimestamp = DateTime.Now;

                www = new UnityWebRequest(queue[index].fileURL);

                if (canResume)
                {
                    int rangeFrom = 0;
                    int rangeTo = queue[index].size;
                    if (File.Exists(queue[index].destination)) rangeFrom = (int)new FileInfo(queue[index].destination).Length;
                    if (rangeFrom < rangeTo) www.SetRequestHeader("Range", "bytes=" + rangeFrom + "-" + rangeTo);
                }
                
                if (queue[index].destination == "")
                {
                    // Download as bytes array.
                    www.downloadHandler = new DownloadHandlerBuffer();
                }
                else
                {
                    // Download as file.
                    status.currentDestionationFile = queue[index].destination;
                    status.filesPath.Add(status.currentDestionationFile);
                    if (canResume)
                    {
                        www.downloadHandler = new DownloadHandlerFile(status.currentDestionationFile, true);
                    } else
                    {
                        www.downloadHandler = new DownloadHandlerFile(status.currentDestionationFile);
                    } 
                }

                www.SendWebRequest();

                RunDownload(index);
            }

            private async void RunDownload(int index)
            {
                await Task.Delay(delay);

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
                        if (index < queue.Count)
                        {
                            // Download as bytes array.
                            if (queue[index].destination == "") status.filesData.Add(www.downloadHandler.data);
                            SingleFileDownload(index);
                        }
                        else
                        {
                            // Download as file.
                            if (queue[index - 1].destination == "") status.filesData.Add(www.downloadHandler.data);

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

            #endregion


            #region " HELPERS "

            /// <summary>
            /// Get a full local destination file combining the destination folder and the file name extracted from the URL.
            /// </summary>
            /// <param name="fileURL"></param>
            /// <param name="destinationFolder"></param>
            /// <returns></returns>
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

            #endregion

        }

#endregion


#region " UPLOAD "

        /// <summary>
        /// Initialize a file Upload instance.
        /// </summary>
        public class Upload
        {

#region " VARIABLES "

            private string securityCode = "";
            private string uploaderSystemURL = "";

            private class Queue
            {
                public string localFile = "";
                public string destinationRemoteFolder = "";
                public int size = 0;
            }
            private List<Queue> queue;

            private UnityWebRequest www;
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
            /// Set the speed with which the upload informations are collected and released.
            /// </summary>
            public int delay = 250;

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

#endregion

#region " UPLOAD + ADD LINK "

            /// <summary>
            /// Initialize the Upload engine.
            /// <para>uploaderSystemURL : the URL to the Uploader application.</para>
            /// <para>securityCode : the securuty code which protects the application.</para>
            /// </summary>
            /// <param name="localFiles"></param>
            public Upload(string uploaderSystemURL, string securityCode)
            {
                this.uploaderSystemURL = uploaderSystemURL;
                this.securityCode = securityCode;
                queue = new List<Queue>();
            }

            /// <summary>
            /// Add a file to send to the Server.
            /// <para>localFileName : must contain the full path and file name to the local file.</para>
            /// <para>destinationRemoteFolder: the folder (and subfolders) where to store the uploaded file. If it doesn't exist, folders are created online. It mustn't start with slash (i.e. upload/assets/models/)</para>
            /// </summary>
            public void AddLink(string localFileName, string destinationRemoteFolder)
            {
                if (localFileName == "") { Console.Error("Try to add an empty local file name to the Upload queue."); return; }
                if (destinationRemoteFolder == "") { Console.Error("No destination URL."); return; }

                queue.Add(new Queue
                {
                    localFile = localFileName,
                    destinationRemoteFolder = destinationRemoteFolder
                });
            }

#endregion

#region " START / STOP "

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
                status.totalFiles = queue.Count;

                foreach (var q in queue)
                {
                    q.size = (int)new FileInfo(q.localFile).Length;
                    status.totalBytes += q.size;
                }

                StartUploadQueue(0);
            }

#endregion


#region " START UPLOADING FILES "

            private void StartUploadQueue(int index)
            {
                if (index >= queue.Count) return;

                var data = File.ReadAllBytes(queue[index].localFile);

                var fileName = GetFileName(queue[index].localFile);

                WWWForm form = new WWWForm();
                form.AddField("folder", queue[index].destinationRemoteFolder);
                form.AddField("code", securityCode);
                form.AddBinaryData("files[]", data, fileName);

                www = UnityWebRequest.Post(uploaderSystemURL, form);
                www.SendWebRequest();

                lastTimestamp = DateTime.Now;
                status.currentDestionationFile = Path.Combine(queue[index].destinationRemoteFolder, fileName);
                status.currentFileURL = queue[index].localFile;
                status.currentFileIndex = index + 1;
                status.currentFileSize = queue[index].size;

                RunUpload(index);
            }

            private async void RunUpload(int index)
            {
                await Task.Delay(delay);

                if (IsError())
                {
                    status.error = www.error;
                    if (onErrorCallBack != null) onErrorCallBack.Invoke(status);
                }
                else if (www.isDone)
                {
                    index++;
                    if (index < queue.Count)
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

#endregion


#region " HELPER "

            /// <summary>
            /// Extract the file name only (with extension) from a full path.
            /// </summary>
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


            #endregion

        }

#endregion


    }

}

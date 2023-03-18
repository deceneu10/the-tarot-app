using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace TigerForge
{


    #region " VARIABLES & STRUCTURES "

    public class Response
    {
        public string result = "";
        public string data = "";
    }

    public class Action
    {
        public string tokenl = "";
        public string tokenr = "";
        public string tokenw = "";

        public string data = "";
        public string dberror = "";
        public string dbquery = "";

        /// <summary>
        /// Return true if 'data' is an empty JSON string (data == {}).
        /// </summary>
        public bool DataIsEmptyJson()
        {
            return data == "{}";
        }
    }

    public class ActionResponse
    {
        public string result = "";
        public Action reply = new Action();
    }

    public class RestPackage
    {
        public string action = "";
        public string data = "";
        public string extra = "";

        /// <summary>
        /// Add an * flag to the action if UpdateReadingToken or UpdateWritingToken are 'true'.
        /// </summary>
        public void Init()
        {
            // DEPRECATED 3.4 16/05/2022
            //if (action == "READ")
            //{
            //    if (UniRESTClient.UpdateReadToken) action = "*" + action;
            //} else if (action == "WRITE" || action == "UPDATE" || action == "DELETE" || action == "MATH" || action == "UPDATEJSON")
            //{
            //    if (UniRESTClient.UpdateWriteToken) action = "*" + action;
            //}
        }
    }

    #endregion



    #region " REST ENGINE "

    public static class RestAsync
    {
        public class HTTPTokens
        {
            public static string tokenL = "";
            public static string tokenR = "";
            public static string tokenW = "";
            public static string userId = "";
        }

        public class HttpResponse
        {
            public enum Error
            {
                NONE,
                HTTP_ERROR,
                SYSTEM_ERROR
            }

            /// <summary>
            /// True if the HTTP Request has been done correctly and there is a response (data).
            /// </summary>
            public bool isArrived = false;

            /// <summary>
            /// The data string of the Server reply. Usually it's a JSON string that should be converted with JsonUtility.
            /// </summary>
            public string data = "";

            /// <summary>
            /// The HTTP response code.
            /// </summary>
            public long code = 0;

            /// <summary>
            /// The error message in case of HTTP failure. It can be both a Server message or a C# error.
            /// </summary>
            public string error = "";

            /// <summary>
            /// The kind of error: NONE default, HTTP_ERROR if Server error, SYSTEM_ERROR if C# error.
            /// </summary>
            public Error errorType = Error.NONE;

            /// <summary>
            /// Convert the JSON http response into a T object.
            /// </summary>
            public T GetJson<T>()
            {
                return JsonUtility.FromJson<T>(data);
            }

            /// <summary>
            /// Return the Response data decrypted.
            /// </summary>
            public string GetDecryptedResponse()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return EncryptionHelper.Decrypt(json.data);
            }

            /// <summary>
            /// Return the Response data as received.
            /// </summary>
            public string GetResponse()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return json.data;
            }

            /// <summary>
            /// Return the Response status as received.
            /// </summary>
            public string GetStatus()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return json.result;
            }

            /// <summary>
            /// True if the Response status = OK.
            /// </summary>
            public bool StatusIsOK()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return json.result == "OK";
            }

            /// <summary>
            /// Return the Response decrypted data as an integer value.
            /// </summary>
            public int GetDecryptedResponseAsInt()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return int.Parse(EncryptionHelper.Decrypt(json.data));
            }

            /// <summary>
            /// Return the Response data as received in integer format.
            /// </summary>
            public int GetResponseAsInt()
            {
                var json = JsonUtility.FromJson<Response>(data);
                return int.Parse(json.data);
            }
        }

        private static readonly string serverUrl = UniRESTClientConfig.ServerUrl.EndsWith("/") ? UniRESTClientConfig.ServerUrl : UniRESTClientConfig.ServerUrl + "/";

        /// <summary>
        /// Perform an asynchronouse HTTP request and releases an HttpResponse.
        /// <para>url : the http url to call (only the API part, ending with '/'; the http url is added by the call).</para>
        /// <para>content : the object to send. It must be an object. This object is converted to JSON and then encrypted by the call.</para>
        /// </summary>
        public static async UniTask<HttpResponse> Http(string url, object content)
        {
            HttpResponse resp = new HttpResponse();

            var jsonData = EncryptionHelper.Encrypt(JsonUtility.ToJson(content));
            var myURL = GetUrlToServer(url);
            var httpRequest = new UnityWebRequest();

            try
            {
                using (httpRequest)
                {
                    httpRequest.url = myURL;
                    httpRequest.method = UnityWebRequest.kHttpVerbPOST;
                    httpRequest.downloadHandler = new DownloadHandlerBuffer();
                    httpRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

                    httpRequest.SetRequestHeader("Content-Type", "application/json");
                    httpRequest.SetRequestHeader("Accept", "application/json");

                    httpRequest.SetRequestHeader("TOKENL", HTTPTokens.tokenL);
                    httpRequest.SetRequestHeader("TOKENR", HTTPTokens.tokenR);
                    httpRequest.SetRequestHeader("TOKENW", HTTPTokens.tokenW);
                    httpRequest.SetRequestHeader("TOKENI", HTTPTokens.userId);

                    await httpRequest.SendWebRequest();

                    var data = httpRequest.downloadHandler.text;

                    if (httpRequest.isDone && httpRequest.downloadHandler.isDone)
                    {
                        resp = new HttpResponse { data = data, code = httpRequest.responseCode, error = "", isArrived = true, errorType = HttpResponse.Error.NONE };
                    }
                    else
                    {
                        resp = new HttpResponse { data = data, code = httpRequest.responseCode, error = httpRequest.error, isArrived = false, errorType = HttpResponse.Error.HTTP_ERROR };
                    }
                }
            }
            catch (Exception e)
            {
                resp = new HttpResponse { data = "", code = 0, error = e.Message, isArrived = false, errorType = HttpResponse.Error.SYSTEM_ERROR };
                httpRequest.downloadHandler.Dispose();
                httpRequest.uploadHandler.Dispose();
                httpRequest.Dispose();
            }

            return resp;
        }

        /// <summary>
        /// Send a RestPackage ('action', 'data', 'extra') to the Server.
        /// <para>Reply: an ActionResponse, containing 'result' (OK or ERROR) and the Action 'data' (with tokens, db errors, and data).</para>
        /// <para>Notes: the Reply updates the Tokens.</para>
        /// </summary>
        public static async UniTask<ActionResponse> Action(string url, string action, object data, string extra)
        {
            var content = new RestPackage { action = action, data = JsonUtility.ToJson(data), extra = extra };

            var result = await Http(url, content);
            if (result.isArrived)
            {                
                var decoded = result.GetDecryptedResponse();
                var jAction = JsonUtility.FromJson<Action>(decoded);

                UpdateTokens(jAction.tokenl, jAction.tokenr, jAction.tokenw, UniRESTClient.UserID.ToString());
                UniRESTClient.DBerror = jAction.dberror;
                UniRESTClient.DBquery = jAction.dbquery;

                Console.Log("", url, JsonUtility.ToJson(content), decoded, jAction);
                return new ActionResponse { result = result.StatusIsOK() ? "OK" : "ERROR", reply = jAction };
            }
            else
            {

                Debug.LogWarning(result.errorType);
                Debug.LogWarning(result.error);
                Debug.LogWarning(url);

                if (result.errorType == HttpResponse.Error.HTTP_ERROR)
                {
                    UniRESTClient.DBerror = "CALL_URL_ERROR";
                    Console.Log("ERROR", url, JsonUtility.ToJson(content));
                    return new ActionResponse { result = "ERROR", reply = null };
                }
                else
                {
                    UniRESTClient.DBerror = "ERROR: " + result.error;
                    Console.Log("ERROR", url, result.error, UniRESTClient.DBerror);
                    return new ActionResponse { result = "ERROR", reply = null };
                }
            }

        }

        /// <summary>
        /// Send a call to the FileManager (Server).
        /// <para>Reply: a Response containing 'result' (OK or ERROR) and 'data' (two strings).</para>
        /// </summary>
        public static async UniTask<Response> FileManager(string url, string action, string data, string fileName)
        {
            var content = new RestPackage { action = action, data = data, extra = fileName };

            var result = await Http(url, content);

            if (result.isArrived)
            {
                return result.GetJson<Response>();
            }
            else
            {
                if (result.errorType == HttpResponse.Error.HTTP_ERROR)
                {
                    return new Response { result = "ERROR", data = "CALL_URL_ERROR" };
                }
                else
                {
                    return new Response { result = "ERROR", data = result.error };
                }
            }
        }

        /// <summary>
        /// Update the Tokens with the data arrived from the Server.
        /// </summary>
        public static void UpdateTokens(string tokenL, string tokenR, string tokenW, string userId)
        {
            HTTPTokens.tokenL = tokenL;
            HTTPTokens.tokenR = tokenR;
            HTTPTokens.tokenW = tokenW;
            HTTPTokens.userId = userId;
        }

        /// <summary>
        /// Update the Tokens if they are provided (empty strings are ignored).
        /// </summary>
        public static void UpdateSignleTokens(string tokenL, string tokenR, string tokenW)
        {
            if (tokenL != "") HTTPTokens.tokenL = tokenL;
            if (tokenR != "") HTTPTokens.tokenR = tokenR;
            if (tokenW != "") HTTPTokens.tokenW = tokenW;
        }

        /// <summary>
        /// Return the full Url to the Server, combined with the given path.
        /// </summary>
        public static string GetUrlToServer(string path)
        {
            return Path.Combine(serverUrl + "api/v2/", path);
        }
    }

    public static class Rest
    {
        private static HttpClient client = new HttpClient();
        private static readonly string serverUrl = UniRESTClientConfig.ServerUrl.EndsWith("/") ? UniRESTClientConfig.ServerUrl : UniRESTClientConfig.ServerUrl + "/";

        /// <summary>
        /// Send a generic data ('content') to the Server.
        /// <para>Reply: a Response containing 'result' (OK or ERROR) and 'data' (two strings).</para>
        /// <para>Notes: 'content' object is serialized to JSON and then encrypted. The received Reply is in clear text, but 'data' may be encrypted.</para>
        /// </summary>
        public static Task<Response> Send(string url, object content)
        {
            var tcs = new TaskCompletionSource<Response>();
            
            try
            {
                if (client.BaseAddress == null) Initialize();

                var responseTask = client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(JsonUtility.ToJson(content)));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    Console.Internal(url, readTask.Result);

                    var response = JsonUtility.FromJson<Response>(readTask.Result);
                    tcs.SetResult(response);

                }
                else
                {
                    var response = new Response { result = "ERROR", data = "" };
                    tcs.SetResult(response);
                }
            }
            catch (Exception)
            {
                var response = new Response { result = "ERROR", data = "" };
                tcs.SetResult(response);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Send a RestPackage ('action', 'data', 'extra') to the Server.
        /// <para>Reply: an ActionResponse, containing 'result' (OK or ERROR) and the Action 'data' (with tokens, db errors, and data).</para>
        /// <para>Notes: the Reply updates the Tokens.</para>
        /// </summary>
        public static Task<ActionResponse> Action(string url, RestPackage content)
        {
            var tcs = new TaskCompletionSource<ActionResponse>();
            Task<HttpResponseMessage> responseTask;
            HttpResponseMessage result;
            string decoded = "";
            string jsonContent = "";
            Action action = new Action();
            Response response = new Response();
            
            try
            {
                if (client.BaseAddress == null) Initialize();

                content.Init();
                jsonContent = JsonUtility.ToJson(content);
                responseTask = client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(jsonContent));
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    response = JsonUtility.FromJson<Response>(readTask.Result);

                    decoded = EncryptionHelper.Decrypt(response.data);
                    action = JsonUtility.FromJson<Action>(decoded);
                    SetTokens(action.tokenl, action.tokenr, action.tokenw, UniRESTClient.UserID.ToString());
                    tcs.SetResult(new ActionResponse { result = (response.result == "OK") ? "OK" : "ERROR", reply = action });
                    UniRESTClient.DBerror = action.dberror;
                    UniRESTClient.DBquery = action.dbquery;

                }
                else
                {
                    tcs.SetResult(new ActionResponse { result = "ERROR", reply = null });
                    UniRESTClient.DBerror = "CALL_URL_ERROR";
                }
            }
            catch (Exception e)
            {
                tcs.SetResult(new ActionResponse { result = "ERROR", reply = null });
                UniRESTClient.DBerror = "ERROR: " + e.Message;
            }

            Console.Log("", url, jsonContent, decoded, action, response);

            return tcs.Task;
        }

        /// <summary>
        /// Send a call to the FileManager (Server).
        /// <para>Reply: a Response containing 'result' (OK or ERROR) and 'data' (two strings).</para>
        /// </summary>
        public static Task<Response> FileManager(string url, string action, string data, string fileName)
        {
            var tcs = new TaskCompletionSource<Response>();

            var content = new RestPackage { action = action, data = data, extra = fileName };

            try
            {
                if (client.BaseAddress == null) Initialize();

                var responseTask = client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(JsonUtility.ToJson(content)));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    Console.Internal(url, readTask.Result);

                    var response = JsonUtility.FromJson<Response>(readTask.Result);
                    tcs.SetResult(response);

                }
                else
                {
                    var response = new Response { result = "ERROR", data = "" };
                    tcs.SetResult(response);
                }
            }
            catch (Exception)
            {
                var response = new Response { result = "ERROR", data = "" };
                tcs.SetResult(response);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Set the basic URL to call and the application/json format.
        /// </summary>
        public static void Initialize()
        {
            client.BaseAddress = new Uri(serverUrl + "api/v2/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Put the Tokens into the request Header (WebGL supported).
        /// </summary>
        public static void SetTokens(string tokenL, string tokenR, string tokenW, string userId)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("TOKENL", tokenL);
            client.DefaultRequestHeaders.Add("TOKENR", tokenR);
            client.DefaultRequestHeaders.Add("TOKENW", tokenW);
            client.DefaultRequestHeaders.Add("TOKENI", userId);
        }

        /// <summary>
        /// Return the full Url to the Server, combined with the given path.
        /// </summary>
        public static string GetUrlToServer(string path)
        {
            return Path.Combine(serverUrl + "api/v2/", path);
        }


        #region " ASYNC "

        /// <summary>
        /// Send a generic data ('content') to the Server.
        /// <para>Reply: a Response containing 'result' (OK or ERROR) and 'data' (two strings).</para>
        /// <para>Notes: 'content' object is serialized to JSON and then encrypted. The received Reply is in clear text, but 'data' may be encrypted.</para>
        /// </summary>
        public static async Task<Response> SendAsync(string url, object content)
        {
            try
            {
                if (client.BaseAddress == null) Initialize();

                var responseTask = await client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(JsonUtility.ToJson(content)));
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = await responseTask.Content.ReadAsStringAsync();

                    Console.Internal(url, readTask);

                    return JsonUtility.FromJson<Response>(readTask);
                }
                else
                {
                    return new Response { result = "ERROR", data = "" };
                }
            }
            catch (Exception)
            {
                return new Response { result = "ERROR", data = "" };
            }

        }

        /// <summary>
        /// Send a RestPackage ('action', 'data', 'extra') to the Server.
        /// <para>Reply: an ActionResponse, containing 'result' (OK or ERROR) and the Action 'data' (with tokens, db errors, and data).</para>
        /// <para>Notes: the Reply updates the Tokens.</para>
        /// </summary>
        public static async Task<ActionResponse> ActionAsync(string url, RestPackage content)
        {
            try
            {
                if (client.BaseAddress == null) Initialize();

                content.Init();
                var jsonContent = JsonUtility.ToJson(content);
                var result = await client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(jsonContent));

                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    var response = JsonUtility.FromJson<Response>(readTask);

                    var decoded = EncryptionHelper.Decrypt(response.data);
                    var action = JsonUtility.FromJson<Action>(decoded);
                    SetTokens(action.tokenl, action.tokenr, action.tokenw, UniRESTClient.UserID.ToString());
                    UniRESTClient.DBerror = action.dberror;
                    UniRESTClient.DBquery = action.dbquery;

                    Console.Log("", url, jsonContent, decoded, action, response);
                    return new ActionResponse { result = (response.result == "OK") ? "OK" : "ERROR", reply = action };
                }
                else
                {
                    UniRESTClient.DBerror = "CALL_URL_ERROR";
                    Console.Log("ERROR", url, jsonContent);
                    return new ActionResponse { result = "ERROR", reply = null };
                   
                }
            }
            catch (Exception e)
            {
                UniRESTClient.DBerror = "ERROR: " + e.Message;
                Console.Log("ERROR", url, e.Message);
                return new ActionResponse { result = "ERROR", reply = null };
                
            }
        }

        /// <summary>
        /// Send a call to the FileManager (Server).
        /// <para>Reply: a Response containing 'result' (OK or ERROR) and 'data' (two strings).</para>
        /// </summary>
        public static async Task<Response> FileManagerAsync(string url, string action, string data, string fileName)
        {
            var content = new RestPackage { action = action, data = data, extra = fileName };

            try
            {
                if (client.BaseAddress == null) Initialize();

                var result = await client.PostAsync(url, EncryptionHelper.HttpContentEncrypt(JsonUtility.ToJson(content)));
  
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();

                    Console.Internal(url, readTask);

                    var response = JsonUtility.FromJson<Response>(readTask);
                    return response;

                }
                else
                {
                    return new Response { result = "ERROR", data = "" };
                }
            }
            catch (Exception e)
            {
                return new Response { result = "ERROR", data = e.Message };
            }

        }

        #endregion

    }

    #endregion



    #region " ENCRYPTION / DECRYPTION "

    /// <summary>
    /// The system tool for encrypting and decrypting strings.
    /// </summary>
    public static class EncryptionHelper
    {
        private static byte[] password = Encoding.ASCII.GetBytes(UniRESTClientConfig.Key1);
        private static byte[] iv = Encoding.ASCII.GetBytes(UniRESTClientConfig.Key2);
        public static Encoding encoder = Encoding.UTF8;

        public static HttpContent HttpContentEncrypt(string plainText)
        {
            string encoded = Encrypt(plainText);
            return new StringContent(encoded, Encoding.UTF8);
        }

        /// <summary>
        /// Convert the given string into an encrypted string.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            if (plainText == "") return "";

            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = password;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            byte[] plainBytes = encoder.GetBytes(plainText);

            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            return cipherText;
        }

        /// <summary>
        /// Decrypt the given string into the original, clear-text string.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            if (cipherText == "") return "";

            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = password;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            string plainText = String.Empty;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] plainBytes = memoryStream.ToArray();

                plainText = encoder.GetString(plainBytes, 0, plainBytes.Length);
            }
            catch (Exception)
            {
                Console.Error("Something goes wrong with the Encryption system. In your UniRESTClientConfig script, check that the security Keys are the same generated by the UniREST Server application.");
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            return plainText;
        }

        /// <summary>
        /// Return the given string hashed into a Sha256 string.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Sha256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }

    #endregion


}

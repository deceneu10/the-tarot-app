
/*
 * =================================
 *  TIGERFORGE UniREST Client Tools
 * ---------------------------------
 *           Binary String
 * =================================
 */


// ==================================================================================
// If you want to use Odin Serialization, uncomment the following row:

// #define USE_ODIN_SERIALIZER

// ==================================================================================
// You must also set the UseOdinSerializer = true into the UniRESTOdinConfig.cs file.
// ==================================================================================


using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace TigerForge
{
    /// <summary>
    /// A Class for creating and decoding Binary Strings (Base64 Strings), working with Binary data, saving and loading Binary files.
    /// </summary>
    public class UniRESTBinary
    {

        #region " VARIABLES "

        /// <summary>
        /// The method to use for serialization and deserialization.
        /// <para>BinaryFormatter : .NET build-in technology for serialization in binary format.</para>
        /// <para>OdinSerializer : serialization technology developed by Sirenix IVS.</para>
        /// </summary>
        public enum SerializerType
        {
            BinaryFormatter,
            OdinSerializer
        }

        SerializerType serializerType = SerializerType.BinaryFormatter;
        List<Object> unityObjectReferences = new List<Object>();

        /// <summary>
        /// Where the given source is located.
        /// <para>UserDefined : the path specified by the user.</para>
        /// <para>Resources : the Unity's Resourcea folder.</para>
        /// <para>PersistentDataPath : the application folder for persistent data.</para>
        /// <para>AssetsPath : the application main folder.</para>
        /// <para>StreamingAssetsPath : the path to the StreamingAssets folder.</para>
        /// <para>TemporaryCachePath : the path to a temporary data / cache directory.</para>
        /// </summary>
        public enum SourcePosition
        {
            UserDefined,
            Resources,
            PersistentDataPath,
            AssetsPath,
            StreamingAssetsPath,
            TemporaryCachePath
        }

        /// <summary>
        /// Where the given source is locally located.
        /// <para>UserDefined : the path specified by the user.</para>
        /// <para>PersistentDataPath : the application folder for persistent data.</para>
        /// <para>AssetsPath : the application main folder.</para>
        /// <para>StreamingAssetsPath : the path to the StreamingAssets folder.</para>
        /// <para>TemporaryCachePath : the path to a temporary data / cache directory.</para>
        /// </summary>
        public enum LocalPosition
        {
            UserDefined,
            PersistentDataPath,
            AssetsPath,
            StreamingAssetsPath,
            TemporaryCachePath
        }

        /// <summary>
        /// The type of Resource you are going to load. To be used with SourcePosition = Resources.
        /// <para>TextAsset : the specified Resource is a text or binary file (.txt .html .xml .bytes .json .csv .yaml .fnt).</para>
        /// <para>Texture2D_* :  the specified Resource is a texture of the given type.</para>
        /// <para>Sprite_* :  the specified Resource is a Sprite that uses a texture of the given type.</para>
        /// <para>AudioClip :  the specified Resource is an audio file.</para>
        /// <para>None (default): it's not a Resource.</para>
        /// </summary>
        public enum ResourceType
        {
            TextAsset,
            Texture2D_JPG,
            Texture2D_PNG,
            Texture2D_EXR,
            Texture2D_TGA,
            Sprite_EXR,
            Sprite_JPG,
            Sprite_PNG,
            Sprite_TGA,
            AudioClip,
            None
        }

        byte[] bytes;

        #endregion


        #region " CONSTRUCTOR "

        /// <summary>
        /// Initialize a new UniREST Binary String encoding / decoding system.
        /// <para>serializerType (optional, BinaryFormatter): the method to use for serializing and deserializing data objects (when LoadFromData() method is used).</para>
        /// </summary>
        public UniRESTBinary(SerializerType serializerType = SerializerType.BinaryFormatter)
        {
            this.serializerType = serializerType;
        }

        #endregion


        #region " BYTE[] LOADER "

        /// <summary>
        /// Load binary data from a file and return true if data has been correctly loaded.
        /// <para>filePath : the file / resource name.</para>
        /// <para>sourcePosition (optional) : where the file / resource is located (PersistentDataPath by default).</para>
        /// <para>resourceType (optional) : when sourcePosition = Resources, the resource type.</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sourcePosition"></param>
        /// <param name="resourceType"></param>
        public bool LoadFromFile(string filePath, SourcePosition sourcePosition = SourcePosition.PersistentDataPath, ResourceType resourceType = ResourceType.None)
        {
            bytes = null;

            switch (sourcePosition)
            {
                case SourcePosition.UserDefined:
                    if (File.Exists(filePath)) bytes = File.ReadAllBytes(filePath);
                    break;
                case SourcePosition.PersistentDataPath:
                    filePath = Path.Combine(Application.persistentDataPath, filePath);
                    if (File.Exists(filePath)) bytes = File.ReadAllBytes(filePath); else bytes = default;
                    break;
                case SourcePosition.AssetsPath:
                    filePath = Path.Combine(Application.dataPath, filePath);
                    if (File.Exists(filePath)) bytes = File.ReadAllBytes(filePath); else bytes = default;
                    break;
                case SourcePosition.StreamingAssetsPath:
                    filePath = Path.Combine(Application.streamingAssetsPath, filePath);
                    if (File.Exists(filePath)) bytes = File.ReadAllBytes(filePath); else bytes = default;
                    break;
                case SourcePosition.TemporaryCachePath:
                    filePath = Path.Combine(Application.temporaryCachePath, filePath);
                    if (File.Exists(filePath)) bytes = File.ReadAllBytes(filePath); else bytes = default;
                    break;
                default:
                    break;
            }
            
            if (sourcePosition == SourcePosition.Resources)
            {
                switch (resourceType)
                {
                    case ResourceType.TextAsset:
                        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
                        if (textAsset != null) bytes = textAsset.bytes;
                        break;
                    case ResourceType.Texture2D_EXR:
                        Texture2D t2DAssetEXR = Resources.Load<Texture2D>(filePath);
                        if (t2DAssetEXR != null) bytes = t2DAssetEXR.EncodeToEXR();
                        break;
                    case ResourceType.Texture2D_JPG:
                        Texture2D t2DAssetJPG = Resources.Load<Texture2D>(filePath);
                        if (t2DAssetJPG != null) bytes = t2DAssetJPG.EncodeToJPG();
                        break;
                    case ResourceType.Texture2D_PNG:
                        Texture2D t2DAssetPNG = Resources.Load<Texture2D>(filePath);
                        if (t2DAssetPNG != null) bytes = t2DAssetPNG.EncodeToPNG();
                        break;
                    case ResourceType.Texture2D_TGA:
                        Texture2D t2DAssetTGA = Resources.Load<Texture2D>(filePath);
                        if (t2DAssetTGA != null) bytes = t2DAssetTGA.EncodeToTGA();
                        break;
                    case ResourceType.Sprite_EXR:
                        Sprite sprAssetEXR = Resources.Load<Sprite>(filePath);
                        if (sprAssetEXR != null) bytes = sprAssetEXR.texture.EncodeToEXR();
                        break;
                    case ResourceType.Sprite_JPG:
                        Sprite sprAssetJPG = Resources.Load<Sprite>(filePath);
                        if (sprAssetJPG != null) bytes = sprAssetJPG.texture.EncodeToJPG();
                        break;
                    case ResourceType.Sprite_PNG:
                        Sprite sprAssetPNG = Resources.Load<Sprite>(filePath);
                        if (sprAssetPNG != null) bytes = sprAssetPNG.texture.EncodeToPNG();
                        break;
                    case ResourceType.Sprite_TGA:
                        Sprite sprAssetTGA = Resources.Load<Sprite>(filePath);
                        if (sprAssetTGA != null) bytes = sprAssetTGA.texture.EncodeToTGA();
                        break;
                    case ResourceType.AudioClip:
                        AudioClip audioAsset = Resources.Load<AudioClip>(filePath);
                        if (audioAsset != null)
                        {
                            float[] audioData = new float[audioAsset.samples * audioAsset.channels];
                            audioAsset.GetData(audioData, 0);
                            bytes = ByteFromFloat(audioData);
                        }
                        break;
                    default:
                        break;
                }
            }

            return bytes != null;

        }

        /// <summary>
        /// Load the binary data from an object and return true if data has been loaded correctly.
        /// </summary>
        /// <param name="data"></param>
        public bool LoadFromData(object data)
        {
            bytes = null;

            switch (serializerType)
            {
                case SerializerType.BinaryFormatter:
                    bytes = SerializeBinary(data);
                    break;
                case SerializerType.OdinSerializer:
                    bytes = SerializeOdin(data);
                    break;
                default:
                    break;
            }

            return bytes != null;
        }

        /// <summary>
        /// Load the given byte array.
        /// </summary>
        /// <param name="bytes"></param>
        public void LoadFromBytes(byte[] bytes)
        {
            this.bytes = bytes;
        }

        /// <summary>
        /// Load the binary data from a Binary String.
        /// </summary>
        /// <param name="binaryString"></param>
        public void LoadFromBinaryString(string binaryString)
        {
            var bytes = FromBase64String(binaryString);
            this.bytes = bytes;
        }

        #endregion


        #region " TO... "

        /// <summary>
        /// Compress the loaded bytes.
        /// </summary>
        public void CompressLoadedBytes()
        {
            bytes = Compress(bytes);
        }

        /// <summary>
        /// Decompress the loaded bytes.
        /// </summary>
        public void DecompressLoadedBytes()
        {
            bytes = Decompress(bytes);
        }

        /// <summary>
        /// Return the loaded byte array as a Binary String suitable to be saved in a remote/local file or Database table.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToBase64String(bytes);
        }

        /// <summary>
        /// Convert the given Binary String into an object of the given data-type.
        /// <para>binaryString : the Binary String to convert.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryString"></param>
        /// <returns></returns>
        public T ToObject<T>(string binaryString)
        {
            var bytes = FromBase64String(binaryString);

            switch (serializerType)
            {
                case SerializerType.BinaryFormatter:
                    return (T)DeserializeBinary(bytes);
                case SerializerType.OdinSerializer:
                    return DeserializeOdin<T>(bytes);
                default:
                    return default;
            }

        }

        /// <summary>
        /// Convert the loaded Binary data into an object of the given data-type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToObject<T>()
        {
            if (bytes == null) return default;

            switch (serializerType)
            {
                case SerializerType.BinaryFormatter:
                    return (T)DeserializeBinary(bytes);
                case SerializerType.OdinSerializer:
                    return DeserializeOdin<T>(bytes);
                default:
                    return default;
            }

        }

        /// <summary>
        /// Convert the given Binary String into a Texture2D.
        /// <para>binaryString : the Binary String to convert.</para>
        /// <para>width, height : the dimension of the texture to produce.</para>
        /// </summary>
        /// <param name="binaryString"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Texture2D ToTexture2D(string binaryString, int width, int height)
        {
            var texture = new Texture2D(width, height);

            var bytes = FromBase64String(binaryString);

            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// Convert the loaded Binary data into a Texture2D.
        /// <para>width, height : the dimension of the texture to produce.</para>
        /// </summary>
        public Texture2D ToTexture2D(int width, int height)
        {
            if (bytes == null) return null;

            var texture = new Texture2D(width, height);

            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// Convert the given Binary String into an Audio Clip. Channels and sampleRate must be exactly the same values of the original audio file.
        /// <para>binaryString : the Binary String to convert.</para>
        /// <para>channels(optional) : the number of audio channels.</para>
        /// <para>sampleRate(optional) : the sample rate.</para>
        /// <para>name (optional) : a name for the Audio Clip.</para>
        /// </summary>
        /// <param name="binaryString"></param>
        /// <param name="name"></param>
        /// <param name="channels"></param>
        /// <param name="sampleRate"></param>
        /// <returns></returns>
        public AudioClip ToAudioClip(string binaryString, int channels = 2, int sampleRate = 44100, string name = "AudioClip")
        {
            var bytes = FromBase64String(binaryString);

            float[] audioData = FloatFromByte(bytes);

            var ac = AudioClip.Create(name, audioData.Length, channels, sampleRate, false);
            ac.SetData(audioData, 0);
            return ac;

        }

        /// <summary>
        /// Convert the loaded Binary data into an Audio Clip. Channels and sampleRate must be exactly the same values of the original audio file.
        /// <para>channels(optional) : the number of audio channels.</para>
        /// <para>sampleRate(optional) : the sample rate.</para>
        /// <para>name (optional) : a name for the Audio Clip.</para>
        /// </summary>
        public AudioClip ToAudioClip(int channels = 2, int sampleRate = 44100, string name = "AudioClip")
        {
            if (bytes == null) return null;

            float[] audioData = FloatFromByte(bytes);

            var ac = AudioClip.Create(name, audioData.Length, channels, sampleRate, false);
            ac.SetData(audioData, 0);
            return ac;

        }

        /// <summary>
        /// Save the given Binary String into a local File.
        /// <para>binaryString : the Binary String to save.</para>
        /// <para>filePath : the file name, with the extension, and optionally the part of path to add to the source position.</para>
        /// <para>localPosition (optional) : where the file has to be created (PersistentDataPath by defaul).</para>
        /// </summary>
        /// <param name="binaryString"></param>
        /// <param name="filePath"></param>
        /// <param name="sourcePosition"></param>
        public void ToFile(string binaryString, string filePath, LocalPosition localPosition = LocalPosition.PersistentDataPath)
        {
            var bytes = FromBase64String(binaryString);

            switch (localPosition)
            {
                case LocalPosition.UserDefined:
                    
                    break;
                case LocalPosition.PersistentDataPath:
                    filePath = Path.Combine(Application.persistentDataPath, filePath);
                    
                    break;
                case LocalPosition.AssetsPath:
                    filePath = Path.Combine(Application.dataPath, filePath);

                    break;
                case LocalPosition.StreamingAssetsPath:
                    filePath = Path.Combine(Application.streamingAssetsPath, filePath);

                    break;
                case LocalPosition.TemporaryCachePath:
                    filePath = Path.Combine(Application.temporaryCachePath, filePath);

                    break;
                default:
                    break;
            }

            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Save the loaded Binary data into a local File.
        /// <para>filePath : the file name, with the extension, and optionally the part of path to add to the source position.</para>
        /// <para>localPosition (optional) : where the file has to be created (PersistentDataPath by defaul).</para>
        /// </summary>
        public void ToFile(string filePath, LocalPosition localPosition = LocalPosition.PersistentDataPath)
        {
            if (bytes == null) return;

            switch (localPosition)
            {
                case LocalPosition.UserDefined:

                    break;
                case LocalPosition.PersistentDataPath:
                    filePath = Path.Combine(Application.persistentDataPath, filePath);

                    break;
                case LocalPosition.AssetsPath:
                    filePath = Path.Combine(Application.dataPath, filePath);

                    break;
                case LocalPosition.StreamingAssetsPath:
                    filePath = Path.Combine(Application.streamingAssetsPath, filePath);

                    break;
                case LocalPosition.TemporaryCachePath:
                    filePath = Path.Combine(Application.temporaryCachePath, filePath);

                    break;
                default:
                    break;
            }

            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Return the loaded byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetLoadedBytes()
        {
            return bytes;
        }

        #endregion


        #region " BYTE[] ENCRYPTION / DECRYPTION "

        /// <summary>
        /// Password protect the loaded bytes.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public byte[] Encrypt(string password)
        {
            password = (password + "easyfilesavesecure1234").Substring(0, 16);

            byte[] key = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] iv = System.Text.Encoding.UTF8.GetBytes(password);

            using (var rijndaelManaged = new System.Security.Cryptography.RijndaelManaged())
            {
                rijndaelManaged.KeySize = key.Length * 8;
                rijndaelManaged.Key = key;

                rijndaelManaged.BlockSize = iv.Length * 8;
                rijndaelManaged.IV = iv;

                using (var encryptor = rijndaelManaged.CreateEncryptor())
                using (var ms = new MemoryStream())
                using (var cryptoStream = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return ms.ToArray();
                }
            }

        }

        /// <summary>
        /// Unprotect loaded bytes.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public byte[] Decrypt(string password)
        {
            password = (password + "easyfilesavesecure1234").Substring(0, 16);

            byte[] key = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] iv = System.Text.Encoding.UTF8.GetBytes(password);

            using (var rijndaelManaged = new System.Security.Cryptography.RijndaelManaged())
            {
                rijndaelManaged.KeySize = key.Length * 8;
                rijndaelManaged.Key = key;

                rijndaelManaged.BlockSize = iv.Length * 8;
                rijndaelManaged.IV = iv;

                using (var decryptor = rijndaelManaged.CreateDecryptor())
                using (var ms = new MemoryStream(bytes))
                using (var cryptoStream = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                {

                    var dycrypted = new byte[bytes.Length];
                    var bytesRead = cryptoStream.Read(dycrypted, 0, bytes.Length);

                    return dycrypted.Take(bytesRead).ToArray();
                }
            }
        }

        #endregion


        #region " BYTE[] SERIALIZERS / DESERIALIZERS "


        private byte[] SerializeOdin(object data)
        {
#if USE_ODIN_SERIALIZER
            return OdinSerializer.SerializationUtility.SerializeValue(data, OdinSerializer.DataFormat.Binary, out unityObjectReferences);
#else
            return null;
#endif
        }

        private T DeserializeOdin<T>(byte[] bytes)
        {
#if USE_ODIN_SERIALIZER
            var obj = OdinSerializer.SerializationUtility.DeserializeValue<object>(bytes, OdinSerializer.DataFormat.Binary, unityObjectReferences);
            return (T)obj;
#else
            return default;
#endif
        }

        private byte[] SerializeBinary(object data)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            return stream.ToArray();

        }

        private object DeserializeBinary(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(bytes);
            var obj = formatter.Deserialize(stream);
            return obj;
        }

#endregion


#region " BYTE[] COMPRESSION / DECOMPRESSION "

        private byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        private byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

#endregion


#region " BASE64 STRING CONVERSION "

        /// <summary>
        /// Conver the given Base64 String into a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] FromBase64String(string data)
        {
            return System.Convert.FromBase64String(data);
        }

        /// <summary>
        /// Convert the given byte array into a Base64 String.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ToBase64String(byte[] data)
        {
            return System.Convert.ToBase64String(data);
        }

#endregion


#region " BYTE[] / FLOAT[] CONVERSION "

        private byte[] ByteFromFloat(float[] floatArray)
        {
            var byteArray = new byte[floatArray.Length * 4];
            System.Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        private float[] FloatFromByte(byte[] byteArray)
        {
            var floatArray = new float[byteArray.Length / 4];
            System.Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);
            return floatArray;
        }

#endregion


#region " UTILS "

        /// <summary>
        /// Execute a comparison between the loaded data and the compressed data, so as to estimate the rate compression. The instance must be initialized with useCompression = false.
        /// </summary>
        /// <returns></returns>
        public double GetRateWithCompression()
        {
            var compressed = Compress(bytes);
            double percent = (compressed.Length / (double)bytes.Length) * 100;
            percent = System.Math.Round(100 - percent, 2);
            return percent;
        }

        /// <summary>
        /// Generate a new Sprite with the given texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }

        /// <summary>
        /// Play the given Audio Clip.
        /// </summary>
        /// <param name="audioClip"></param>
        public void PlayAudioClip(AudioClip audioClip)
        {
            var GO = new GameObject();
            GO.AddComponent<AudioSource>();
            var audioSource = GO.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// Delete a file.
        /// <para>fileName : the name of the file to delete.</para>
        /// <para>localPosition (optional) : where the file is located (PersistentDataPath by default).</para>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="localPosition"></param>
        public void FileDelete(string fileName, LocalPosition localPosition = LocalPosition.PersistentDataPath)
        {
            switch (localPosition)
            {
                case LocalPosition.UserDefined:

                    break;
                case LocalPosition.PersistentDataPath:
                    fileName = Path.Combine(Application.persistentDataPath, fileName);

                    break;
                case LocalPosition.AssetsPath:
                    fileName = Path.Combine(Application.dataPath, fileName);

                    break;
                case LocalPosition.StreamingAssetsPath:
                    fileName = Path.Combine(Application.streamingAssetsPath, fileName);

                    break;
                case LocalPosition.TemporaryCachePath:
                    fileName = Path.Combine(Application.temporaryCachePath, fileName);

                    break;
                default:
                    break;
            }

            if (File.Exists(fileName)) File.Delete(fileName);
        }

#endregion


    } // End Class

} // End Namespace

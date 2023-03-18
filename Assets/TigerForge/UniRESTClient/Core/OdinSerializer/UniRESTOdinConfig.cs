using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class releases a list of data-types that Odin Serializer can serialize when you export your game for the WebGL platform.
/// Because generating a complete  list is pretty difficult, add the data-types may be missed here.
/// </summary>

namespace TigerForge
{
    public class UniRESTOdinConfig
    {
        /// <summary>
        /// Set this property to TRUE for enabling the use of the Sirenix Odin Serializer.
        /// </summary>
        /// =====================================================================================
        /// You must also uncomment the #define USE_ODIN_SERIALIZER in the UniRESTBinary.cs file.
        /// =====================================================================================
         
        public static bool UseOdinSerializer = false;
        
        /// =====================================================================================


        public static List<Type> Types(List<Type> foundTypes)
        {
            var types = new List<Type>();

            types.Add(typeof(Quaternion));
            types.Add(typeof(Vector4));
            types.Add(typeof(Vector3));
            types.Add(typeof(Vector2));
            types.Add(typeof(GameObject));
            types.Add(typeof(Color));
            types.Add(typeof(Color32));
            types.Add(typeof(Rect));

            types.Add(typeof(Quaternion[]));
            types.Add(typeof(Vector4[]));
            types.Add(typeof(Vector3[]));
            types.Add(typeof(Vector2[]));
            types.Add(typeof(GameObject[]));
            types.Add(typeof(Color[]));
            types.Add(typeof(Color32[]));
            types.Add(typeof(Rect[]));

            types.Add(typeof(List<Quaternion>));
            types.Add(typeof(List<Vector4>));
            types.Add(typeof(List<Vector3>));
            types.Add(typeof(List<Vector2>));
            types.Add(typeof(List<GameObject>));
            types.Add(typeof(List<Color>));
            types.Add(typeof(List<Color32>));
            types.Add(typeof(List<Rect>));

            types.Add(typeof(Dictionary<string, Quaternion>));
            types.Add(typeof(Dictionary<string, Vector4>));
            types.Add(typeof(Dictionary<string, Vector3>));
            types.Add(typeof(Dictionary<string, Vector2>));
            types.Add(typeof(Dictionary<string, GameObject>));
            types.Add(typeof(Dictionary<string, Color>));
            types.Add(typeof(Dictionary<string, Color32>));
            types.Add(typeof(Dictionary<string, Rect>));

            types.Add(typeof(byte));
            types.Add(typeof(byte[]));

            foreach (var ft in foundTypes) types.Add(ft);

            return types;
        }
    }

}

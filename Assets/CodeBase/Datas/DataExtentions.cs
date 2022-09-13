using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Datas
{
    public static class DataExtensions
    {
        private static readonly System.Random _rnd = new System.Random();

        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);

        public static T GetRandomItem<T>(this List<T> list)
        {
            int v = _rnd.Next(0, list.Count);

            return list[v];
        }
    }
}

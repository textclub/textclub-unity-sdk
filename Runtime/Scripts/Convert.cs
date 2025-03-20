using UnityEngine;

namespace Textclub
{
    public static class Convert
    {
        public static string ToString<T>(T obj)
        {
            var type = typeof(T);
            if (type.IsPrimitive || obj is Vector2 || obj is Vector3)
            {
                return obj.ToString();
            }
            else
            {
                return JsonUtility.ToJson(obj);
            }
        }

        public static T FromString<T>(string value)
        {
            System.Type type = typeof(T);
            return type switch
            {
                System.Type t when t == typeof(int) => (T)(object)int.Parse(value),
                System.Type t when t == typeof(float) => (T)(object)float.Parse(value),
                System.Type t when t == typeof(bool) => (T)(object)bool.Parse(value),
                System.Type t when t == typeof(System.Enum) => (T)System.Enum.Parse(type, value),
                System.Type t when t == typeof(Vector3) => (T)(object)ParseVector3(value),
                System.Type t when t == typeof(Vector2) => (T)(object)ParseVector2(value),
                _ => JsonUtility.FromJson<T>(value)
            };
        }

        private static Vector3 ParseVector3(string value)
        {
            var components = SplitIntoComponents(value, 3);
            return new Vector3(components[0], components[1], components[2]);
        }

        private static Vector2 ParseVector2(string value)
        {
            var components = SplitIntoComponents(value, 2);
            return new Vector3(components[0], components[1]);
        }

        private static float[] SplitIntoComponents(string value, int compomentCount)
        {
            // Remove the parentheses and split the string
            string[] components = value.Replace("(", "").Replace(")", "").Split(',');

            if (components.Length != compomentCount)
            {
                throw new System.ArgumentException("String format must be (x,y,z)");
            }

            return System.Array.ConvertAll(components, (string comp) => float.Parse(comp));
        }
    }
}

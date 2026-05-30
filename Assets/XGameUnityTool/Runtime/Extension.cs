using System;

namespace XGame
{
    /// <summary>
    /// 拓展
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// object 转 long
        /// </summary>
        public static long ToXLong(this object obj)
        {
            try
            {
                return (long)obj;
            }
            catch (Exception e)
            {
                return long.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// object 转 int
        /// </summary>
        public static int ToXInt(this object obj)
        {
            try
            {
                return (int)obj;
            }
            catch (Exception e)
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// object 转 float
        /// </summary>
        public static float ToXFloat(this object obj)
        {
            try
            {
                return (float)obj;
            }
            catch (Exception e)
            {
                return float.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// object 转 double
        /// </summary>
        public static double ToXDouble(this object obj)
        {
            try
            {
                return (double)obj;
            }
            catch (Exception e)
            {
                return double.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// object 转 bool
        /// </summary>
        public static bool ToXBool(this object obj)
        {
            return (bool)obj;
        }

        /// <summary>
        /// object 转 json
        /// </summary>
        public static string ToXJson(this object obj)
        {
            return XJson.ToJson(obj);
        }
    }
}
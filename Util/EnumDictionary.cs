using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace DianPing.BA.ReportCenter.Domain.Util
{
    public class EnumDictionary
    {
        /// <summary>
        ///   公共方法,生成字典用列表
        /// </summary>
        /// <param name="type"> 枚举对象 </param>
        /// <returns> value是与字段关联的文本值 text是Description的值 </returns>
        public static Dictionary<string, string> GetEnumIDictionary(Type type)
        {
            var result = new Dictionary<string, string>();
            foreach (FieldInfo fi in type.GetFields())
            {
                object[] arr = fi.GetCustomAttributes(typeof (DescriptionAttribute), true);
                if (arr.Length <= 0) continue;
                var key = fi.GetRawConstantValue();
                result.Add(key.ToString(), ((DescriptionAttribute) arr[0]).Description.ToString(
                    CultureInfo.InvariantCulture));
            }
            return result;
        }
    }
}
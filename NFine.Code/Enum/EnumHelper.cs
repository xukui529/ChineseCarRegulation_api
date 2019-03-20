using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code.Enum
{
    public static class EnumHelper
    {
        #region 枚举
        public static string GetEnumOptText(Type type, int value)
        {
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];
                if (((int)System.Enum.Parse(type, field.Name)).ToString() == value.ToString())
                {
                    object[] objs = field.GetCustomAttributes(typeof(EnumTextValue), false);
                    if (objs == null || objs.Length == 0)
                    {
                        return field.Name;
                    }
                    else
                    {
                        EnumTextValue da = (EnumTextValue)objs[0];
                        return da.Text;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 根据枚举类型返回类型中的所有值，文本及描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string[]> GetEnumOpt(Type type)
        {
            List<string[]> Strs = new List<string[]>();
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                string[] strEnum = new string[3];
                FieldInfo field = fields[i];
                //值列
                strEnum[1] = ((int)System.Enum.Parse(type, field.Name)).ToString();
                //文本列赋值
                strEnum[2] = field.Name;

                object[] objs = field.GetCustomAttributes(typeof(EnumTextValue), false);
                if (objs == null || objs.Length == 0)
                {
                    strEnum[0] = field.Name;
                }
                else
                {
                    EnumTextValue da = (EnumTextValue)objs[0];
                    //  DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    strEnum[0] = da.Text;
                }

                Strs.Add(strEnum);
            }
            return Strs;
        }
        #endregion

 

    }
}

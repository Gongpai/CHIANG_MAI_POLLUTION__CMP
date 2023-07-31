using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Unity.VisualScripting;

namespace GDD
{
    public static class MemberInfoGetting
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static List<object> GetFieldValues(object object_var)
        {
            var fieldValues = object_var.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Select(field => field.GetValue(object_var)).ToList();
            
            return fieldValues;
        }

        public static void SetFieldValues(List<object> value, object Object)
        {
            var fieldValues = Object.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            int i = 0;
            var _value = value.ToList();
               
            foreach (var _object in _value)
            {
                Type type_fieldValues = fieldValues[i].GetValue(Object).GetType();
                if (_object.GetType() != fieldValues[i].GetType())
                {
                    fieldValues[i].SetValue(Object, _object.ConvertTo(type_fieldValues));
                }
                else
                {
                    fieldValues[i].SetValue(Object, _object);
                }
                   
                i++;
            }
        }
    }
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Dependencies.NCalc;

namespace grimhawk.core
{
    public class PersistantData<T>
    {
        #region Public Variables
        public static implicit operator T (PersistantData<T> pData) => pData.value;
        public static List<string> keyList = new List<string>();
        #endregion

        #region Private Variables

        private T _localValue;
        private string _savedKey;
        private SettingTypes _type;

        #endregion

        public PersistantData(string key, T defaultValue)
        {
            _type = ValidateType();
            _savedKey = key;
            if (keyList.Contains(_savedKey))
                Debug.LogWarningFormat("Duplicate keys: {0}!", _savedKey);
            keyList.Add(_savedKey);
            _localValue = defaultValue;
            LoadFromPrefs();
            SaveToPrefs();
        }
        public T value
        {
            set
            {
                _localValue = value;
                SaveToPrefs();
                
            }
            get
            {
                return _localValue;
            }
        }

        private void SaveToPrefs()
        {
            switch (_type)
            {
                default:
                    Debug.LogError("Pref saving not defined for this type");
                    break;
                case SettingTypes._bool:
                    {
                        bool locBoolValue = (bool)Convert.ChangeType(_localValue, typeof(bool));
                        int prefValue = (locBoolValue ? 1 : 0);
                        PlayerPrefs.SetInt(_savedKey, prefValue);
                    }
                    break;
                case SettingTypes._int:
                    {
                        int locValue = (int)Convert.ChangeType(_localValue, typeof(int));
                        PlayerPrefs.SetInt(_savedKey, locValue);
                    }
                    break;
                case SettingTypes._uLong:
                    {
                        int lowBits, highBits;
                        ulong locValue = (ulong)Convert.ChangeType(_localValue, typeof(ulong));
                        SplituLong(locValue, out lowBits, out highBits);
                        PlayerPrefs.SetInt(_savedKey + "_lowBits", lowBits);
                        PlayerPrefs.SetInt(_savedKey + "_highBits", highBits);
                    }
                    break;
                case SettingTypes._float:
                    {
                        float locValue = (float)Convert.ChangeType(_localValue, typeof(float));
                        PlayerPrefs.SetFloat(_savedKey, locValue);
                    }
                    break;
                case SettingTypes._string:
                    {
                        string locValue = (string)Convert.ChangeType(_localValue, typeof(string));
                        PlayerPrefs.SetString(_savedKey, locValue);
                    }
                    break;
                case SettingTypes._dateTime:
                    {
                        DateTime dateValue = (DateTime)Convert.ChangeType(_localValue, typeof(DateTime));
                        string dateValueStr = dateValue.Ticks.ToString();
                        PlayerPrefs.SetString(_savedKey, dateValueStr);
                    }
                    break;
                case SettingTypes._vector2:
                case SettingTypes._vector3:
                case SettingTypes._vector4:
                case SettingTypes._color:
                case SettingTypes._quaternion:
                        Set(_savedKey, _localValue);
                 
                    break;
            }
        }
        private void LoadFromPrefs()
        {
            if (PlayerPrefs.HasKey(_savedKey) || PlayerPrefs.HasKey(_savedKey + "_lowBits") || PlayerPrefs.HasKey(_savedKey + "_highBits"))
            {
                switch (_type)
                {
                    default:
                        Debug.LogError("Pref loading not defined for this type");
                        break;
                    case SettingTypes._bool:
                        {
                            int prefValue = PlayerPrefs.GetInt(_savedKey);
                            bool prefBool = (prefValue != 0);
                            _localValue = (T)Convert.ChangeType(prefBool, typeof(T));
                        }
                        break;
                    case SettingTypes._int:
                        {
                            int prefValue = PlayerPrefs.GetInt(_savedKey);
                            _localValue = (T)Convert.ChangeType(prefValue, typeof(T));
                        }
                        break;
                    case SettingTypes._uLong:
                        {
                            int lowBits = PlayerPrefs.GetInt(_savedKey + "_lowBits");
                            int highBits = PlayerPrefs.GetInt(_savedKey + "_highBits");

                            ulong ret = (uint)highBits;
                            ret = (ret << 64);
                            ulong prefValue =(ulong)(ret | (ulong)(uint)lowBits);
                            _localValue = (T)Convert.ChangeType(prefValue, typeof(T));
                        }
                        break;
                    case SettingTypes._float:
                        {
                            float prefValue = PlayerPrefs.GetFloat(_savedKey);
                            _localValue = (T)Convert.ChangeType(prefValue, typeof(T));
                        }
                        break;
                    case SettingTypes._string:
                        {
                            string prefValue = PlayerPrefs.GetString(_savedKey);
                            _localValue = (T)Convert.ChangeType(prefValue, typeof(T));
                        }
                        break;
                    case SettingTypes._dateTime:
                        {
                            string prefValue = PlayerPrefs.GetString(_savedKey);
                            long ticks = long.Parse(prefValue);
                            DateTime dateValue = new DateTime(ticks);
                            _localValue = (T)Convert.ChangeType(dateValue, typeof(T));
                        }
                        break;
                    case SettingTypes._vector2:
                    case SettingTypes._vector3:
                    case SettingTypes._vector4:
                    case SettingTypes._color:
                    case SettingTypes._quaternion:
                         _localValue = Get(_savedKey);
                        break;
                }
            }
        }
        private SettingTypes ValidateType()
        {
            if (typeof(T) == typeof(bool))
                return SettingTypes._bool;
            else if (typeof(T) == typeof(int))
                return SettingTypes._int;
            else if (typeof(T) == typeof(ulong))
                return SettingTypes._uLong;
            else if (typeof(T) == typeof(float))
                return SettingTypes._float;
            else if (typeof(T) == typeof(string))
                return SettingTypes._string;
            else if (typeof(T) == typeof(DateTime))
                return SettingTypes._dateTime;
            else if (typeof(T) == typeof(Vector2))
                return SettingTypes._vector2;
            else if (typeof(T) == typeof(Vector3))
                return SettingTypes._vector3;
            else if (typeof(T) == typeof(Vector4))
                return SettingTypes._vector4;
            else if (typeof(T) == typeof(Quaternion))
                return SettingTypes._quaternion;
            else if (typeof(T) == typeof(Color))
                return SettingTypes._color;
            else
            {
                Debug.LogError("Type is Undefined!");
                return SettingTypes._undefined;

            }
        }
        static T Get(string key)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }

        static void Set(string key, T value)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }
        public enum SettingTypes
        {
            _bool,
            _int,
            _uLong,
            _float,
            _string,
            _dateTime,
            _vector2,
            _vector3,
            _vector4,
            _color,
            _quaternion,
            _undefined
        }
        #region Refactor These Methods
        public static long GetLong(string key, long defaultValue)
        {
            int lowBits, highBits;
            SplitLong(defaultValue, out lowBits, out highBits);
            lowBits = PlayerPrefs.GetInt(key + "_lowBits", lowBits);
            highBits = PlayerPrefs.GetInt(key + "_highBits", highBits);

            // unsigned, to prevent loss of sign bit.
            ulong ret = (uint)highBits;
            ret = (ret << 32);
            return (long)(ret | (ulong)(uint)lowBits);
        }

        public static long GetLong(string key)
        {
            int lowBits = PlayerPrefs.GetInt(key + "_lowBits");
            int highBits = PlayerPrefs.GetInt(key + "_highBits");

            // unsigned, to prevent loss of sign bit.
            ulong ret = (uint)highBits;
            ret = (ret << 32);
            return (long)(ret | (ulong)(uint)lowBits);
        }

        public static ulong GetuLong(string key, ulong defaultValue)
        {
            int lowBits, highBits;
            SplituLong(defaultValue, out lowBits, out highBits);
            lowBits = PlayerPrefs.GetInt(key + "_lowBits", lowBits);
            highBits = PlayerPrefs.GetInt(key + "_highBits", highBits);

            // unsigned, to prevent loss of sign bit.
            ulong ret = (uint)highBits;
            ret = (ret << 64);
            return (ulong)(ret | (ulong)(uint)lowBits);
        }

        public static ulong GetuLong(string key)
        {
            int lowBits = PlayerPrefs.GetInt(key + "_lowBits");
            int highBits = PlayerPrefs.GetInt(key + "_highBits");

            // unsigned, to prevent loss of sign bit.
            ulong ret = (uint)highBits;
            ret = (ret << 64);
            return (ulong)(ret | (ulong)(uint)lowBits);
        }

        private static void SplitLong(long input, out int lowBits, out int highBits)
        {
            // unsigned everything, to prevent loss of sign bit.
            lowBits = (int)(uint)(ulong)input;
            highBits = (int)(uint)(input >> 32);
        }

        private static void SplituLong(ulong input, out int lowBits, out int highBits)
        {
            // unsigned everything, to prevent loss of sign bit.
            lowBits = (int)(uint)(ulong)input;
            highBits = (int)(uint)(input >> 64);
        }

        public static void SetLong(string key, long value)
        {
            int lowBits, highBits;
            SplitLong(value, out lowBits, out highBits);
            PlayerPrefs.SetInt(key + "_lowBits", lowBits);
            PlayerPrefs.SetInt(key + "_highBits", highBits);
        }

        public static void SetuLong(string key, ulong value)
        {
            int lowBits, highBits;
            ulong locValue = (ulong)Convert.ChangeType(value, typeof(ulong));
            SplituLong(locValue, out lowBits, out highBits);
            PlayerPrefs.SetInt(key + "_lowBits", lowBits);
            PlayerPrefs.SetInt(key + "_highBits", highBits);
        }
        #endregion
    }



}


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

    public static class SavingSystem<T>
    {
        public static void SaveThisToJson(T toSave, string fileName) => SaveThisToJson(toSave, "", fileName);
        public static void SaveThisToJson(T toSave, string path, string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, path, (fileName + ".json"));
            Directory.CreateDirectory(Application.persistentDataPath + "/" + path);
            string toSaveString = JsonUtility.ToJson(toSave);
            File.WriteAllText(filePath, toSaveString);
            Debug.Log("saved" + toSave + " at: " + filePath );
        }

        public static T LoadData(T input, string filename) => LoadData(input, "", filename, out bool h);

        public static T LoadData(T input, string path, string filename, out bool success)
        {
            string filePath = Path.Combine(Application.persistentDataPath, path, (filename + ".json"));

            //deciding how to read the data properly
            if (File.Exists(filePath))
            {
                string cachedSaveString = File.ReadAllText(filePath);
            
                T cachedgeneric = input;
                if(input is MonoBehaviour || input is ScriptableObject)
                {
                    JsonUtility.FromJsonOverwrite(cachedSaveString, cachedgeneric);
                    //cachedgeneric = JsonConvert.DeserializeObject<T>(cachedSaveString);
                }
                else
                {
                    cachedgeneric = JsonUtility.FromJson<T>(cachedSaveString);
                //cachedgeneric = JsonConvert.DeserializeObject<T>(cachedSaveString);
                }
                success = true;
                return cachedgeneric;
            }
            else
            {
                //Debug.Log("No File found here" + filePath);
                success = false;
                return input;
            }
        }

        public static T LoadData(T input, string path, out bool success)
        {
            string filePath = path;

            //deciding how to read the data properly
            if (File.Exists(filePath))
            {
                string cachedSaveString = File.ReadAllText(filePath);
            
                T cachedgeneric = input;
                if(input is MonoBehaviour || input is ScriptableObject)
                {
                    JsonUtility.FromJsonOverwrite(cachedSaveString, cachedgeneric);
                    //cachedgeneric = JsonConvert.DeserializeObject<T>(cachedSaveString);
                }
                else
                {
                    cachedgeneric = JsonUtility.FromJson<T>(cachedSaveString);
                //cachedgeneric = JsonConvert.DeserializeObject<T>(cachedSaveString);
                }
                success = true;
                return cachedgeneric;
            }
            else
            {
                //Debug.Log("No File found here" + filePath);
                success = false;
                return input;
            }
        }
    }

    public static class SavingSystem{
        public static void SaveBinaryStringDictionary(Dictionary<string, string> value, string path){
            string filePath = Path.Combine(Application.persistentDataPath, (path + ".dat"));

            using(var stream = File.Open(filePath, FileMode.Create)){
                using(var writer = new BinaryWriter(stream, Encoding.UTF8, false)){
                    writer.Write(value.Count);
                    foreach (var item in value)
                    {
                        writer.Write(item.Key);
                        writer.Write(item.Value);
                    }
                }
            }
            Debug.Log("saved dictionary at: " + filePath + " with " + value.Count + " entries");
        }
        public static void ReadBinaryStringDictionary(ref Dictionary<string,string> bufferValue, string path){
            string filePath = Path.Combine(Application.persistentDataPath, (path + ".dat"));
            Dictionary<string,string> values = new Dictionary<string, string>();

            if(File.Exists(filePath)){
                using(var stream = File.Open(filePath, FileMode.Open)){
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false)){
                        int length = reader.ReadInt32(); 
                        for (int i = 0; i < length; i++)
                        {
                            values.Add(reader.ReadString(), reader.ReadString());
                        }
                    }   
                }
                bufferValue = values;
            }
        }
    }

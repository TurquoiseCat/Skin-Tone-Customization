using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using SkinTone.Patches;

namespace SkinTone
{
    public class Harmony_Patch
    {
        public static readonly string mod_name = "SkinToneCustomization";

        public static Dictionary<int, UnityEngine.Color> charaSkinTones;

        //modified from https://discussions.unity.com/t/simplest-dictionary-serialization-to-a-file/81321/2
        private static string savedDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/savedData";
        private static string dictionaryName = "PersistentDictionary.txt";
        private static string dictionaryFullPath = savedDataPath + "/" + dictionaryName;


        public Harmony_Patch()
        {
            try
            {
                var harmony = HarmonyInstance.Create(mod_name);
                harmony.PatchAll(typeof(Harmony_Patch).Assembly);
                loadDict();
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/error/error_log.txt", ex.Message);
            }
        }

        private UnityEngine.Color colorFromString(string r_str, string g_str, string b_str)
        {
            float r = float.Parse(r_str);
            float g = float.Parse(g_str);
            float b = float.Parse(b_str);
            return (new UnityEngine.Color(r, g, b));
        }

        public static void addToDict(int id, UnityEngine.Color color)
        {
            charaSkinTones[id] = color;
            saveDict();
        }

        private static void saveDict()
        {
            //FileLog.Log("saving dictionary");
            if (charaSkinTones != null)
            {
                string fileContent = "";
                foreach (var key in charaSkinTones.Keys)
                {
                    UnityEngine.Color color = charaSkinTones[key];
                    if (color == UnityEngine.Color.white)
                    {
                        continue;
                    }
                    fileContent += key + "," + color.r + "," + color.g + "," + color.b + "\n";
                }
                File.WriteAllText(dictionaryFullPath, fileContent);
            }
        }

        private void loadDict()
        {
            charaSkinTones = new Dictionary<int, UnityEngine.Color>();
            Directory.CreateDirectory(savedDataPath);
            if (File.Exists(dictionaryFullPath))
            {
                //FileLog.Log("loading dictionary");
                string[] fileContent = File.ReadAllLines(dictionaryFullPath);

                foreach (string line in fileContent)
                {
                    // saved as "name,r,g,b"
                    string[] buffer = line.Split(',');
                    if (buffer.Length == 4)
                    {
                        try
                        {
                            UnityEngine.Color color = colorFromString(buffer[1], buffer[2], buffer[3]);
                            charaSkinTones.Add(int.Parse(buffer[0]), color);
                        }
                        catch (Exception ex)
                        {
                            // skip line
                            File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/error/error_log.txt", "Error parsing dictionary, each line should be in the form \'id,red_value,green_value,blue_value\'. Error message: " + ex.Message);
                        }
                    }
                }
            }
            //else
            //{
            //    FileLog.Log("cant find dictionary");
            //}
        }


    }
}

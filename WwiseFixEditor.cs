/* Copyright (C) 2019 Adrian Babilinski
* You may use, distribute and modify this code under the
* terms of the MIT License
*
*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:
*
*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.
*
*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*
*For more information contact adrian@blackboxrealities.com or visit blackboxrealities.com
*/
#if UNITY_EDITOR
namespace Common.Tools
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    /// <summary>
    /// A fix for Cloud Build while using Wwise. Add to Editor Folder
    /// </summary>
    public class WwiseFixEditor : MonoBehaviour
    {
        [MenuItem("Tools/Fix Wwise Cloud Build")]
        public static void CreateNewGameObject()
        {
       
            string path = EditorUtility.OpenFolderPanel("Wwise Root Folder in Assets", "", "");
            GetDirectories(path);
            AssetDatabase.Refresh();
        }
 
        private static void GetDirectories(string path)
        {
            string[] directories = Directory.GetDirectories(path);
            ReplaceFiles(Directory.GetFiles(path), path);
 
            foreach (string directory in directories)
            {
                GetDirectories(directory);
            }
        }
        private static void ReplaceFiles(string[] files, string path)
        {
 
            foreach (string file in files)
            {
                if (file.EndsWith(".cs"))
                {
 
                    var text = File.ReadAllText(file);
 
 
                    var split = text.Split(Environment.NewLine.ToCharArray());
 
                    for (int i = 0; i < split.Length; i++)
                    {
                        //MAC
                        if (split[i].Contains("#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX"))
                        {
                            var old = split[i];
                            split[i] = old.Replace("#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX",
                                                    "#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || (UNITY_EDITOR_OSX && !UNITY_STANDALONE_WIN)");
                            continue;
                        }
                        if (split[i].Equals("#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX", StringComparison.OrdinalIgnoreCase))
                        {
 
                            split[i] = "#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || (UNITY_EDITOR_OSX && !UNITY_STANDALONE_WIN)";
                            continue;
                        }
 
 
                        //WIN
                        if (split[i].Contains("#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN"))
                        {
                            var old = split[i];
                            split[i] = old.Replace("#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN",
                                                   "#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA");
                            continue;
                        }
                        if (split[i].Equals("#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN",
                                            StringComparison.OrdinalIgnoreCase))
                        {
 
                            split[i] =
                                "#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA";
                            continue;
                        }
                    }
                    File.WriteAllLines(file, split);
 
 
 
 
             
 
                }
            }
        }
 
 
 
 
 
        public static void SetReleaseSetting()
        {
 
            AkPluginActivator.ActivateRelease();
 
        }
 
 
    }
}
    #endif

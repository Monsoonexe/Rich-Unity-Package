using UnityEditor;
using Sirenix.OdinInspector.Editor;
using RichPackage.Editor;
using Sirenix.OdinInspector;
using UnityEngine;
using RichPackage.Collections;
using static UnityEngine.Rendering.DebugUI;
using System;

namespace RichPackage.SaveSystem.Editor
{
    // Could this be a unit test?
    public class ES3TesterWindow : OdinEditorWindow
    {
        public ES3SerializableSettings settings;
        public Foo saveState;
        public string key = "Key1";

        [MenuItem(RichEditorUtility.WindowMenuName + "ES3 Testing Window")]
        public static void Init() => GetWindow<ES3TesterWindow>();

        protected override void OnEnable()
        {
            base.OnEnable();
            settings = new ES3SerializableSettings(true);
            settings.path += ".test.json";
        }

        [Button]
        public void OpenSaveFile()
        {
            System.Diagnostics.Process.Start(settings.FullPath);
        }

        [Button]
        public void TestSaving()
        {
            saveState.myString = "Hello saving world! \"this is a complicated string\".";
            saveState.derpIt = 344;
            saveState.flops = 3.14f;
            saveState.space = Space.World;
            saveState.time = System.DateTime.Now;
            saveState.table.Set("myKey2", "This is an embedded string. G[t Fuck]e{}");

            saveState.table.Set("myBar", new Bar()
            {
                barInt1 = 33,
                barFloat2 = 3.14123341231f,
                barString = "This is an even deeper, bar, embedded string. G[t Fuck]e{}",
                barTime = DateTime.Now
            });

            ES3.Save(key, saveState, settings);
        }

        [Button]
        public void TestLoading()
        {

        }

        [System.Serializable]
        public class Foo
        {
            public string myString;
            public int derpIt;
            public float flops;
            public Space space;
            public System.DateTime time;
            public JsonTable table = new JsonTable();
        }


        [System.Serializable]
        public class Bar
        {
            public int barInt1;
            public float barFloat2;
            public string barString;
            public System.DateTime barTime;
            public Vector3 vect;
        }
    }
}

using UnityEngine;
using UnityEditor;
using System;

namespace RichPackage.Editor
{
    /// <summary>
    /// Wrapper Unity's scripting symbols.
    /// </summary>
    public static class ScriptingSymbolsUtilities
    {
        public static void Define(string symbol)
        {
            // validate
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol));

            // dew it
            if (!IsDefined(symbol))
                SetSymbol(symbol);
        }

        public static void Undefine(string symbol)
        {
            // validate
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException(nameof(symbol));

            // dew it
            if (IsDefined(symbol))
                ClearSymbol(symbol);
        }

        public static bool IsDefined(string symbol)
        {
            return GetSymbols().Contains(symbol);
        }

        /// <summary>
        /// Make sure the symbol ends in a semicolon.
        /// </summary>
        private static void EnsureSemicolon(ref string symbol)
        {
            if (!symbol.QuickEndsWith(";"))
                symbol += ";";
        }

        private static string GetSymbols()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        }

        private static void SetSymbol(string symbol)
        {
            EnsureSemicolon(ref symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone,
                symbol + GetSymbols());
        }

        private static void ClearSymbol(string symbol)
        {
            EnsureSemicolon(ref symbol);
            string symbols = GetSymbols().Remove(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        }

        private static void PrintScriptingSymbols()
        {
            foreach (var s in GetSymbols().Split(';'))
                Debug.Log(s);
        }
    }
}

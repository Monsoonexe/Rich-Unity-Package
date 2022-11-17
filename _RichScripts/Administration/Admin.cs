using UnityEngine;
using System;

namespace RichPackage.Administration
{
    /// <summary>
    /// Admin account of the game. Can enable cheat codes!
    /// </summary>
    public static class Admin
    {
        private const bool StartInAdminMode = true;
        private const bool AlwaysAdminInUnityEditor = true;

        /// <summary>
        /// If true, any attempt to login will be successful.
        /// </summary>
        private const bool EasyLoginInEditor = true;
        private const string Admin_Password = "steward1";

        public static bool IsAdmin { get; private set; }

        public static event Action OnLogin;
        public static event Action OnLoginFail;
        public static event Action OnLogout;

        public static bool Login(string password)
        {
            //quick exit
            if (IsAdmin)
            {
                Debug.Log("Already logged in as admin.");
            }
            else
			{
                if (EasyEditorLogin() || ValidatePassword(password))
                {
                    LoginInternal();
                }
                else
                {
                    Debug.Log($"Login failed: incorrect password <{password}>.");
                    LoginFailInternal();
                }
            }

            return IsAdmin;
        }

        public static void Logout()
        {
            if (IsAdmin)
            {
                LogoutInternal();
            }
        }

        private static bool EasyEditorLogin()
		{
            return EasyLoginInEditor && Application.isEditor;
		}

        private static void LoginFailInternal()
        {
            OnLoginFail?.Invoke();
        }

        private static void LoginInternal()
		{
            IsAdmin = true;
            Debug.Log(RichText.Green("Admin logged in."));
            OnLogin?.Invoke();
        }

		private static void LogoutInternal()
        {
            IsAdmin = false;
            Debug.Log("Admin logged out.");
            OnLogout?.Invoke();
        }

        private static bool ValidatePassword(string maybePassword)
        {
            //super complex password table lookup here
            return maybePassword == Admin_Password;
        }
    }

}

// https://docs.unity3d.com/2022.3/Documentation/Manual/PlatformDependentCompilation.html
// Common scripting symbols
public static partial class ScriptingSymbols
{
    // Unity Editor
    public const string UNITY_EDITOR = "UNITY_EDITOR";
    public const string UNITY_EDITOR_WIN = "UNITY_EDITOR_WIN";
    public const string UNITY_EDITOR_OSX = "UNITY_EDITOR_OSX";
    public const string UNITY_EDITOR_LINUX = "UNITY_EDITOR_LINUX";

    // Standalone Platforms
    public const string UNITY_STANDALONE_OSX = "UNITY_STANDALONE_OSX";
    public const string UNITY_STANDALONE_WIN = "UNITY_STANDALONE_WIN";
    public const string UNITY_STANDALONE_LINUX = "UNITY_STANDALONE_LINUX";

    // Mobile Platforms
    public const string UNITY_IOS = "UNITY_IOS";
    public const string UNITY_IPHONE = "UNITY_IPHONE";
    public const string UNITY_VISIONOS = "UNITY_VISIONOS";
    public const string UNITY_ANDROID = "UNITY_ANDROID";
    public const string UNITY_TVOS = "UNITY_TVOS";

    // Web Platforms
    public const string UNITY_WEBGL = "UNITY_WEBGL";

    // Universal Windows Platform
    public const string UNITY_WSA = "UNITY_WSA";
    public const string UNITY_WSA_10_0 = "UNITY_WSA_10_0";

    // Others
    public const string UNITY_EMBEDDED_LINUX = "UNITY_EMBEDDED_LINUX";
    public const string UNITY_QNX = "UNITY_QNX";
    public const string UNITY_SERVER = "UNITY_SERVER";
    public const string UNITY_ANALYTICS = "UNITY_ANALYTICS";
    public const string UNITY_ASSERTIONS = "UNITY_ASSERTIONS";
    public const string UNITY_64 = "UNITY_64";
    public const string UNITY_CLOUD_BUILD = "UNITY_CLOUD_BUILD";

    // 
    public const string CSHARP_7_3_OR_NEWER = "CSHARP_7_3_OR_NEWER";
    public const string ENABLE_MONO = "ENABLE_MONO";
    public const string ENABLE_IL2CPP = "ENABLE_IL2CPP";
    public const string ENABLE_VR = "ENABLE_VR";

    // Input System
    public const string ENABLE_INPUT_SYSTEM = "ENABLE_INPUT_SYSTEM";
    public const string ENABLE_LEGACY_INPUT_MANAGER = "ENABLE_LEGACY_INPUT_MANAGER";
    public const string DEVELOPMENT_BUILD = "DEVELOPMENT_BUILD";
}

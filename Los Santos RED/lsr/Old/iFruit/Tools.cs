using Rage;
using Rage.Native;

namespace iFruitAddon2
{
    public static class Helper
    {
        public static string SetBold(this string text, bool bold)
        {
            if (bold) return text.ToLower();
            else return text.ToUpper();
        }
    }
}

public static class Tools
{
    internal static class Scripts
    {
        internal static void DestroyPhone(int handle)
        {
            NativeFunction.Natives.DESTROY_MOBILE_PHONE(handle);
        }

        internal static void StartScript(string scriptName, int buffer)
        {
            NativeFunction.Natives.REQUEST_SCRIPT(scriptName);

            while (!NativeFunction.Natives.HAS_SCRIPT_LOADED< bool>(scriptName))
            {
                NativeFunction.Natives.REQUEST_SCRIPT(scriptName);
                GameFiber.Yield();
            }

            NativeFunction.Natives.START_NEW_SCRIPT(scriptName, buffer);
            NativeFunction.Natives.SET_SCRIPT_AS_NO_LONGER_NEEDED(scriptName);
        }

        internal static void TerminateScript(string scriptName)
        {
            NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME(scriptName);
        }

    }
}
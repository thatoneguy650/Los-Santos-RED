using Rage.Native;

namespace iFruitAddon2
{
    public abstract class PhoneImage
    {
        /// <summary>
        /// Name of the image asset
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        /// Initialize the class.
        /// </summary>
        /// <param name="name">Name of the texture dictionary.</param>
        public PhoneImage(string name)
        {
            LoadTextureDict(name);
            Name = name;
        }

        /// <summary>
        /// Load a texture dictionary by name.
        /// </summary>
        /// <param name="txd">Name of the texture dictionary.</param>
        private void LoadTextureDict(string txd)
        {
            if (!NativeFunction.Natives.HAS_STREAMED_TEXTURE_DICT_LOADED<bool>(txd))
                NativeFunction.Natives.REQUEST_STREAMED_TEXTURE_DICT(txd, 0);
        }
    }
}
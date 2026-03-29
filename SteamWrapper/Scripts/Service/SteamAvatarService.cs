using Steamworks;
using System;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamAvatarService : SteamComponent
    {
        private Callback<AvatarImageLoaded_t> _avatarLoaded;

        public event Action<Sprite> AvatarLoaded;

        public override void Initialize()
        {
            _avatarLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarLoaded);
        }

        public Sprite GetMyLargeAvatar()
        {
            CSteamID steamId = SteamUser.GetSteamID();
            int imageId = SteamFriends.GetLargeFriendAvatar(steamId);

            if (imageId == -1)
                return null;

            if (imageId == 0)
                return null;

            return CreateSpriteFromImage(imageId);
        }

        private void OnAvatarLoaded(AvatarImageLoaded_t callback)
        {
            if (callback.m_steamID != SteamUser.GetSteamID())
                return;

            Sprite sprite = CreateSpriteFromImage(callback.m_iImage);
            if (sprite != null)
                AvatarLoaded?.Invoke(sprite);
        }

        private Sprite CreateSpriteFromImage(int imageId)
        {
            if (!SteamUtils.GetImageSize(imageId, out uint width, out uint height))
                return null;

            byte[] image = new byte[width * height * 4];

            if (!SteamUtils.GetImageRGBA(imageId, image, image.Length))
                return null;

            Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(image);
            texture.Apply();

            texture = FlipTexture(texture);

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }

        private Texture2D FlipTexture(Texture2D original)
        {
            Texture2D flipped = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);

            for (int y = 0; y < original.height; y++)
            {
                for (int x = 0; x < original.width; x++)
                {
                    flipped.SetPixel(x, y, original.GetPixel(x, original.height - y - 1));
                }
            }

            flipped.Apply();
            return flipped;
        }
    }
}
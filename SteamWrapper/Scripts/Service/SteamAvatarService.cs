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

            int rgbaSize = (int)(width * height * 4);
            byte[] rawImage = new byte[rgbaSize];

            if (!SteamUtils.GetImageRGBA(imageId, rawImage, rgbaSize))
                return null;

            // Создаем массив для "правильного" порядка строк
            byte[] flippedImage = new byte[rgbaSize];
            int stride = (int)width * 4; // Длина одной строки в байтах

            for (int y = 0; y < height; y++)
            {
                // Берем строку y и кладем её в позицию (height - 1 - y)
                Array.Copy(rawImage, y * stride, flippedImage, (height - 1 - y) * stride, stride);
            }

            Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(flippedImage);
            texture.Apply();

            // Теперь нам не нужно вызывать FlipTexture и удалять промежуточные текстуры,
            // так как мы создали сразу правильную текстуру из массива.
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

            // ВАЖНО: Texture2D — это unmanaged ресурс в контексте GPU.
            // Если мы создаем временную текстуру для обработки (например, для флипа),
            // старую нужно удалять через Destroy(), иначе возникнет утечка видеопамяти.
            Destroy(original); 
            return flipped;
        }

        private void OnDestroy()
        {
            if (_avatarLoaded != null)
            {
                _avatarLoaded.Dispose();
                _avatarLoaded = null;
            }
        }
    }
}
#nullable enable

using UnityEngine;
using UnityEngine.UI;

namespace Masks
{
    [ExecuteInEditMode]
    public class UIParallaxRawImage : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private Vector2 _parallaxSpeed = new Vector2(0.1f, 0f);
        [SerializeField] private float _scale;

        private Vector2 uvOffset;

        private int _lastWidth;
        private int _lastHeight;
        private float _lastScale;

        private void Start()
        {
            RecalculateUvRect();
        }

        void Update()
        {
            if (_rawImage == null) return;

            RecalculateUvRect();

            uvOffset += _parallaxSpeed * Time.deltaTime;
            _rawImage.uvRect = new Rect(uvOffset, _rawImage.uvRect.size);
        }

        private void RecalculateUvRect()
        {
            if (_rawImage == null) return;

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;

            if (_lastWidth == screenWidth && _lastHeight == screenHeight &&
                Mathf.Approximately(_lastScale, _scale)) return;

            float screenAspect = (float)screenWidth / screenHeight;
            float textureAspect = (float)_rawImage.texture.width / _rawImage.texture.height;

            Rect uv = _rawImage.uvRect;

            if (screenAspect > textureAspect)
            {
                // Screen is wider → crop top/bottom
                float scale = _scale * textureAspect / screenAspect;
                uv = new Rect(0, (1f - scale) / 2f, _scale, scale);
            }
            else
            {
                // Screen is taller → crop left/right
                float scale = _scale * screenAspect / textureAspect;
                uv = new Rect((1f - scale) / 2f, 0, scale, _scale);
            }

            _rawImage.uvRect = uv;

            _lastWidth = screenWidth;
            _lastHeight = screenHeight;
            _lastScale = _scale;
        }
    }
}
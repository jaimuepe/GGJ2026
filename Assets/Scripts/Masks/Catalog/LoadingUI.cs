#nullable enable

using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Masks.Catalog
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _titleContent;

        [SerializeField] private TextMeshProUGUI _loadingLabel;

        private void Awake()
        {
            _content.anchoredPosition = new Vector2(0.0f, -1200.0f);
            _titleContent.anchoredPosition = new Vector2(0.0f, -1200.0f);
        }

        private void Start()
        {
            StartCoroutine(TextCor());
        }

        private IEnumerator TextCor()
        {
            var i = 0;
            while (true)
            {
                _loadingLabel.text = i switch
                {
                    0 => "Getting ready for the \nparty",
                    1 => "Getting ready for the \nparty.",
                    2 => "Getting ready for the \nparty..",
                    3 => "Getting ready for the \nparty...",
                    _ => _loadingLabel.text
                };

                i = (i + 1) % 4;
                
                yield return new WaitForSeconds(0.5f);
            }
        }

        public Sequence Show()
        {
            var seq = DOTween.Sequence();
            seq.Append(_content.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));
            seq.Insert(0.3f, _titleContent.DOAnchorPosY(0.0f, 0.5f).SetEase(Ease.OutBack));

            return seq;
        }
    }
}
#nullable enable

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Masks
{
    public class GameButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action onClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}
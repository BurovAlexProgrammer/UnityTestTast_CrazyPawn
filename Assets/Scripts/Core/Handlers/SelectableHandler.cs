using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class SelectableHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, ReadOnly(true)] private bool _selected;
        
        public event Action<bool> SelectionChanged;

        public bool Selected => _selected;

        public void OnPointerClick(PointerEventData eventData)
        {
            _selected = !_selected;
            SelectionChanged?.Invoke(_selected);
        }

        public void SetSelection(bool selected, bool withNotify = true)
        {
            if (_selected == selected) return;

            _selected = selected;
            
            if (withNotify)
                SelectionChanged?.Invoke(selected);
        }
    }
}
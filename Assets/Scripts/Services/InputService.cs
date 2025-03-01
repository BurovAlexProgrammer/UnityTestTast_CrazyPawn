using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputService : ITickable
{
    public static event Action<GameObject, PointerEventData> OnDragStarted;
    public static event Action<GameObject, PointerEventData> OnDragging;
    public static event Action<GameObject, PointerEventData> OnDragFinished;
    public static event Action<GameObject, PointerEventData> OnClicked;

    private GameObject _pointerObject;
    private Vector3 _dragStartPos;
    private bool _isDraggingMode;
    private bool _isDragStarted;
    private bool _mouseDown;

    private const float DragThreshold = 5f;

    public void Tick()
    {
        if (_mouseDown && Vector3.Distance(_dragStartPos, Input.mousePosition) > DragThreshold)
        {
            _isDraggingMode = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mouseDown = true;
            _dragStartPos = Input.mousePosition;
        }
        
        if (Input.GetMouseButton(0) && _isDraggingMode)
        {
            ContinueDrag();
            TryStartDrag();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;

            FinishDrag();
        }
    }

    private void TryStartDrag()
    {
        if (!_isDraggingMode || _isDragStarted) return;

        _isDragStarted = true;
        UpdateObjectUnderPointer();
        OnDragStarted?.Invoke(_pointerObject, GetEventData());
        Debug.Log($"-- OnDragStarted object:{_pointerObject?.name} screenPos:{GetEventData().position}");
    }

    private void ContinueDrag()
    {
        if (!_isDragStarted) return;

        UpdateObjectUnderPointer();
        OnDragging?.Invoke(_pointerObject, GetEventData());
        Debug.Log($"OnDragging object:{_pointerObject?.name} screenPos:{GetEventData().position}");
    }

    private void FinishDrag()
    {
        UpdateObjectUnderPointer();
        
        if (_isDraggingMode)
        {
            OnDragFinished?.Invoke(_pointerObject, GetEventData());
            Debug.Log($"OnDragFinished object:{_pointerObject?.name} screenPos:{GetEventData().position}");   
        }
        else
        {
            OnClicked?.Invoke(_pointerObject, GetEventData());
            Debug.Log($"OnClicked object:{_pointerObject?.name} screenPos:{GetEventData().position}");   
        }

        _isDraggingMode = false;
        _isDraggingMode = false;
        _pointerObject = null;
    }

    private PointerEventData GetEventData()
    {
        return new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
    }

    private void UpdateObjectUnderPointer()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        _pointerObject = Physics.Raycast(ray, out var hit) ? 
            hit.collider.gameObject : 
            null;
    }
}
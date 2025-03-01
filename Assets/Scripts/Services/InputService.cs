using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputService : ITickable
{
    public event Action<GameObject, PointerEventData> DragStarted;
    public event Action<GameObject, PointerEventData> Dragging;
    public event Action<GameObject, PointerEventData> DragFinished;
    public event Action<GameObject, PointerEventData> OnClicked;

    private GameObject _pointerObject;
    private Vector3 _dragStartPos;
    private bool _isDraggingMode;
    private bool _isDragStarted;
    private bool _mouseDown;

    private const float DragThreshold = 5f;

    public void Tick()
    {
        if (Input.GetKey(KeyCode.Escape)) 
            Application.Quit();
        
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
        DragStarted?.Invoke(_pointerObject, GetEventData());
    }

    private void ContinueDrag()
    {
        if (!_isDragStarted) return;

        UpdateObjectUnderPointer();
        Dragging?.Invoke(_pointerObject, GetEventData());
    }

    private void FinishDrag()
    {
        UpdateObjectUnderPointer();
        
        if (_isDraggingMode)
        {
            DragFinished?.Invoke(_pointerObject, GetEventData());
        }
        else
        {
            OnClicked?.Invoke(_pointerObject, GetEventData());
        }

        _isDraggingMode = false;
        _isDragStarted = false;
        _pointerObject = null;
    }

    private PointerEventData GetEventData()
    {
        return new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition,
            pointerEnter = _pointerObject
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
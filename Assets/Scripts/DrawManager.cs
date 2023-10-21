using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public static Camera _camera;

    [SerializeField] private Line _linePrefab;

    public const float resolution = 0.1f;

    private Line _currentLine;

    public Action OnMouseLifted;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = Vector2.zero;
        
        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(_linePrefab, mousePos, quaternion.identity);
            _currentLine.SetStartPoint(mousePos);
        }
        if (Input.GetMouseButton(0))
        {
            _currentLine.SetPosition(mousePos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseLifted?.Invoke();
        }
        
        if (Input.touches[0].phase == TouchPhase.Began)
        {
            touchPos = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            _currentLine = Instantiate(_linePrefab, touchPos, quaternion.identity);
            _currentLine.SetStartPoint(touchPos);
        }
        if (Input.touches[0].phase == TouchPhase.Moved)
        {
            touchPos = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            _currentLine.SetPosition(mousePos);
        }
        if (Input.touches[0].phase == TouchPhase.Ended)
        {
            OnMouseLifted?.Invoke();
        }
    }

    public void ClearLine()
    {
        Destroy(_currentLine.gameObject);
    }
}

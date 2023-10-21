using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DrawManager _drawManager;
    [SerializeField] private Circle _circlePrefab;
    [SerializeField] private float minCircleCount = 5;
    [SerializeField] private float maxCircleCount = 10;
    [SerializeField] private Button _retryButton;
    
    private List<Circle> _circles = new List<Circle>();

    private float _minDistance;
    private Camera _camera;

    public void SpawnCircles()
    {
        int range = (int)Random.Range(minCircleCount, maxCircleCount + 1);

        List<Vector2> points = new List<Vector2>();
        
        for (int i = 0; i < range; i++)
        {
            float x = Random.Range(0.15f, 0.85f);
            float y = Random.Range(0.10f, 0.95f);
            Vector3 pos = new Vector3(x, y, 0f);
            points.Add(pos);
            if (points.Count > 1)
            {
                if (Vector3.Distance(points[points.Count - 2], points[points.Count - 1]) < _minDistance)
                {
                    points[points.Count - 1] += Vector2.one * _minDistance;
                }
            }

            pos = _camera.ViewportToWorldPoint(points[points.Count - 1]);
            pos.z = 0f;
            
            
            var shape = Instantiate(_circlePrefab, transform);
            shape.transform.position = pos;
            _circles.Add(shape);
        }
        _drawManager.gameObject.SetActive(true);
    }

    private void Start()
    {
        _drawManager.OnMouseLifted += CheckCrossedCircles;
        _retryButton.onClick.AddListener(ResetGame);
        _drawManager.gameObject.SetActive(false);
        _camera = Camera.main;
        _minDistance = _circlePrefab.GetComponent<CircleCollider2D>().radius;
        SpawnCircles();
    }

    private void CheckCrossedCircles()
    {
        _drawManager.gameObject.SetActive(false);

        foreach (var VARIABLE in _circles)
        {
            if (VARIABLE.isCrossed)
            {
                //Do tween
                
                VARIABLE.gameObject.SetActive(false);
            }
        }
        _drawManager.gameObject.SetActive(false);
        ToggleRestartButton(true);
        //_drawManager.ClearLine();
        //ClearCircles();
    }

    private void ClearCircles()
    {
        _drawManager.gameObject.SetActive(false);
        foreach (var shape in _circles)
        {
            Destroy(shape.gameObject);
        }
        
        _circles = new List<Circle>();
    }

    private void ToggleRestartButton(bool toggle)
    {
        _retryButton.gameObject.SetActive(toggle);
    }
    
    private void ReloadCircles()
    {
        _drawManager.gameObject.SetActive(false);
        SpawnCircles();
    }

    public void ResetGame()
    {
        _drawManager.ClearLine();
        ClearCircles();
        ReloadCircles();
        ToggleRestartButton(false);
    }
}

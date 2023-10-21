using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EdgeCollider2D _collider;

    private List<Vector2> _points = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        _collider.transform.position -= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector2 pos)
    {
        if(!CanExtendLine(pos)) return;

        _points.Add(pos);
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pos);

        _collider.points = _points.ToArray();
    }

    public void SetStartPoint(Vector2 pos)
    {
        _lineRenderer.SetPosition(0, pos);
        _points.Insert(0,pos);
    }
    
    private bool CanExtendLine(Vector2 pos)
    {
        if (_lineRenderer.positionCount == 0)
        {
            return false;
        }

        return (Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), pos) > DrawManager.resolution);
    }
}

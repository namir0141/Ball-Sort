using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Transform> points = new List<Transform>();
    private Transform lastPoint;
    private bool isDrawing = false;
    private bool isStartingPointSet = false;
    public string startingPointTag = "StartingPoint"; // Tag for the starting point object

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void MakeLine(Transform newPoint)
    {
        points.Add(newPoint);
        SetupLine();
    }

    private void SetupLine()
    {
        int pointCount = points.Count;
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isStartingPointSet)
            {
                GameObject startingObj = FindStartingPoint();
                if (startingObj != null)
                {
                    lastPoint = startingObj.transform;
                    isStartingPointSet = true;
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!isDrawing)
                    {
                        lastPoint = hit.collider.transform;
                        isDrawing = true;
                        MakeLine(lastPoint);
                    }
                    else if (hit.collider.transform != lastPoint && !points.Contains(hit.collider.transform))
                    {
                        lastPoint = hit.collider.transform;
                        MakeLine(lastPoint);
                    }
                    else if (points.Contains(hit.collider.transform))
                    {
                        ResetLine();
                        isStartingPointSet = false;
                    }
                }
            }
        }

        if (isDrawing && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform != lastPoint && !points.Contains(hit.collider.transform))
                {
                    lastPoint = hit.collider.transform;
                    MakeLine(lastPoint);
                }
            }
        }
    }

    private GameObject FindStartingPoint()
    {
        GameObject[] startingPoints = GameObject.FindGameObjectsWithTag(startingPointTag);

        if (startingPoints.Length > 0)
        {
            return startingPoints[0];
        }

        return null;
    }

    private void ResetLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        isDrawing = false;
        lastPoint = null;
    }
}

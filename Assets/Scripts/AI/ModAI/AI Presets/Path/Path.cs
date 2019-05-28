using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    public bool showMarkers = true;
    public List<Transform> pathMarkers = new List<Transform>();


    // Update is called once per frame
    void Update()
    {
        for (int i = pathMarkers.Count - 1; i >= 0; i--)
        {
            if (!showMarkers)
            {
                pathMarkers[i].GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                pathMarkers[i].GetComponent<MeshRenderer>().enabled = true;
            }
            if (i == 0)
            {
                Debug.DrawRay(pathMarkers[i].position, gameObject.transform.position - pathMarkers[i].position, Color.red);
            }
            else
            {
                Debug.DrawRay(pathMarkers[i].position, pathMarkers[i - 1].position - pathMarkers[i].position, Color.green);
            }
            Debug.DrawRay(pathMarkers[0].position, pathMarkers[pathMarkers.Count - 1].position - pathMarkers[0].position, Color.green);
        }
    }

    public void NewMarker()
    {
        Transform marker;
        if (pathMarkers.Count == 0)
        {
            NewMarker(gameObject.GetComponent<AI_Controller>().transform.position);
        }
        if (pathMarkers.Count > 1)
        {
            marker = Instantiate(Resources.Load<GameObject>(@"Prefabs/Marker"), pathMarkers[pathMarkers.Count - 1].TransformPoint(DirectionToSpawn(pathMarkers[pathMarkers.Count - 1], pathMarkers[pathMarkers.Count - 2]) * 5), new Quaternion()).transform;
        }
        else
        {
            marker = Instantiate(Resources.Load<GameObject>(@"Prefabs/Marker"), gameObject.transform.TransformPoint(Vector3.forward * 5), new Quaternion()).transform;
        }
        marker.name = "Marker " + pathMarkers.Count;
        pathMarkers.Add(marker);
    }
    public void NewMarker(Vector3 position)
    {
        Transform marker = Instantiate(Resources.Load<GameObject>(@"Prefabs/Marker"), position, new Quaternion()).transform;
        marker.name = "Marker " + pathMarkers.Count;
        pathMarkers.Add(marker);
    }
    public static void PlaceDebugMarker(Vector3 position)
    {
        Destroy(GameObject.Find("Debug Marker"));
        Transform marker = Instantiate(Resources.Load<GameObject>(@"Prefabs/Marker"), position, new Quaternion()).transform;
        marker.name = "Debug Marker";
    }
    public void RemoveMarker(int index)
    {
        if (pathMarkers.Count > 0)
        {
            DestroyImmediate(pathMarkers[index].gameObject);
            pathMarkers.Remove(pathMarkers[index]);
        }
    }

    private Vector3 DirectionToSpawn(Transform start, Transform previousMarker)
    {
        RaycastHit hitForward;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        Physics.Raycast(start.position, start.TransformPoint(Vector3.forward) - start.position, out hitForward, 2000);
        Physics.Raycast(start.position, start.TransformPoint(Vector3.left) - start.position, out hitLeft, 2000);
        Physics.Raycast(start.position, start.TransformPoint(Vector3.right) - start.position, out hitRight, 2000);
        if (hitForward.distance > hitLeft.distance && hitForward.distance > hitRight.distance)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.forward * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, 0, hitLeft.distance, hitRight.distance);
            }
            else
            {
                return Vector3.forward;
            }
        }
        else if (hitLeft.distance > hitForward.distance && hitLeft.distance > hitRight.distance)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.left * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, hitForward.distance, 0, hitRight.distance);
            }
            else
            {
                return Vector3.left;
            }
        }
        else if (hitRight.distance > hitForward.distance && hitRight.distance > hitLeft.distance)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.right * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, hitForward.distance, hitLeft.distance, 0);
            }
            else
            {
                return Vector3.right;
            }
        }
        else
        {
            return Vector3.up;
        }
    }
    private Vector3 DirectionToSpawn(Transform start, Transform previousMarker, float hF, float hL, float hR)
    {
        if (hF > hL && hF > hR)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.forward * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, 0, hL, hR);
            }
            else
            {
                return Vector3.forward;
            }
        }
        else if (hL > hF && hL > hR)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.left * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, hF, 0, hR);
            }
            else
            {
                return Vector3.left;
            }
        }
        else if (hR > hF && hR > hL)
        {
            if (Vector3.Distance(start.TransformPoint(Vector3.right * 5), previousMarker.position) < Vector3.Distance(start.position, previousMarker.position))
            {
                return DirectionToSpawn(start, previousMarker, hF, hL, 0);
            }
            else
            {
                return Vector3.right;
            }
        }
        else
        {
            return Vector3.up;
        }
    }
}

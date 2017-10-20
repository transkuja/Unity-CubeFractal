using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalCube : MonoBehaviour {

    [SerializeField]
    Vector3[] offsets;

    [SerializeField]
    [Range(0,4)]int iterations = 1;
    List<Vector3> finalPositions = new List<Vector3>();

    void Start () {
        finalPositions.Add(Vector3.zero);
        ComputeCube(iterations);
        DrawCubes();
    }

    void ComputeCube(int iterations)
    {
        if (iterations > 0)
        {
            List<Vector3> tmpPositions = new List<Vector3>();
            foreach (Vector3 position in finalPositions)
            {
                foreach (Vector3 offset in offsets)
                {
                    tmpPositions.Add((position + offset) / 3);
                }
            }
            finalPositions = tmpPositions;

            ComputeCube(--iterations);
        }
    }

    void DrawCubes()
    {
        foreach (Vector3 position in finalPositions)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = position;
            go.transform.localScale *= Mathf.Pow((1.0f/3), iterations);
        }
    }
}

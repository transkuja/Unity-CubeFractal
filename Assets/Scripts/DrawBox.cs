using UnityEngine;
using System.Collections;

public class DrawBox : MonoBehaviour
{

    public int instanceCount;
    public Mesh instanceMesh;
    public Material instanceMaterial;

    private int cachedInstanceCount = -1;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer colorBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };


    [SerializeField]
    Vector3 generationSize;

    void Start()
    {
        instanceCount = (int)generationSize.x * (int)generationSize.y * (int)generationSize.z;
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();
    }

    void Update()
    {

        // Update starting position buffer
        if (cachedInstanceCount != instanceCount)
            UpdateBuffers();

        // Pad input
        if (Input.GetAxisRaw("Horizontal") != 0.0f)
            instanceCount = (int)Mathf.Clamp(instanceCount + Input.GetAxis("Horizontal") * 40000, 1.0f, 5000000.0f);

        // Render

        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
    }

    void OnGUI()
    {

        GUI.Label(new Rect(265, 25, 200, 30), "Instance Count: " + instanceCount.ToString());
        instanceCount = (int)GUI.HorizontalSlider(new Rect(25, 20, 200, 30), (float)instanceCount, 1.0f, 5000000.0f);
    }

    void UpdateBuffers()
    {

        // positions
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = new ComputeBuffer(instanceCount, 16);
        Vector4[] positions = new Vector4[instanceCount];

        int sizeX = (int)generationSize.x;
        int sizeY = (int)generationSize.y;
        int sizeZ = (int)generationSize.z;

        for (int i = 0; i < generationSize.x; i++)
        {
            for (int j = 0; j < generationSize.y; j++)
            {
                for (int k = 0; k < generationSize.z; k++)
                {
                    positions[sizeX  * sizeY * k + sizeX * j + i] = new Vector4(i * 1.5f, j * 1.5f, k * 1.5f, 1.0f);
                }
            }
        }
        positionBuffer.SetData(positions);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

        // color

        // positions
        if (colorBuffer != null)
            colorBuffer.Release();
        colorBuffer = new ComputeBuffer(instanceCount, 12);
        Vector3[] colors = new Vector3[instanceCount];
        for(int i = 0; i < instanceCount; i++)
        {
            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);

            colors[i] = new Vector3(r, g, b);
        }
        colorBuffer.SetData(colors);
        instanceMaterial.SetBuffer("colorBuffer", colorBuffer);

        // indirect args
        uint numIndices = (instanceMesh != null) ? (uint)instanceMesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)instanceCount;
        argsBuffer.SetData(args);

        cachedInstanceCount = instanceCount;
    }

    void OnDisable()
    {

        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }
}
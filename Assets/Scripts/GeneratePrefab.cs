using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePrefab : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Vector3 generationSize;

    // Use this for initialization
    void Start () {


        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;

        for (int i = 0; i < generationSize.x; i++)
        {
            for(int j = 0; j < generationSize.y; j++)
            {
                for(int k = 0; k < generationSize.z; k++)
                {
                    GameObject go = Instantiate(prefab, new Vector3(i * 1.5f, j * 1.5f, k * 1.5f), Quaternion.identity);
                    go.transform.parent = transform;

                    float r = Random.Range(0.0f, 1.0f);
                    float g = Random.Range(0.0f, 1.0f);
                    float b = Random.Range(0.0f, 1.0f);
                    props.SetColor("_Color", new Color(r, g, b));
                    


                    renderer = go.GetComponent<MeshRenderer>();
                    renderer.SetPropertyBlock(props);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

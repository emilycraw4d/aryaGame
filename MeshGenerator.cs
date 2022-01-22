 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public float MinHeight = 0;
    public float MaxHeight = 0;

    public bool Reset_Min_Max;

    public int Octaves = 6;
    public int Scale = 50;

    public float offsetX = 0f;
    public float offsetY = 0f;

    public float Frequency_01 = 5f;
    public float FreqAmp_01 = 3f;

    public float Frequency_02 = 6f;
    public float FreqAmp_02 = 2.5f;

    public float Frequency_03 = 3f;
    public float FreqAmp_03 = 1.5f;

    public float Frequency_04 = 2.5f;
    public float FreqAmp_04 = 1f;

    public float Frequency_05 = 2f;
    public float FreqAmp_05 = .7f;

    public float Frequency_06 = 1f;
    public float FreqAmp_06 = .5f;

    // Start is called before the first frame update
    void Start()
    {
       mesh = new Mesh();
       GetComponent<MeshFilter>().mesh = mesh;

       offsetX = Random.Range(0f, 99999f);
       offsetY = Random.Range(0f, 99999f);

    //    CreateShape();
    //    UpdateMesh();
    }

    void Update() {
        if (Reset_Min_Max) {
            ResetMinMax();
        }
        CreateShape();
        UpdateMesh();
        
    }

    void ResetMinMax()
    {
        MinHeight = 0f;
        MaxHeight = 0f;
        Reset_Min_Max = false;
    }

    void CreateShape () {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
       
        for (int z = 0, i = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                

                float y = Calculate(x, z);
                vertices[i] = new Vector3 (x, y, z);

                if (y > MaxHeight)
                    MaxHeight = y;
                if (y < MinHeight)
                    MinHeight = y;

                i++;

            }
        }
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
          for (int x = 0; x < xSize; x++){ 
            triangles[tris + 0] = vert + 0;
            triangles[tris + 1] = vert + xSize + 1;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + xSize + 1;
            triangles[tris + 5] = vert + xSize + 2;
            vert++;
            tris += 6;
            } 
            vert++; 
        }  
         
    }

    float Calculate(float x, float z)
    {
        float[] octaveFrequencies = new float[] { Frequency_01, Frequency_02, Frequency_03, Frequency_04, Frequency_05, Frequency_06 };
        float[] octaveAmplitudes = new float[] { FreqAmp_01, FreqAmp_02, FreqAmp_03, FreqAmp_04, FreqAmp_05, FreqAmp_06 };
        float y = 0;

        for (int i = 0; i < Octaves; i++)
        {
            y += octaveAmplitudes[i] * Mathf.PerlinNoise(
                     octaveFrequencies[i] * x + offsetX * Scale,
                     octaveFrequencies[i] * z + offsetY * Scale) ;

        }

        return y;
    }


    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    
}

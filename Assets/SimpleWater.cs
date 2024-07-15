using UnityEngine;

public class SimpleWater : MonoBehaviour
{
    // Wave parameters
    public float waveSpeed = 1.0f;
    public float waveHeight = 0.5f;
    public float waveFrequency = 1.0f;
    public float scale = 1.0f; // Scale factor for the wave effect

    // Mesh data
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;

    void Start()
    {
        // Get the mesh and its vertices
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        AnimateWaves();
    }

    void AnimateWaves()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            vertex.y = Mathf.Sin(Time.time * waveSpeed + vertex.x * waveFrequency * scale + vertex.z * waveFrequency * scale) * waveHeight * scale;
            modifiedVertices[i] = vertex;
        }

        // Update the mesh with the modified vertices
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }
}

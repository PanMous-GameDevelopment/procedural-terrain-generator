using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Perlin noise is a type of gradient noise  that is commonly used to create natural-looking terrains.
public class ProceduralGenerationTerrain : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    [Header("Procedural Generation Parameters")]
    public float perlinNoiseX = 0.1f; // Controls the scale of the noise.
    public float perlinNoiseY = 0.1f; // Controls the scale of the noise.
    public float perlinNoiseHeight = 2f; // Controls the amplitude of the noise.
    public int xSize = 100;
    public int zSize = 100;

    [Header("Trees Parameters")]
    public int minimumTrees;
    public int maximumTrees;
    public GameObject[] trees;

    [Header("Foliage Parameters")]
    public int minimumFoliage;
    public int maximumFoliage;
    public GameObject[] foliage;

    [Header("Rocks Parameters")]
    public int minimumRocks;
    public int maximumRocks;
    public GameObject[] rocks;

    void Start()
    {

        mesh = new Mesh(); // Instantiate a new mesh object.
        GetComponent<MeshFilter>().mesh = mesh; // Get the MeshFilter component of the current object, and set its mesh to the newly created mesh.
        CreateShape();
        UpdateMesh();

        SpawnTrees();
        SpawnFoliage();
        SpawnRocks();
    }

    // Creates the initial vertices and triangles of the mesh.
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; // Calculate the number of vertices needed based on the size of the terrain.

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * perlinNoiseX, z * perlinNoiseY) * perlinNoiseHeight; // Multiply by the perlinNoiseHeight to scale the height of the vertex.
                vertices[i] = new Vector3(x, y, z); // The vertex position is stored in the array as a new Vector3 object with the x, y, and z coordinates.
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6]; // Calculate the number of triangles needed based on the size of the terrain.

        int vert = 0;
        int tris = 0;

        // Generate triangles for the grid.
        for (int z = 0; z < zSize; z++) // Go through the z axis.
        {
            for (int x = 0; x < xSize; x++) // Go through the x axis.
            {
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

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Sets a mesh collider for the terrain.
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        // Calculate the UV coordinates of the terrain.
        Vector2[] uv = new Vector2[vertices.Length]; // Holds the UV coordinates of each vertex.
        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x / xSize, vertices[i].z / zSize); // Calculates the UV coordinates based on the x and z of the vertex and the size of the grid.
        }
        mesh.uv = uv; // Assign the uv array to the uv property of the mesh.

        mesh.RecalculateNormals(); // Unity calculates the normals of the mesh.
    }

    void SpawnTrees()
    {
        int numTrees = Random.Range(minimumTrees, maximumTrees);
        for (int i = 0; i < numTrees; i++)
        {
            // Randomly select a prefab from the array.
            GameObject randomTreePrefab = trees[Random.Range(0, trees.Length)];

            // Randomly generate a position within the bounds of the terrain.
            Vector3 randomPos = new Vector3(Random.Range(0, xSize), 0, Random.Range(0, zSize));

            // Raycast downwards from the random position to find the height of the terrain at that position.
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomPos.x, 1000f, randomPos.z), Vector3.down, out hit, Mathf.Infinity))
            {
                randomPos.y = hit.point.y;
            }

            // Instantiate the selected prefab at the surface of the terrain.
            Instantiate(randomTreePrefab, randomPos, Quaternion.identity);

            // Set the rotation of the instantiated prefab to face upwards.
            randomTreePrefab.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    void SpawnRocks()
    {
        int numRocks = Random.Range(minimumRocks, maximumRocks);
        for (int i = 0; i < numRocks; i++)
        {
            // Randomly select a prefab from the array.
            GameObject randomRockPrefab = rocks[Random.Range(0, rocks.Length)];

            // Randomly generate a position within the bounds of the terrain.
            Vector3 randomPos = new Vector3(Random.Range(0, xSize), 0, Random.Range(0, zSize));

            // Raycast downwards from the random position to find the height of the terrain at that position.
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomPos.x, 1000f, randomPos.z), Vector3.down, out hit, Mathf.Infinity))
            {
                randomPos.y = hit.point.y;
            }

            // Instantiate the selected prefab at the surface of the terrain.
            Instantiate(randomRockPrefab, randomPos, Quaternion.identity);

            // Set the rotation of the instantiated prefab to face upwards.
            randomRockPrefab.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    void SpawnFoliage()
    {
        int numFoliage = Random.Range(minimumFoliage, maximumFoliage);
        for (int i = 0; i < numFoliage; i++)
        {
            // Randomly select a prefab from the array.
            GameObject randomFoliagePrefab = foliage[Random.Range(0, foliage.Length)];

            // Randomly generate a position within the bounds of the terrain.
            Vector3 randomPos = new Vector3(Random.Range(0, xSize), 0, Random.Range(0, zSize));

            // Raycast downwards from the random position to find the height of the terrain at that position.
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomPos.x, 1000f, randomPos.z), Vector3.down, out hit, Mathf.Infinity))
            {
                randomPos.y = hit.point.y;
            }

            // Instantiate the selected prefab at the surface of the terrain.
            Instantiate(randomFoliagePrefab, randomPos, Quaternion.identity);

            // Set the rotation of the instantiated prefab to face upwards.
            randomFoliagePrefab.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }
}

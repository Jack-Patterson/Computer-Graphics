using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Model
{
    internal List<Vector3> vertices;
    internal List<Vector3Int> faces;
    internal List<Vector2> textureCoordinates;
    internal List<Vector3Int> textureIndexList;
    internal List<Vector3> normals;

    private Material _texture;

    public Model()
    {
        vertices = new();
        faces = new();
        textureCoordinates = new();
        textureIndexList = new();
        normals = new();

        AddVertices();
        AddFaces();
        AddNormals();
    }

    public GameObject CreateUnityGameObject()
    {
        Mesh mesh = new Mesh();
        GameObject newGO = new GameObject("ComputerGraphicsJ");
        MeshFilter meshFilter = newGO.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newGO.AddComponent<MeshRenderer>();

        List<Vector3> coordinates = new();
        List<int> dummyIndices = new();
        List<Vector2> textCoordinates = new();
        List<Vector3> normalsNew = new();

        for (int i = 0; i < faces.Count; i++)
        {
            //Vector3 normal_for_face = normals[i / 3];
            //normal_for_face = new Vector3(normal_for_face.x, normal_for_face.y, -normal_for_face.z);
            coordinates.Add(vertices[faces[i].x]); dummyIndices.Add(i * 3); textCoordinates.Add(textureCoordinates[textureIndexList[i].x]); //normalsNew.Add(normal_for_face);
            coordinates.Add(vertices[faces[i].y]); dummyIndices.Add(i * 3 + 2); textCoordinates.Add(textureCoordinates[textureIndexList[i].y]); //normalsNew.Add(normal_for_face);
            coordinates.Add(vertices[faces[i].z]); dummyIndices.Add(i * 3 + 1); textCoordinates.Add(textureCoordinates[textureIndexList[i].z]); //normalsNew.Add(normal_for_face);
        }

        mesh.vertices = coordinates.ToArray();
        mesh.triangles = dummyIndices.ToArray();
        mesh.uv = textCoordinates.ToArray();
        mesh.normals = normalsNew.ToArray(); ;
        meshFilter.mesh = mesh;

        return newGO;
    }

    #region Filling In Lists
    private void AddVertices()
    {

        // Front
        vertices.Add(new Vector3(0, 0, 0.5f)); textureCoordinates.Add(new Vector2(295, 440)); //0
        vertices.Add(new Vector3(-2, 4, 0.5f)); textureCoordinates.Add(new Vector2(150, 140)); //1
        vertices.Add(new Vector3(3, 4, 0.5f)); textureCoordinates.Add(new Vector2(510, 140));//2
        vertices.Add(new Vector3(-2, 3, 0.5f)); textureCoordinates.Add(new Vector2(150, 220));//3
        vertices.Add(new Vector3(3, 3, 0.5f)); textureCoordinates.Add(new Vector2(510, 220));//4
        vertices.Add(new Vector3(0, 3, 0.5f)); textureCoordinates.Add(new Vector2(295, 220));//5
        vertices.Add(new Vector3(1, 3, 0.5f)); textureCoordinates.Add(new Vector2(365, 220));//6
        vertices.Add(new Vector3(0, -2, 0.5f)); textureCoordinates.Add(new Vector2(295, 585));//7
        vertices.Add(new Vector3(1, -2, 0.5f)); textureCoordinates.Add(new Vector2(365, 585));//8
        vertices.Add(new Vector3(0, -3, 0.5f)); textureCoordinates.Add(new Vector2(295, 665));//9
        vertices.Add(new Vector3(-3, -2, 0.5f)); textureCoordinates.Add(new Vector2(80, 585));//10
        vertices.Add(new Vector3(-3, -3, 0.5f)); textureCoordinates.Add(new Vector2(80, 665));//11

        // Back
        vertices.Add(new Vector3(0, 0, -0.5f)); textureCoordinates.Add(new Vector2(795, 440));//12
        vertices.Add(new Vector3(-2, 4, -0.5f)); textureCoordinates.Add(new Vector2(940, 140));//13
        vertices.Add(new Vector3(3, 4, -0.5f)); textureCoordinates.Add(new Vector2(580, 140));//14
        vertices.Add(new Vector3(-2, 3, -0.5f)); textureCoordinates.Add(new Vector2(940, 220));//15
        vertices.Add(new Vector3(3, 3, -0.5f)); textureCoordinates.Add(new Vector2(580, 220));//16
        vertices.Add(new Vector3(1, 3, -0.5f)); textureCoordinates.Add(new Vector2(725, 220));//17
        vertices.Add(new Vector3(0, 3, -0.5f)); textureCoordinates.Add(new Vector2(795, 220));//18
        vertices.Add(new Vector3(0, -2, -0.5f)); textureCoordinates.Add(new Vector2(795, 585));//19
        vertices.Add(new Vector3(1, -2, -0.5f)); textureCoordinates.Add(new Vector2(725, 585));//20
        vertices.Add(new Vector3(0, -3, -0.5f)); textureCoordinates.Add(new Vector2(795, 665));//21
        vertices.Add(new Vector3(-3, -2, -0.5f)); textureCoordinates.Add(new Vector2(1010, 585));//22
        vertices.Add(new Vector3(-3, -3, -0.5f)); textureCoordinates.Add(new Vector2(1010, 665));//23

        // Right
        textureCoordinates.Add(new Vector2(510, 300));//24
        textureCoordinates.Add(new Vector2(440, 300));//25
        textureCoordinates.Add(new Vector2(440, 585));//26
        textureCoordinates.Add(new Vector2(440, 670));//27
        textureCoordinates.Add(new Vector2(360, 740));//28

        // Bottom
        textureCoordinates.Add(new Vector2(295, 740));//29
        textureCoordinates.Add(new Vector2(80, 740));//30

        // Left
        textureCoordinates.Add(new Vector2(5, 665));//31
        textureCoordinates.Add(new Vector2(5, 585));//32
        textureCoordinates.Add(new Vector2(80, 515));//33
        textureCoordinates.Add(new Vector2(220, 515));//34
        textureCoordinates.Add(new Vector2(225, 300));//35
        textureCoordinates.Add(new Vector2(150, 300));//36
        textureCoordinates.Add(new Vector2(80, 220));//37
        textureCoordinates.Add(new Vector2(80, 140));//38

        // Top
        textureCoordinates.Add(new Vector2(150, 65));//39
        textureCoordinates.Add(new Vector2(510, 65));//40

        textureCoordinates = GetRelativeValues(textureCoordinates, 1024, 1024);
    }

    private List<Vector2> GetRelativeValues(List<Vector2> pixelCoords, int resX, int resY)
    {
        List<Vector2> tempCoords = new List<Vector2>();
        foreach (Vector2 v in pixelCoords)
        {
            tempCoords.Add(new Vector2(v.x / resX, 1-v.y / resY));
        }
        return tempCoords;
    }

    private void AddFaces()
    {
        // Front
        faces.Add(new Vector3Int(1, 2, 4)); textureIndexList.Add(new Vector3Int(1, 2, 4));
        faces.Add(new Vector3Int(1, 4, 3)); textureIndexList.Add(new Vector3Int(1, 4, 3));
        faces.Add(new Vector3Int(5, 6, 0)); textureIndexList.Add(new Vector3Int(5, 6, 0));
        faces.Add(new Vector3Int(6, 8, 0)); textureIndexList.Add(new Vector3Int(6, 8, 0));
        faces.Add(new Vector3Int(0, 8, 7)); textureIndexList.Add(new Vector3Int(0, 8, 7));
        faces.Add(new Vector3Int(7, 8, 9)); textureIndexList.Add(new Vector3Int(7, 8, 9));
        faces.Add(new Vector3Int(10, 7, 11)); textureIndexList.Add(new Vector3Int(10, 7, 11));
        faces.Add(new Vector3Int(11, 7, 9)); textureIndexList.Add(new Vector3Int(11, 7, 9));

        // Back
        faces.Add(new Vector3Int(13, 16, 14)); textureIndexList.Add(new Vector3Int(13, 16, 14));
        faces.Add(new Vector3Int(13, 15, 16)); textureIndexList.Add(new Vector3Int(13, 15, 16));
        faces.Add(new Vector3Int(18, 12, 17)); textureIndexList.Add(new Vector3Int(18, 12, 17));
        faces.Add(new Vector3Int(17, 12, 20)); textureIndexList.Add(new Vector3Int(17, 12, 20));
        faces.Add(new Vector3Int(12, 19, 20)); textureIndexList.Add(new Vector3Int(12, 19, 20));
        faces.Add(new Vector3Int(19, 21, 20)); textureIndexList.Add(new Vector3Int(19, 21, 20));
        faces.Add(new Vector3Int(22, 23, 19)); textureIndexList.Add(new Vector3Int(22, 23, 19));
        faces.Add(new Vector3Int(23, 21, 19)); textureIndexList.Add(new Vector3Int(23, 21, 19));

        // Top
        faces.Add(new Vector3Int(1, 13, 2)); textureIndexList.Add(new Vector3Int(1, 39, 2));
        faces.Add(new Vector3Int(13, 14, 2)); textureIndexList.Add(new Vector3Int(39, 40, 2));

        // Bottom
        faces.Add(new Vector3Int(23, 11, 9)); textureIndexList.Add(new Vector3Int(30, 11, 9));
        faces.Add(new Vector3Int(23, 9, 21)); textureIndexList.Add(new Vector3Int(30, 9, 29));
        faces.Add(new Vector3Int(9, 8, 21)); textureIndexList.Add(new Vector3Int(9, 8, 28));
        faces.Add(new Vector3Int(8, 20, 21)); textureIndexList.Add(new Vector3Int(8, 27, 28));

        // Left
        faces.Add(new Vector3Int(13, 1, 3)); textureIndexList.Add(new Vector3Int(38, 1, 3));
        faces.Add(new Vector3Int(13, 3, 15)); textureIndexList.Add(new Vector3Int(38, 3, 37));
        faces.Add(new Vector3Int(15, 3, 18)); textureIndexList.Add(new Vector3Int(36, 3, 35));
        faces.Add(new Vector3Int(3, 5, 18)); textureIndexList.Add(new Vector3Int(3, 5, 35));
        faces.Add(new Vector3Int(5, 7, 18)); textureIndexList.Add(new Vector3Int(5, 7, 35));
        faces.Add(new Vector3Int(7, 19, 18)); textureIndexList.Add(new Vector3Int(7, 34, 35));
        faces.Add(new Vector3Int(7, 10, 19)); textureIndexList.Add(new Vector3Int(7, 10, 34));
        faces.Add(new Vector3Int(19, 10, 22)); textureIndexList.Add(new Vector3Int(34, 10, 33));
        faces.Add(new Vector3Int(10, 11, 22)); textureIndexList.Add(new Vector3Int(10, 11, 32));
        faces.Add(new Vector3Int(22, 11, 23)); textureIndexList.Add(new Vector3Int(32, 11, 31));

        // Right
        faces.Add(new Vector3Int(2, 14, 16)); textureIndexList.Add(new Vector3Int(2, 14, 16));
        faces.Add(new Vector3Int(2, 16, 4)); textureIndexList.Add(new Vector3Int(2, 16, 4));
        faces.Add(new Vector3Int(4, 16, 17)); textureIndexList.Add(new Vector3Int(4, 24, 25));
        faces.Add(new Vector3Int(6, 4, 17)); textureIndexList.Add(new Vector3Int(6, 4, 25));
        faces.Add(new Vector3Int(6, 17, 20)); textureIndexList.Add(new Vector3Int(6, 25, 26));
        faces.Add(new Vector3Int(6, 20, 8)); textureIndexList.Add(new Vector3Int(6, 26, 8));
    }

    private void AddNormals()
    {
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0 - 1));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(0, 1.3f, 0));
        normals.Add(new Vector3(0, -1, 0));
    }

    #endregion
}
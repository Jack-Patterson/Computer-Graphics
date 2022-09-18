using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3Int> faces = new List<Vector3Int>();
    List<Vector2> _textureCoordinates = new List<Vector2>();
    List<Vector3Int> _textureIndexList = new List<Vector3Int>();
    List<Vector3> normals = new List<Vector3>();

    public GameObject CreateUnityGameObject()
    {
        AddVertices();
        AddFaces();
        AddNormals();
        
        Mesh mesh = new Mesh();
        GameObject newGO = new GameObject("ComputerGraphicsJ");
        MeshFilter mesh_filter = newGO.AddComponent<MeshFilter>();
        MeshRenderer mesh_renderer = newGO.AddComponent<MeshRenderer>();

        List<Vector3> coordinates = new List<Vector3>();
        List<int> dummyIndices = new List<int>();
        List<Vector2> textureCoordinatess = new List<Vector2>();
        List<Vector3> normalsNew = new List<Vector3>();

        for (int i = 0; i < faces.Count; i++)
        {
            Vector3 normal_for_face = normals[i / 3];
            normal_for_face = new Vector3(normal_for_face.x, normal_for_face.y, -normal_for_face.z);
            coordinates.Add(vertices[faces[i].x]); dummyIndices.Add(i * 3); textureCoordinatess.Add(_textureCoordinates[_textureIndexList[i].x]); normalsNew.Add(normal_for_face);
            coordinates.Add(vertices[faces[i].y]); dummyIndices.Add(i * 3 + 1); textureCoordinatess.Add(_textureCoordinates[_textureIndexList[i].y]); normalsNew.Add(normal_for_face);
            coordinates.Add(vertices[faces[i].z]); dummyIndices.Add(i * 3 + 2); textureCoordinatess.Add(_textureCoordinates[_textureIndexList[i].z]); normalsNew.Add(normal_for_face);
        }

        mesh.vertices = coordinates.ToArray();
        mesh.triangles = dummyIndices.ToArray();
        mesh.uv = textureCoordinatess.ToArray();
        mesh.normals = normalsNew.ToArray(); ;
        mesh_filter.mesh = mesh;

        return newGO;
    }

    #region Filling In Lists
    private void AddVertices()
    {
        // Front
        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(-2, 4, 0.5f));
        vertices.Add(new Vector3(3, 4, 0.5f));
        vertices.Add(new Vector3(-2, 3, 0.5f));
        vertices.Add(new Vector3(-3, 3, 0.5f));
        vertices.Add(new Vector3(0, 3, 0.5f));
        vertices.Add(new Vector3(1, 3, 0.5f));
        vertices.Add(new Vector3(0, -2, 0.5f));
        vertices.Add(new Vector3(1, -2, 0.5f));
        vertices.Add(new Vector3(0, -3, 0.5f));
        vertices.Add(new Vector3(-3, -2, 0.5f));
        vertices.Add(new Vector3(-3, -3, 0.5f));

        // Back
        vertices.Add(new Vector3(0, 0, -0.5f));
        vertices.Add(new Vector3(2, 4, -0.5f));
        vertices.Add(new Vector3(-3, 4, -0.5f));
        vertices.Add(new Vector3(2, 3, -0.5f));
        vertices.Add(new Vector3(-3, 3, -0.5f));
        vertices.Add(new Vector3(-1, 3, -0.5f));
        vertices.Add(new Vector3(0, 3, -0.5f));
        vertices.Add(new Vector3(0, -2, -0.5f));
        vertices.Add(new Vector3(-1, -2, -0.5f));
        vertices.Add(new Vector3(0, -3, -0.5f));
        vertices.Add(new Vector3(3, -2, -0.5f));
        vertices.Add(new Vector3(3, -2, -0.5f));
    }

    private void AddFaces()
    {
        // Front
        faces.Add(new Vector3Int(1, 4, 2));
        faces.Add(new Vector3Int(1, 3, 4));
        faces.Add(new Vector3Int(5, 0, 6));
        faces.Add(new Vector3Int(6, 0, 8));
        faces.Add(new Vector3Int(0, 7, 8));
        faces.Add(new Vector3Int(7, 9, 8));
        faces.Add(new Vector3Int(10, 11, 7));
        faces.Add(new Vector3Int(11, 9, 7));

        // Back
        faces.Add(new Vector3Int(13, 16, 14));
        faces.Add(new Vector3Int(13, 15, 16));
        faces.Add(new Vector3Int(18, 12, 17));
        faces.Add(new Vector3Int(16, 12, 20));
        faces.Add(new Vector3Int(12, 19, 20));
        faces.Add(new Vector3Int(19, 21, 20));
        faces.Add(new Vector3Int(22, 23, 19));
        faces.Add(new Vector3Int(23, 21, 19));

        // Top
        faces.Add(new Vector3Int(1, 13, 2));
        faces.Add(new Vector3Int(13, 14, 2));

        // Bottom
        faces.Add(new Vector3Int(23, 9, 11));
        faces.Add(new Vector3Int(23, 21, 9));
        faces.Add(new Vector3Int(9, 21, 8));
        faces.Add(new Vector3Int(8, 21, 20));

        // Left
        faces.Add(new Vector3Int(13, 3, 1));
        faces.Add(new Vector3Int(13, 15, 3));
        faces.Add(new Vector3Int(15, 18, 3));
        faces.Add(new Vector3Int(3, 18, 4));
        faces.Add(new Vector3Int(5, 18, 7));
        faces.Add(new Vector3Int(7, 18, 19));
        faces.Add(new Vector3Int(7, 19, 10));
        faces.Add(new Vector3Int(19, 22, 10));
        faces.Add(new Vector3Int(10, 22, 11));
        faces.Add(new Vector3Int(22, 23, 11));

        // Right
        faces.Add(new Vector3Int(2, 14, 16));
        faces.Add(new Vector3Int(2, 16, 4));
        faces.Add(new Vector3Int(4, 16, 17));
        faces.Add(new Vector3Int(6, 4, 17));
        faces.Add(new Vector3Int(6, 17, 20));
        faces.Add(new Vector3Int(6, 20, 8));
    }

    private void AddNormals()
    {

    }
    #endregion
}

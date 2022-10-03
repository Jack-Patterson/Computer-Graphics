using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Pipeline : MonoBehaviour
{
    Model model;

    float angle = 0;
    Vector3 axis = new Vector3(0, 0, 0);

    void Start()
    {
        model = gameObject.AddComponent<Model>();
        model.CreateUnityGameObject();

        PrintToFileVertices(model.vertices);

        angle = -10;
        axis = new Vector3(16, 1, 1).normalized;
        Matrix4x4 rotMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, axis), Vector3.one);
        PrintToFileMatrix(rotMatrix);

        List<Vector3> imageAfterRot = GetImage(model.vertices, rotMatrix);
        PrintToFileVertices(imageAfterRot);
    }

    #region Write to File

    private void PrintToFileVertices(List<Vector3> vertices)
    {
        string path = "Assets/gp_Vertices.txt";
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine("Vertices");
        foreach (Vector3 p in vertices)
        {
            sw.WriteLine(p.x + "    ,   " + p.y + "    ,    " + p.z + "    ,    ");
        }
        sw.Close();
    }

    private void PrintToFileMatrix(Matrix4x4 matrix)
    {
        string path = "Assets/gp_Matrixes.txt";
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine("Matrix");
        for (int i = 0; i < 4; i++)
        {
            Vector4 row = matrix.GetRow(i);
            sw.WriteLine(row.x + "    " + row.y + "    " + row.z + "    " + row.w);
        }
        sw.Close();
    }

    #endregion

    #region Matrix

    private List<Vector3> GetImage(List<Vector3> vertices, Matrix4x4 transMatrix)
    {
        List<Vector3> imageVertices = new List<Vector3>();
        foreach (Vector3 v in vertices)
        {
            imageVertices.Add(transMatrix * v);
        }
        return imageVertices;
    }

    #endregion
}

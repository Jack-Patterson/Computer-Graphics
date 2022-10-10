using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    Model model;

    float angle = 0;
    Vector3 axis = new Vector3();
    Vector3 scale = new Vector3();
    Vector3 translation = new Vector3();

    Vector3 camPos = new Vector3();
    Vector3 camLookAt = new Vector3();
    Vector3 camUp = new Vector3();

    float projection = 0;

    void Start()
    {
        model = new Model();
        model.CreateUnityGameObject();
            
        TransformMatrix();
        ViewingMatrix();
        ProjectionMatrix();
    }

    #region Matrix

    private void ProjectionMatrix()
    {
        projection = -1f;

        List<Vector3> viewAfterProj = new List<Vector3>();
        foreach (Vector3 v in ViewingMatrix())
        {
            viewAfterProj.Add(new Vector3(v.x, v.y, projection));
        }
        PrintToFileVertices(viewAfterProj);
    }

    private List<Vector3> ViewingMatrix()
    {
        camPos = new Vector3(18, 4, 51);
        camLookAt = new Vector3(1, 4, 1);
        camUp = new Vector3(2, 1, 16);
        Matrix4x4 viewMatrix = Matrix4x4.LookAt(camPos, camLookAt, camUp);
        //PrintToFileMatrix(viewMatrix);

        List<Vector3> imageAfterViewing = GetImage(TransformMatrix(), viewMatrix);
        //PrintToFileVertices(imageAfterViewing);

        return imageAfterViewing;
    }

    private List<Vector3> TransformMatrix()
    {
        //PrintToFileVertices(model.vertices);

        angle = -10;
        axis = new Vector3(16, 1, 1).normalized;
        Matrix4x4 rotMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, axis), Vector3.one);
        //PrintToFileMatrix(rotMatrix);

        List<Vector3> imageAfterRot = GetImage(model.vertices, rotMatrix);
        //PrintToFileVertices(imageAfterRot);

        scale = new Vector3(4, 3, 1);
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        // PrintToFileMatrix(scaleMatrix);

        List<Vector3> imageAfterScale = GetImage(imageAfterRot, scaleMatrix);
        //PrintToFileVertices(imageAfterScale);

        translation = new Vector3(-5, -1, 2);
        Matrix4x4 translationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);
        //PrintToFileMatrix(translationMatrix);

        List<Vector3> imageAfterTranslation = GetImage(imageAfterScale, translationMatrix);
        //PrintToFileVertices(imageAfterTranslation);

        Matrix4x4 transformMatrix = translationMatrix * scaleMatrix * rotMatrix;
        //PrintToFileMatrix(transformMatrix);

        List<Vector3> imageAfterTransform = GetImage(model.vertices, transformMatrix);
        //PrintToFileVertices(imageAfterTransform);

        return imageAfterTransform;
    }

    private List<Vector3> GetImage(List<Vector3> vertices, Matrix4x4 matrix)
    {
        List<Vector3> imageVertices = new List<Vector3>();
        foreach (Vector3 v in vertices)
        {
            imageVertices.Add(matrix * v);
        }
        return imageVertices;
    }

    #endregion

    #region Write to File

    private void PrintToFileVertices(List<Vector3> vertices)
    {
        string path = "Assets/gp_Vertices.txt";
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine("Vertices");
        foreach (Vector3 p in vertices)
        {
            sw.WriteLine(p.x + "    ,   " + p.y + "    ,    " + p.z);
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
}

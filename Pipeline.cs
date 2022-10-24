using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    Model model;
    Outcode outcode;

    #region Matrix Variables
    float angle;
    Vector3 axis;
    Vector3 scale;
    Vector3 translation;

    Vector3 camPos;
    Vector3 camLookAt;
    Vector3 camUp;

    Matrix4x4 _transformMatrix;
    Matrix4x4 _viewingMatrix;
    Matrix4x4 _projectionMatrix;

    List<Vector3> _imageAfterTransform;
    List<Vector3> _imageAfterViewing;
    List<Vector3> _imageAfterProjection;
    #endregion

    void Start()
    {
        #region Model
        model = new();
        model.CreateUnityGameObject();
        #endregion

        #region Matrix Methods
        TransformMatrix();
        ViewingMatrix();
        ProjectionMatrix();
        ProjectionByHand();
        EverythingMatrix();
        #endregion

        #region Clipping
        outcode = new();

        CompareAnd(new(new(0,0)), new(new(0,0)));
        CompareOr();

        #endregion
    }

    #region Matrixes

    private void EverythingMatrix()
    {
        Matrix4x4 everythingMatrix = _projectionMatrix * _viewingMatrix * _transformMatrix;
        List<Vector3> imageAfterEverything = GetImage(model.vertices, everythingMatrix);
        
        /*PrintToFileMatrix(everythingMatrix);
        PrintToFileVertices(imageAfterEverything);*/
    }

    private void ProjectionMatrix()
    {
        Matrix4x4 projMatrix = Matrix4x4.Perspective(90, 1, 1, 1000);
        List<Vector3> imageAfterProj = GetImage(_imageAfterViewing, projMatrix);

        /*PrintToFileMatrix(projMatrix);
        PrintToFileVertices(imageAfterProj);*/

        _projectionMatrix = projMatrix;
        _imageAfterProjection = imageAfterProj;
    }

    private void ProjectionByHand()
    {
        List<Vector2> projByHand = new List<Vector2>();
        foreach (Vector3 v in _imageAfterViewing)
        {
            projByHand.Add(new Vector2(v.x / v.z, v.y / v.z));
        }

        //PrintToFile2d(projByHand);
    }

    private void ViewingMatrix()
    {
        camPos = new Vector3(18, 4, 51);
        camLookAt = new Vector3(1, 4, 1);
        camUp = new Vector3(2, 1, 16);
        Matrix4x4 viewMatrix = Matrix4x4.LookAt(camPos, camLookAt, camUp);
        List<Vector3> imageAfterViewing = GetImage(_imageAfterTransform, viewMatrix);

        /*PrintToFileMatrix(viewMatrix);
        PrintToFileVertices(imageAfterViewing);*/

        _viewingMatrix = viewMatrix;
        _imageAfterViewing = imageAfterViewing;
    }

    private void TransformMatrix()
    {
        

        angle = -10;
        axis = new Vector3(16, 1, 1).normalized;
        Matrix4x4 rotMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle, axis), Vector3.one);
        List<Vector3> imageAfterRot = GetImage(model.vertices, rotMatrix);

        scale = new Vector3(4, 3, 1);
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        List<Vector3> imageAfterScale = GetImage(imageAfterRot, scaleMatrix);

        translation = new Vector3(-5, -1, 2);
        Matrix4x4 translationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);
        List<Vector3> imageAfterTranslation = GetImage(imageAfterScale, translationMatrix);

        Matrix4x4 transformMatrix = translationMatrix * scaleMatrix * rotMatrix;
        List<Vector3> imageAfterTransform = GetImage(model.vertices, transformMatrix);

        /*PrintToFileVertices(model.vertices);
        PrintToFileMatrix(rotMatrix);
        PrintToFileVertices(imageAfterRot);
        PrintToFileMatrix(scaleMatrix);
        PrintToFileVertices(imageAfterScale);
        PrintToFileMatrix(translationMatrix);
        PrintToFileVertices(imageAfterTranslation);
        PrintToFileMatrix(transformMatrix);
        PrintToFileVertices(imageAfterTransform);*/

        _transformMatrix = transformMatrix;
        _imageAfterTransform = imageAfterTransform;
    }

    private List<Vector3> GetImage(List<Vector3> vertices, Matrix4x4 matrix)
    {
        List<Vector3> imageVertices = new List<Vector3>();
        foreach (Vector3 v in vertices)
        {
            Vector4 v2 = new Vector4(v.x, v.y, v.z, 1);
            imageVertices.Add(matrix * v2);
        }
        return imageVertices;
    }

    #endregion

    #region Clipping

    private void CompareAnd(Outcode a, Outcode b)
    {
        Debug.Log((a * b).Print());
    }

    private void CompareOr()
    {
        outcode = new(new Vector2(-.1f, .4f));
        Outcode outcode2 = new(new Vector2(-1, .4f));
        //Debug.Log(Outcode.CompareAnd(outcode, outcode2));

        outcode = new(new Vector2(.2f, 3f));
        outcode2 = new(new Vector2(1.2f, 4f));
        //Debug.Log(Outcode.CompareAnd(outcode, outcode2));

        outcode = new();
        outcode2 = new();
        //Debug.Log(Outcode.CompareAnd(outcode, outcode2));
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

    private void PrintToFile2d(List<Vector2> points2d)
    {
        string path = "Assets/gp_2D.txt";
        StreamWriter sw = new StreamWriter(path, true);
        sw.WriteLine("2D");
        foreach (Vector2 p in points2d)
        {
            sw.WriteLine(p.x + "    ,   " + p.y);
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

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

    private Texture2D screen;

    void Start()
    {
        #region Model
        //model = new();
        //model.CreateUnityGameObject();
        #endregion

        #region Matrix Methods
        /*TransformMatrix();
        ViewingMatrix();
        ProjectionMatrix();
        ProjectionByHand();
        EverythingMatrix();*/
        #endregion

        #region Clipping
        outcode = new();

        #region AND and OR
        /*CompareAnd(new(new(0,0)), new(new(0,0))); // In Screen
        CompareAnd(new(new(-1.5f, 1.5f)), new(new(-2, 4))); // Top Left
        CompareAnd(new(new(0, 2)), new(new(0, 5))); // Top Center
        CompareAnd(new(new(2, 10)), new(new(4.64f, 1.002f))); // Top Right
        CompareAnd(new(new(-4.5f, 0.5f)), new(new(-5, 0))); // Middle Left
        CompareAnd(new(new(5.8f, .4f)), new(new(1.00001f, .0001f))); // Middle Right
        CompareAnd(new(new(-1.5f, -3)), new(new(-10, -4.5f))); // Bottom Left
        CompareAnd(new(new(0, -3)), new(new(-.55555f, -4))); // Bottom Center
        CompareAnd(new(new(4, -4)), new(new(5, -5.556f))); // Bottom Right
        CompareAnd(new(new(4, -.4f)), new(new(0.5f, -5.556f))); // Invalid AND*/


        /*CompareOr(new(new(0, 0)), new(new(0, 0))); // In Screen
        CompareOr(new(new(-1.5f, -1.5f)), new(new(-2, 4))); // Top Left
        CompareOr(new(new(0, 2)), new(new(0, -5))); // Top Center
        CompareOr(new(new(2, -10)), new(new(4.64f, 1.002f))); // Top Right
        CompareOr(new(new(-4.5f, 0.5f)), new(new(-5, 0))); // Middle Left
        CompareOr(new(new(5.8f, -.4f)), new(new(1.00001f, .0001f))); // Middle Right
        CompareOr(new(new(-1.5f, -3)), new(new(-10, 4.5f))); // Bottom Left
        CompareOr(new(new(0, 3)), new(new(-.55555f, -4))); // Bottom Center
        CompareOr(new(new(4, -4)), new(new(-5, -5.556f))); // Bottom Right
        CompareOr(new(new(4, -.4f)), new(new(0.5f, -5.556f))); // Valid OR*/
        #endregion

        #region Line Clip

        /*TestLineClip(new(0.8f, -0.8f), new(0.2f, 0.3f), "Both in screen: ");
        TestLineClip(new(0.8f, -0.8f), new(-10, -6), "One in screen (start point in screen): ");
        TestLineClip(new(-5, -2), new(0.2f, 0.3f), "One in screen (end point in screen): ");
        TestLineClip(new(-5, -2), new(-10, -6), "None in screen: ");
        TestLineClip(new(5, 0.5f), new(-5, -0.5f), "None in screen but line is: ");*/

        #endregion
        Vector2 start = new(.5f,.5f);
        Vector2 end = new();

        if (LineClip(ref start, ref end))
        {
            
        }

        #endregion

        #region Rasterisation
        TestBresh(new(2, 4), new(8, 5), "-- Normal Bresh: --");
        TestBresh(new(8, 4), new(5, 5), "-- End x less than start x: --");
        TestBresh(new(2, 8), new(8, 5), "-- End y less than start y: --");
        TestBresh(new(3, 3), new(8, 9), "-- dx less than dy: --");

        screen = new Texture2D(256, 256);
        Renderer screenPlane = FindObjectOfType<Renderer>();
        screenPlane.material.mainTexture = screen;
        #endregion

        
    }

    void Plot(List<Vector2Int> bresh)
    {
        foreach (Vector2Int v in bresh)
        {
            screen.SetPixel(v.x, v.y, Color.red);
        }
        screen.Apply();
    }

    void Convert (Vector2 v)
    {

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

    private bool LineClip(ref Vector2 start, ref Vector2 end)
    {
        Outcode startOutCode = new(start);
        Outcode endOutCode = new(end);
        Outcode inScreen = new();

        if ((startOutCode + endOutCode) == inScreen)
        {
            return true;
        }
        if ((startOutCode * endOutCode) != inScreen)
        {
            return false;
        }

        if (startOutCode == inScreen)
        {
            return LineClip(ref end, ref start);
        }

        List<Vector2> points = IntersectEdge(start, end, startOutCode);
        foreach (Vector2 v in points)
        {
            Outcode pointOutcode = new(v);
            if (pointOutcode == inScreen)
            {
                start = v;
                return LineClip(ref start, ref end);
            }
        }

        return false;
    }

    private List<Vector2> IntersectEdge(Vector2 start, Vector2 end, Outcode pointOutcode)
    {
        float m = (end.y - start.y) / (end.x - start.x);
        List<Vector2> intersections = new();


        if (pointOutcode._up)
        {
            intersections.Add(new(start.x + (1 / m) * (1 - start.y), 1));
        }
        if (pointOutcode._down)
        {
            intersections.Add(new(start.x + (1 / m) * (-1 - start.y), -1));
        }
        if (pointOutcode._left)
        {
            intersections.Add(new(-1, start.y + m * (-1 - start.x)));
        }
        if (pointOutcode._right)
        {
            intersections.Add(new(1, start.y + m * (1 - start.x)));
        }

        return intersections;
    }

    #region AND and OR
    private void CompareAnd(Outcode a, Outcode b)
    {
        Debug.Log((a * b).Print());
    }

    private void CompareOr(Outcode a, Outcode b)
    {
        Debug.Log((a + b).Print());
    }
    #endregion

    #endregion

    #region Rasterisation

    private List<Vector2Int> Bresh(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> hold = new();

        int dx = end.x - start.x;
        int dy = end.y - start.y;
        int twoDy = 2 * dy;
        int twoDyDx = 2 * (dy - dx);
        int y = start.y;
        int p = (2 * dy) - dx;

        if (dx < 0) return Bresh(end, start);
        if (dy < 0) return NegY(Bresh(NegY(start), NegY(end)));
        if (dy > dx) return SwapXy(Bresh(SwapXy(start), SwapXy(end)));

        for (int x = start.x; x <= end.x; x++)
        {
            hold.Add(new Vector2Int(x, y));
            if (p <= 0) p += twoDy;
            else
            {
                p += twoDyDx;
                y++;
            }
        }

        return hold;
    }

    private List<Vector2Int> NegY(List<Vector2Int> bresh)
    {
        List<Vector2Int> breshFixed = new();
        foreach (Vector2Int v in bresh)
        {
            breshFixed.Add(NegY(v));
        }
        return breshFixed;
    }

    private Vector2Int NegY(Vector2Int point)
    {
        return new(point.x, -point.y);
    }

    private List<Vector2Int> SwapXy(List<Vector2Int> bresh)
    {
        List<Vector2Int> breshFixed = new();
        foreach (Vector2Int v in bresh)
        {
            breshFixed.Add(SwapXy(v));
        }
        return breshFixed;
    }

    private Vector2Int SwapXy(Vector2Int point)
    {
        return new(point.y, point.x);
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

    #region Tests

    #region Clipping

    private void TestLineClip(Vector2 start, Vector2 end, string msg)
    {
        Debug.Log(msg + LineClip(ref start, ref end));
    }

    
    #endregion

    #region Rasterisation

    private void TestBresh(Vector2Int start, Vector2Int end, string msg)
    {
        Debug.Log(msg + $"\n{start}, {end}");
        foreach (Vector2Int i in Bresh(start, end))
        {
            Debug.Log(i);
        }
    }

    #endregion

    #endregion
}
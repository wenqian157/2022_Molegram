using MLS_Backend_Structure;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MeshData
{
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;

    public void Init(MlsMesh mlsMesh)
    {
        this.parseVertices(mlsMesh.Vertices);
        this.parseTriangles(mlsMesh.Triangles);
        this.parseNormals(mlsMesh.Normals);
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = this.vertices;
        mesh.triangles = this.triangles;
        mesh.normals = this.normals;

        return mesh;
    }

    private void parseVertices(Coordinates vertices)
    {
        this.vertices = new Vector3[vertices.X.Length];

        for (int i = 0; i < this.vertices.Length; i++)
        {
            this.vertices[i] = new Vector3(vertices.X[i], vertices.Y[i], vertices.Z[i]);
        }
    }

    private void parseNormals(Coordinates normals)
    {
        this.normals = new Vector3[normals.X.Length];

        for (int i = 0; i < this.normals.Length; i++)
        {
            this.normals[i] = new Vector3(normals.X[i], normals.Y[i], normals.Z[i]);
        }
    }

    private void parseTriangles(Triangles triangles)
    {
        List<int> tList = new List<int>();
        for (int i = 0; i < triangles.A.Length; i++)
        {
            tList.Add(triangles.A[i]);
            tList.Add(triangles.B[i]);
            tList.Add(triangles.C[i]);
        }
        this.triangles = tList.ToArray() ;

    }
}


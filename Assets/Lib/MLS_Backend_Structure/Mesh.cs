using System;
using System.Collections.Generic;
using System.Text;

namespace MLS_Backend_Structure
{
    public class MlsMesh
    {
        public Coordinates Vertices { get; set; }
        public string[] VerticeColors { get; set; }
        public Coordinates Normals { get; set; }
        public Triangles Triangles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MLS_Backend_Structure
{
    public class Molecule: MoleculeBase
    {
        public BallAndStick BallAndStick { get; set; }
        public Ligand Ligand { get; set; }
        public int LigandCount { get; set; }
        public MlsMesh VdwSurface { get; set; }
        public MlsMesh SasSurface { get; set; }
        public MlsMesh CartoonSurface { get; set; }
        public MlsMesh PocketSurface { get; set; }
        public Coordinate BoxSize { get; set; }

    }
}

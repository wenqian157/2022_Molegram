using System;
using System.Collections.Generic;
using System.Text;

namespace MLS_Backend_Structure
{
    public class MoleculeBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int AtomCount { get; set; }
        public int SequenceCount { get; set; }
        public string Method { get; set; }
        public string Resolution { get; set; }
        public DateTime LastModified { get; set; }
        public bool MoleculeSizeIsOkay { get; set; }

    }
}

using MLS_Backend_Structure;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeViewModel
{

    #region Properties

    public string Code
    {
        get
        {
            if (this.DataModel == null)
                return "";
            else
                return this.DataModel.Code;
        }
    }

    public Molecule DataModel;

    [HideInInspector]
    public List<LayerViewModel> LayerViewModels;

    #endregion

    #region Public functions

    public float GetMaxMoleculeBoxEdgeLength()
    {
        float length = 25;

        if (this.DataModel.BoxSize == null)
        {
            Debug.Log("BoxSize of molecule is null! Boxing won't work.");
        }
        else
        {
            Coordinate boxSize = this.DataModel.BoxSize;
            length = boxSize.X;

            if (boxSize.Y > length)
                length = boxSize.Y;

            if (boxSize.Z > length)
                length = boxSize.Z;
        }
        return length;
    }

    #endregion

}

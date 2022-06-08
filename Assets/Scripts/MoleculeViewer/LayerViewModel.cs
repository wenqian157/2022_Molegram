using MLS_Backend_Structure;
using UnityEngine;

public class LayerViewModel
{

    #region Constructor

    public LayerViewModel(string moleculeCode, MeshData meshData, EMoleculeLayerType layerType)
    {
        this.MeshData = meshData;
        this.MoleculeLayerType = layerType;

        this.ID = "[MOL]" + moleculeCode + "[LAYER]" + this.MoleculeLayerType.ToString();
    }

    public LayerViewModel(string moleculeCode, BallAndStick ballAndStick, EMoleculeLayerType layerType)
    {
        this.BallAndStick = ballAndStick;
        this.MoleculeLayerType = layerType;

        this.ID = "[MOL]" + moleculeCode + "[LAYER]" + this.MoleculeLayerType.ToString();

        Debug.Log(string.Format("Counts: atoms={0} / sticks={1}", ballAndStick.Balls.X.Length, ballAndStick.Sticks.Length), null);
    }

    #endregion

    #region Hidden Properties

    [HideInInspector]
    public EMoleculeLayerType MoleculeLayerType;

    public MeshData MeshData;

    public BallAndStick BallAndStick;

    private string _iD = "";

    public string ID
    {
        get
        {
            return this._iD;
        }
        private set
        {
            this._iD = value;
        }
    }

    //public SyncBoolBroadcaster IsActive;

    #endregion

    #region Public functions

    #endregion

}

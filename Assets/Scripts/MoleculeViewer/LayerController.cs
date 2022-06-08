using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{

    #region Properties

    #region Public

    public LayerViewModel ViewModel;

    public GameObject AtomBallPrefab;
    public GameObject AtomStickPrefab;

    [Space(10)]
    public float AtomBallLocalScale;
    public float AtomStickLocalScale;


    [Space(10)]
    public Material SaS_Material;
    public Material VdW_Material;
    public Material Cartoon_Material;
    public Material Pocket_Material;

    public Material MAT_0x0c86d4;
    public Material MAT_0x0df20d;
    public Material MAT_0x1ea0f2;
    public Material MAT_0x41d7c7;
    public Material MAT_0x44b0f4;
    public Material MAT_0x096aa7;
    public Material MAT_0x0318ff;
    public Material MAT_0x088208;
    public Material MAT_0x606060;
    public Material MAT_0x707070;
    public Material MAT_0x892307;
    public Material MAT_0x900000;
    public Material MAT_0xa8a8a8;
    public Material MAT_0xa72b09;
    public Material MAT_0xb62e0a;
    public Material MAT_0xc2a10b;
    public Material MAT_0xc30bc5;
    public Material MAT_0xd1730f;
    public Material MAT_0xee8412;
    public Material MAT_0xf2f20d;
    public Material MAT_0xf20d0d;
    public Material MAT_0xf20df2;
    public Material MAT_0xf0912a;
    public Material MAT_0xff8000;


    #endregion

    #region Private

    #endregion

    #endregion

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    #region Framework Functions

    void Awake()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.meshFilter = this.GetComponent<MeshFilter>();
        this.meshCollider = this.GetComponent<MeshCollider>();
    }

    void Update()
    {

    }

    #endregion

    #region Public Functions

    public void Initialize(LayerViewModel layerViewModel)
    {
        this.ViewModel = layerViewModel;

        if (layerViewModel.MoleculeLayerType == EMoleculeLayerType.BallsAndSticks || layerViewModel.MoleculeLayerType == EMoleculeLayerType.Ligand)
        {
            this.spawnBallsAndSticks();
        }
        else
        {
            this.processMeshData();
        }

        this.setLayer();
        //this.transform.localPosition = asm.TestCurrentLayerOffset;
    }

    #endregion

    #region Private Functions

    private void processMeshData()
    {
        MeshData meshData = this.ViewModel.MeshData;

        Material layerMaterial = null;

        switch (this.ViewModel.MoleculeLayerType)
        {
            case EMoleculeLayerType.VdW:
                layerMaterial = this.VdW_Material;
                break;
            case EMoleculeLayerType.Sas:
                layerMaterial = this.SaS_Material;
                break;
            case EMoleculeLayerType.Cartoon:
                layerMaterial = this.Cartoon_Material;
                break;
            case EMoleculeLayerType.Pocket:
                layerMaterial = this.Pocket_Material;
                break;
        }

        this.meshRenderer.material = layerMaterial;
        this.meshFilter.mesh = meshData.GetMesh();
        this.meshCollider.sharedMesh = meshData.GetMesh();


        Debug.Log("Done preparing MoleculeLayer: " + this.ViewModel.MoleculeLayerType.ToString(), this);
    }

    private void spawnBallsAndSticks()
    {
        List<Transform> spawnedAtomBalls = new List<Transform>();

        var balls = this.ViewModel.BallAndStick.Balls;
        var sticks = this.ViewModel.BallAndStick.Sticks;
        var colors = this.ViewModel.BallAndStick.BallColors;

        for (int i = 0; i < balls.X.Length; i++)
        {
            Transform newAtomBall = GameObject.Instantiate(this.AtomBallPrefab).transform;
            newAtomBall.SetParent(this.transform);

            newAtomBall.GetComponent<MeshRenderer>().material = this.getMaterial(colors[i]);

            Vector3 pos = new Vector3(balls.X[i], balls.Y[i], balls.Z[i]);
            newAtomBall.localPosition = pos;
            newAtomBall.name = string.Format("Atom_{0}", i);

            newAtomBall.localScale = new Vector3(this.AtomBallLocalScale, this.AtomBallLocalScale, this.AtomBallLocalScale);
        }

        for (int i = 0; i < sticks.Length; i = i + 2)
        {
            Transform newAtomStick = GameObject.Instantiate(this.AtomStickPrefab).transform;
            newAtomStick.SetParent(this.transform);


            int from = sticks[i];
            int to = sticks[i + 1];

            if (from >= balls.X.Length || to >= balls.X.Length)
            {
                Debug.Log("Error generating BallsAndSticks! Stick connection out of range!");
            }
            else
            {
                newAtomStick.name = string.Format("AtomStick_{0}_{1}", from, to);

                Vector3 posFrom = new Vector3(balls.X[from], balls.Y[from], balls.Z[from]);
                Vector3 posTo = new Vector3(balls.X[to], balls.Y[to], balls.Z[to]);

                Vector3 stickPos = (posFrom + posTo) / 2f;

                float distance = Vector3.Distance(posFrom, posTo);
                newAtomStick.transform.localScale = new Vector3(this.AtomStickLocalScale, this.AtomStickLocalScale, distance);

                newAtomStick.localPosition = stickPos;

                newAtomStick.LookAt(this.transform.TransformPoint(posFrom));
            }
        }

        //Debug.SetRendererAndColliderState(this.gameObject, false);
        Debug.Log("Done preparing MoleculeLayer: " + this.ViewModel.MoleculeLayerType.ToString(), this);
    }

    private void setLayer()
    {
        switch (this.ViewModel.MoleculeLayerType)
        {
            case EMoleculeLayerType.BallsAndSticks:
                foreach (Transform child in this.GetComponentsInChildren<Transform>())
                    child.gameObject.layer = LayerMask.NameToLayer("BallsAndSticks");
                break;
            case EMoleculeLayerType.VdW:
                this.gameObject.layer = LayerMask.NameToLayer("VanDerWaals");
                break;
            case EMoleculeLayerType.Sas:
                this.gameObject.layer = LayerMask.NameToLayer("SAS");
                break;
            case EMoleculeLayerType.Cartoon:
                this.gameObject.layer = LayerMask.NameToLayer("Cartoon");
                break;
            case EMoleculeLayerType.Pocket:
                this.gameObject.layer = LayerMask.NameToLayer("Pocket");
                break;
            case EMoleculeLayerType.Ligand:
                foreach (Transform child in this.GetComponentsInChildren<Transform>())
                    child.gameObject.layer = LayerMask.NameToLayer("Ligand");
                break;
        }
    }


    private Material getMaterial(string moeHexString)
    {
        Material retMaterial = null;

        switch (moeHexString)
        {
            case "0x0c86d4":
                retMaterial = this.MAT_0x0c86d4;
                break;
            case "0x0df20d":
                retMaterial = this.MAT_0x0df20d;
                break;
            case "0x1ea0f2":
                retMaterial = this.MAT_0x1ea0f2;
                break;
            case "0x41d7c7":
                retMaterial = this.MAT_0x41d7c7;
                break;
            case "0x44b0f4":
                retMaterial = this.MAT_0x44b0f4;
                break;
            case "0x096aa7":
                retMaterial = this.MAT_0x096aa7;
                break;
            case "0x0318ff":
                retMaterial = this.MAT_0x0318ff;
                break;
            case "0x088208":
                retMaterial = this.MAT_0x088208;
                break;
            case "0x606060":
                retMaterial = this.MAT_0x606060;
                break;
            case "0x707070":
                retMaterial = this.MAT_0x707070;
                break;
            case "0x892307":
                retMaterial = this.MAT_0x892307;
                break;
            case "0x900000":
                retMaterial = this.MAT_0x900000;
                break;
            case "0xa8a8a8":
                retMaterial = this.MAT_0xa8a8a8;
                break;
            case "0xa72b09":
                retMaterial = this.MAT_0xa72b09;
                break;
            case "0xb62e0a":
                retMaterial = this.MAT_0xb62e0a;
                break;
            case "0xc2a10b":
                retMaterial = this.MAT_0xc2a10b;
                break;
            case "0xc30bc5":
                retMaterial = this.MAT_0xc30bc5;
                break;
            case "0xd1730f":
                retMaterial = this.MAT_0xd1730f;
                break;
            case "0xee8412":
                retMaterial = this.MAT_0xee8412;
                break;
            case "0xf2f20d":
                retMaterial = this.MAT_0xf2f20d;
                break;
            case "0xf20d0d":
                retMaterial = this.MAT_0xf20d0d;
                break;
            case "0xf20df2":
                retMaterial = this.MAT_0xf20df2;
                break;
            case "0xf0912a":
                retMaterial = this.MAT_0xf0912a;
                break;
            case "0xff8000":
                retMaterial = this.MAT_0xff8000;
                break;

        }

        if (retMaterial == null)
            Debug.Log("Unexpected color, couldn't find: " + moeHexString, this);

        return retMaterial;
    }

    #endregion

}

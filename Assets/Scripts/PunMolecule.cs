using System.Collections;
using System.Collections.Generic;
using MLS_Backend_Structure;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;

public class PunMolecule : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public GameObject MoleculeLayerPrefab;
    public float RemoteChangedDelay = 0.25f;

    private string moleculeCode = "";
    private string moleculeOwner = "";
    private string BaseUrl = "";

    private int verticeLimit = 50000;

    private Molecule currentMolecule;
    private MoleculeViewModel currentViewModel = new MoleculeViewModel();

    private GameObject listMoleculesGO;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        moleculeCode = (string)instantiationData[0];
        moleculeOwner = (string)instantiationData[1];
        BaseUrl = (string)instantiationData[2];
    }

    private void Start()
    {
        GameObject tableAnchor = GameObject.Find("TableAnchor");
        if (tableAnchor)
        {
            Debug.Log("found table");
            transform.SetParent(tableAnchor.transform);
        }

        listMoleculesGO = GameObject.Find("/Functions/listMolecules");
        bool ifLoaded = listMoleculesGO.GetComponent<listMolecules>().instantiateCodeButton(BaseUrl, moleculeCode);

        if (moleculeCode == ""||!ifLoaded)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        Debug.Log(string.Format("{0} start loading molecule {1} for everyone", moleculeOwner, moleculeCode));
        gameObject.name = moleculeCode;

        load();
    }

    [PunRPC]
    public void PunRPC_resetMolecule()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
    }

    private void load()
    {
        Debug.Log("start loading: " + moleculeCode);
        StartCoroutine(loadWithDelay(RemoteChangedDelay, moleculeCode));
    }

    private IEnumerator loadWithDelay(float delay, string code)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(loadMolecule(code));
    }

    private IEnumerator loadMolecule(string moleculeCode)
    {
        string jsonUrl = string.Format("{0}/{1}", BaseUrl, moleculeCode);
        using (WWW www = new WWW(jsonUrl))
        {
            yield return www;

            this.currentMolecule = JsonConvert.DeserializeObject<Molecule>(www.text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (this.currentMolecule != null)
            {
                this.currentMolecule.Code = moleculeCode;

                MoleculeViewModel mvm = new MoleculeViewModel();
                mvm.DataModel = this.currentMolecule;
                mvm.LayerViewModels = new List<LayerViewModel>();

                LayerViewModel ballsAndSticksViewMode = new LayerViewModel(moleculeCode, this.currentMolecule.BallAndStick, EMoleculeLayerType.BallsAndSticks);
                mvm.LayerViewModels.Add(ballsAndSticksViewMode);

                MeshData vdw = this.generateMeshData(this.currentMolecule.VdwSurface, EMoleculeLayerType.VdW);
                LayerViewModel vdwViewModel = new LayerViewModel(moleculeCode, vdw, EMoleculeLayerType.VdW);
                mvm.LayerViewModels.Add(vdwViewModel);

                MeshData sas = this.generateMeshData(this.currentMolecule.SasSurface, EMoleculeLayerType.Sas);
                LayerViewModel sasViewModel = new LayerViewModel(moleculeCode, sas, EMoleculeLayerType.Sas);
                mvm.LayerViewModels.Add(sasViewModel);

                if (this.currentMolecule.CartoonSurface != null)
                {
                    MeshData cartoon = this.generateMeshData(this.currentMolecule.CartoonSurface, EMoleculeLayerType.Cartoon);
                    LayerViewModel cartoonViewModel = new LayerViewModel(moleculeCode, cartoon, EMoleculeLayerType.Cartoon);
                    mvm.LayerViewModels.Add(cartoonViewModel);
                }

                if (this.currentMolecule.LigandCount > 0 && this.currentMolecule.PocketSurface != null)
                {
                    MeshData pocket = this.generateMeshData(this.currentMolecule.PocketSurface, EMoleculeLayerType.Pocket);
                    LayerViewModel pocketViewModel = new LayerViewModel(moleculeCode, pocket, EMoleculeLayerType.Pocket);
                    mvm.LayerViewModels.Add(pocketViewModel);

                    LayerViewModel ligandViewModel = new LayerViewModel(moleculeCode, this.currentMolecule.Ligand.BallAndStick, EMoleculeLayerType.Ligand);
                    mvm.LayerViewModels.Add(ligandViewModel);
                }

                Debug.Log(string.Format("Finished loading Molecule {0}. " +
                    "\nAtomcount={1}, Ligandcount={2}, BoxSize={3} ", mvm.Code, mvm.DataModel.AtomCount, mvm.DataModel.LigandCount, mvm.GetMaxMoleculeBoxEdgeLength()), this);

                this.currentViewModel = mvm;

                Debug.Log("start spawning");
                this.StartCoroutine(this.spawnWithDelay(RemoteChangedDelay));

                }
        }
    }

    private MeshData generateMeshData(MlsMesh mlsMesh, EMoleculeLayerType layerType)
    {
        Debug.Log("Start generating MoleculeLayer " + layerType.ToString());

        MeshData newLayer = new MeshData();

        int verticeCount = mlsMesh.Vertices.X.Length;
        int triangleCount = mlsMesh.Triangles.A.Length * 3;
        int normalsCount = mlsMesh.Normals.X.Length;

        Debug.Log(string.Format("Counts: vertices={0} / triangles={1} / normals={2}", verticeCount, triangleCount, normalsCount));

        if (verticeCount < this.verticeLimit)
        {
            newLayer.Init(mlsMesh);
        }
        else
        {
            Debug.Log(string.Format("FATAL ERROR: Too many vertices in mesh {0}, {1} ", layerType.ToString(), verticeCount));

        }

        return newLayer;
    }

    public IEnumerator spawnWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(string.Format("Start spawnig {0}, Ligand count = {1}", this.currentViewModel.Code, this.currentViewModel.DataModel.LigandCount), this);

        int LayerIndex = 0;
        foreach (LayerViewModel lvm in this.currentViewModel.LayerViewModels)
        {
            Debug.Log(LayerIndex);
            this.StartCoroutine(this.spawnLayerWithDelay(lvm, 0.5f * LayerIndex));
            LayerIndex += 1;
        }
    }

    private IEnumerator spawnLayerWithDelay(LayerViewModel lvm, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.instantiateMoleculeLayerContainer(lvm);
    }

    private void instantiateMoleculeLayerContainer(LayerViewModel lvm)
    {
        GameObject newLayerContainer = GameObject.Instantiate(this.MoleculeLayerPrefab);

        newLayerContainer.transform.name = this.currentViewModel.Code + "_" + lvm.MoleculeLayerType.ToString();
        newLayerContainer.transform.SetParent(this.transform);
        newLayerContainer.transform.localPosition = Vector3.zero;
        newLayerContainer.transform.localScale = new Vector3(1, 1, 1);

        newLayerContainer.GetComponent<LayerController>().Initialize(lvm);

        Debug.Log("Instatiated MoleculeLayer: " + lvm.MoleculeLayerType.ToString(), this);
    }
}

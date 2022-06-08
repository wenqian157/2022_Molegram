using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Photon.Pun;
using MLS_Backend_Structure;
using UnityEngine.UI;


public class loadMolecules : MonoBehaviour
{
    #region Properties
    public string BaseURL = "http://molegram.ethz.ch/api/molecule";
    public float RemoteChangedDelay = 0.25f;

    [Space(10)]
    public Text MoleculeCode;
    public GameObject PunMoleculePrefab;
    public GameObject MoleculeLayerPrefab;

    private Molecule currentMolecule;
    private int verticeLimit = 50000;
    private GameObject punMolecule;

    private MoleculeViewModel currentViewModel = new MoleculeViewModel();

    #endregion

    public void LoadTestGO()
    {
        PhotonNetwork.Instantiate("testGO", new Vector3(0.0f, -0.2f, 0.4f), Quaternion.identity);
    }

    public void Load()
    {
        if (MoleculeCode.text == "")
        {
            Debug.Log("Code is invalid, please re-enter!!");
            return;
        }

        Debug.Log("start loading: " + MoleculeCode.text);
        StartCoroutine(loadWithDelay(RemoteChangedDelay, MoleculeCode.text));
    }

    private IEnumerator loadWithDelay(float delay, string moleculeCode)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(loadMolecule(moleculeCode));
    }

    private IEnumerator loadMolecule(string moleculeCode)
    {
        string jsonUrl = string.Format("{0}/{1}", BaseURL, moleculeCode);
        Debug.Log("load molecule: " + moleculeCode + " \nfrom: " + jsonUrl);

        using (WWW www = new WWW(jsonUrl))
        {
            yield return www;

            if(string.IsNullOrEmpty(www.error))
            {
                Debug.Log("here here not error");
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
            else Debug.Log("Code is invalid, please re-enter!!");

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

        //punMolecule = Instantiate(PunMoleculePrefab, this.transform);
        punMolecule = Instantiate(PunMoleculePrefab, transform.position, transform.rotation);
        punMolecule.name = currentMolecule.Code;

        int LayerIndex = 0;

        foreach (LayerViewModel lvm in this.currentViewModel.LayerViewModels)
        {
            Debug.Log(LayerIndex);
            this.StartCoroutine(this.spawnLayerWithDelay(lvm, 0.5f * LayerIndex));
            LayerIndex += 1;
        }
    }

    public IEnumerator spawnLayerWithDelay(LayerViewModel lvm, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.instantiateMoleculeLayerContainer(lvm);
    }

    private void instantiateMoleculeLayerContainer(LayerViewModel lvm)
    {
        GameObject newLayerContainer = GameObject.Instantiate(this.MoleculeLayerPrefab);

        newLayerContainer.transform.name = this.currentViewModel.Code + "_" + lvm.MoleculeLayerType.ToString();
        newLayerContainer.transform.SetParent(punMolecule.transform);
        newLayerContainer.transform.localPosition = Vector3.zero;
        newLayerContainer.transform.localScale = new Vector3(1, 1, 1);

        newLayerContainer.GetComponent<LayerController>().Initialize(lvm);

        Debug.Log("Instatiated MoleculeLayer: " + lvm.MoleculeLayerType.ToString(), this);
    }
}

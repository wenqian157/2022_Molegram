using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Test1 : MonoBehaviour
{
    private delegate void eventHandler();
    private event eventHandler OnSuchEvent;

    [SerializeField]
    private UnityEvent myUnityEvent;

    void Start()
    {
        OnSuchEvent += eventHandlingMethod;
        //myUnityEvent += eventHandlingMethod;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //OnSuchEvent?.Invoke();
            myUnityEvent?.Invoke();
        }
    }

    private void eventHandlingMethod()
    {
        Debug.Log("event handler test");
    }

    public void unityEventMethod()
    {
        Debug.Log("unity even handling method");
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {

    [Header("set in Inspector")]
    public float sensitivity = 1f;
    public float minOffset = -10f;
    public float maxOffset = 10f;

    private CinemachineComposer composer;
	// Use this for initialization
	void Start () {

        composer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>();

    }
	
	// Update is called once per frame
	void Update () {
        float verticalLook = Input.GetAxis("Mouse Y") * sensitivity;
        composer.m_TrackedObjectOffset.y += verticalLook;
        composer.m_TrackedObjectOffset.y = Mathf.Clamp(composer.m_TrackedObjectOffset.y, minOffset, maxOffset);
    }
}

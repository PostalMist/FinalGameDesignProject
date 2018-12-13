using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonInput : MonoBehaviour {

    protected ThirdPersonUserControl Control;
    float cameraAngle;
    float cameraAngleSpeed = 2f;

    // Use this for initialization
    void Start () {
        Control = GetComponent<ThirdPersonUserControl>();
	}
	
	// Update is called once per frame
	void Update () {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        cameraAngle += md.x * cameraAngleSpeed;
        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * new Vector3(0.708f, 3.3980f, 7.706f);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing;

	Vector3 offset;

	void Start(){
		offset = transform.position - target.position; //The camera position - the player position
	}

	//Uses FixedUpdate because a camera is a physical object, and the player too...
	void FixedUpdate(){
        if(target == null)
        {
            return;
        }
		Vector3 targetCamPos = target.position + offset;

		//the position of the camera is moved between the current position and the playerpos+offset
		transform.position = targetCamPos;
	}

}

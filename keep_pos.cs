using UnityEngine;
using System.Collections;

public class keep_pos : MonoBehaviour {

	Transform targetObject;


	public void TargetObject(Transform transform){
		targetObject = transform;
	}
		

	// Update is called once per frame
	void FixedUpdate () {
		this.transform.position = targetObject.transform.position;
	}
}

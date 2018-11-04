using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

	// Use this for initialization
	public float playerSpeed;
	GameObject Germ1, Germ2;
	public float rand_force_range_min;
	public float rand_force_range_max;
	public List <GameObject> Germs; // except for myself

	void Start () {
		playerSpeed = 60.0f;
		rand_force_range_min= -300;
		rand_force_range_max = 300;

	}
	
	// Update is called once per frame
	void Update () {
		instantiate_gameObjects ();
		float translation_x = Input.GetAxis ("Horizontal") * playerSpeed * Time.deltaTime;
		float translation_y = Input.GetAxis ("Vertical") * playerSpeed * Time.deltaTime;
		this.transform.position  += (new Vector3 (translation_x, translation_y, 0));
		//Germ1.transform.Translate (new Vector3 (1, 1, 0) );
//		for (int i = 0; i < Germs.Count; ++i) {
//			Germs[i].GetComponent <Rigidbody2D> ().AddForce (new Vector2 (random_generate (), random_generate ()));
//		}



	}

	float random_generate(){
		return Random.Range (rand_force_range_min, rand_force_range_max);
	}

	void instantiate_gameObjects(){
		for (int i = 0; i < Germs.Count; ++i){
			Instantiate (Germs[i], this.transform.parent);
		}
	}
//	void OnTriggerEnter(Collider other){
//		if (other.tag != "me") {
//			
//		} 
//		else {
//			
//		}
//	}
}

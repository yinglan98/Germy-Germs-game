using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerControllter2 : MonoBehaviour {
	public List<GameObject> Germs;
	public int num_germ1, num_germ2, num_germ3, num_myGerm, num_lives, num_points;
	public float force_low, force_high, F_size_Increase;
	Vector3 rotation_origin, player_original_position;
	Quaternion quat_original;
	GameObject spawn_objects;
	public GameObject OBJ_player;
	bool start_instantiated;
	public float player_speed;
	public Rigidbody2D [] spawned_children;
	public List<Vector2> List_start_spawn_location;
	public List<int> List_spawn_index;
	public bool B_triggered_me;
	public GameObject OBJ_lives;
	public Text TXT_lives, TXT_points;
	public Image IMG_background;
	public float Time_collision;
	public Transform TRANS_border_left, TRANS_border_right, TRANS_border_top, TRANS_border_bottom;
	public GameObject OBJ_end_game_screen, OBJ_title_screen, OBJ_game_screen;
	public Image IMG_end_game_screen;
	public static int mode; // 0: just changed mode, 1: title screen, 2: play, 3: tutorial screen 4: restart from endgame screen
	public bool B_NeedToRestart;

	// Use this for initialization
	void Start () {
		num_germ1 = 1; 
		num_germ2 = 2;
		num_germ3 = 1;
		num_myGerm = 1;
		num_lives = 5;
		force_low = -150;
		force_high = 150;
		F_size_Increase = 0.2f;
		rotation_origin = new Vector3 (0f, 0f, 0f);
		quat_original = Quaternion.identity;
		spawn_objects = GameObject.Find ("Spawned Germs");
		start_instantiated = false;
		player_speed = 250f;
		initialize_start_position ();
		B_triggered_me = false;
		TXT_lives = OBJ_lives.GetComponent <Text> ();
		num_points = 0;

		TRANS_border_left.position = new Vector3 (0, Screen.height / 2, 0);
		TRANS_border_right.position = new Vector3 (Screen.width, Screen.height, 0);
		TRANS_border_top.position = new Vector3 (Screen.width / 2, Screen.height, 0);
		TRANS_border_bottom.position = new Vector3 (Screen.width / 2, 0, 0);
		mode = 0;
		player_original_position = OBJ_player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//play ();
		switch (mode) {
		case 0:
			break;
		case 1:
			OBJ_title_screen.SetActive (true);
			mode = 0;
			break;
		case 2:
			//OBJ_end_game_screen.SetActive (false);
			OBJ_title_screen.SetActive (false);
			OBJ_game_screen.SetActive (true);
			play ();
			break;
		case 3:
			break;
		case 4:
			OBJ_end_game_screen.SetActive (false);
			OBJ_game_screen.SetActive (true);
			if (B_NeedToRestart) {
				restart ();
				B_NeedToRestart = false;
			} else {
				play ();
			}
			break;
		}

	}

	Vector3 random_location_on_screen(){
		float rand_x = Random.Range (0.10f, 0.70f);
		float rand_y = Random.Range (0.10f, 0.70f);
		return new Vector3 (rand_x * Screen.width, rand_y * Screen.height, 0f);
	}

	Vector3 random_spawning_position (){
		int index = Random.Range (0, List_start_spawn_location.Count);
		Vector3 chosen_location = new Vector3 (List_start_spawn_location[index].x, List_start_spawn_location[index].y, 0f);
		List_start_spawn_location.RemoveAt (index);
		if (List_start_spawn_location.Count == 0) {
			initialize_start_position ();
		}
		return chosen_location;
	}

	Vector2 rand_force(){
		float x = Random.Range (force_low, force_high);
		float y = Random.Range (force_low, force_high);
		return new Vector2 (x, y);
	}

	void instantiate_objects(){
		//Germ1
		for (int i = 0; i < num_germ1; ++i){
			instantiate_one_object (0, rotation_origin);
		}
		//Germ2
		for (int i = 0; i < num_germ2; ++i){
			instantiate_one_object(1, rotation_origin);
		}
		//Germ3
		for (int i = 0; i < num_germ3; ++i){
			instantiate_one_object (2, rotation_origin);
		}
		//Germ4
		for (int i = 0; i < num_myGerm; ++i){
			instantiate_one_object (3, rotation_origin);
		}
				
	}
		
	//i is the index of the gameobject in the Germ list to be instantiated; vector 3 is the rotation
	void instantiate_one_object (int i, Vector3 rotation){
		
		//Vector3 location = random_location_on_screen ();
		Vector3 location = random_spawning_position ();
		Instantiate (Germs[i], location, quat_original, spawn_objects.transform);
	}

	void move_player(){
		float translation_x = Input.GetAxis ("Horizontal") * player_speed * Time.deltaTime;
		float translation_y = Input.GetAxis ("Vertical") * player_speed * Time.deltaTime;
		//Limiting the movement of the player to screen
		if ((OBJ_player.transform.position.x + translation_x < Screen.width) && (OBJ_player.transform.position.x + translation_x > 0)
			&& (OBJ_player.transform.position.y + translation_y < Screen.height) && (OBJ_player.transform.position.y + translation_y > 0)) {
			OBJ_player.transform.position += (new Vector3 (translation_x, translation_y, 0));
		}
	}

	void add_random_forces(){
		spawned_children = spawn_objects.GetComponentsInChildren<Rigidbody2D> ();
		//Debug.Log ("spawned children length : " + spawned_children.Length);
		int child_count = spawn_objects.transform.childCount;
		for (int i = 0; i < child_count; ++i) {
			//Debug.Log ("index " + i);
			Vector2 force = rand_force ();
			spawned_children [i].AddForce (force);
		}
	}

	void initialize_start_position(){
		for (int i = 1; i <= 9; ++i) {
			List_start_spawn_location.Add (new Vector2(i * 0.1f * Screen.width, 0.7f * Screen.height));
		}
		for (int i = 0; i < List_start_spawn_location.Count; ++i) {
			List_spawn_index.Add (i);
		}
	}

	void spawn_random_enemy (int count){
		for (int i = 0; i < count; ++i) {
			int index = Random.Range (0,3);
			instantiate_one_object (index, rotation_origin);
		}
	}


//	void OnTriggerEnter2D(Collider2D other){
//		Debug.Log ("Collider tag " + other.tag);
//		Debug.Log ("Collider name " + other.gameObject.name);
//		if (other.tag != null && other.tag == "me" && other.name != "Player") {
//			Destroy (other.gameObject);
//			B_triggered_me = true;
//		} else {
//			if (other.gameObject.GetComponent <Rigidbody2D> ()) { // collider against wall doesnt count
//				--num_lives;
//				TXT_lives.text = "Lives Left: " + num_lives.ToString ();
//				other.gameObject.GetComponent <Rigidbody2D> ().AddForce (rand_force ());
//			}
//
//		}
//	}

	void OnCollisionEnter2D (Collision2D other){
		Debug.Log ("collision detected");
		if (other.collider.tag != null && other.collider.tag == "me" && other.collider.name != "Player") {
			if (num_lives > 0){
				Destroy (other.gameObject);
				//B_triggered_me = true;
				instantiate_one_object (3, rotation_origin); // another myGerm appears
				//increase points
				++num_points;
				TXT_points.text = "Point Count: " + num_points.ToString ();
				if (num_points % 3 == 0 && num_points != 0) {
					spawn_random_enemy (1);
				}
				if (num_points % 5 == 0 && num_points != 0) {
					force_low -= 20;
					force_high += 20;
				}
			}
		} else {
			if (other.gameObject.GetComponent <Rigidbody2D> ()) { // collider against wall doesnt count
				--num_lives;
				if (num_lives >= 0) {
					TXT_lives.text = "Lives Left: " + num_lives.ToString ();
					//background becomes red
					Time_collision = Time.time;
					IMG_background.color = Color.red; 
				}
				other.gameObject.GetComponent <Rigidbody2D> ().AddForce (new Vector2 (10000, 500));

			}
		}
	}

	void play(){
		//		Debug.Log ("screen width: " + Screen.width); 
		//		Debug.Log ("screen height: " + Screen.height);
		if (!start_instantiated) {
			instantiate_objects ();
			start_instantiated = true;
		}
		if (num_lives > 0) {
			move_player ();
		}
		add_random_forces ();
		if ((IMG_background.color != Color.white && Time.time > (Time_collision + 0.2f)) ) {
			IMG_background.color = Color.white;
			if (num_lives <= 0) {
				OBJ_end_game_screen.SetActive (true);
			}
		}

		if (OBJ_end_game_screen.activeSelf) {
			if (IMG_end_game_screen.color.a < 1f) {
				float new_a = Mathf.MoveTowards (IMG_end_game_screen.color.a, 1f, 0.05f);
				IMG_end_game_screen.color = new Vector4 (255f, 255f, 255f, new_a);
			} else {
				mode = 0;
			}
		}
		//		if (B_triggered_me) {
		//			this.transform.localScale += new Vector3(F_size_Increase, F_size_Increase, 1f);
		//			B_triggered_me = false;
		//		}
	}

	void restart(){
		num_lives = 5;
		num_points = 0;
		TXT_lives.text = "Lives Left: " + num_lives.ToString ();
		TXT_points.text = "Point Count: " + num_points.ToString ();
		OBJ_player.transform.position = player_original_position;
	}
}



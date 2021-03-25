using Firebase.Auth;
using UnityEngine;

public class Game : MonoBehaviour {
	void Start() {
		var user = FirebaseAuth.DefaultInstance.CurrentUser;
		Debug.Log(user.UserId);
	}
}
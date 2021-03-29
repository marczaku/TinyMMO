using Firebase.Auth;
using UnityEngine;

public class Game : MonoBehaviour {
	public Player player;
	void Start() {
		var user = FirebaseAuth.DefaultInstance.CurrentUser;
		this.player.PlayerId = user.UserId;
		this.player.IsAuthenticated = true;
		this.player.Load();
	}
}
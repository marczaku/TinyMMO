using System.Collections;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
	IEnumerator Start() {
		FirebaseApp.LogLevel = LogLevel.Info;
		
		// init firebase
		var initTask = FirebaseApp.CheckAndFixDependenciesAsync();
		while (!initTask.IsCompleted) {
			yield return null;
		}
		if (initTask.Exception != null) {
			// TODO: throw new InitException(userTask.Exception);
			Debug.LogException(initTask.Exception);
			yield break;
		}
		
		var auth = FirebaseAuth.DefaultInstance;

		// This is commented out for now, so we can use the Login Screen
		// Kind of like a LogOut-Screen, because we always get a new User
		// When starting from the Login-Screen:
		
		// if the user is authenticated already, just use that sign-in
		// if (auth.CurrentUser != null) {
		// 	OnSignInSuccessful();
		// 	yield break;
		// }
		
		var userTask = auth.SignInAnonymouslyAsync();
		while (!userTask.IsCompleted) {
			yield return null;
		}
		if (userTask.Exception != null) {
			// TODO: throw new LoginException(userTask.Exception);
			Debug.LogException(userTask.Exception);
			yield break;
		}

		OnSignInSuccessful();
	}

	void OnSignInSuccessful() {
		SceneManager.LoadScene("Game");
	}
}
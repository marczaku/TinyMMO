using UnityEngine;

public class Player : MonoBehaviour {

    public string PlayerId { get; set; }
    public bool IsAuthenticated { get; set; }
    
    Vector2 Position {
        get {
            const string defaultPositionJson = "{\"x\":0,\"y\":0}";
            var positionJson = PlayerPrefs.GetString(this.PlayerId, defaultPositionJson);
            return JsonUtility.FromJson<Vector2>(positionJson);
        }
        set {
            var positionJson = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(this.PlayerId, positionJson);
        }
    }
    void Update() {
        this.transform.position = this.Position;
        if (this.IsAuthenticated) {
            if (Input.GetKey(KeyCode.D)) {
                this.Position += new Vector2(0.1f, 0f);
            }
            if (Input.GetKey(KeyCode.A)) {
                this.Position += new Vector2(-0.1f, 0f);
            }
            if (Input.GetKey(KeyCode.W)) {
                this.Position += new Vector2(0f, 0.1f);
            }
            if (Input.GetKey(KeyCode.S)) {
                this.Position += new Vector2(0f, -0.1f);
            }
        }
    }
}

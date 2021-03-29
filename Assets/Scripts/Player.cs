using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class Player : MonoBehaviour {

    public string PlayerId { get; set; }
    public bool IsAuthenticated { get; set; }

    public async void Load() {
        await LoadDatabaseValue();
    }

    async Task LoadDatabaseValue() {
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        var value = await reference.GetValueAsync();
        if (!value.Exists) {
            this.Position = Vector2.zero;
            return;
        }

        var positionJson = (string) value.Value;
        this.Position = JsonUtility.FromJson<Vector2>(positionJson);
    }

    Vector2 Position {
        get => this.transform.position;
        set {
            this.transform.position = value;
            SetDatabaseValue(value);
        }
    }

    void SetDatabaseValue(Vector2 value) {
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        var positionJson = JsonUtility.ToJson(value);
        reference.SetValueAsync(positionJson);
    }

    void Update() {
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

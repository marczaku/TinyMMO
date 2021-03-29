using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class Player : MonoBehaviour {

    public string PlayerId { get; set; }
    public bool IsAuthenticated { get; set; }

    public async void Load() {
        // Load the value from database on game start
        // and Cache it, so we do not have to read it every frame
        await LoadDatabaseValue();
    }

    async Task LoadDatabaseValue() {
        // this is how we define, which value we want to access in the database
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        // we wait for the asynchronous call to the database to complete
        var value = await reference.GetValueAsync();
        
        // if there is no value yet, we just start at zero
        if (!value.Exists) {
            this.Position = Vector2.zero;
            return;
        }

        // else, we deserialize the value from the database
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
        // this is how we define, which value we want to access in the database
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        // we serialize the value
        var positionJson = JsonUtility.ToJson(value);
        // and update it in the database
        // note: we do not have to await here, because we do not depend on the value to be set before continuing execution
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

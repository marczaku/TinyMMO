using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class Player : MonoBehaviour {

    public string PlayerId { get; set; }
    public bool IsAuthenticated { get; set; }

    public void Load() {
        LoadDatabaseValue();
    }

    void LoadDatabaseValue() {
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        reference.ValueChanged  += OnDatabaseChange;
    }

    void OnDatabaseChange(object sender, ValueChangedEventArgs e) {
        // else, we deserialize the value from the database
        var positionJson = (string) e.Snapshot.Value;
        this.transform.position = JsonUtility.FromJson<Vector2>(positionJson);
    }
    
    
    // ValueChanged: 2 -> set view
    // Property: if(2!=0) Database.Set(2)
    // ValueChanged: 2 -> SetProperty 2
    // Property: if(2!=2) return;

    Vector2 Position {
        get => this.transform.position;
        set {
            Debug.Log("Set Database value to: "+value);
            if ((Vector2) this.transform.position == value)
                return;
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

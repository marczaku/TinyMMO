using Firebase.Database;
using UnityEngine;

public class Player : MonoBehaviour {

    public string PlayerId { get; set; }
    public bool IsAuthenticated { get; set; }

    public void Load() {
        LoadDatabaseValue();
    }

    void LoadDatabaseValue() {
        // this is how we define, which value we want to access in the database
        var reference = FirebaseDatabase.DefaultInstance.GetReference(this.PlayerId);
        // listen to database changes and update
        reference.ValueChanged  += OnDatabaseChange;
    }

    void OnDatabaseChange(object sender, ValueChangedEventArgs e) {
        // set the view value when the value has changed in the database
        var positionJson = (string) e.Snapshot.Value;
        this.transform.position = JsonUtility.FromJson<Vector2>(positionJson);
    }

    Vector2 Position {
        get => this.transform.position;
        set {
            // do not update the value, if it hasn't changed
            if (Vector2.Distance(this.transform.position, value) > Vector2.kEpsilon)
                return;
            
            // update the view (for more responsiveness)
            this.transform.position = value;
            
            // and the database value (for persistence)
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

using Firebase.Database;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ListenForFriendStatusChanges(string friendId)
    {
        dbReference.Child("users").Child(friendId).Child("status").ValueChanged += (sender, args) =>
        {
            string status = args.Snapshot.Value.ToString();
            Debug.Log($"Amigo {friendId} se ha {status}");
        };
    }
}

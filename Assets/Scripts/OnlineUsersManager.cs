using Firebase.Database;
using UnityEngine;

public class OnlineUsersManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        ListenForOnlineUsers();
    }

    public void UpdateUserStatus(string userId, bool isOnline)
    {
        dbReference.Child("users").Child(userId).Child("status").SetValueAsync(isOnline ? "online" : "offline");
    }

    public void ListenForOnlineUsers()
    {
        dbReference.Child("users").OrderByChild("status").EqualTo("online").ValueChanged += HandleOnlineUsersChanged;
    }

    void HandleOnlineUsersChanged(object sender, ValueChangedEventArgs args)
    {
        foreach (var user in args.Snapshot.Children)
        {
            string userId = user.Key;
            Debug.Log("Usuario en línea: " + userId);
        }
    }
}

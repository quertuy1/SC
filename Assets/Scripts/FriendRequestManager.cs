using Firebase.Database;
using UnityEngine;

public class FriendRequestManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SendFriendRequest(string senderId, string receiverId)
    {
        dbReference.Child("friendRequests").Child(receiverId).Child(senderId).SetValueAsync("pending");
    }

    public void AcceptFriendRequest(string senderId, string receiverId)
    {
        dbReference.Child("friendRequests").Child(receiverId).Child(senderId).RemoveValueAsync();
        dbReference.Child("friends").Child(receiverId).Child(senderId).SetValueAsync(true);
        dbReference.Child("friends").Child(senderId).Child(receiverId).SetValueAsync(true);
    }
}

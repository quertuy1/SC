using Firebase.Database;
using UnityEngine;

public class MatchmakingManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void JoinMatchmakingQueue(string userId)
    {
        dbReference.Child("matchmakingQueue").Child(userId).SetValueAsync(true);
        dbReference.Child("matchmakingQueue").ValueChanged += HandleMatchmakingQueueChanged;
    }

    void HandleMatchmakingQueueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.Snapshot.ChildrenCount >= 2)
        {
            var enumerator = args.Snapshot.Children.GetEnumerator();
            enumerator.MoveNext();
            string user1 = enumerator.Current.Key;
            enumerator.MoveNext();
            string user2 = enumerator.Current.Key;

            dbReference.Child("matchmakingQueue").Child(user1).RemoveValueAsync();
            dbReference.Child("matchmakingQueue").Child(user2).RemoveValueAsync();

            Debug.Log("Emparejamiento exitoso entre " + user1 + " y " + user2);
        }
    }
}

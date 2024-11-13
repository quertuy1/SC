using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference dbReference;
    private FirebaseUser currentUser;

    public InputField emailInputField;
    public InputField passwordInputField;
    public Text feedbackText;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies.");
            }
        });
    }

    public void RegisterUser()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                feedbackText.text = "Registro cancelado.";
                return;
            }
            if (task.IsFaulted)
            {
                feedbackText.text = "Error en el registro: " + task.Exception?.Message;
                return;
            }

            currentUser = task.Result;
            SaveUserToDatabase(currentUser.UserId, email);
            feedbackText.text = "Registro exitoso.";
        });
    }

    public void LoginUser()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                feedbackText.text = "Inicio de sesión cancelado.";
                return;
            }
            if (task.IsFaulted)
            {
                feedbackText.text = "Error en el inicio de sesión: " + task.Exception?.Message;
                return;
            }

            currentUser = task.Result;
            feedbackText.text = "Inicio de sesión exitoso. ¡Bienvenido!";
        });
    }

    private void SaveUserToDatabase(string userId, string email)
    {
        dbReference.Child("users").Child(userId).Child("email").SetValueAsync(email);
        dbReference.Child("users").Child(userId).Child("status").SetValueAsync("online");
    }

    public void LogoutUser()
    {
        if (currentUser != null)
        {
            dbReference.Child("users").Child(currentUser.UserId).Child("status").SetValueAsync("offline");
            auth.SignOut();
            feedbackText.text = "Sesión cerrada.";
        }
    }
}

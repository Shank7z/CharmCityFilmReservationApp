using UnityEditor.Search;
using UnityEngine;

public abstract class PopupBox : MonoBehaviour
{
    protected float showSpeed;
    protected float hideSpeed;

    // Makes own gameobject active
    public abstract void Show();

    // Makes own gameobject inactive
    public abstract void Hide();

    // Refresh UI with updated database info
    public abstract void Refresh();

    // Hides gameobject and applies any changes made to database
    public abstract void Confirm();

    // Hides gameobject without changing database
    public abstract void Close();

}

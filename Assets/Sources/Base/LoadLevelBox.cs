using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LoadLevelBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BasePlayer basePlayer))
        {
            SceneManager.LoadScene("EnvDungL1");
        }
    }
}
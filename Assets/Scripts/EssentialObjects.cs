using UnityEngine;

public class EssentialObjects : MonoBehaviour
{
    void Awake()
    {
        if (!GameManager.isReturning)
            DontDestroyOnLoad(this.gameObject);
        else
            Destroy(gameObject);
    }
}

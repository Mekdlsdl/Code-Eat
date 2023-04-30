using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player") && (!GameManager.isProblemMode)) {
            StartCoroutine(collision.gameObject.GetComponent<PlayerInteract>().FlickExclamation());
            StartCoroutine(GameManager.instance.StartProblemMode(gameObject.scene.name, enemyType, collision.transform.localPosition));
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    public EnemyType enemy_type => enemyType;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player") && (!GameManager.isProblemMode)) {
            SoundManager.instance.PlaySFX("Encounter");
            StartCoroutine(collision.gameObject.GetComponent<PlayerInteract>().FlickExclamation());
            StartCoroutine(GameManager.instance.StartProblemMode(enemyType, collision.transform.localPosition));
            GameManager.encounteredEnemyset.Add(enemyType.enemyName);
        }
    }
}

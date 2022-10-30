
using UnityEngine;

public class Trap : MonoBehaviour
{
    private HealthManager playerHealthManager;
    [SerializeField] private int trapDamageAmount=0;
    [SerializeField] private GameObject playerRespawn;
    private void Start() {
        playerHealthManager=GameObject.Find("Player").GetComponent<HealthManager>();
        
    }
  private void OnTriggerEnter(Collider other) {
      if(other.gameObject.tag == "Player"){
         // playerHealthManager.takeDamage(trapDamageAmount);
         GameObject.Find("Player").transform.position=playerRespawn.transform.position;
      }
  }
}

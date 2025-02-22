using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject Target;
    public float health;
    public float dusmanDarbeGucu;
    GameObject anaKontrolcum;
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        anaKontrolcum = GameObject.FindWithTag("AnaKontrolcum");
    }

    public void HedefBelirle(GameObject objem)
    {
        Target = objem;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(Target.transform.position);
    }

    public void EnemyHit(float hit)
    {
        health -= hit;
        if (health <= 0)
        {
            Die();
            gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Protect"))
        {
            anaKontrolcum.GetComponent<GameController>().DarbeAl(dusmanDarbeGucu);
            Die();
        }
    }

    public void Die()
    {
        anaKontrolcum.GetComponent<GameController>().Dusman_sayisi_guncelle();
        animator.Play("Death");
        Destroy(gameObject, 5f);
    }

}

using UnityEngine;
using TMPro;

public class Possesion : MonoBehaviour
{
    public float small_radius;
    public float mid_radius;
    public float big_radius;
    public float possesion_radius;
    public LayerMask layer;
    public GameObject UI;
    public GameObject Host;
    public bool isPossessing;
    
    private GameObject Target;
    public TextMeshProUGUI Hint;
    public TextMeshProUGUI SeverityText;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, small_radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mid_radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, big_radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, possesion_radius);
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, possesion_radius, layer))
        {
            UI.SetActive(true);

            SeverityText.enabled = true;
            Collider[] targets = Physics.OverlapSphere(transform.position, possesion_radius, layer);
            Target = targets[0].gameObject;

            if (Input.GetKeyDown(KeyCode.F))
            {
                Collider[] SusTargets = Physics.OverlapSphere(transform.position, small_radius, layer);
                if (SusTargets.Length > 1)
                {
                    FindObjectOfType<Suspicion>().IncreaseSeverity(3);
                }
                else
                {
                    Collider[] MidTargets = Physics.OverlapSphere(transform.position, mid_radius, layer);
                    if (MidTargets.Length > 1)
                    {
                        FindObjectOfType<Suspicion>().IncreaseSeverity(2);
                    }
                    else
                    {
                        Collider[] BigTargets = Physics.OverlapSphere(transform.position, big_radius, layer);
                        if (BigTargets.Length > 1)
                        {
                            FindObjectOfType<Suspicion>().IncreaseSeverity(1);
                        }
                    }
                }
                Hint.text = "R";
                isPossessing = true;
                Host.SetActive(true);
                Host.transform.position = transform.position;
                Camera cam = Camera.main;
                cam.transform.parent = Host.transform;
                cam.GetComponent<MouseLook>().Player = Host.transform;
                cam.GetComponent<FixedTeleportation>().Player = Host;
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 0.968f, cam.transform.localPosition.z);
                FindObjectOfType<HostHealth>().StartHost(Target.GetComponent<EnemyHealth>().currentHealth);
                Target.GetComponent<EnemyHealth>().TakeDamage(100);
                FindObjectOfType<Lifetime>().ChangeState(true , Host.GetComponent<HostHealth>().currentHealth);
                gameObject.SetActive(false);
            }
        }
        else
        {
            UI.SetActive(false);
            Target = null;
        }
    }
}

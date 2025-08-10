//using UnityEngine;

//public class DisperseOnHazard : MonoBehaviour
//{
//    public string hazardTag = "Hazard";
//    public GameObject disperseFX;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag(hazardTag)) return;

//        if (disperseFX) Instantiate(disperseFX, transform.position, Quaternion.identity);
//        Destroy(transform.root.gameObject); // kill the whole player, not just a child
//    }
//}


using UnityEngine;
using System.Collections;

public class HazardRespawn : MonoBehaviour
{
    public string hazardTag = "Hazard";
    public float respawnDelay = 1f;
    public Transform respawnPoint;       // leave empty to use start position
    public GameObject disperseFX;        // optional particle

    Vector3 startPos; Quaternion startRot;
    Rigidbody rb; Collider[] cols; Renderer[] rends;
    bool dead;

    void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
        cols = GetComponentsInChildren<Collider>(true);
        rends = GetComponentsInChildren<Renderer>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!dead && other.CompareTag(hazardTag))
            StartCoroutine(DieAndRespawn());
    }

    IEnumerator DieAndRespawn()
    {
        dead = true;

        if (disperseFX) Instantiate(disperseFX, transform.position, Quaternion.identity);

        // �disappear�
        foreach (var r in rends) r.enabled = false;
        foreach (var c in cols) c.enabled = false;
        if (rb) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        yield return new WaitForSeconds(respawnDelay);

        // move to spawn
        var p = respawnPoint ? respawnPoint.position : startPos;
        var q = respawnPoint ? respawnPoint.rotation : startRot;
        transform.SetPositionAndRotation(p, q);

        // �reappear�
        if (rb) rb.isKinematic = false;
        foreach (var c in cols) c.enabled = true;
        foreach (var r in rends) r.enabled = true;

        dead = false;
    }
}

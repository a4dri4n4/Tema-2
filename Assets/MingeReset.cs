using UnityEngine;

public class MingeReset : MonoBehaviour
{
    public Vector3 pozitieStart = new Vector3(0f, 1f, 1f);

    private TargetScore tinta;
    private Rigidbody rb;

    void Start()
    {
        tinta = FindAnyObjectByType<TargetScore>();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (transform.position.y < -5f)
        {
            tinta.ResetCombo();
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = pozitieStart;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
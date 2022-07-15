using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField]
  string tagToHit = "Player";
  [SerializeField]
  float maxTtl = 20f;
  [SerializeField]
  float speed = 10f;
  public int damage = 1;

  float ttl = 0;

  public delegate void OnHit(Projectile obj);
  public OnHit onHit; // To return to object pool, play explosion, etc.
  public OnHit onFadeAway;
  public OnHit onGotShielded;

  // Start is called before the first frame update
  void Start()
  {

  }

  void OnEnable()
  {
    ttl = maxTtl;
  }

  // Update is called once per frame
  void Update()
  {
    ttl -= Time.deltaTime;
    transform.position += transform.right * Time.deltaTime * speed;
    if (ttl <= 0)
    {
      onFadeAway?.Invoke(this);
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag(tagToHit))
    {
      IHealth hitModule = other.GetComponent<IHealth>();
      if (hitModule != null)
        hitModule.Damage(damage);

      onHit?.Invoke(this);
    }
    // Return to pool

  }

  public void Shield()
  {
    onGotShielded?.Invoke(this);
  }
}

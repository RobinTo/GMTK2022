using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
  [SerializeField]
  float speed = 4;
  public int damage = 1;

  Transform target;
  public delegate void OnHit(HomingProjectile obj, Transform target);
  public OnHit onHit; // To return to object pool, play explosion, etc.
  public OnHit onFadeAway;
  public OnHit onGotShielded;

  public void SetTarget(Transform target)
  {
    this.target = target;
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (target == null)
    {
      onFadeAway?.Invoke(this, target);
      return;
    }
    transform.rotation = Quaternion.Slerp(transform.rotation, GetLookRotation(), Time.deltaTime * 10f);
    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    if (Vector3.Distance(transform.position, target.position) < 0.1f)
    {
      IHealth hitModule = target.GetComponent<IHealth>();
      if (hitModule != null)
        hitModule.Damage(damage);
      onHit?.Invoke(this, target);
    }
  }

  private Quaternion GetLookRotation()
  {
    Vector3 dir = target.transform.position - transform.position;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }

  public void Shield()
  {
    onGotShielded?.Invoke(this, target);
  }
}

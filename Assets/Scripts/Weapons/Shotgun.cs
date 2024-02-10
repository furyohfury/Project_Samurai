using UnityEngine;
namespace Samurai
{
    public class Shotgun : Weapon
    {
        [SerializeField]
        private int _numberOfShells;
        [SerializeField, Range(0, 15f)]
        private float _shellAngleSpread;

        public override Vector3 WeaponPositionWhenPicked { get => new Vector3(0.1f, -0.1f, 0); }
        public override Vector3 WeaponRotationWhenPicked { get => new Vector3(145, -90, 0); }


        // public Vector4 TestShit;
        protected override void Start()
        {
            base.Start();
            // transform.SetPositionAndRotation(new Vector3(0.1f, -0.1f, 0), Quaternion.Euler(new Vector3(180, -120, -90)));
            // transform.position = new Vector3(0.1f, -0.1f, 0);            
        }
        private void Update()
        {
            // TestShit = new Vector4(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
        public override void Shoot()
        {
            for (var i = 0; i < _numberOfShells; i++)
            {
                var proj = Instantiate(WeaponProjectilePrefab);
                proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, this.transform.eulerAngles.y + Random.Range(-_shellAngleSpread, _shellAngleSpread), 0);
            }
            CheckIfEmpty();
        }
    }
}
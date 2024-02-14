using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(NPCAIMelee))]
    public class MeleeUnitInput : EnemyInput, IAttackMelee, IInputMelee
    {
        [SerializeField]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        public bool CanHit { get; private set; } = true;
        [SerializeField]
        private float _meleeAttackCooldown = 3;
        public float MeleeAttackCooldown {get => _meleeAttackCooldown; private set => _meleeAttackCooldown = value;}
        private Coroutine _meleeAttackCDCor;

        // Ne naebatsya s exit time v animatore
        private bool _inMeleeAttack = false;
        public bool InMeleeAttack
        {
            get => _inMeleeAttack;
            private set
            {
                if (value == true)
                {
                    _inMeleeAttack = true;
                    CanMove = false;
                    CanHit = false;
                }
                else
                {
                    _inMeleeAttack = false;
                    CanMove = true;
                    CanHit = true;
                }
            }
        }

        public Collider MeleeAttackHitbox { get; private set; }

        private bool Hit;

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
            MeleeAttackHitbox = MeleeWeapon.GetComponentInChildren<Collider>();
        }
        private void OnEnable()
        {
            NPCAI.OnAttack += () => Hit = true;
        }
        protected override void Start()
        {
            base.Start();
            StartCoroutine(MeleeAttackBoxCoroutine());
        }
        private void OnDisable()
        {
            // NPCAI.OnAttack -= MeleeAttack;
        }
        protected override void Update()
        {
            base.Update();

            //todo delete temporary cringe
            /* if (Vector3.Distance(this.transform.position, Agent.destination) < 1)
            {
                Agent.destination = transform.position;
                MeleeAttack();
            } */
        }
        #endregion
        private IEnumerator MeleeAttackBoxCoroutine()
        {
            while (true)
            {
                if (Hit)
                {
                    MeleeAttack();
                    Hit = false;
                }
                yield return null;
            }            
        }
        public void MeleeAttack()
        {
            if (CanHit && _meleeAttackCDCor == null)
            {
                UnitAnimator.SetTrigger("MeleeAttack");
                InMeleeAttack = true;
                _meleeAttackCDCor = StartCoroutine(MeleeAttackCD());
            }            
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
            _meleeAttackCDCor = null;
        }
        #region UnityEvents
        public void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            // InMeleeAttack = true;
        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            InMeleeAttack = false;
        }
        public void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            MeleeAttackHitbox.enabled = true;
        }
        public void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            MeleeAttackHitbox.enabled = false;
            MeleeWeapon.Parrying = false;
        }
        #endregion
    }

}
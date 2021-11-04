using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Asset.Script.Effect
{
    public class Attack : MonoBehaviour
    {
        protected float speed=4;
        protected bool moving;
        protected Vector3 lastPos;
        private Unit UnitTarget;
        private float DamageSuffered;

        public void CreateAttackEffect(Unit UnitTarget, float DamageSuffered)
        {
            this.DamageSuffered = DamageSuffered;
            this.UnitTarget = UnitTarget;
            SetlastClickedPos( UnitTarget.GetWordPositon());
            SetMoving(true);
        }

        private void SetMoving(bool moving)
        {
            this.moving = moving;
        }

        private void SetlastClickedPos(Vector3 lastClickedPos)
        {
            this.lastPos =lastClickedPos;
        }

        protected void EffectAfterAttack()
        {
            GameObject f = Instantiate(AssetManage.i.DestroyEffect, lastPos, Quaternion.identity);
            Destroy(f, f.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);

            PopupDamage.CreatePopupDamage(DamageSuffered, lastPos);
            UnitTarget.TakeDame(DamageSuffered);
        }

        public virtual void Update()
        {
        }
    }
}

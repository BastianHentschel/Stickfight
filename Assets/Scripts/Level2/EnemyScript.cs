using UnityEngine;
using DualPantoFramework;

namespace Level2
{
    public class EnemyScript : MonoBehaviour
    {
        private PantoHandle _itHandle, _meHandle;
        private GameObject _enemy, _player, _weapon;
        private Vector3 _oldPosition;

        public PlayerScript.WeaponPosition weaponPosition;
        public PlayerScript.WeaponSide weaponSide;

        // Start is called before the first frame update
        async void Start()
        {
            FindGameObjects();

            _oldPosition = _enemy.transform.position;

            InvokeRepeating(nameof(RotateWeapon), 0, 0.1f);
        }

        private void FixedUpdate()
        {
            PositionWeapon();
        }

        private void FindGameObjects()
        {
            _meHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
            _itHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
            _enemy = GameObject.Find("Enemy");
            _player = GameObject.Find("Player");
            _weapon = GameObject.Find("EnemyWeapon");
        }

        private void RotateWeapon()
        {
            float weaponDistance = 0.75f;
            var handleRotation = _meHandle.GetRotation() -90;
            var curRotation = _weapon.transform.eulerAngles.y;
            var targetRotation = 1f;
            if (Mathf.Abs(curRotation - handleRotation) > 181)
            {
                if (curRotation < targetRotation)
                    curRotation += 360;
                else
                    handleRotation += 360;
            }
            


            _weapon.transform.eulerAngles = new Vector3(0, curRotation+(handleRotation-curRotation)/20, 0);
            var rot = _enemy.transform.eulerAngles;
            _enemy.transform.eulerAngles = new Vector3(rot.x, curRotation+(handleRotation-curRotation)/20 - 90, rot.z);
            _weapon.transform.position = _enemy.transform.position - new Vector3(
                Mathf.Sin((curRotation+(handleRotation-curRotation)/10 + 90) / 360 * 2 * Mathf.PI) * weaponDistance, 0,
                Mathf.Cos((curRotation+(handleRotation-curRotation)/10 + 90) / 360 * 2 * Mathf.PI) * weaponDistance);
        }

        private void PositionWeapon()
        {
            var enemyPosition = _enemy.transform.position;

            _weapon.transform.position += new Vector3(0, 0, enemyPosition.z - _oldPosition.z);

            _oldPosition = enemyPosition;
        }
    }
}
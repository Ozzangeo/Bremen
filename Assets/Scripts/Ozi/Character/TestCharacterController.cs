using UnityEngine;

namespace Ozi.Character {
    public class TestCharacterController : MonoBehaviour {
        [field: Header("Requires")]
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        private void Update() {
            float forward = 0.0f;
            if (Input.GetKey(KeyCode.W)) {
                forward += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S)) {
                forward -= Time.deltaTime;
            }
            var move = transform.forward * (forward * 350.0f);

            Rigidbody.AddForce(move, ForceMode.Acceleration);

            if (Input.GetKeyDown(KeyCode.Space)) {
                Rigidbody.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
            }

            float turn = 0.0f;
            if (Input.GetKey(KeyCode.A)) {
                turn -= Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D)) {
                turn += Time.deltaTime;
            }

            float rotate = turn * 90.0f;

            Rigidbody.rotation = Rigidbody.rotation * Quaternion.Euler(0.0f, rotate, 0.0f);
        }
    }
}
// using MessagePipe;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using VContainer;

// public class InputBall : MonoBehaviour
// {
//     [SerializeField] private float moveSpeed = 5.0f;
//     private Rigidbody rb;
//     private Vector2 moveInput;
//     private PlayerInput playerInput;
//     private Animator animator;

//     [Inject]
//     private IPublisher<int> _publisher;


//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         playerInput = GetComponent<PlayerInput>();
//         animator = GetComponent<Animator>();

//         playerInput.actions["Move"].started += OnMovePerformed;
//         playerInput.actions["Move"].performed += OnMovePerformed;
//         playerInput.actions["Move"].canceled += OnMoveCanceled;
//         playerInput.actions["LightAttack"].started += OnLightAttackStarted;
//         playerInput.actions["LightAttack"].canceled += OnLightAttackCanceled;
//     }



//     private void FixedUpdate()
//     {
//         // 前後左右への移動と回転を処理
//         if (rb != null)
//         {
//             var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
//             var move = moveDirection * moveSpeed * Time.fixedDeltaTime;
//             // Debug.Log("[FixedUpdate] move/movedirection " + move.ToString() + "/" + moveDirection.ToString());
//             rb.MovePosition(rb.position + move);

//             if (moveDirection != Vector3.zero)
//             {
//                 rb.MoveRotation(Quaternion.LookRotation(moveDirection));
//             }
//         }
//     }

//     private void OnMovePerformed(InputAction.CallbackContext context)
//     {
//         if (animator.GetBool("LightAttack")) return;
//         if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightAttack")) return;
//         // Moveアクションの値を取得
//         moveInput = context.ReadValue<Vector2>();
//         if (moveInput == Vector2.zero)
//         {
//             animator.SetBool("Run", false);
//             return;
//         }
//         animator.SetBool("Run", true);
//     }

//     private void OnMoveCanceled(InputAction.CallbackContext context)
//     {
//         // Moveの入力が無くなったら移動を止める
//         moveInput = Vector2.zero;
//         animator.SetBool("Run", false);
//     }
//     private void OnLightAttackStarted(InputAction.CallbackContext context)
//     {
//         moveInput = Vector2.zero;
//         animator.SetBool("LightAttack", true);
//         _publisher.Publish(Random.Range(10, 200));
//     }
//     private void OnLightAttackCanceled(InputAction.CallbackContext context)
//     {
//         animator.SetBool("LightAttack", false);
//     }

// }
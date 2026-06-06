using UnityEngine; 
using UnityEngine.InputSystem; 
public class XRDetailedInputDebugger : MonoBehaviour 
{ 
 [Header("Left Hand")] 
 public InputActionReference leftThumbstick; 
 public InputActionReference leftTrigger; 
 public InputActionReference leftGrip; 
 public InputActionReference leftPrimaryButton; 
 public InputActionReference leftSecondaryButton; 
 public InputActionReference leftThumbstickClick; 
 public InputActionReference leftMenu; 
 [Header("Right Hand")] 
 public InputActionReference rightThumbstick; 
 public InputActionReference rightTrigger; 
 public InputActionReference rightGrip;
 public InputActionReference rightPrimaryButton; 
 public InputActionReference rightSecondaryButton; 
 public InputActionReference rightThumbstickClick; 
 void OnEnable() 
 { 
 leftThumbstick.action.Enable(); leftTrigger.action.Enable(); 
 leftGrip.action.Enable(); leftPrimaryButton.action.Enable();  leftSecondaryButton.action.Enable(); leftThumbstickClick.action.Enable();  leftMenu.action.Enable(); 
 rightThumbstick.action.Enable(); rightTrigger.action.Enable(); 
 rightGrip.action.Enable(); rightPrimaryButton.action.Enable();  rightSecondaryButton.action.Enable(); rightThumbstickClick.action.Enable();  } 
 void OnDisable() 
 { 
 leftThumbstick.action.Disable(); leftTrigger.action.Disable(); 
 leftGrip.action.Disable(); leftPrimaryButton.action.Disable();  leftSecondaryButton.action.Disable(); leftThumbstickClick.action.Disable();  leftMenu.action.Disable(); 
 rightThumbstick.action.Disable(); rightTrigger.action.Disable(); 
 rightGrip.action.Disable(); rightPrimaryButton.action.Disable();  rightSecondaryButton.action.Disable(); rightThumbstickClick.action.Disable();  } 
 void Update() 
 { 
 Debug.Log("=== LEFT ==="); 
 Debug.Log("Thumbstick: " + leftThumbstick.action.ReadValue<Vector2>());  Debug.Log("Trigger: " + leftTrigger.action.ReadValue<float>());  Debug.Log("Grip: " + (leftGrip.action.ReadValue<float>() > 0.5f));  Debug.Log("Primary (X): " + leftPrimaryButton.action.IsPressed());  Debug.Log("Secondary (Y): " + leftSecondaryButton.action.IsPressed());  Debug.Log("Stick Click: " + leftThumbstickClick.action.IsPressed());  Debug.Log("Menu: " + leftMenu.action.IsPressed()); 
 Debug.Log("=== RIGHT ==="); 
 Debug.Log("Thumbstick: " + rightThumbstick.action.ReadValue<Vector2>());  Debug.Log("Trigger: " + rightTrigger.action.ReadValue<float>());  Debug.Log("Grip: " + (rightGrip.action.ReadValue<float>() > 0.5f));  Debug.Log("Primary (A): " + rightPrimaryButton.action.IsPressed());  Debug.Log("Secondary (B): " + rightSecondaryButton.action.IsPressed());  Debug.Log("Stick Click: " + rightThumbstickClick.action.IsPressed());  } 
} 

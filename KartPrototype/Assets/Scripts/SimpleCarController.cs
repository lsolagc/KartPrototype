using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCarController : MonoBehaviour {
  public List<AxleInfo> axleInfos; // the information about each individual axle
  public float maxMotorTorque; // maximum torque the motor can apply to wheel
  public float maxSteeringAngle; // maximum steer angle the wheel can have
  public float brakeForce = 3;
  public float wheelDampingRate = 0.001f;
  public int RPM_limit = 500;
  private float motor = 0;

  public void Start(){
    foreach (AxleInfo axleInfo in axleInfos) {
      axleInfo.leftWheel.wheelDampingRate	= wheelDampingRate;
      axleInfo.rightWheel.wheelDampingRate	= wheelDampingRate;
    }
  }

  public void FixedUpdate()
  {
    motor = maxMotorTorque * Input.GetAxis("Vertical");
    float brake = maxMotorTorque * brakeForce;
    float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

    foreach (AxleInfo axleInfo in axleInfos) {
      if (axleInfo.steering) {
          axleInfo.leftWheel.steerAngle = steering;
          axleInfo.rightWheel.steerAngle = steering;
      }
      if (axleInfo.motor) {
        axleInfo.leftWheel.motorTorque = motor;
        axleInfo.rightWheel.motorTorque = motor;
        limitRpm(axleInfo.leftWheel);
        Debug.Log(axleInfo.leftWheel.rpm);
        limitRpm(axleInfo.rightWheel);
      }
      if(Input.GetButton("Jump")){
        axleInfo.leftWheel.brakeTorque = brake;
        axleInfo.rightWheel.brakeTorque = brake;
      }
      else{
        axleInfo.leftWheel.brakeTorque = 0;
        axleInfo.rightWheel.brakeTorque = 0;
      }


    }
  }

  private void limitRpm(WheelCollider wheel){
    if(wheel.rpm > RPM_limit){
      Mathf.Clamp(wheel.rpm, 0, RPM_limit);
      wheel.wheelDampingRate = wheelDampingRate * wheelDampingRate;
    }
  }

}


[System.Serializable]
public class AxleInfo {
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor; // is this wheel attached to motor?
  public bool steering; // does this wheel apply steer angle?
}
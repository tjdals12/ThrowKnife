using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Target {
    public enum SpeedPattern {
        Stop = 0,
        Slow,
        Keep,
        Fast
    }

    public enum Direction {
        Left = 0,
        Right
    }

    public class RotatePattern {
        public SpeedPattern speedPattern { get; private set ; }
        public Direction direction { get; private set; }
        public float duration { get; private set; }

        public RotatePattern(SpeedPattern speedPattern, Direction direction, float duration) {
            this.speedPattern = speedPattern;
            this.direction = direction;
            this.duration = duration;
        }

        public float GetSpeed(float speed) {
            switch (this.speedPattern) {
                case SpeedPattern.Stop:
                    return 0f;
                case SpeedPattern.Slow:
                    return speed * 0.5f;
                case SpeedPattern.Fast:
                    return speed * 1.5f;
                case SpeedPattern.Keep:
                default:
                    return speed;
            }
        }

        public Vector3 GetDirection() {
            switch (this.direction) {
                case Direction.Left:
                    return Vector3.back;
                case Direction.Right:
                default:
                    return Vector3.forward;
            }
        }
    }

    public class RotatePatternTemplate {
        public SpeedPattern initialSpeedPattern { get; private set; }
        public List<RotatePattern> rotatePatterns { get; private set; }
    
        public RotatePatternTemplate(SpeedPattern initialSpeedPattern, List<RotatePattern> rotatePatterns) {
            this.initialSpeedPattern = initialSpeedPattern;
            this.rotatePatterns = rotatePatterns;
        }

        public float GetInitialSpeed(float speed) {
            switch (this.initialSpeedPattern) {
                case SpeedPattern.Stop:
                    return 0f;
                case SpeedPattern.Slow:
                    return speed * 0.5f;
                case SpeedPattern.Fast:
                    return speed * 1.5f;
                case SpeedPattern.Keep:
                default:
                    return speed;
            }
        }
    }

    public enum RotatePatternTemplateType {
        LeftKeep,
        RightKeep,
        LeftSlowFast,
        RightSlowFast,
        LeftKeepFast,
        RightKeepFast,
        LeftStopKeep,
        RightStopKeep,
        LeftStopKeepAndRightStopKeep,
        RightStopKeepAndLeftStopKeep,
        LeftStopFast,
        RightStopFast,
        LeftStopFastAndRightStopFast,
        RightStopFastAndLeftStopFast,
        LeftStopSlowStopFast,
        RightStopSlowStopFast,
        LeftStopSlowStopFastRightStopSlowStopFast,
        RightStopSlowStopFastLeftStopSlowStopFast,
    }
    public class Rotator : MonoBehaviour
    {
        RotatePatternTemplate rotatePatternTemplate;
        RotatePattern rotatePattern;
        int currentIndex;
        float elapsedTime;
        float lerpTime;
        float rotateSpeed;
        float fromSpeed;
        float toSpeed;

        #region Unity Method
        void Update() {
            if (this.rotatePatternTemplate == null) return;
            if (this.lerpTime >= 1f) {
                this.currentIndex++;
                if (this.currentIndex == this.rotatePatternTemplate.rotatePatterns.Count) {
                    this.currentIndex = 0;
                }
                this.elapsedTime = 0;
                this.lerpTime = 0;
                this.fromSpeed = this.toSpeed;
            }
            this.rotatePattern = this.rotatePatternTemplate.rotatePatterns[currentIndex];
            this.elapsedTime += Time.deltaTime;
            this.lerpTime = this.elapsedTime / 1;
            this.toSpeed = Mathf.Lerp(this.fromSpeed, this.rotatePattern.GetSpeed(this.rotateSpeed), this.lerpTime);
            this.transform.Rotate(50f * this.toSpeed * this.rotatePattern.GetDirection() * Time.deltaTime);
        }
        #endregion

        public void Setup(float speed, RotatePatternTemplate template) {
            this.currentIndex = 0;
            this.elapsedTime = 0;
            this.lerpTime = 0;
            this.rotateSpeed = speed;
            this.fromSpeed = template.GetInitialSpeed(speed);
            this.rotatePatternTemplate = template;
        }

        public void Reset() {
            this.rotatePatternTemplate = null;
        }
    }
}
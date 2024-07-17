using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawMouth : PawAI
{
    protected override IEnumerator MoveInCircle()
    {
        float angle = 0f;

        while (true)
        {
            if (isActive && !isReturning)
            {
                // Custom movement logic for PawMouth
                float x = player.position.x + Mathf.Cos(angle) * circleRadius;
                float y = player.position.y + Mathf.Sin(angle) * circleRadius;
                Vector3 targetPosition = new Vector3(x, y, 0);

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                Vector3 direction = targetPosition - transform.position;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                angle += movementSpeed * Time.deltaTime;
                if (angle >= 360f) angle -= 360f;

                yield return null; // Wait for the next frame
            }
            else
            {
                yield return null; // Wait for the next frame
            }
        }
    }
}

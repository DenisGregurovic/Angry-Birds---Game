using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
	[SerializeField] private LayerMask slingShotAreaMask;

	public bool IsWithinSlingSHotArea()
		{
			Vector2 worldPostion = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			if (Physics2D.OverlapPoint(worldPostion, slingShotAreaMask))
			{
				return true;
			}
			else 
			{ 
				return false; 
			} 
			
		}
}

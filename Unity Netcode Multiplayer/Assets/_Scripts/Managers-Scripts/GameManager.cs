using _Scripts.Controllers_Scripts;
using _Scripts.InputSystem;
using Artem_Library.Attribute_Scripts;
using Artem_Library.Library_Scripts.ScriptsTemplate_Scripts;
using Artem_Library.Library_Scripts.Systems_Scripts.ObjectPooling_System;
using UnityEngine;

namespace _Scripts
{
	public class GameManager: Singleton_MonoBehaviour<GameManager>
	{
		[field: Header("Controller's")]
		[SerializeField, ExpandableScript] public ThirdPerson_Controller thirdPersonController;
    
		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
    
		[field: Header("Manager")]
		[SerializeField, ExpandableScript] public ObjectPooler_Manager poolManager;
    
		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
		
		[field: Header("Player Systems")]
		[SerializeField, ExpandableScript] public PlayerInput_Handler inputHandler;
	}
}
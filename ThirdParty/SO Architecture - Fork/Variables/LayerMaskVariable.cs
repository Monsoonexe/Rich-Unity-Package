using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "LayerMaskVariable.asset",
	    menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "LayerMask",
	    order = 120)]
	public class LayerMaskVariable : BaseVariable<LayerMask>
	{
	}
}

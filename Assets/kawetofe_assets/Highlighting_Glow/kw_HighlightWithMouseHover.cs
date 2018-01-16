using UnityEngine;
using System.Collections;

public class kw_HighlightWithMouseHover : MonoBehaviour {
	
	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void OnMouseOver(){
		if(HasHighlightScript()){
			//SendMessage("BouncingHighlight");
			SendMessage("Highlight");
		}
	}
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit(){
		if(HasHighlightScript()){
			SendMessage("DeleteHighlight");
		}
	}
	
	/// <summary>
	/// Determines whether this instance has highlight script.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has highlight script; otherwise, <c>false</c>.
	/// </returns>
	private bool HasHighlightScript(){
		bool boolean;
		if(GetComponent<kw_HighlightableObject>() == null){
			boolean = false;
		} else {
			boolean = true;
		}
		return boolean;
		
	}
	
}

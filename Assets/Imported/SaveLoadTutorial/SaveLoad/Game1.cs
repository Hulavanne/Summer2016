using UnityEngine;
using System.Collections;

public class Game1 : MonoBehaviour
{ //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static Game current;
	public Character knight;
	public Character rogue;
	public Character wizard;

	public Game1 () {
		knight = new Character();
		rogue = new Character();
		wizard = new Character();
	}
		
}

using System;

// Base class of character Abilities
public class Ability {

	public string name;
	public string text;
	public string target;
	public string type;
	public Action effect;
	public Action rhythmSetup;

	public Ability (string aName, string aText, string aTarget, string aType, Action aEffect, Action rhythm) {
		name = aName;
		text = aText;
		target = aTarget;
		type = aType;
		effect = aEffect;
		rhythmSetup = rhythm;
	}
}


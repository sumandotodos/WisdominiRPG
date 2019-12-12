using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JustACubeScript : WisdominiObject {

	alertImageController imgControl;

	Material matRef;
	Renderer rendRef;



	enum State { FirstAlert, SecondAlert, ColorChange, Idle };
	State state;

	// Use this for initialization
	new public void Start () {
	
		base.Start ();
		imgControl = GameObject.Find ("AlertImage").GetComponent<alertImageController> ();

		string[] instr0 = { "delay", "5.0" };
		//string[] instr1 = { "startAction", "AlertImage", "setAlertMessage", "non-blocking", "Destroy!" };
		//string[] instr2 = { "startAction", "AlertImage", "setSelfTimeout", "blocking", "3.0" };
		string[] instr3 = { "sendMessage", "this", "setMatColor", "1.0", "0.0", "0.0" };
		string[] instr4 = { "playSound", "insect1" };
		string[] instr5 = { "playMusic", "putaMierdasDeMusica" };

			this.addProgram ();
			this.addInstructionToProgram (instr5);
			this.addInstructionToProgram (instr0);
			//this.addCommandToProgram(instr1);
			//this.addCommandToProgram(instr2);
			this.addInstructionToProgram (instr3);
			this.addInstructionToProgram (instr0);
			this.addInstructionToProgram (instr4);
			this.startProgram (0);

		
		imgControl.reset ();
		//imgControl.setAlertMessage ("Destroy");
		//imgControl.setSelfTimeout (3.0f);
		//waitForActionToComplete (imgControl);
		//state = State.FirstAlert;

		rendRef = this.GetComponent<Renderer> ();
		//matRef = this.GetComponent<Renderer> ().GetComponent<Material> ();
		//matRef = this.GetComponent<Material> ();
		//matRef.color = new Color (1.0f, 1.0f, 0.0f);
		rendRef.material.color = new Color (1.0f, 1.0f, 0.0f);

	}
	
	// Update is called once per frame
	new void Update () {

		UpdateProgram ();

		/*switch (state) {

		case State.FirstAlert:
			if (isWaitingForActionToComplete)
				break;
			imgControl.setAlertMessage ("Destroy");
			imgControl.setSelfTimeout (6.0f);
			waitForActionToComplete (imgControl);
			state = State.SecondAlert;
			break;
		case State.SecondAlert:
			if (isWaitingForActionToComplete)
				break;
			state = State.ColorChange;
			break;
		case State.ColorChange:
			rendRef.material.color = new Color (1.0f, 0.0f, 0.0f);
			state = State.Idle;
			break;

		}
		*/
	
	}

	public override void sendMessage(params string[] p) {

		string msg;
		msg = p [2];
		if (msg.Equals ("setMatColor")) { // accept message: setMat
			float r, g, b;
			float.TryParse (p [3], out r);
			float.TryParse (p [4], out g);
			float.TryParse (p [5], out b);
			setMatColor (r, g, b);
		}


	}

	new public void startAction(params string[] p) {



	}

	public void setMatColor(float r, float g, float b) {

		rendRef.material.color = new Color (r, g, b);

	}
}

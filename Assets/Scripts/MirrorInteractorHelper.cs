using UnityEngine;
using System.Collections;

public class MirrorInteractorHelper : WisdominiObject {

	/* references */

	public ShadowSpawnController shadowSpawn;


	public void _wm_callShadows() {

		shadowSpawn.callShadows (this.transform.position);

	}

}

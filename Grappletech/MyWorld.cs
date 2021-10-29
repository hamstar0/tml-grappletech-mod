using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace Grappletech {
	partial class GrappletechWorld : ModWorld {
		public override void PostDrawTiles() {
			if( this.CanDrawGrappleableTilesOverlay(out bool isGrappling) ) {
				this.DrawGrappleableTilesOverlayIf( isGrappling );
			}
		}
	}
}
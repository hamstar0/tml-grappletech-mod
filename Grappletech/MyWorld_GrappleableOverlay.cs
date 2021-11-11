using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;


namespace Grappletech {
	partial class GrappletechWorld : ModWorld {
		private const string DrawGrappleLingerTimerName = "GrappletechUngrappleOverlay";



		////////////////

		//private Vector2 LastProjectilePos;

		private bool CanDrawGrappleableTilesOverlay( out bool isGrappling ) {
			int ticksRemaining = Timers.GetTimerTickDuration( GrappletechWorld.DrawGrappleLingerTimerName );
			if( ticksRemaining >= 1 ) {
				isGrappling = false;
				return true;
			}

			//if( Main.LocalPlayer.grapCount <= 0 ) {	<- Isn't non-zero when initially grappling??
			//	if( ticksRemaining <= 0 ) {
			//		isGrappling = false;
			//		return false;
			//	}
			//}
			//
			//Projectile grappProj = Main.projectile.FirstOrDefault( p =>
			//	p?.active == true
			//	&& p.aiStyle == 7
			//	&& !p.npcProj
			//	&& p.owner == Main.myPlayer
			//);
			//
			//Vector2 pos;
			//
			//if( grappProj?.active == true ) {
			//	Timers.SetTimer( timerName, 60, true, () => false );
			//	pos = grappProj.Center;
			//	this.LastProjectilePos = pos;
			//} else {
			//	pos = this.LastProjectilePos;
			//}

			isGrappling = Main.projectile.Any( p =>
				p?.active == true
				&& p.aiStyle == 7
				&& !p.npcProj
				&& p.owner == Main.myPlayer
			);
			return isGrappling;
		}

		////

		private void DrawGrappleableTilesOverlayIf( bool isGrappling ) {
			if( isGrappling ) {
				Timers.SetTimer( GrappletechWorld.DrawGrappleLingerTimerName, 120, true, () => false );
			}

			//

			Main.spriteBatch.Begin();
			GrappletechWorld.DrawGrappleableTilesOverlayNear( Main.LocalPlayer.MountedCenter );
			Main.spriteBatch.End();
		}
	}
}
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;


namespace Grappletech {
	partial class GrappletechWorld : ModWorld {
		private const string DrawGrappleLingerTimerName = "GrappletechUngrappleOverlay";



		////////////////

		//private Vector2 LastProjectilePos;

		private bool CanDrawGrappleableTilesOverlay( out bool isGrappling ) {
			isGrappling = Main.projectile.Any( p =>
				p?.active == true
				&& p.aiStyle == 7
				&& p.type != ProjectileID.TrackHook
				&& !p.npcProj
				&& p.owner == Main.myPlayer
			);

			int ticksRemaining = Timers.GetTimerTickDuration( GrappletechWorld.DrawGrappleLingerTimerName );
			return isGrappling || ticksRemaining >= 1;
		}

		////

		private void DrawGrappleableTilesOverlay( bool isGrappling ) {
			if( isGrappling ) {
				Timers.SetTimer( GrappletechWorld.DrawGrappleLingerTimerName, 90, true, () => false );
			}

			//

			Main.spriteBatch.Begin();
			GrappletechWorld.DrawGrappleableTilesOverlayNear( Main.spriteBatch, Main.LocalPlayer.MountedCenter );
			Main.spriteBatch.End();
		}
	}
}
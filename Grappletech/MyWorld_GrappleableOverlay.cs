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
			Player plr = Main.LocalPlayer;

			// Not grappling or lingering after a grapple
			if( plr.grapCount <= 0 ) {
				int ticksRemaining = Timers.GetTimerTickDuration( GrappletechWorld.DrawGrappleLingerTimerName );
				if( ticksRemaining <= 0 ) {
					isGrappling = false;
					return false;
				}
			}

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
			return true;
		}

		////

		private void DrawGrappleableTilesOverlayIf( bool isGrappling ) {
			if( isGrappling ) {
				Timers.SetTimer( GrappletechWorld.DrawGrappleLingerTimerName, 60, true, () => false );
			}

			//

			//int tileX = (int)pos.X / 16;
			//int tileY = (int)pos.Y / 16;
			int tileX = (int)Main.MouseWorld.X / 16;
			int tileY = (int)Main.MouseWorld.Y / 16;

			Main.spriteBatch.Begin();
			GrappletechWorld.DrawGrappleableTilesOverlayNear( tileX, tileY );
			Main.spriteBatch.End();
		}
	}
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.UI;
using Grappletech.Logic;


namespace Grappletech {
	partial class GrappletechWorld : ModWorld {
		public static void DrawGrappleableTilesOverlayNear( SpriteBatch sb, Vector2 worldOrigin ) {
			int rad = 28;

			int tileX = (int)worldOrigin.X / 16;
			int tileY = (int)worldOrigin.Y / 16;

			//

			float radSqr = rad * rad;

			int origTileX = (int)worldOrigin.X / 16;
			int origTileY = (int)worldOrigin.Y / 16;

			int minX = Math.Max( tileX - rad, 1 );
			int maxX = Math.Min( tileX + rad, Main.maxTilesX );
			int minY = Math.Max( tileY - rad, 1 );
			int maxY = Math.Min( tileY + rad, Main.maxTilesY );

			for( int i=minX; i<maxX; i++ ) {
				for( int j=minY; j<maxY; j++ ) {
					bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( i, j );
					if( !isGrappleable.HasValue || !isGrappleable.Value ) {
						continue;
					}

					if( Lighting.Brightness(i, j) < 0.05f ) {
						continue;
					}

					//

					int origDiffX = origTileX - i;
					int origDiffY = origTileY - j;
					int origDiffSqr = (origDiffX * origDiffX) + (origDiffY * origDiffY);
					if( origDiffSqr >= radSqr ) {
						continue;
					}

					//

					int xDiff = i - tileX;
					int yDiff = j - tileY;
					float diffSqr = (xDiff * xDiff) + (yDiff * yDiff);

					float intensity = 1f - (diffSqr / radSqr);
					intensity = 0.1f + (intensity * 0.8f);

					GrappletechWorld.DrawGrappleableTileOverlayAt( sb, i, j, intensity );
				}
			}
		}

		
		////

		public static void DrawGrappleableTileOverlayAt(
					SpriteBatch sb,
					int tileX,
					int tileY,
					float intensityPercent ) {
			var wldPos = new Vector2( tileX * 16, tileY * 16 );
			var scrPos = wldPos - Main.screenPosition;
			var scrPosOffsetFromCenter = scrPos - (new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f);
			var centerZoomedScrPos = (scrPos - scrPosOffsetFromCenter) + (scrPosOffsetFromCenter * Main.GameZoomTarget);
			//Vector2 scrPos = UIZoomLibraries.ConvertToScreenPosition(		// <- TODO: Review!
			//	worldCoords: new Vector2(tileX * 16, tileY * 16),
			//	uiZoomState: null,
			//	gameZoomState: false
			//);

			float pulse = (float)Main.mouseTextColor / 255f;

			//

			sb.Draw(
				//texture: GrappletechMod.Instance.DisabledItemTex,
				texture: GrappletechMod.Instance.GrappleIconTex,
				position: centerZoomedScrPos,
				sourceRectangle: null,
				color: Color.Lime * intensityPercent * pulse * pulse,
				rotation: 0f,
				origin: default,
				scale: 0.7f,
				effects: SpriteEffects.None,
				layerDepth: 0
			);
		}
	}
}
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
		public static void DrawGrappleableTilesOverlayNear( int tileX, int tileY ) {
			int rad = 5;
			float radSqr = rad * rad;

			int minX = Math.Max( tileX - rad, 1 );
			int maxX = Math.Min( tileX + rad, Main.maxTilesX );
			int minY = Math.Max( tileY - rad, 1 );
			int maxY = Math.Min( tileY + rad, Main.maxTilesY );

			for( int i=minX; i<maxX; i++ ) {
				for( int j=minY; j<maxY; j++ ) {
					bool? isGrappleable = ProjectileLogic.IsTileNormallyGrappleable( i, j );

					if( isGrappleable.HasValue && isGrappleable.Value ) {
						int xDiff = i - tileX;
						int yDiff = j - tileY;
						float diffSqr = (xDiff * xDiff) + (yDiff * yDiff);

						float intensity = 1f - (diffSqr / radSqr);
						intensity = 0.1f + (intensity * 0.9f);

						GrappletechWorld.DrawGrappleableTileOverlayAt( i, j, intensity );
					}
				}
			}
		}

		
		public static void DrawGrappleableTileOverlayAt( int tileX, int tileY, float intensityPercent ) {
			Vector2 scrPos = UIZoomLibraries.ConvertToScreenPosition( new Vector2(tileX * 16, tileY * 16), null, true );

			Main.spriteBatch.Draw(
				//texture: GrappletechMod.Instance.DisabledItemTex,
				texture: GrappletechMod.Instance.GrappleIconTex,
				position: scrPos,
				sourceRectangle: null,
				color: Color.Lime * intensityPercent,
				rotation: 0f,
				origin: default,
				scale: 1f,
				effects: SpriteEffects.None,
				layerDepth: 0
			);
		}
	}
}
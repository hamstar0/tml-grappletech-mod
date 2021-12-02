using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace Grappletech.Logic {
	static partial class ProjectileLogic {
		public static bool IsTileIgnoringGrapple( Tile tile ) {
			return ProjectileLogic.IsTileStateIgnoringGrapple( tile )
				|| ProjectileLogic.IsTileTypeIgnoringGrapple( tile.type );
		}
		
		public static bool IsTileStateIgnoringGrapple( Tile tile ) {
			if( tile?.active() != true ) {
				return true;
			}
			if( tile.inActive() ) {	// actuated
				return true;
			}

			return false;
		}

		public static bool IsTileTypeIgnoringGrapple( int tileType ) {
			if( !Main.tileSolid[tileType] ) {
				if( tileType != TileID.MinecartTrack ) {
					return true;
				}
			}

			return false;
		}
		
		////
		
		public static bool IsTileTypeAlwaysGrappleable( int tileType ) {
			var config = GrappletechConfig.Instance;

			if( config.Get<bool>( nameof(config.GrappleableTileWhitelistWoodAndPlatforms) ) ) {
				switch( tileType ) {
				case TileID.Platforms:
				case TileID.PlanterBox:
				case TileID.MinecartTrack:  // i guess
				case TileID.WoodBlock:
				case TileID.BorealWood:
				case TileID.DynastyWood:
				case TileID.PalmWood:
				case TileID.Ebonwood:
				case TileID.Shadewood:
				case TileID.Pearlwood:
				case TileID.SpookyWood:
				case TileID.LivingWood:
				case TileID.LeafBlock:  // sure
				case TileID.LivingMahogany:
				case TileID.RichMahogany:
				case TileID.LivingMahoganyLeaves:   // sure
					return true;
				}
			}

			//

			var whitelist = config.Get<HashSet<string>>( nameof( config.GrappleableTileWhitelist ) );
			string tileUid = TileID.GetUniqueKey( tileType );

			return whitelist.Contains( tileUid );
		}
		
		////

		public static bool IsTilePositionGrappleable( int tileX, int tileY ) {
			if( tileX <= 1 || tileX >= Main.maxTilesX-1 ) {
				return false;
			}
			if( tileY <= 1 || tileY >= Main.maxTilesY-1 ) {
				return false;
			}

			//

			var config = GrappletechConfig.Instance;
			if( !config.Get<bool>( nameof(config.GrappleableTileNarrowFormations) ) ) {
				return false;
			}

			//

			var tilesNear = new List<Tile>( 8 );
			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY-1 ) ); //nw
			tilesNear.Add( Framing.GetTileSafely( tileX, tileY-1 ) );   //n
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY-1 ) ); //ne

			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY ) );   //w
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY ) );   //e

			tilesNear.Add( Framing.GetTileSafely( tileX-1, tileY+1 ) ); //sw
			tilesNear.Add( Framing.GetTileSafely( tileX, tileY+1 ) );   //s
			tilesNear.Add( Framing.GetTileSafely( tileX+1, tileY+1 ) ); //se

			//

			int neighbors = 0;
			//int consecutiveNeighbors = 0;

			for( int k=0; k<tilesNear.Count; k++ ) {
				int next = k < (tilesNear.Count - 1) ? k + 1 : 0;
				Tile tileNear = tilesNear[k];
				Tile tileNext = tilesNear[next];

				bool hereIgnore = ProjectileLogic.IsTileIgnoringGrapple( tileNear );
				bool nextIgnore = ProjectileLogic.IsTileIgnoringGrapple( tileNext );

				if( !hereIgnore ) {
					neighbors++;
				}

				//if( !hereIgnore && !nextIgnore ) {
				//	consecutiveNeighbors++;
				//}
			}

			if( neighbors <= 3 ) {
				return true;
			}

			//if( consecutiveNeighbors <= 2 ) {
			//	return true;
			//}

			return false;
		}
		
		////

		public static bool? IsTileNormallyGrappleable( int tileX, int tileY ) {
			Tile tile = Main.tile[tileX, tileY];

			if( ProjectileLogic.IsTileIgnoringGrapple(tile) ) {
				return null;
			}

			if( ProjectileLogic.IsTileTypeAlwaysGrappleable(tile.type) ) {
				return true;
			}
			if( ProjectileLogic.IsTilePositionGrappleable(tileX, tileY) ) {
				return true;
			}

			return false;
		}
	}
}

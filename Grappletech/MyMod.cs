using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Grappletech {
	public class GrappletechMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-grappletech-mod";


		////////////////

		public static GrappletechMod Instance => ModContent.GetInstance<GrappletechMod>();



		////////////////

		//public Texture2D DisabledItemTex { get; private set; }
		public Texture2D GrappleIconTex { get; private set; }



		////////////////

		public override void Load() {
			if( Main.netMode != NetmodeID.Server ) {   // Not server
				//this.DisabledItemTex = ModContent.GetTexture( "Terraria/MapDeath" );

				this.GrappleIconTex = Main.itemTexture[ItemID.Hook];
			}
		}
	}
}
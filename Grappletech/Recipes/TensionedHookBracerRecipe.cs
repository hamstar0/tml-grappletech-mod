using Terraria.ID;
using Terraria.ModLoader;


namespace Grappletech.Items {
	public class TensionedHookBracerRecipe : ModRecipe {
		public TensionedHookBracerRecipe( TensionedHookBracerItem myitem ) : base( GrappletechMod.Instance ) {
			this.AddIngredient( ItemID.Shackle, 3 );
			this.AddIngredient( ItemID.Leather, 5 );
			this.AddIngredient( ItemID.Harpoon, 1 );
			this.AddTile( TileID.Anvils );
			this.SetResult( myitem );
		}


		public override bool RecipeAvailable() {
			return GrappletechConfig.Instance.Get<bool>( nameof(GrappletechConfig.TensionedHookBracerRecipeEnabled) );
		}
	}
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Grappletech.Items {
	[AutoloadEquip( EquipType.HandsOn )]
	public class TensionedHookBracerItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Tensioned Hook Bracer" );
			this.Tooltip.SetDefault( "Enables grappling to any surface" );
		}

		public override void SetDefaults() {
			this.item.width = 16;
			this.item.height = 16;
			this.item.accessory = true;
			this.item.value = Item.buyPrice( 0, 5, 0, 0 );
			this.item.rare = ItemRarityID.LightRed;
		}


		////

		public override void AddRecipes() {
			var recipe = new TensionedHookBracerRecipe( this );
			recipe.AddRecipe();
		}
	}
}
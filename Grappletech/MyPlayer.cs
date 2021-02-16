using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Players;
using Grappletech.Items;


namespace Grappletech {
	class GrappletechPlayer : ModPlayer {
		public bool IsEquippingTensionedHookBracer { get; internal set; } = false;



		////////////////

		public override void UpdateEquips( ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff ) {
			int maxAccSlot = 3 + PlayerItemHelpers.GetCurrentVanillaMaxAccessories( this.player );
			int hookBracerType = ModContent.ItemType<TensionedHookBracerItem>();

			this.IsEquippingTensionedHookBracer = false;

			for( int i=3; i<maxAccSlot; i++ ) {
				if( !(this.player.armor[i]?.IsAir ?? false) && this.player.armor[i].type == hookBracerType ) {
					this.IsEquippingTensionedHookBracer = true;
					break;
				}
			}
		}
	}
}
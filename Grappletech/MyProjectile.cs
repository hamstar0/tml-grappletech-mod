using Terraria;
using Terraria.ModLoader;
using Grappletech.Logic;


namespace Grappletech {
	class GrappletechProjectile : GlobalProjectile {
		public override bool PreAI( Projectile projectile ) {
			if( projectile.aiStyle == 7 && !projectile.npcProj ) {
				ProjectileLogic.UpdateForGrappleProjectile( Main.player[projectile.owner], projectile );
			}

			return base.PreAI( projectile );
		}

		public override void GrapplePullSpeed( Projectile projectile, Player player, ref float speed ) {
			ProjectileLogic.UpdateGrapplePullSpeed( player, projectile, ref speed );
		}
	}
}
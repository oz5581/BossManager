
# BossManager
Un complemento TShock para Terraria que agrega más herramientas de gestión de jefes. [Download](https://github.com/Soof4/BossManager/releases/latest/download/BossManager.dll)

(Traducido por [FrankV22](https://github.com/itsFrankV22))

## Cómo instalar
- Descargue el archivo del complemento [aquí](https://github.com/Soof4/BossManager/releases/latest/download/BossManager.dll)
- Deje el archivo `BossManager.dll` en la carpeta `ServerPlugins`
- Inicie su servidor TShock

## Funciones (se pueden modificar en el archivo de configuración)
- Evita que los jugadores se unan mientras un jefe está activo
- Evitar que aparezcan jefes específicos
- Requerir una cantidad específica de jugadores en línea para generar un jefe
- Más comandos relacionados con jefes

## Comandos y permisos 
Deshacer comando de jefe
- Nombre ​​de comando: ` /undoboss `, ` /uboss `
- Sintaxis del comando: ` /undoboss <nombre del jefe/alias> `
- Descripción: Alternar el estado de derrota de un jefe
- Permiso: ` bossmgr.undoboss `

Comando de jefe de lista: 
- Alias ​​de comando: ` /listboss `, ` /lboss ` , ` /bosses `
- Descripción: Lista todos los jefes y eventos derrotados
- Permiso: ` bossmgr.listboss `

Habilitar comando de jefe: 
- Alias ​​de comando: ` /enableboss `, ` /enblb ` 
- Descripción: Permitir/Evitar que aparezca un jefe específico
- Permiso: ` bossmgr.enableboss `
- ¡Los jefes también se pueden cambiar manualmente a través del archivo de configuración!

Comando de recarga del jefe jefe: 
- Alias ​​de comando: ` /bossrel `
- Descripción: Recargar el Plugin BossMgr
- Permiso: ` bossmgr.reload `

## Cómo prevenir/permitir un jefe
#### Método 1
- Abra el archivo de configuración en `/tshock/BossManagerConfig.json`
- Cambiar el estado habilitado del jefe especificado de "true" a "false"
- Utilice ` /bossrel ` en el chat para recargar sus configuraciones 
#### Método 2
- Utilice el comando ` /enableboss ` seguido de un nombre de jefe

## Configuration File Options

AllowJoinDuringBoss
- Default: ` true `
- Descripción: permite que los jugadores se unan al servidor mientras un jefe está activo en el mundo.

PreventIllegalBoss (Experimental)
- Default: ` false `
- Descripción: Evita que los jefes aparezcan en el estado de progresión del juego incorrecto (por ejemplo, generar Plantera antes de que se derrote Wall of Flesh)
- Por ahora, esta opción de configuración solo se aplica a los jefes que están en *HardMode antes del HardMode*

RequiredPlayersforBoss
- Default: ` 1 `
- Descripción: La cantidad necesaria de jugadores en línea para generar un jefe
- Esta opción de configuración no aplica para jefes que ya han sido derrotados.

## Nombres de los Bosses (para command identifiers)

King Slime
- ` kingslime `
- ` king `
- ` ks `

Eye of Cthulhu: 
- ` eyeofcthulhu `
- ` eye `
- ` eoc `

Brain of Cthulhu / Eater of Worlds:
- ` evilboss `
- ` boc `
- ` eow `
- ` eaterofworlds `
- ` brainofcthulhu `
- ` brain `
- ` eater `

Deerclops
- ` deerclops `
- ` deer `
- ` dc `

Skeletron
- ` skeletron `
- ` sans `

Queen Bee
- ` queenbee `
- ` qb `

Hardmode / Wall of Flesh
- ` hardmode `
- ` wallofflesh `
- ` wof `
Queen Slime
- ` queenslime `
- ` qs `

The Destroyer
- ` thedestroyer `
- ` destroyer `

The Twins
- ` thetwins `
- ` twins `

Skeletron Prime
- ` skeletronprime `
- ` prime `

Plantera
- ` plantera `

Golem
- ` golem `

Duke Fishron
- ` dukefishron `
- ` duke `
- ` fishron `

Lunatic Cultist
- ` lunaticcultist `
- ` cultist `
- ` lunatic `

Empress of Light
- ` empressoflight `
- ` empress `
- ` eol `

Moonlord
- ` moonlord `
- ` ml `

----

Ultima Prueba Funcional: 24/8/2024 (Terraria 1.4.4.9)

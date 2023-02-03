/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_DUNGEON = 1754227292U;
        static const AkUniqueID PLAY_SFX_ENEMY_DAMAGED = 2216620102U;
        static const AkUniqueID PLAY_SFX_ENEMY_DEATH = 2078250621U;
        static const AkUniqueID PLAY_SFX_ENEMY_MELEEATTACK_CONTAINER = 1682262311U;
        static const AkUniqueID PLAY_SFX_PLAYER_DAMAGED = 1844877881U;
        static const AkUniqueID PLAY_SFX_PLAYER_DODGE_CONTAINER = 3953012785U;
        static const AkUniqueID PLAY_SFX_PLAYER_MELEEATTACK_CONTAINER = 1421029036U;
        static const AkUniqueID PLAY_SFX_ROOTSCONTAINER = 2177193998U;
        static const AkUniqueID PLAY_TESTSFX = 2013398331U;
        static const AkUniqueID PLAY_UI_MENU_CANCEL = 1739779019U;
        static const AkUniqueID PLAY_UI_MENU_CONFIRM = 3500366795U;
        static const AkUniqueID PLAY_UI_MENU_SELECTION = 2157261217U;
        static const AkUniqueID PLAY_UI_MENU_SLIDERCHANGE = 204465150U;
        static const AkUniqueID PLAY_UI_PURCHASE = 1924313358U;
        static const AkUniqueID STOP_DUNGEON = 3594239982U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMEPLAY
        {
            static const AkUniqueID GROUP = 89505537U;

            namespace STATE
            {
                static const AkUniqueID DANGER = 4174463524U;
                static const AkUniqueID DEATH = 779278001U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID NORMAL = 1160234136U;
                static const AkUniqueID VICTORY = 2716678721U;
            } // namespace STATE
        } // namespace GAMEPLAY

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID SFXVOLUME = 988953028U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID DUNGEONBANK = 1411839323U;
        static const AkUniqueID MENUBANK = 2718747754U;
        static const AkUniqueID TESTBANK = 3291379323U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSICBUS = 2886307548U;
        static const AkUniqueID SFXBUS = 3803850708U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__

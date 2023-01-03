using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class Services : MonoBehaviour
    {
        private static Services instance;
        public static Services Instance => instance ??= new Services();

        public EventService EventService { get; } = new EventService();
        
        public static void ClearInstance()
        {
            instance.EventService.Deinit();
            instance = null;
        }
    }
}
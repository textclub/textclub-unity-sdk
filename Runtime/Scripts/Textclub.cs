using System.Runtime.InteropServices;
using UnityEngine;

namespace Textclub
{
    public sealed class Textclub
    {
        public readonly Notifications notifications = new();

        public readonly Analytics analytics = new();

        public readonly Player player = new();
    }
}

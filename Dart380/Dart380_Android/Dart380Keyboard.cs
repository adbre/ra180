using System;
using Android.Views;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Graphics;
using System.Collections.Generic;
using Android.Widget;
using System.Linq;

namespace Dart380_Android
{

	public static class Dart380Keyboard
	{
		public static IEnumerable<Dart380Button> GetButtons()
		{
			// The coordinates assumes the Dart380 image is 1363x1018 pixels in size.
			return new List<Dart380Button> {
				new Dart380Button("F1", 143,353, 61,59),
				new Dart380Button("F2", 224,353, 61,59),
				new Dart380Button("F3", 306,353, 61,59),
				new Dart380Button("F4", 388,353, 61,59),
				new Dart380Button("FMT", 470,353, 61,59),
				new Dart380Button("REP", 143,435, 61,59),
				new Dart380Button("RAD", 224,435, 61,59),
				new Dart380Button("KVI", 306,435, 61,59),
				new Dart380Button("SKR", 388,435, 61,59),
				new Dart380Button("DDA", 470,435, 61,59),
				new Dart380Button("SND", 143,516, 61,59),
				new Dart380Button("EKV", 224,516, 61,59),
				new Dart380Button("MOT", 306,516, 61,59),
				new Dart380Button("AVS", 388,516, 61,59),
				new Dart380Button("ISK", 470,516, 61,59),
				new Dart380Button("OPM", 604,435, 61,59),
				new Dart380Button("EFF", 686,435, 61,59),
				new Dart380Button("ÄND", 767,435, 61,59),
				new Dart380Button("BEL", 604,516, 61,59),
				new Dart380Button("SLT", 686,516, 61,59),
				new Dart380Button("ENT", 767,516, 61,59),
				new Dart380Button("1", 879,272, 61,59),
				new Dart380Button("2", 961,272, 61,59),
				new Dart380Button("3", 1042,272, 61,59),
				new Dart380Button("4", 879,353, 61,59),
				new Dart380Button("5", 961,353, 61,59),
				new Dart380Button("6", 1042,353, 61,59),
				new Dart380Button("7", 879,435, 61,59),
				new Dart380Button("8", 961,435, 61,59),
				new Dart380Button("9", 1042,435, 61,59),
				new Dart380Button("*", 879,516, 61,59),
				new Dart380Button("0", 961,516, 61,59),
				new Dart380Button("#", 1042,516, 61,59),
				new Dart380Button("Q", "!", 143,639, 61,59),
				new Dart380Button("W", "\"", 224,639, 61,59),
				new Dart380Button("E", "#", 306,639, 61,59),
				new Dart380Button("R", "@", 388,639, 61,59),
				new Dart380Button("T", "%", 470,639, 61,59),
				new Dart380Button("Y", "&", 551,639, 61,59),
				new Dart380Button("U", "/", 632,639, 61,59),
				new Dart380Button("I", "/", 714,639, 61,59),
				new Dart380Button("O", "(", 796,639, 61,59),
				new Dart380Button("P", ")", 877,639, 61,59),
				new Dart380Button("Å", "+", 958,639, 61,59),
				new Dart380Button("DEL", 1040,639, 61,59),
				new Dart380Button("A", 184,721, 61,59),
				new Dart380Button("S", 266,721, 61,59),
				new Dart380Button("D", 347,721, 61,59),
				new Dart380Button("F", 429,721, 61,59),
				new Dart380Button("G", "´", 510,721, 61,59),
				new Dart380Button("H", "^", 592,721, 61,59),
				new Dart380Button("J", "$", 673,721, 61,59),
				new Dart380Button("K", "<", 755,721, 61,59),
				new Dart380Button("L", ">", 837,721, 61,59),
				new Dart380Button("Ö", "*", 918,721, 61,59),
				new Dart380Button("Ä", "?", 1000,721, 61,59),
				new Dart380Button("SHIFT", 143,802, 61,59),
				new Dart380Button("Z", 224,802, 61,59),
				new Dart380Button("X", 306,802, 61,59),
				new Dart380Button("C", 388,802, 61,59),
				new Dart380Button("V", 470,802, 61,59),
				new Dart380Button("B", 551,802, 61,59),
				new Dart380Button("N", 632,802, 61,59),
				new Dart380Button("M", 714,802, 61,59),
				new Dart380Button(",", ";", 796,802, 61,59),
				new Dart380Button(".", ":", 877,802, 61,59),
				new Dart380Button("-", "_", 958,802, 61,59),
				new Dart380Button("ENT", 1040,802, 61,59),
				new Dart380Button("LARROW", 224,884, 61,59),
				new Dart380Button("RARROW", 306,884, 61,59),
				new Dart380Button("UARROW", 388,884, 61,59),
				new Dart380Button("DARROW", 470,884, 61,59),
				new Dart380Button("SPACE", 551,884, 307,59),
				new Dart380Button("PGUP", 877,884, 61,59),
				new Dart380Button("PGDOWN", 958,884, 61,59),
			};
		}
	}
	
}

function Ra180DisplayCharacter() {
	var me = this;
	me.character = ko.observable(" ");
	me.isBlinking = ko.observable(false);
	me.hasUnderscore = ko.observable(false);
}

function Ra180ViewModel() {
	var me = this;
	me.MOD_OFF = 3;
	me.MOD_KLAR = 4;
	me.MOD_SKYDD = 5;
	me.MOD_DRELAY = 6;
	me.MOD_DEBUG = 9;

	me.tid = ko.observable("000000");
	me.dat = ko.observable("0101");

	me.display = {
		char1: new Ra180DisplayCharacter(),
		char2: new Ra180DisplayCharacter(),
		char3: new Ra180DisplayCharacter(),
		char4: new Ra180DisplayCharacter(),
		char5: new Ra180DisplayCharacter(),
		char6: new Ra180DisplayCharacter(),
		char7: new Ra180DisplayCharacter(),
		char8: new Ra180DisplayCharacter(),
		
		getPlainText: function () {
			var result = "";
			result += me.display.char1.character();
			result += me.display.char2.character();
			result += me.display.char3.character();
			result += me.display.char4.character();
			result += me.display.char5.character();
			result += me.display.char6.character();
			result += me.display.char7.character();
			result += me.display.char8.character();
			return result;
		},

		setInputText: function (text) {
			me.display.setText(text);
			me.display.char8.hasUnderscore(text.length >= 7);
			me.display.char7.hasUnderscore(text.length == 6);
			me.display.char6.hasUnderscore(text.length == 5);
			me.display.char5.hasUnderscore(text.length == 4);
			me.display.char4.hasUnderscore(text.length == 3);
			me.display.char3.hasUnderscore(text.length == 2);
			me.display.char2.hasUnderscore(text.length == 1);
			me.display.char1.hasUnderscore(text.length == 0);
			me.display.char8.isBlinking(text.length >= 8);
			me.display.char1.isBlinking(text.length > 0 && text[0] == ":");
			me.display.char2.isBlinking(text.length > 1 && text[1] == ":");
			me.display.char3.isBlinking(text.length > 2 && text[2] == ":");
			me.display.char4.isBlinking(text.length > 3 && text[3] == ":");
			me.display.char5.isBlinking(text.length > 4 && text[5] == ":");
			me.display.char6.isBlinking(text.length > 5 && text[5] == ":");
			me.display.char7.isBlinking(text.length > 6 && text[6] == ":");
			me.display.char8.isBlinking(text.length > 7 && text[7] == ":");
		},
		
		setText: function (text) {
			me.display.char1.character(text.length > 0 ? text[0] : " ");
			me.display.char2.character(text.length > 1 ? text[1] : " ");
			me.display.char3.character(text.length > 2 ? text[2] : " ");
			me.display.char4.character(text.length > 3 ? text[3] : " ");
			me.display.char5.character(text.length > 4 ? text[4] : " ");
			me.display.char6.character(text.length > 5 ? text[5] : " ");
			me.display.char7.character(text.length > 6 ? text[6] : " ");
			me.display.char8.character(text.length > 7 ? text[7] : " ");
			me.display.setInputPos(-1);
		},

		getCharacter: function (pos) {
			switch (pos) {
				case 0: return me.display.char1; break;
				case 1: return me.display.char2; break;
				case 2: return me.display.char3; break;
				case 3: return me.display.char4; break;
				case 4: return me.display.char5; break;
				case 5: return me.display.char6; break;
				case 6: return me.display.char7; break;
				case 7: return me.display.char8; break;
			}
		},

		setCharacterBlinking: function (pos, value) {
			var character = me.display.getCharacter(pos);
			character.isBlinking(value);
		},
		
		setInputPos: function (pos) {
			me.display.setCharacterBlinking(0, pos == 0);
			me.display.setCharacterBlinking(1, pos == 1);
			me.display.setCharacterBlinking(2, pos == 2);
			me.display.setCharacterBlinking(3, pos == 3);
			me.display.setCharacterBlinking(4, pos == 4);
			me.display.setCharacterBlinking(5, pos == 5);
			me.display.setCharacterBlinking(6, pos == 6);
			me.display.setCharacterBlinking(7, pos == 7);
			me.display.char1.hasUnderscore(pos == 0);
			me.display.char2.hasUnderscore(pos == 1);
			me.display.char3.hasUnderscore(pos == 2);
			me.display.char4.hasUnderscore(pos == 3);
			me.display.char5.hasUnderscore(pos == 4);
			me.display.char6.hasUnderscore(pos == 5);
			me.display.char7.hasUnderscore(pos == 6);
			me.display.char8.hasUnderscore(pos == 7);
		}
	};
	
	me.channel = ko.observable(1);
	me.volume = ko.observable(4);
	me.mod = ko.observable(me.MOD_OFF);
	
	me.mod.subscribe(function(newValue) {
		if (newValue != me.MOD_OFF) {
			me.display.setText("TEST");
			me.display.setText("TEST OK");
			me.display.setText("NOLLST");
			me.display.setText("");
		} else {
			me.display.setText("");
		}
	});	

	me.getVredClass = function(value) {
		return "vred-0" + value;
	};

	me.currentMenu = null;

	me.sendKey = function (key) {
		if (me.mod() == me.MOD_OFF) return;
		if (me.mod() == me.MOD_DEBUG) {
			me.display.setText(key);
			me.display.setInputPos(key.length);
		} else if (me.currentMenu) {
			me.currentMenu.sendKey(key);
		} else {
			var onSubmenuDone = function () {
				me.currentMenu = null;
				me.display.setText("");
			};

			var getSubmenu = function () {
				switch (key) {
					case "1": return new Ra180TidMenu(me, onSubmenuDone);
					case "2": return new Ra180RdaMenu(me, onSubmenuDone);
				}
			};

			me.currentMenu = getSubmenu();
		}
	};

	me.channelVred = ko.computed(function() { return me.getVredClass(me.channel()); }, me);
	me.volumeVred = ko.computed(function() { return me.getVredClass(me.volume()); }, me);
	me.modVred = ko.computed(function() { return me.getVredClass(me.mod()); }, me);
	
	me.setChannel1 = function() { me.channel(1); };
	me.setChannel2 = function() { me.channel(2); };
	me.setChannel3 = function() { me.channel(3); };
	me.setChannel4 = function() { me.channel(4); };
	me.setChannel5 = function() { me.channel(5); };
	me.setChannel6 = function() { me.channel(6); };
	me.setChannel7 = function() { me.channel(7); };
	me.setChannel8 = function() { me.channel(8); };
	
	me.setVolume1 = function() { me.volume(1); };
	me.setVolume2 = function() { me.volume(2); };
	me.setVolume3 = function() { me.volume(3); };
	me.setVolume4 = function() { me.volume(4); };
	me.setVolume5 = function() { me.volume(5); };
	me.setVolume6 = function() { me.volume(6); };
	me.setVolume7 = function() { me.volume(7); };
	me.setVolume8 = function() { me.volume(8); };

	me.setModOff = function() { me.mod(me.MOD_OFF); }
	me.setModKlar = function() { me.mod(me.MOD_KLAR); }
	me.setModSkydd = function() { me.mod(me.MOD_SKYDD); }
	me.setModDRelay = function() { me.mod(me.MOD_DRELAY); }
	me.setModDebug = function() { me.mod(me.MOD_DEBUG); }

	me.sendKey1 = function() { me.sendKey("1"); }
	me.sendKey2 = function() { me.sendKey("2"); }
	me.sendKey3 = function() { me.sendKey("3"); }
	me.sendKey4 = function() { me.sendKey("4"); }
	me.sendKey5 = function() { me.sendKey("5"); }
	me.sendKey6 = function() { me.sendKey("6"); }
	me.sendKey7 = function() { me.sendKey("7"); }
	me.sendKey8 = function() { me.sendKey("8"); }
	me.sendKey9 = function() { me.sendKey("9"); }
	me.sendKey0 = function() { me.sendKey("0"); }
	me.sendKeyAsterix = function() { me.sendKey("*"); }
	me.sendKeyHashtag = function() { me.sendKey("#"); }
	me.sendKeyReset = function() { me.sendKey("RESET"); }
	me.sendKeyOpm = function() { me.sendKey("OPM"); }
	me.sendKeyEff = function() { me.sendKey("EFF"); }
	me.sendKeyAnd = function() { me.sendKey("AND"); }
	me.sendKeyBel = function() { me.sendKey("BEL"); }
	me.sendKeySlt = function() { me.sendKey("SLT"); }
	me.sendKeyEnt = function() { me.sendKey("ENT"); }
}
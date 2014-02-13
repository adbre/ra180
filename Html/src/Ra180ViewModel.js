function Ra180DisplayCharacter() {
	var me = this;
	me.character = ko.observable(" ");
	me.isBlinking = ko.observable(false);
	me.hasUnderscore = ko.observable(false);
}

function Ra180ChannelData() {
	var me = this;
	me.fr = ko.observable();
	me.bd1 = ko.observable();
	me.bd2 = ko.observable();
	me.pny = ko.observable();
	me.nyk = ko.observable();
	me.nyk1 = ko.observable();
	me.nyk2 = ko.observable();

	me.reset = function () {
		me.fr("");
		me.bd1("");
		me.bd2("");
		me.pny("");
		me.nyk("");
		me.nyk1("");
		me.nyk2("");
	}
}

function Ra180Data() {
	var me = this;
	me.tid = ko.observable("000000");
	me.dat = ko.observable("0101");

	me.synk = ko.observable();
	me.eff = ko.observable();
	me.sdx = ko.observable();
	me.opmtn = ko.observable();
	me.rap = ko.observable();
	me.bel = ko.observable();

	me.channel1 = new Ra180ChannelData();
	me.channel2 = new Ra180ChannelData();
	me.channel3 = new Ra180ChannelData();
	me.channel4 = new Ra180ChannelData();
	me.channel5 = new Ra180ChannelData();
	me.channel6 = new Ra180ChannelData();
	me.channel7 = new Ra180ChannelData();
	me.channel8 = new Ra180ChannelData();

	me.isEmpty = ko.observable(true);

	me.reset = function () {
		me.synk(false);
		me.eff("LÅG");
		me.sdx(false);
		me.opmtn(false);
		me.rap("UPPK");
		me.bel(3);
	
		me.channel1.reset();
		me.channel1.fr("42000");
		me.channel1.bd1("3040");
		me.channel1.bd2("5060");

		me.channel2.reset();
		me.channel2.fr("52000");
		me.channel2.bd1("3040");
		me.channel2.bd2("5060");

		me.channel3.reset();
		me.channel3.fr("62000");
		me.channel3.bd1("3040");
		me.channel3.bd2("5060");

		me.channel4.reset();
		me.channel4.fr("72000");
		me.channel4.bd1("3040");
		me.channel4.bd2("5060");

		me.channel5.reset();
		me.channel5.fr("41125");
		me.channel5.bd1("3040");
		me.channel5.bd2("5060");

		me.channel6.reset();
		me.channel6.fr("51125");
		me.channel6.bd1("3040");
		me.channel6.bd2("5060");

		me.channel7.reset();
		me.channel7.fr("61125");
		me.channel7.bd1("3040");
		me.channel7.bd2("5060");

		me.channel8.reset();
		me.channel8.fr("71125");
		me.channel8.bd1("3040");
		me.channel8.bd2("5060");

		me.isEmpty(false);
	};
}


function Ra180ViewModel() {
	var me = this;
	me.MOD_OFF = 3;
	me.MOD_KLAR = 4;
	me.MOD_SKYDD = 5;
	me.MOD_DRELAY = 6;
	me.MOD_DEBUG = 9;

	me.data = new Ra180Data();

	me.pnyCalc = new Ra180PnyCalculator();

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
			if (!character) {
				return;
			}
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
			if (me.data.isEmpty()) {
				me.display.setText("NOLLST");
				me.data.reset();
			}
			me.display.setText("");
		} else {
			me.display.setText("");
		}
	});

	me.getVredClass = function(value) {
		return "vred-0" + value;
	};

	me.getChannelData = function () {
		switch (me.channel()) {
			case 1: return me.data.channel1;
			case 2: return me.data.channel2;
			case 3: return me.data.channel3;
			case 4: return me.data.channel4;
			case 5: return me.data.channel5;
			case 6: return me.data.channel6;
			case 7: return me.data.channel7;
			case 8: return me.data.channel8;
		}
	};

	me.menuOptions = {
		tid: {
			title: "TID",
			submenus: [
				{
					prefix: "T",
					maxInputTextLength: 6,
					canEdit: true,
					saveInput: function (text) { 
						if (text.length == 6) {
							me.data.tid(text);
							return true;
						}
						return false;
					},
					getValue: function () { return me.data.tid(); },
				},
				{
					prefix: "DAT",
					maxInputTextLength: 4,
					canEdit: true,
					saveInput: function (text) {
						if (text.length == 4) {
							me.data.dat(text);
							return true;
						}
						return false;
					},
					getValue: function () { return me.data.dat(); },
				}
			]
		},
		rda: {
			title: "RDA",
			submenus: [
				{
					prefix: "SDX",
					getValue: function () { return "NEJ"; },
				},
				{
					prefix: "OPMTN",
					getValue: function () { return "JA"; },
				},
				{
					prefix: "BAT",
					getValue: function () { return "12.5"; },
				}
			]
		},
		dtm: {
			title: "DTM"
		},
		kda: {
			title: "KDA",
			submenus: [
				{
					prefix: "FR",
					maxInputTextLength: 5,
					canEdit: true,
					getValue: function () { return me.getChannelData().fr(); },
					saveInput: function (text) {
						if (text.length != 5) {
							return false;
						}

						var fr = parseInt(text);
						if (fr < 30000 || fr > 87975) {
							return false;
						}
						
						me.getChannelData().fr(text);
						return true;
					}
				},
				{
					prefix: "BD1",
					maxInputTextLength: 4,
					canEdit: true,
					getValue: function () {
						return me.getChannelData().bd1();
					},
					saveInput: function (text, menu) {
						if (text.length != 4) {
							return;
						}

						me.getChannelData().bd1(text);
						menu.stopInput();
						menu.nextSubmenu();
						return false;
					},
				},
				{
					prefix: "BD2",
					maxInputTextLength: 4,
					canEdit: true,
					getValue: function () { return me.getChannelData().bd2(); },
					saveInput: function (text, menu) {
						if (text.length != 4) {
							return false;
						}

						me.getChannelData().bd2(text);
						menu.previousSubmenu();
						return true;
					},
					onKey: function (key, menu) {
						if (key == "AND" && !menu.isEditing) {
							menu.previousSubmenu();
						}

						return false;
					},
				},
				{
					prefix: "SYNK",
					canSelect: function () {
						return me.data.synk() ? true : false;
					},
					getValue: function () {
						return me.data.synk() ? "JA" : "NEJ";
					}
				},
				{
					pn: ["", "", "", "", "", "", "", ""],
					pnIndex: 0,
					maxInputTextLength: 4,
					prefix: function (menu) {
						if (menu.isEditing) {
							return "PN" + (menu.currentSubmenu.pnIndex + 1);
						} else {
							return "PNY";
						}
					},
					canEdit: true,
					getValue: function () {
						var pny = me.getChannelData().pny();
						if (pny && pny.length == 3) {
							return pny;
						} else {
							return "###";
						}
					},
					onKey: function (key, menu) {
						if (key == "AND" && !menu.isEditing) {
							menu.currentSubmenu.pnIndex = 0;
						}
						return false;
					},
					saveInput: function (text, menu) {
						var self = menu.currentSubmenu;

						if (!me.pnyCalc.isValidPn(text)) {
							return false;
						}

						self.pn[self.pnIndex] = text;
						self.pnIndex = self.pnIndex + 1;

						if (self.pnIndex == self.pn.length) {
							var pny = me.pnyCalc.calculatePny(self.pn);
							me.getChannelData().pny(pny);
							return true;
						}

						return false;
					},
				},
			]
		},
		niv: {
			title: "NIV"
		},
		rap: {
			title: "RAP"
		},
		nyk: {
			title: "NYK",
			submenus: [
				{
					prefix: "NYK",
					getValue: function () {
						return "###";
					},
				}
			]
		},
		tjk: {
			title: "TJK"
		}
	};

	me.currentMenu = null;

	me.sendKey = function (key, count) {
		count = typeof count !== 'undefined' ? count : 1;
		if (count > 1) {
			for (var i=0; i < count; i++) {
				me.sendKey(key, 1);
			}
			return;
		} else if (count < 1) {
			return;
		}

		if (me.mod() == me.MOD_OFF) return;
		if (me.mod() == me.MOD_DEBUG) {
			me.display.setText(key);
			me.display.setInputPos(key.length);
		} else if (me.currentMenu) {
			me.currentMenu.sendKey(key);
		} else {

			var getMenuOptions = function () {
				switch (key) {
					case "1": return me.menuOptions.tid;
					case "2": return me.menuOptions.rda;
					case "3": return me.menuOptions.dtm;
					case "4": return me.menuOptions.kda;
					case "5": return me.menuOptions.niv;
					case "6": return me.menuOptions.rap;
					case "7": return me.menuOptions.nyk;
					case "8": return null;
					case "9": return me.menuOptions.tjk;
				}
			};

			var options = getMenuOptions();
			if (options) {
				options.completed = function () {
					me.currentMenu = undefined;
					me.display.setText("");
				};

				me.currentMenu = new Ra180Menu(options, me);
				me.currentMenu.refreshDisplay();
			}
		}
	};
	
	me.sendKeys = function (keys) {
		keys.split("").forEach(function (c) {
			me.sendKey(c);
		});
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
	me.sendKeyEnt = function(count) { me.sendKey("ENT", count); }
}
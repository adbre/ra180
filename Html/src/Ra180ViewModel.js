function zeroFill( number, width )
{
  width -= number.toString().length;
  if ( width > 0 )
  {
    return new Array( width + (/\./.test( number ) ? 2 : 1) ).join( '0' ) + number;
  }
  return number + ""; // always return a string
}

function Ra180DisplayCharacter() {
	var me = this;
	me.character = ko.observable(" ");
	me.isBlinking = ko.observable(false);
	me.hasUnderscore = ko.observable(false);
}

function Ra180ChannelData() {
	var me = this;
	var subscribers = [];
	me.fr = ko.observable();
	me.bd1 = ko.observable();
	me.bd2 = ko.observable();
	me.pny = ko.observable();
	me.pny2 = ko.observable();
	me.nyk = ko.observable();

	me.pny.subscribe(notifySubscribers);
	me.pny2.subscribe(notifySubscribers);
	me.nyk.subscribe(notifySubscribers);
	
	me.reset = function () {
		me.fr("");
		me.bd1("");
		me.bd2("");
		me.pny("");
		me.pny2("");
		me.nyk("");
	}

	me.subscribe = function (fn) {
		subscribers.push(fn);
	};

	function notifySubscribers() {
		for (var i=0; i < subscribers.length; i++) {
			var subscriber = subscribers[i];
			subscriber();
		}
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
	
	function setNotEmpty() {
		me.isEmpty(false);
	}
	
	me.channel1.subscribe(setNotEmpty);
	me.channel2.subscribe(setNotEmpty);
	me.channel3.subscribe(setNotEmpty);
	me.channel4.subscribe(setNotEmpty);
	me.channel5.subscribe(setNotEmpty);
	me.channel6.subscribe(setNotEmpty);
	me.channel7.subscribe(setNotEmpty);
	me.channel8.subscribe(setNotEmpty);

	me.tick = function () {
		var reTid = /([0-9]{2})([0-9]{2})([0-9]{2})/;
		var reDat = /([0-9]{2})([0-9]{2})/;
		var match;
		match = reDat.exec(me.dat());
		var month = parseInt(match[1]);
		var date = parseInt(match[2]);
		match = reTid.exec(me.tid());
		var hour = parseInt(match[1]);
		var minute = parseInt(match[2]);
		var second = parseInt(match[3]);
		second++;
		if (second == 60) {
			second = 0;
			minute++;
		}
		if (minute == 60) {
			minute = 0;
			hour++;
		}
		if (hour == 24) {
			hour = 0;
			date++;
		}

		if (date > 31) {
			date = 1;
			month++;
		}
		if (month > 12) {
			month = 1;
		}

		month = zeroFill(month, 2);
		date = zeroFill(date, 2);
		hour = zeroFill(hour, 2);
		minute = zeroFill(minute, 2);
		second = zeroFill(second, 2);

		me.tid(hour + minute + second);
		me.dat(month + date);
	};

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

		me.isEmpty(true);
	};
}


function Ra180ViewModel() {
	var me = this;
	me.MOD_OFF = 3;
	me.MOD_KLAR = 4;
	me.MOD_SKYDD = 5;
	me.MOD_DRELAY = 6;
	me.MOD_DEBUG = 9;
	me.EFF_LOW = "LÅG";
	me.EFF_NRM = "NRM";
	me.EFF_HIG = "HÖG";
	me.SELFTEST_INTERVAL = 2000;
	
	me.pnyCalc = new Ra180PnyCalculator();
	me.synchronizationContext = undefined;

	function tick() {
		me.data.tick();
		if (me.currentMenu) {
			me.currentMenu.refreshDisplay();
		}
	}
	
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
	me.eff = ko.observable(me.EFF_LOW);
	me.isEnabled = ko.observable(false);

	function start(mod, shouldStartAsync) {
		me.display.setText("");
		me.mod(mod);
		if (shouldStartAsync) {
			me.synchronizationContext.setInterval(tick, 1000);
		}
		me.isEnabled(mod != me.MOD_OFF);
	}
	
	me.setMod = function (newValue) {
		me.mod(newValue);
		if (newValue != me.MOD_OFF) {
			var shouldStartAsync = false;
			if (!me.data) {
				me.data = new Ra180Data();
				me.data.reset();
				shouldStartAsync = true;
			}
			
			me.display.setText("TEST");
			me.synchronizationContext.setTimeout(function() {
				me.display.setText("TEST OK");
				me.synchronizationContext.setTimeout(function() {
					if (me.data.isEmpty()) {
						me.display.setText("NOLLST");
						me.synchronizationContext.setTimeout(function() {
							start(newValue, shouldStartAsync);
						}, me.SELFTEST_INTERVAL);
					} else {
						start(newValue, shouldStartAsync);
					}
				}, me.SELFTEST_INTERVAL);
			}, me.SELFTEST_INTERVAL);
		} else {
			start(newValue, shouldStartAsync);
		}
	};

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
					canEdit: false,
					canSelect: function () {
						var pny = me.getChannelData().pny();
						var nyk = me.getChannelData().nyk();
						return (pny && pny.length == 3) || (nyk && nyk.length == 3);
					},
					getValue: function (index) {
						var nyk = me.getChannelData().nyk();
						return (nyk && nyk.length == 3) ? nyk : "###";
					},
					nextOption: function (index) {
						var channeldata = me.getChannelData();
						var nyk = channeldata.nyk();
						var pny = channeldata.pny();
						channeldata.nyk(pny);
						channeldata.pny(nyk);
					},
					getOptions: function () {
						var pny = me.getChannelData().pny();
						var pny2 = me.getChannelData().pny2();
						var result = new Array();
						if (pny) {
							result.push(pny);
						}
						if (pny2) {
							result.push(pny2);
						}
						result.push("###");
						return result;
					}
				}
			]
		},
		tjk: {
			title: "TJK"
		}
	};
	
	me.effMenu = new function () {
		var self = this;
		self.sendKey = function (key) {
			if (key == "SLT") {
				self.completed();
				return;
			}

			if (key == "EFF") {
				var eff = me.eff();
				switch (eff) {
					case me.EFF_LOW: eff = me.EFF_NRM; break;
					case me.EFF_NRM: eff = me.EFF_HIG; break;
					case me.EFF_HIG: eff = me.EFF_LOW; break;
					default: eff = me.EFF_LOW; break;
				}
				me.eff(eff);
				self.refreshDisplay();
			}
		};
		self.refreshDisplay = function () {
			me.display.setText("EFF:" + me.eff());
		};
		self.completed = function () {
		};
	};

	me.currentMenu = null;

	function closeMenu() {		
		me.currentMenu = undefined;
		me.display.setText("");
	}

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

		if (!me.isEnabled()) return;
		if (me.mod() == me.MOD_DEBUG) {
			me.display.setText(key);
			me.display.setInputPos(key.length);
			return;
		}

		if (key == "RESET") {
			me.data.reset();
			me.isEnabled(false);
			me.setMod(me.mod());
			return;
		}
		
		if (me.currentMenu) {
			me.currentMenu.sendKey(key);
		} else if (key.length > 1) {
			if (key == "EFF") {
				me.currentMenu = me.effMenu;
				me.currentMenu.completed = closeMenu;
				me.currentMenu.refreshDisplay();
			}
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
				options.completed = closeMenu;
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

	me.setModOff = function() { me.setMod(me.MOD_OFF); }
	me.setModKlar = function() { me.setMod(me.MOD_KLAR); }
	me.setModSkydd = function() { me.setMod(me.MOD_SKYDD); }
	me.setModDRelay = function() { me.setMod(me.MOD_DRELAY); }
	me.setModDebug = function() { me.setMod(me.MOD_DEBUG); }

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
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

	me.channel = ko.observable(1);
	me.volume = ko.observable(4);
	me.mod = ko.observable(me.MOD_OFF);
	me.bel = ko.observable(3);
	me.eff = ko.observable(me.EFF_LOW);
	me.isEnabled = ko.observable(false);
	me.context = ko.observable();
	
	me.synchronizationContext = undefined;

	me.display = new Ra180Display();

	var clock = new Ra180Clock();
	var pnyCalculator = new Ra180PnyCalculator();
	var currentMenu = null;

	var menuOptions = {
		tid: {
			title: "TID",
			submenus: [
				{
					prefix: "T",
					maxInputTextLength: 6,
					canEdit: true,
					saveInput: function (text) { 
						if (text.length == 6) {
							return clock.setTid(text);
						}
						return false;
					},
					getValue: function () { return clock.getTid(); },
				},
				{
					prefix: "DAT",
					maxInputTextLength: 4,
					canEdit: true,
					saveInput: function (text) {
						if (text.length == 4) {
							return clock.setDat(text);
						}
						return false;
					},
					getValue: function () { return clock.getDat(); },
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
					prefix: function () {
						if (getChannelData().isKlarDisabled()) {
							return "**";
						} else {
							return "FR";
						}
					},
					maxInputTextLength: 5,
					canEdit: true,
					getValue: function () {
						var channelData = getChannelData();
						if (channelData.isKlarDisabled() && me.mod() == me.MOD_KLAR) {
							return "00000";
						} else {
							return channelData.fr();
						}
					},
					saveInput: function (text) {
						if (/^\*{2,}$/.exec(text)) {
							if (me.mod() == me.MOD_KLAR) return false;
							var channelData = getChannelData();
							var isKlarDisabled = channelData.isKlarDisabled();
							channelData.isKlarDisabled(!isKlarDisabled);
							return true;
						}

						if (text.length != 5) {
							return false;
						}

						var fr = parseInt(text);
						if (fr < 30000 || fr > 87975) {
							return false;
						}
						
						getChannelData().fr(text);
						return true;
					}
				},
				{
					prefix: "BD1",
					maxInputTextLength: 4,
					canEdit: true,
					getValue: function () {
						return getChannelData().bd1();
					},
					hidden: function () { return me.mod() == me.MOD_KLAR; },
					saveInput: function (text, menu) {
						if (text.length != 4) {
							return;
						}

						getChannelData().bd1(text);
						menu.stopInput();
						menu.nextSubmenu();
						return false;
					},
				},
				{
					prefix: "BD2",
					hidden: function () {
						if (me.mod() == me.MOD_KLAR) return true;
						return getChannelData().bd1() == "9000";
					},
					maxInputTextLength: 4,
					canEdit: true,
					getValue: function () { return getChannelData().bd2(); },
					saveInput: function (text, menu) {
						if (text.length != 4) {
							return false;
						}

						getChannelData().bd2(text);
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
					hidden: function () { return me.mod() == me.MOD_KLAR; },
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
					hidden: function () { return me.mod() == me.MOD_KLAR; },
					canEdit: true,
					getValue: function () {
						var pny = getChannelData().pny();
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

						if (!pnyCalculator.isValidPn(text)) {
							return false;
						}

						self.pn[self.pnIndex] = text;
						self.pnIndex = self.pnIndex + 1;

						if (self.pnIndex == self.pn.length) {
							var pny = pnyCalculator.calculatePny(self.pn);
							getChannelData().pny(pny);
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
						var pny = getChannelData().pny();
						var nyk = getChannelData().nyk();
						return (pny && pny.length == 3) || (nyk && nyk.length == 3);
					},
					getValue: function (index) {
						var nyk = getChannelData().nyk();
						return (nyk && nyk.length == 3) ? nyk : "###";
					},
					nextOption: function (index) {
						var channeldata = getChannelData();
						var nyk = channeldata.nyk();
						var pny = channeldata.pny();
						channeldata.nyk(pny);
						channeldata.pny(nyk);
					},
					getOptions: function () {
						var pny = getChannelData().pny();
						var pny2 = getChannelData().pny2();
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
		},
		eff: new function() {
			var self = this;

			function nextEffValue(value) {
				switch (value) {
					case me.EFF_LOW: return me.EFF_NRM;
					case me.EFF_NRM: return me.EFF_HIG;
					case me.EFF_HIG: return me.EFF_LOW;
					default: return me.EFF_LOW;
				}
			}

			self.isStandAlone = true;
			self.prefix = "EFF";

			self.getValue = function () {
				return me.eff();
			};

			self.sendKey = function (key) {
				if (key == "SLT" || key == "ENT") {
					self.completed();
				}
				
				if (key == "EFF") {
					var currentValue = me.eff();
					var nextValue = nextEffValue(currentValue);
					me.eff(nextValue);
					self.refreshDisplay();
				}
			};

			self.refreshDisplay = function () {
				var text = self.prefix + ":" + self.getValue();
				me.display.setText(text);
			};

			self.completed = function() {};
		}
	};

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

		if (me.mod() != me.MOD_OFF && key == "BEL") {
			me.display.toggleBrightness();
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
			runSelfTest(function() {});
			return;
		}
		
		if (currentMenu) {
			currentMenu.sendKey(key);
		} else {
			openMenu(key);
		}
	};
	
	me.sendKeys = function (keys) {
		keys.split("").forEach(function (c) {
			me.sendKey(c);
		});
	};
	
	me.setChannel1 = function() { setChannel(1); };
	me.setChannel2 = function() { setChannel(2); };
	me.setChannel3 = function() { setChannel(3); };
	me.setChannel4 = function() { setChannel(4); };
	me.setChannel5 = function() { setChannel(5); };
	me.setChannel6 = function() { setChannel(6); };
	me.setChannel7 = function() { setChannel(7); };
	me.setChannel8 = function() { setChannel(8); };
	
	me.setVolume1 = function() { me.volume(1); };
	me.setVolume2 = function() { me.volume(2); };
	me.setVolume3 = function() { me.volume(3); };
	me.setVolume4 = function() { me.volume(4); };
	me.setVolume5 = function() { me.volume(5); };
	me.setVolume6 = function() { me.volume(6); };
	me.setVolume7 = function() { me.volume(7); };
	me.setVolume8 = function() { me.volume(8); };

	me.setModOff = function() { setMod(me.MOD_OFF); }
	me.setModKlar = function() { setMod(me.MOD_KLAR); }
	me.setModSkydd = function() { setMod(me.MOD_SKYDD); }
	me.setModDRelay = function() { setMod(me.MOD_DRELAY); }
	me.setModDebug = function() { setMod(me.MOD_DEBUG); }

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

	me.sendKeyMod = function () {
		switch (me.mod())
		{
			case me.MOD_OFF: me.setModKlar(); break;
			case me.MOD_KLAR: me.setModSkydd(); break;
			case me.MOD_SKYDD: me.setModDRelay(); break;
			case me.MOD_DRELAY: me.setModOff(); break;
		}
	};

	me.sendKeyChannel = function () {
		switch (me.channel())
		{
			case 1: me.setChannel2(); break;
			case 2: me.setChannel3(); break;
			case 3: me.setChannel4(); break;
			case 4: me.setChannel5(); break;
			case 5: me.setChannel6(); break;
			case 6: me.setChannel7(); break;
			case 7: me.setChannel8(); break;
			case 8: me.setChannel1(); break;
		}
	};

	me.sendKeyVolume = function () {
		switch (me.volume())
		{
			case 1: me.setVolume2(); break;
			case 2: me.setVolume3(); break;
			case 3: me.setVolume4(); break;
			case 4: me.setVolume5(); break;
			case 5: me.setVolume6(); break;
			case 6: me.setVolume7(); break;
			case 7: me.setVolume8(); break;
			case 8: me.setVolume1(); break;
		}
	};

	me.css = new function() {
		var self = this;
		
		self.brightness = ko.computed(function() {
			return "ra180-display-bel" + me.display.brightness();
		}, me.display);
		
		self.channel = ko.computed(function() {
			return "vred-0" + me.channel();
		}, self);

		self.volume = ko.computed(function() {
			return "vred-0"+ me.volume();
		}, self);

		self.mod = ko.computed(function() {
			return "vred-0"+ me.mod();
		}, self);
	};

	function tick() {
		clock.tick();
		if (currentMenu) {
			currentMenu.refreshDisplay();
		}
	}
	
	function setMod(newValue) {
		var currentValue = me.mod();
		me.mod(newValue);
		refreshDisplay();

		var shouldStartAsync = false;

		function start() {
			me.display.setText("");
			me.isEnabled(newValue != me.MOD_OFF);
			if (!shouldStartAsync) return;
			me.synchronizationContext.setInterval(tick, 1000);
		}
		
		if (newValue != me.MOD_OFF && currentValue == me.MOD_OFF) {
			if (!me.data) {
				me.data = new Ra180Data();
				me.data.reset();
				shouldStartAsync = true;
			}
			
			runSelfTest(function() {
				start();
			});
		} else if (newValue == me.MOD_OFF) {
			if (currentMenu) {
				currentMenu.close();
			}

			start();
		}
	}

	var isExecutingSelfTest = false;
	function runSelfTest(fn) {
		if (isExecutingSelfTest) return;
		function complete() {
			me.display.setText("");
			refreshContext();
			fn();
			isExecutingSelfTest = false;
		}
		isExecutingSelfTest = true;
		me.display.setText("TEST");
		me.synchronizationContext.setTimeout(function() {
			if (me.mod() == me.MOD_OFF) return;
			me.display.setText("TEST OK");
			me.synchronizationContext.setTimeout(function() {
				if (me.mod() == me.MOD_OFF) return;
				if (me.data.isEmpty()) {
					me.display.setText("NOLLST");
					me.synchronizationContext.setTimeout(function() {
						if (me.mod() == me.MOD_OFF) return;
						complete();
					}, me.SELFTEST_INTERVAL);
				} else {
					complete();
				}
			}, me.SELFTEST_INTERVAL);
		}, me.SELFTEST_INTERVAL);
	}

	function getChannelData() {
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
	}

	function openMenu(key) {
		var options = getMenu(key);
		if (!options) return;

		options.completed = closeMenu;

		if (typeof options.isStandAlone === 'undefined' || !options.isStandAlone) {
			options = new Ra180Menu(options, me);
		}

		currentMenu = options;
		currentMenu.refreshDisplay();
	}

	function closeMenu() {		
		currentMenu = undefined;
		me.display.setText("");
	}

	function getMenu(key) {
		switch (key) {
			case "1": return menuOptions.tid;
			case "2": return menuOptions.rda;
			case "3": return menuOptions.dtm;
			case "4": return menuOptions.kda;
			case "5": return menuOptions.niv;
			case "6": return menuOptions.rap;
			case "7": return menuOptions.nyk;
			case "8": return null;
			case "9": return menuOptions.tjk;
			case "EFF": return menuOptions.eff;
		}
	}

	function refreshDisplay() {
		if (currentMenu) {
			currentMenu.refreshDisplay();
		}
	}

	function setChannel(value) {
		me.channel(value);
		refreshDisplay();
		refreshContext();
	}

	function refreshContext() {
		var ctx = "";
		if (me.data) {
			ctx += getChannelData().fr();
		}

		me.context(ctx);
	}

	function zeroFill( number, width ) {
		width -= number.toString().length;
		if (width > 0) {
			return new Array(width + (/\./.test( number ) ? 2 : 1)).join('0') + number;
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
		me.isKlarDisabled = ko.observable(false);

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

	function Ra180Clock() {
		var me = this;

		var seconds = 0;
		var minutes = 0;
		var hours = 0;
		var day = 1;
		var month = 1;

		me.getTid = function () {
			var text = zeroFill(hours, 2) + zeroFill(minutes, 2) + zeroFill(seconds, 2);
			return text;
		};
		
		me.getDat = function () {
			var text = zeroFill(month, 2) + zeroFill(day, 2);
			return text;
		};

		me.setTid = function (value) {
			var regex = /^([0-9]{2})([0-9]{2})([0-9]{2})$/;
			var match = regex.exec(value);
			if (!match) return false;
			var sHours = match[1];
			var sMinutes = match[2];
			var sSeconds = match[3];
			var nHours = parseInt(sHours);
			var nMinutes = parseInt(sMinutes);
			var nSeconds = parseInt(sSeconds);
			if (nHours < 0 || nHours > 23) return false;
			if (nMinutes < 0 || nMinutes > 59) return false;
			if (nSeconds < 0 || nSeconds > 59) return false;
			seconds = nSeconds;
			minutes = nMinutes;
			hours = nHours;
			return true;
		};

		me.setDat = function (value) {
			var regex = /^([0-9]{2})([0-9]{2})$/;
			var match = regex.exec(value);
			if (!match) return false;
			var sMonth = match[1];
			var sDay = match[2];
			var nMonth = parseInt(sMonth);
			var nDay = parseInt(sDay);
			if (nMonth < 1 || nMonth > 12) return false;
			if (nDay < 1 || nDay > getDaysInMonth(nMonth)) return false;
			month = nMonth;
			day = nDay;
			return true;
		};

		me.tick = function () {
			seconds++;
			
			if (seconds >= 60) {
				seconds = 0;
				minutes++;
			}

			if (minutes >= 60) {
				minutes = 0;
				hours++;
			}

			if (hours >= 24) {
				hours = 0;
				day++;
			}

			var daysInMonth = getDaysInMonth(month);
			if (day > daysInMonth) {
				day = 1;
				month++;
			}

			if (month > 12) {
				month = 1;
			}
		};

		function getDaysInMonth(month) {
			switch (month) {
				case 2:
					return 28;

				case 4:
				case 6:
				case 9:
				case 11:
					return 30;

				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
				default:
					return 31;
			}
		}
	}

	function Ra180Data() {
		var me = this;

		me.synk = ko.observable();
		me.eff = ko.observable();
		me.sdx = ko.observable();
		me.opmtn = ko.observable();
		me.rap = ko.observable();

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

		me.reset = function () {
			me.synk(false);
			me.eff("LÅG");
			me.sdx(false);
			me.opmtn(false);
			me.rap("UPPK");
		
			me.channel1.reset();
			me.channel1.fr("30025");
			me.channel1.bd1("9000");
			me.channel1.bd2("");

			me.channel2.reset();
			me.channel2.fr("40025");
			me.channel2.bd1("9000");
			me.channel2.bd2("");

			me.channel3.reset();
			me.channel3.fr("50025");
			me.channel3.bd1("9000");
			me.channel3.bd2("");

			me.channel4.reset();
			me.channel4.fr("60025");
			me.channel4.bd1("9000");
			me.channel4.bd2("");

			me.channel5.reset();
			me.channel5.fr("70025");
			me.channel5.bd1("9000");
			me.channel5.bd2("");

			me.channel6.reset();
			me.channel6.fr("80025");
			me.channel6.bd1("9000");
			me.channel6.bd2("");

			me.channel7.reset();
			me.channel7.fr("87975");
			me.channel7.bd1("9000");
			me.channel7.bd2("");

			me.channel8.reset();
			me.channel8.fr("42025");
			me.channel8.bd1("9000");
			me.channel8.bd2("");

			me.isEmpty(true);
		};
	}

	function Ra180Display() {
		var self = this;
		self.char1 = new Ra180DisplayCharacter();
		self.char2 = new Ra180DisplayCharacter();
		self.char3 = new Ra180DisplayCharacter();
		self.char4 = new Ra180DisplayCharacter();
		self.char5 = new Ra180DisplayCharacter();
		self.char6 = new Ra180DisplayCharacter();
		self.char7 = new Ra180DisplayCharacter();
		self.char8 = new Ra180DisplayCharacter();

		self.brightness = ko.observable(5);

		self.toggleBrightness = function () {
			var value = self.brightness();
			value++;
			if (value > 5) value = 0;
			self.brightness(value);
		};
		
		self.getPlainText = function () {
			var result = "";
			result += self.char1.character();
			result += self.char2.character();
			result += self.char3.character();
			result += self.char4.character();
			result += self.char5.character();
			result += self.char6.character();
			result += self.char7.character();
			result += self.char8.character();
			return result;
		};

		self.setInputText = function (text) {
			self.setText(text);
			self.char8.hasUnderscore(text.length >= 7);
			self.char7.hasUnderscore(text.length == 6);
			self.char6.hasUnderscore(text.length == 5);
			self.char5.hasUnderscore(text.length == 4);
			self.char4.hasUnderscore(text.length == 3);
			self.char3.hasUnderscore(text.length == 2);
			self.char2.hasUnderscore(text.length == 1);
			self.char1.hasUnderscore(text.length == 0);
			self.char8.isBlinking(text.length >= 8);
			self.char1.isBlinking(text.length > 0 && text[0] == ":");
			self.char2.isBlinking(text.length > 1 && text[1] == ":");
			self.char3.isBlinking(text.length > 2 && text[2] == ":");
			self.char4.isBlinking(text.length > 3 && text[3] == ":");
			self.char5.isBlinking(text.length > 4 && text[5] == ":");
			self.char6.isBlinking(text.length > 5 && text[5] == ":");
			self.char7.isBlinking(text.length > 6 && text[6] == ":");
			self.char8.isBlinking(text.length > 7 && text[7] == ":");
		};
		
		self.setText = function (text) {
			self.char1.character(text.length > 0 ? text[0] : " ");
			self.char2.character(text.length > 1 ? text[1] : " ");
			self.char3.character(text.length > 2 ? text[2] : " ");
			self.char4.character(text.length > 3 ? text[3] : " ");
			self.char5.character(text.length > 4 ? text[4] : " ");
			self.char6.character(text.length > 5 ? text[5] : " ");
			self.char7.character(text.length > 6 ? text[6] : " ");
			self.char8.character(text.length > 7 ? text[7] : " ");
			self.setInputPos(-1);
		},

		self.getCharacter = function (pos) {
			switch (pos) {
				case 0: return self.char1; break;
				case 1: return self.char2; break;
				case 2: return self.char3; break;
				case 3: return self.char4; break;
				case 4: return self.char5; break;
				case 5: return self.char6; break;
				case 6: return self.char7; break;
				case 7: return self.char8; break;
			}
		};

		self.setCharacterBlinking = function (pos, value) {
			var character = self.getCharacter(pos);
			if (!character) {
				return;
			}
			character.isBlinking(value);
		};
		
		self.setInputPos = function (pos) {
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
		};
	}
}
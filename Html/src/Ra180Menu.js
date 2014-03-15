function Ra180Menu(options, ra180) {
	var me = this;
	me.inputText = "";
	me.isEditing = false;
	me.ra180 = ra180;
	me.selectedIndex = -1;

	function getSafeOptions (options) {
		options = options !== undefined ? options : {};
		options.title = options.title !== undefined ? options.title : "";
		options.submenus = options.submenus !== undefined ? options.submenus : [];
		options.completed = options.completed !== undefined ? options.completed : function () {};
		return options;
	};

	function getSafeSubmenu (submenu) {
		submenu = submenu !== undefined ? submenu : {};
		submenu.prefix = submenu.prefix !== undefined ? submenu.prefix : function () { return "?"; };
		submenu.maxInputLength = submenu.maxInputLength !== undefined ? submenu.maxInputLength : 0;
		submenu.canEdit = submenu.canEdit !== undefined ? submenu.canEdit : function () { return false; };
		submenu.canSelect = submenu.canSelect !== undefined ? submenu.canSelect : function () { return false; };
		submenu.saveInput = submenu.saveInput !== undefined ? submenu.saveInput : function () { return false; };
		submenu.getValue = submenu.getValue !== undefined ? submenu.getValue : function () { return ""; };
		submenu.getOptions = submenu.getOptions !== undefined ? submenu.getOptions : function () { return []; };
		submenu.nextOption = submenu.nextOption !== undefined ? submenu.nextOption : function () { };

		if (typeof submenu.canEdit !== 'function') {
			var canEditValue = submenu.canEdit;
			submenu.canEdit = function () { return canEditValue; };
		}
		if (typeof submenu.canSelect !== 'function') {
			var canSelectValue = submenu.canSelect;
			submenu.canSelect = function () { return canSelectValue; };
		}
		if (typeof submenu.prefix !== 'function') {
			var prefixValue = submenu.prefix;
			submenu.prefix = function () { return prefixValue; };
		}
		if (typeof submenu.nextOption !== 'function') {
			var nextOptionValue = submenu.nextOption;
			submenu.nextOption = function () { return nextOptionValue; }
		}

		return submenu;
	};
	
	me.options = getSafeOptions(options);
	me.currentSubmenuIndex = -1;
	me.currentSubmenu = null;

	me.close = function() {
		me.options.completed();
	};

	me.previousSubmenu = function () {
		me.currentSubmenuIndex = me.currentSubmenuIndex - 1;
		me.refreshSubmenu();
	};

	me.nextSubmenu = function () {
		me.currentSubmenuIndex = me.currentSubmenuIndex + 1;
		me.refreshSubmenu();
	};
	
	me.refreshSubmenu = function () {
		if (me.currentSubmenuIndex < me.options.submenus.length) {
			me.currentSubmenu = getSafeSubmenu(me.options.submenus[me.currentSubmenuIndex]);
			me.refreshDisplay();
		} else if (me.currentSubmenuIndex == me.options.submenus.length) {
			me.currentSubmenu = undefined;
			me.refreshDisplay();
		} else {
			me.currentSubmenu = undefined;
			me.options.completed();
		}
	};

	me.startInput = function () {
		me.inputText = "";
		me.isEditing = true;
		me.refreshDisplay();
	};
	me.stopInput = function () {
		me.inputText = "";
		me.isEditing = false;
		me.refreshDisplay();
	};

	me.onKeyOpm = function () {
	};
	me.onKeyEff = function () {
	};
	me.onKeyAnd = function () {
		if (me.currentSubmenu.canSelect()) {
			var options = me.currentSubmenu.getOptions();
			var index = me.selectedIndex + 1;
			if (index > options.length) {
				index = 0;
			}
			me.selectedIndex = index;
			me.currentSubmenu.nextOption(me.selectedIndex);
			me.refreshDisplay();
			return;
		}

		if (!me.currentSubmenu.canEdit()) {
			return;
		}
		if (!me.isEditing) {
			me.startInput();
		}
	};
	me.onKeyBel = function () {
	};
	me.onKeySlt = function () {
		if (me.isEditing) {
			me.stopInput();
		} else {
			me.stopInput();
			me.options.completed();
		}
	};
	me.onKeyEnt = function () {
		if (me.isEditing) {
			var isValidInput = me.currentSubmenu.saveInput(me.inputText, me);
			if (isValidInput) {
				me.stopInput();
			} else {
				me.startInput();
			}
		} else {
			me.nextSubmenu();
		}
	};
	me.onKeyChar = function (key) {
		if (me.inputText.length >= me.currentSubmenu.maxInputTextLength) {
			return;
		}
		me.inputText = me.inputText + key;
		me.refreshDisplay();
	};
	me.onKeyReset = function () {
	};

	me.sendKey = function (key) {
		var submenu = me.currentSubmenu;
		if (typeof submenu !== 'undefined' && typeof submenu.onKey === 'function' && submenu.onKey(key, me)) {
			return;
		}

		switch (key) {
			case "OPM": me.onKeyOpm(); break;
			case "EFF": me.onKeyEff(); break;
			case "AND": me.onKeyAnd(); break;
			case "BEL": me.onKeyBel(); break;
			case "SLT": me.onKeySlt(); break;
			case "ENT": me.onKeyEnt(); break;
			case "RESET": me.onKeyReset(); break;
			default:
				if (key.length == 1) {
					me.onKeyChar(key);
				}
				break;
		}
	};

	me.refreshDisplay = function () {
		if (me.currentSubmenuIndex == me.options.submenus.length) {
			me.ra180.display.setText("  (" + me.options.title + ")");
			return;
		}
		if (!me.currentSubmenu) {
			me.ra180.display.setText("");
			return;
		}

		var value;
		var prefix = me.currentSubmenu.prefix(me);

		if (me.isEditing) {
			value = me.inputText;
			me.ra180.display.setInputText(prefix + ":" + value);
			if (value.length >= me.currentSubmenu.maxInputTextLength) {
				me.ra180.display.setCharacterBlinking(prefix.length + value.length, true);
			}
		} else {
			value = me.currentSubmenu.getValue();
			if (me.currentSubmenu.canEdit()) {
				me.ra180.display.setText(prefix + ":" + value);
			} else if (me.currentSubmenu.canSelect()) {
				// Enligt SoldR Mtrl Tele, 1993, s.53
				// "text med blinkande kolon, t ex OPMTN:PÅ"
				//  innebär att valmöjligheter finns."
				me.ra180.display.setText(prefix + ":" + value);
				me.ra180.display.setCharacterBlinking(prefix.length);
			} else {
				me.ra180.display.setText(prefix + "=" + value);
			}
		}
	};

	me.nextSubmenu();
}
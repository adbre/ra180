function Ra180TidMenu(ra180, completed) {
	var me = this;
	me.ra180 = ra180;
	me.completed = completed;
	
	me.input = "";
	
	me.tidSubmenu = {
		setDisplayText: function () {
			var tid = me.ra180.tid();
			if (me.isEditing) {
				tid = me.input;
			}
			me.ra180.display.setText("T:" + tid);
			if (me.isEditing) {
				me.ra180.display.setInputPos(me.input.length < 6 ? me.input.length + 2 : 7);
				me.ra180.display.setCharacterBlinking(1, true);
			}
		},
		sendKey: function (key) {
			if (key == "ENT") {
				if (me.input.length != 6) {
					me.startEditing();
				} else {
					me.ra180.tid(me.input);
					me.stopEditing();
				}
				return true;
			}
		}
	};
	me.datSubmenu = {
		setDisplayText: function () {
			if (me.isEditing) {
				me.ra180.display.setInputText("DAT:" + me.input);
			} else {
				me.ra180.display.setText("DAT:" + me.ra180.dat());
			}
		},
		sendKey: function (key) {
			if (key == "ENT") {
				if (me.input.length != 4) {
					me.startEditing();
				} else {
					me.ra180.dat(me.input);
					me.stopEditing();
				}
				return true;
			}
		}
	};
	me.titleSubmenu = {
		setDisplayText: function () {
			me.ra180.display.setText("  (TID)");
		},
		sendKey: function (key) {
		}
	};

	me.isEditing = false;
	me.currentSubmenu = me.tidSubmenu;

	me.startEditing = function () {
		me.isEditing = true;
		me.input = "";
	};
	me.stopEditing = function () {
		me.isEditing = false;
		me.input = "";
	};

	me.setDisplayText = function() {
		me.currentSubmenu.setDisplayText();
	};

	me.sendKey = function (key) {
		if (me.isEditing) {
			if (key == "SLT") {
				me.stopEditing();
				me.setDisplayText();
			} else {
				if (!me.currentSubmenu.sendKey(key)) {
					switch (key) {
						case "1":
						case "2":
						case "3":
						case "4":
						case "5":
						case "6":
						case "7":
						case "8":
						case "9":
						case "0":
							me.input = me.input + key;
							break;
					}
				}

				me.setDisplayText();
			}

			return;
		}

		if (key == "AND") {
			if (me.currentSubmenu == me.tidSubmenu) {
				me.startEditing();
			}
			if (me.currentSubmenu == me.datSubmenu) {
				me.startEditing();
			}

			me.setDisplayText();
			return;
		}
		
		if (key == "ENT") {
			if (me.currentSubmenu == me.tidSubmenu) {
				me.currentSubmenu = me.datSubmenu;
			} else if (me.currentSubmenu == me.datSubmenu) {
				me.currentSubmenu = me.titleSubmenu;
			} else if (me.currentSubmenu == me.titleSubmenu) {
				me.completed();
				return;
			}
		} else if (key == "SLT") {
			me.completed();
			return;
		}

		me.setDisplayText();
	};

	me.setDisplayText();
}

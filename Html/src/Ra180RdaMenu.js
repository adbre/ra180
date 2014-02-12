function Ra180RdaMenu(ra180, completed) {
	var me = this;
	me.ra180 = ra180;
	me.completed = completed;
	
	me.menuText = new Array("SDX=NEJ","OPMTN=JA","BAT=12.5","  (RDA)");
	me.currentIndex = 0;

	me.setDisplayText = function () {
		var text = me.menuText[me.currentIndex];
		me.ra180.display.setText(text);
	};

	me.sendKey = function (key) {
		if (key == "ENT") {
			me.currentIndex = me.currentIndex + 1;
			if (me.currentIndex >= me.menuText.length) {
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

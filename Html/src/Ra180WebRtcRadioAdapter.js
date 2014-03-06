function Ra180WebRtcRadioAdapter(ra180, radio) {
	var me = this;
	
	me.ra180 = ra180;
	me.radio = radio;

	function onRa180ModChanged() {
		if (me.ra180.mod() == me.ra180.MOD_OFF) {
			me.radio.disable();
		} else {
			me.radio.enable();
		}
	}

	me.ra180.mod.subscribe(onRa180ModChanged);

	onRa180ModChanged();
}
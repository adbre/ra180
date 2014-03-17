function Ra180WebRtcRadioAdapter(ra180, radio) {
	var me = this;
	
	me.ra180 = ra180;
	me.radio = radio;

	function onRa180ModChanged() {
		if (!me.ra180.isEnabled()) {
			me.radio.disable();
		} else {
			me.radio.enable();
		}
	}

	me.ra180.isEnabled.subscribe(onRa180ModChanged);
	me.ra180.context.subscribe(function () {
		//me.radio.changeContext(me.ra180.context());
	});

	onRa180ModChanged();
}
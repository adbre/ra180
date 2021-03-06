function RtcRadio(options) {
	var me = this;
	var peerBroker, rtc, myCtx, mainCtx = "ee58a08bf68043539ef41d7c0ed0c553";
	var isAudioMuted = false;

	var onContextCreated = function (brokerContext) {
		console.log("BrokerContext", brokerContext);
	};

	var onLocalStreamCreated = function (stream) {
		console.log("A stream was added, i will soon pop up...", stream);
	};

	var onLocalStream = function (stream) {
		muteLocalAudio(!shouldEnableOut());
	};

	var onRemoteStream = function (event) {
		// First, remove prior audio elements belonging to this Peer
		me.audio.remove(function (audio) {
			return audio.rel() == event.stream.id;
		});

		var audio = new WebRtcRadioAudio(event);
		audio.muted(!shouldEnableIn());
		me.audio.push(audio);
		attachMediaStream($("#" + event.PeerId).get(0), event.stream);
	};

	var onPeerConnectionLost = function (peerConnection) {
		console.log("Lost a peer connection", peerConnection);
		me.audio.remove(function (audio) {
			return audio.id() == peerConnection.PeerId;
		});
	};

	var onRemoteStreamLost = function (stream) {
		console.log("Lost a remote stream", stream);
		me.audio.remove(function (audio) {
			return audio.rel() == stream.StreamId;
		});
	};

	var onPeerConnectionStarted = function (peerConnection) {
		console.log("PeerConnection Started", peerConnection);
	};

	var onContextChange = function (context) {
		// You will recive a list of Peers on the current context
		console.log("Conext changed", context);
	};

	me.brokerUri = brokerUri;	
	me.audio = ko.observableArray([]);
	me.isTransmitting = ko.observable(false);
	me.isEnabled = ko.observable(false);

	me.enable = function () {
		me.isEnabled(true);
		me.receive();
	};

	me.disable = function () {
		me.isEnabled(false);
		muteLocalAudio(true);
		muteRemoteAudio(true);
	};

	me.changeContext = function(ctx) {
		if (myCtx == ctx) return;
		myCtx = ctx;
		if (rtc) {
			console.log("Changing context", ctx);
			var ids = rtc.getRemotePeers();
			for (var i=0; i < ids.length; i++) {
				rtc.removePeerConnection(ids[i]);
			}
			rtc.changeContext(ctx);
		}
	};

	me.isEnabled.subscribe(function (newValue) {
		me.receive();
	});
	
	me.transmit = function () {
		if (!me.isEnabled()) return;
		me.isTransmitting(true);
		muteLocalAudio(false);
		muteRemoteAudio(true);
	};

	me.receive = function () {
		me.isTransmitting(false);
		if (me.isEnabled()) {
			muteLocalAudio(true);
			muteRemoteAudio(false);
		} else {
			muteLocalAudio(true);
			muteRemoteAudio(true);
		}
	};

	me.initialize = function () {
		peerBroker = new XSockets.WebSocket(me.brokerUri);
		peerBroker.on(XSockets.Events.open, function (brokerClient) {
			console.log("brokerClient", brokerClient);

			myCtx = brokerClient.ClientGuid;

			rtc = new XSockets.WebRTC(peerBroker);

			rtc.bind(XSockets.WebRTC.Events.onContextCreated, onContextCreated);
			rtc.bind(XSockets.WebRTC.Events.onlocalStream, onLocalStream);
			rtc.bind(XSockets.WebRTC.Events.onLocalStreamCreated, onLocalStreamCreated);
			rtc.bind(XSockets.WebRTC.Events.onRemoteStream, onRemoteStream);
			rtc.bind(XSockets.WebRTC.Events.onPeerConnectionLost, onPeerConnectionLost);
			rtc.bind(XSockets.WebRTC.Events.onRemoteStreamLost, onRemoteStreamLost);
			rtc.bind(XSockets.WebRTC.Events.onPeerConnectionStarted, onPeerConnectionStarted);
			rtc.bind(XSockets.WebRTC.Events.onContextChange, onContextChange);

			rtc.getUserMedia({audio: true}, function () {
				me.changeContext(myCtx);
			});

			me.receive();
		});
	};

	function WebRtcRadioAudio(event) {
		this.autoplay = ko.observable(true);
		this.id = ko.observable(event.PeerId);
		this.rel = ko.observable(event.stream.id);
		this.muted = ko.observable(false);
	}

	function shouldEnableOut() {
		return me.isEnabled() && me.isTransmitting();
	}

	function shouldEnableIn() {
		return me.isEnabled() && !me.isTransmitting();
	}

	function muteLocalAudio(value) {
		if (!rtc) return;
		console.log("muting local audio", value);
		rtc.muteAudio(function (muted) {
			if (muted != value) {
				rtc.muteAudio();
			}
			isAudioMuted = value;
		});
	}

	function muteRemoteAudio(value) {
		console.log("muting remote audio", value);
		$.each(me.audio(), function (i, audio) {
			audio.muted(value);
		});
	}
}
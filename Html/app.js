import $ from 'jquery';
import ko from 'knockout';

import { SynchronizationContext } from './src/SynchronizationContext.js';
import { Ra180ViewModel } from './src/Ra180.js';
import { RtcRadio } from './src/RtcRadio.js';

// The old XSockets broker (ws://malinrocks.cloudapp.net:4502) is long gone.
// Point this at a compatible one to get voice back; see src/RtcRadio.js.
var BROKER_URI = null;

$(function () {
    var radio = new RtcRadio(BROKER_URI);
    var ra180 = new Ra180ViewModel(new SynchronizationContext(), radio);

    ko.applyBindings(ra180, $('#ra180')[0]);
    ko.applyBindings(radio, $('#webrtc-radio')[0]);
    radio.initialize();
});

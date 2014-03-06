function SynchronizationContext() {
	var me = this;
	me.setInterval = function (fn, interval) {
		setInterval(fn, interval);
	};

	me.setTimeout = function (fn, delay) {
		setTimeout(fn, delay);
	};
	me.clearInterval = function(id) {
		clearInterval(id);
	};
	me.clearTimeout = function(id) {
		clearTimeout(id);
	};
}

function InstantSynchronizationContext() {
	this.setInterval = function (fn, interval) {
		
	};
	this.setTimeout = function (fn, interval) {
		fn();
	};
	this.clearInterval = function (id) {};
	this.clearTimeout = function (id) {};
}

function MockSynchronizationContext() {
	var me = this;
	var elapsedTicks = 0;
	var timers = [];
	
	function Timer(fn, delay, interval, elapsedTicks) {
		var me = this;
		var nextRing;

		me.ring = function (elapsedTicks) {
			if (elapsedTicks == nextRing) {
				calculateNextRing(elapsedTicks, interval);
				fn();
			}
		};

		function calculateNextRing(elapsedTicks, delay) {
			if (delay == -1) {
				nextRing = -1;
			} else {
				nextRing = elapsedTicks + delay;
			}
		}

		calculateNextRing(elapsedTicks, delay);
	}

	function createNewTimer(fn, delay, interval) {
		var timer = new Timer(fn, delay, interval, elapsedTicks);
		timer.id = timers.length + 1;
		timers.push(timer);
		return timer.id;
	}
	
	me.setInterval = function (fn, interval) {
		return createNewTimer(fn, interval, interval);
	};

	me.setTimeout = function (fn, delay) {
		return createNewTimer(fn, delay, -1);
	};
	me.clearInterval = function(id) {
		for (var i=0; i < timers.length; i++) {
			if (timers[i].id == id) {
				timers[i] = null;
			}
		}
	};
	me.clearTimeout = function(id) {
		me.clearInterval(id);
	};
	me.tick = function (ticks) {
		for (var n = 0; n < ticks ; n++) {
			elapsedTicks++;
			for (var i=0; i < timers.length; i++) {
				var timer = timers[i];
				if (timer) {
					timer.ring(elapsedTicks);
				}
			}
		}
	};
}

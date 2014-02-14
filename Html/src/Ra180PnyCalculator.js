function Ra180PnyCalculator() {
	var me = this;
	me.isValidPn = function (text) {
		if (typeof text !== 'string' || text.length != 4) {
			return false;
		}
		if (!text.match(/^[0-9]{4}$/)) {
			return false;
		}

		var challange = text.substring(0, 3);
		var validPn = me.calculatePn(challange);
		var result = validPn == text;
		return result;
	};
	me.calculatePn = function (text) {
		if (typeof text !== 'string' || text.length != 3) {
			return false;
		}
		if (!text.match(/^[0-9]{3}$/)) {
			return false;
		}

		var sum = me.calculateChecksum(text);
		return text + sum;
	};
	me.calculateChecksum = function (text) {
		var sum = 0;
		var chars = text.split('');
		chars.forEach(function(c) {
			var n = parseInt(c);
			sum = sum ^ n;
		});
		return sum;
	};
	me.calculatePny = function (pn) {
		var groups = ["", "", ""];
		for (var i=0; i < pn.length; i++) {
			for (var n=0; n < groups.length; n++) {
				groups[n] += pn[i][n];
			}
		}

		var checksums = [];
		for (var i=0; i < groups.length; i++) {
			var group = groups[i];
			var checksum = me.calculateChecksum(group);
			checksums.push(checksum);
		}

		return result = checksums.join("");
		return result;
	};
	var random = function () {
		var min = 0, max = 7;
		return Math.floor(Math.random() * (max - min + 1)) + min;
	};
	me.generateKeys = function () {
		var count = 8;
		var pn = [];
		for (var i=0; i < count; i++) {
			var s = "";
			s += random();
			s += random();
			s += random();
			s = me.calculatePn(s);
			pn.push(s);
		}
		return {
			pn: pn,
			pny: me.calculatePny(pn)
		};
	};
}
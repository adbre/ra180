describe("Ra180 PNY Calculator", function () {
	var calc;

	beforeEach(function () {
		calc = new Ra180PnyCalculator();
	});

	it("should calculate pn checksum", function () {
		expect(calc.calculatePn("751")).toBe("7513");
		expect(calc.calculatePn("442")).toBe("4422");
		expect(calc.calculatePn("221")).toBe("2211");
		expect(calc.calculatePn("330")).toBe("3300");
		expect(calc.calculatePn("551")).toBe("5511");
		expect(calc.calculatePn("432")).toBe("4325");
		expect(calc.calculatePn("562")).toBe("5621");
		expect(calc.calculatePn("320")).toBe("3201");
		expect(calc.calculatePn("510")).toBe("5104");
		expect(calc.calculatePn("350")).toBe("3506");
	});

	it("should invalidate pn", function () {
		expect(calc.isValidPn("7510")).toBe(false);
		expect(calc.isValidPn("4421")).toBe(false);
		expect(calc.isValidPn("2210")).toBe(false);
	});

	it("should validate pn", function () {
		expect(calc.isValidPn("7513")).toBe(true);
		expect(calc.isValidPn("4422")).toBe(true);
		expect(calc.isValidPn("2211")).toBe(true);
	});

	it("should calculate pny", function () {
		expect(calc.calculatePny(["4422", "2211", "3300", "5511", "4325", "5621", "3201", "5104"])).toBe("762");
	});

	it("should generate valid pn", function () {
		var keys = calc.generateKeys();

		for (var i=0; i < keys.pn.length; i++) {
			expect(calc.isValidPn(keys.pn[i])).toBe(true);
		}

		expect(keys.pny).toBe(calc.calculatePny(keys.pn));
	});
});
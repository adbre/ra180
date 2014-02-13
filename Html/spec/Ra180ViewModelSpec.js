describe("Ra180", function() {
	var ra180;
	
	beforeEach(function() {
		ra180 = new Ra180ViewModel();
	});

	describe("Vred manipulation", function() {

		it("has MOD_OFF to 3", function() {
			expect(ra180.MOD_OFF).toBe(3);
		});
		it("has MOD_KLAR to 4", function() {
			expect(ra180.MOD_KLAR).toBe(4);
		});
		it("has MOD_SKYDD to 5", function() {
			expect(ra180.MOD_SKYDD).toBe(5);
		});
		it("has MOD_DRELAY to 5", function() {
			expect(ra180.MOD_DRELAY).toBe(6);
		});
		it("sets MOD to KLAR", function() {
			ra180.setModKlar();
			expect(ra180.mod()).toBe(ra180.MOD_KLAR);
		});
		it("sets MOD to SKYDD", function() {
			ra180.setModSkydd();
			expect(ra180.mod()).toBe(ra180.MOD_SKYDD);
		});
		it("sets MOD to DRELAY", function() {
			ra180.setModDRelay();
			expect(ra180.mod()).toBe(ra180.MOD_DRELAY);
		});

		it("starts with CHANNEL 1", function() {
			expect(ra180.channel()).toBe(1);
		});

		it("starts with VOLUME 4", function() {
			expect(ra180.volume()).toBe(4);
		});

		it("can change CHANNEL", function() {
			ra180.setChannel8();
			expect(ra180.channel()).toBe(8);
			ra180.setChannel7();
			expect(ra180.channel()).toBe(7);
			ra180.setChannel6();
			expect(ra180.channel()).toBe(6);
			ra180.setChannel5();
			expect(ra180.channel()).toBe(5);
			ra180.setChannel4();
			expect(ra180.channel()).toBe(4);
			ra180.setChannel3();
			expect(ra180.channel()).toBe(3);
			ra180.setChannel2();
			expect(ra180.channel()).toBe(2);
			ra180.setChannel1();
			expect(ra180.channel()).toBe(1);
		});

		it("can change VOLUME", function() {
			ra180.setVolume8();
			expect(ra180.volume()).toBe(8);
			ra180.setVolume7();
			expect(ra180.volume()).toBe(7);
			ra180.setVolume6();
			expect(ra180.volume()).toBe(6);
			ra180.setVolume5();
			expect(ra180.volume()).toBe(5);
			ra180.setVolume4();
			expect(ra180.volume()).toBe(4);
			ra180.setVolume3();
			expect(ra180.volume()).toBe(3);
			ra180.setVolume2();
			expect(ra180.volume()).toBe(2);
			ra180.setVolume1();
			expect(ra180.volume()).toBe(1);
		});
	});

	describe("display manipulation", function () {
	
		beforeEach(function () {
			ra180.setModDebug();
		});
		
		it("changes display text", function() {
			ra180.display.setText("12345678");
			expect(ra180.display.char1.character()).toBe("1");
			expect(ra180.display.char2.character()).toBe("2");
			expect(ra180.display.char3.character()).toBe("3");
			expect(ra180.display.char4.character()).toBe("4");
			expect(ra180.display.char5.character()).toBe("5");
			expect(ra180.display.char6.character()).toBe("6");
			expect(ra180.display.char7.character()).toBe("7");
			expect(ra180.display.char8.character()).toBe("8");
			expect(ra180.display.getPlainText()).toBe("12345678");
		});

		it("clear empty display positions when given short string", function() {
			ra180.display.setText("");
			expect(ra180.display.char1.character()).toBe(" ");
			expect(ra180.display.char2.character()).toBe(" ");
			expect(ra180.display.char3.character()).toBe(" ");
			expect(ra180.display.char4.character()).toBe(" ");
			expect(ra180.display.char5.character()).toBe(" ");
			expect(ra180.display.char6.character()).toBe(" ");
			expect(ra180.display.char7.character()).toBe(" ");
			expect(ra180.display.char8.character()).toBe(" ");
		});

		it("should clear blinking characters when setting text", function() {
			ra180.display.setText("12345678");
			ra180.display.setCharacterBlinking(3, true);
			expect(ra180.display.char4.isBlinking()).toBe(true);
			ra180.display.setText("abcdefgh");
			expect(ra180.display.char4.isBlinking()).toBe(false);
		});
	});

	describe("Menu navigation", function() {
		beforeEach(function() {
			ra180.setModKlar();
		});

		describe("RDA", function() {
			it("should navigate RDA", function() {
				ra180.sendKey2();
				expect(ra180.display.getPlainText()).toBe("SDX=NEJ ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("OPMTN=JA");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("BAT=12.5");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (RDA) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu from RDA on SLT", function() {
				ra180.sendKey2();
				expect(ra180.display.getPlainText()).toBe("SDX=NEJ ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");

				ra180.sendKey2();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("OPMTN=JA");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");

				ra180.sendKey2();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("BAT=12.5");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
				
				ra180.sendKey2();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (RDA) ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});

		describe("TID", function () {

			it("should navigate TID", function () {
				ra180.sendKey1();
				expect(ra180.display.getPlainText()).toBe("T:000000");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("DAT:0101");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (TID) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");

				ra180.sendKey1();
				expect(ra180.display.getPlainText()).toBe("T:000000");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");

				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("DAT:0101");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");

				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (TID) ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should modify T", function () {
				ra180.sendKey1();
				ra180.sendKeyAnd();
				expect(ra180.display.getPlainText()).toBe("T:      ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char3.hasUnderscore()).toBe(true);

				ra180.sendKey2();
				expect(ra180.display.getPlainText()).toBe("T:2     ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char3.hasUnderscore()).toBe(false);
				expect(ra180.display.char4.hasUnderscore()).toBe(true);

				ra180.sendKey0();
				expect(ra180.display.getPlainText()).toBe("T:20    ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char4.hasUnderscore()).toBe(false);
				expect(ra180.display.char5.hasUnderscore()).toBe(true);

				ra180.sendKey3();
				expect(ra180.display.getPlainText()).toBe("T:203   ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char5.hasUnderscore()).toBe(false);
				expect(ra180.display.char6.hasUnderscore()).toBe(true);

				ra180.sendKey4();
				expect(ra180.display.getPlainText()).toBe("T:2034  ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char6.hasUnderscore()).toBe(false);
				expect(ra180.display.char7.hasUnderscore()).toBe(true);

				ra180.sendKey5();
				expect(ra180.display.getPlainText()).toBe("T:20345 ");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char7.hasUnderscore()).toBe(false);
				expect(ra180.display.char8.hasUnderscore()).toBe(true);

				ra180.sendKey6();
				expect(ra180.display.getPlainText()).toBe("T:203456");
				expect(ra180.display.char2.isBlinking()).toBe(true);
				expect(ra180.display.char8.isBlinking()).toBe(true);
				expect(ra180.display.char8.hasUnderscore()).toBe(true);

				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("T:203456");
				expect(ra180.display.char2.isBlinking()).toBe(false);
				expect(ra180.display.char8.isBlinking()).toBe(false);
				expect(ra180.display.char8.hasUnderscore()).toBe(false);
			});

			it("should persist T", function () {
				ra180.sendKey1();
				ra180.sendKeyAnd();
				ra180.sendKey1();
				ra180.sendKey9();
				ra180.sendKey2();
				ra180.sendKey8();
				ra180.sendKey3();
				ra180.sendKey7();
				ra180.sendKeyEnt();
				ra180.sendKeySlt();
				ra180.sendKey1();
				expect(ra180.display.getPlainText()).toBe("T:192837");
			});

			it("should abort input on SLT", function () {
				ra180.sendKey1();
				ra180.sendKeyAnd();
				ra180.sendKey1();
				ra180.sendKey9();
				ra180.sendKey2();
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("T:000000");
			});

			it("should modify DAT", function () {
				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeyAnd();
				expect(ra180.display.getPlainText()).toBe("DAT:    ");
				expect(ra180.display.char4.isBlinking()).toBe(true);
				expect(ra180.display.char5.hasUnderscore()).toBe(true);

				ra180.sendKey0();
				expect(ra180.display.getPlainText()).toBe("DAT:0   ");
				expect(ra180.display.char4.isBlinking()).toBe(true);
				expect(ra180.display.char5.hasUnderscore()).toBe(false);
				expect(ra180.display.char6.hasUnderscore()).toBe(true);

				ra180.sendKey9();
				expect(ra180.display.getPlainText()).toBe("DAT:09  ");
				expect(ra180.display.char4.isBlinking()).toBe(true);
				expect(ra180.display.char6.hasUnderscore()).toBe(false);
				expect(ra180.display.char7.hasUnderscore()).toBe(true);

				ra180.sendKey2();
				expect(ra180.display.getPlainText()).toBe("DAT:092 ");
				expect(ra180.display.char4.isBlinking()).toBe(true);
				expect(ra180.display.char7.hasUnderscore()).toBe(false);
				expect(ra180.display.char8.hasUnderscore()).toBe(true);

				ra180.sendKey8();
				expect(ra180.display.getPlainText()).toBe("DAT:0928");
				expect(ra180.display.char4.isBlinking()).toBe(true);
				expect(ra180.display.char7.hasUnderscore()).toBe(false);
				expect(ra180.display.char8.hasUnderscore()).toBe(true);

				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("DAT:0928");
				expect(ra180.display.char4.isBlinking()).toBe(false);
				expect(ra180.display.char7.hasUnderscore()).toBe(false);
				expect(ra180.display.char8.hasUnderscore()).toBe(false);
			});

			it("should abort input of DAT", function () {
				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeyAnd();
				ra180.sendKey1();
				ra180.sendKey2();
				ra180.sendKey3();
				ra180.sendKey1();
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("DAT:0101");
			});

			it("should persist DAT", function () {
				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeyAnd();
				ra180.sendKey1();
				ra180.sendKey2();
				ra180.sendKey3();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeySlt();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("DAT:1231");
			});
		});

		describe("DTM", function () {
			it("should display title", function () {
				ra180.sendKey3();
				expect(ra180.display.getPlainText()).toBe("  (DTM) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});

		describe("KDA", function() {
			it("should navigate KDA", function () {
				ra180.sendKey4();
				expect(ra180.display.getPlainText()).toMatch(/^FR:[0-9]{5}$/);
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^BD1:[0-9]{4}$/);
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^BD2:[0-9]{4}$/);
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("SYNK=NEJ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PNY:### ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (KDA) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from FR", function() {
				ra180.sendKey4();
				expect(ra180.display.getPlainText()).toMatch(/^FR:[0-9]{5}$/);
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from BD1", function() {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^BD1:[0-9]{4}$/);
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from BD2", function() {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^BD2:[0-9]{4}$/);
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from SYNK", function() {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("SYNK=NEJ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from PNY", function() {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PNY:### ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should return to main menu on SLT from KDA", function() {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (KDA) ");
				ra180.sendKeySlt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});

			it("should allow modification of SYNK when in sync", function() {
				ra180.data.synk(true);
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("SYNK:JA ");
			});

			it("should allow edit of frequency", function () {
				ra180.sendKey4();
				ra180.sendKeyAnd();
				ra180.sendKey6();
				ra180.sendKey5();
				ra180.sendKey4();
				ra180.sendKey3();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				ra180.sendKeySlt();
				ra180.sendKey4();
				expect(ra180.display.getPlainText()).toBe("FR:65431");
			});

			it("should reject frequency lower than 30.000 MHz", function () {
				ra180.sendKey4();
				ra180.sendKeyAnd();
				ra180.sendKey2();
				ra180.sendKey9();
				ra180.sendKey9();
				ra180.sendKey9();
				ra180.sendKey9();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).not.toBe("FR:29999");
			});

			it("should allow frequency equal to 30.000 MHz", function () {
				ra180.sendKey4();
				ra180.sendKeyAnd();
				ra180.sendKey3();
				ra180.sendKey0();
				ra180.sendKey0();
				ra180.sendKey0();
				ra180.sendKey0();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("FR:30000");
			});

			it("should reject frequency higher than 87.975 MHz", function () {
				ra180.sendKey4();
				ra180.sendKeyAnd();
				ra180.sendKey8();
				ra180.sendKey7();
				ra180.sendKey9();
				ra180.sendKey7();
				ra180.sendKey6();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).not.toBe("FR:87976");
			});

			it("should allow frequency equal to 87.975 MHz", function () {
				ra180.sendKey4();
				ra180.sendKeyAnd();
				ra180.sendKey8();
				ra180.sendKey7();
				ra180.sendKey9();
				ra180.sendKey7();
				ra180.sendKey5();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("FR:87975");
			});

			it("should modify BD2 after BD1", function () {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyAnd();
				ra180.sendKey4();
				ra180.sendKey5();
				ra180.sendKey5();
				ra180.sendKey5();
				expect(ra180.display.getPlainText()).toBe("BD1:4555");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("BD2:    ");
				ra180.sendKey6();
				ra180.sendKey5();
				ra180.sendKey7();
				ra180.sendKey5();
				expect(ra180.display.getPlainText()).toBe("BD2:6575");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("BD1:4555");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("BD2:6575");
			});

			it("should modify BD1 on AND for BD2", function () {
				ra180.sendKey4();
				ra180.sendKeyEnt();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^BD2:[0-9]{4}$/);
				ra180.sendKeyAnd();
				expect(ra180.display.getPlainText()).toBe("BD1:    ");
			});

			it("should modify PNY via PN1-8", function () {
				ra180.sendKey4();   // FR
				ra180.sendKeyEnt(); // BD1
				ra180.sendKeyEnt(); // BD2
				ra180.sendKeyEnt(); // SYNK
				ra180.sendKeyEnt(); // PNY=###
				ra180.sendKeyAnd();
				expect(ra180.display.getPlainText()).toBe("PN1:    ");
				ra180.sendKey4();
				ra180.sendKey4();
				ra180.sendKey2();
				ra180.sendKey2();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN2:    ");
				ra180.sendKey2();
				ra180.sendKey2();
				ra180.sendKey1();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN3:    ");
				ra180.sendKey3();
				ra180.sendKey3();
				ra180.sendKey0();
				ra180.sendKey0();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN4:    ");
				ra180.sendKey5();
				ra180.sendKey5();
				ra180.sendKey1();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN5:    ");
				ra180.sendKey4();
				ra180.sendKey3();
				ra180.sendKey2();
				ra180.sendKey5();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN6:    ");
				ra180.sendKey5();
				ra180.sendKey6();
				ra180.sendKey2();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN7:    ");
				ra180.sendKey3();
				ra180.sendKey2();
				ra180.sendKey0();
				ra180.sendKey1();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("PN8:    ");
				ra180.sendKey5();
				ra180.sendKey1();
				ra180.sendKey0();
				ra180.sendKey4();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toMatch(/^PNY:[0-9]{3} $/);
				ra180.sendKeySlt();
				ra180.sendKey4();   // FR
				ra180.sendKeyEnt(); // BD1
				ra180.sendKeyEnt(); // BD2
				ra180.sendKeyEnt(); // SYNK
				ra180.sendKeyEnt(); // PNY=###
				expect(ra180.display.getPlainText()).toMatch(/^PNY:[0-9]{3} $/);
			});
		});

		describe("NIV", function () {
			it("should display title", function () {
				ra180.sendKey5();
				expect(ra180.display.getPlainText()).toBe("  (NIV) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});

		describe("RAP", function () {
			it("should display title", function () {
				ra180.sendKey6();
				expect(ra180.display.getPlainText()).toBe("  (RAP) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});

		describe("NYK", function () {
			it("should not have active key by default", function () {
				ra180.sendKey7();
				expect(ra180.display.getPlainText()).toBe("NYK=### ");
			});

			describe("when entered passive key", function () {
				beforeEach(function () {
					ra180.sendKey4();
					ra180.sendKeyEnt(4);
					ra180.sendKeyAnd();
					ra180.sendKeys("4422");
					ra180.sendKeyEnt();
					ra180.sendKeys("2211");
					ra180.sendKeyEnt();
					ra180.sendKeys("3300");
					ra180.sendKeyEnt();
					ra180.sendKeys("5511");
					ra180.sendKeyEnt();
					ra180.sendKeys("4325");
					ra180.sendKeyEnt();
					ra180.sendKeys("5621");
					ra180.sendKeyEnt();
					ra180.sendKeys("3201");
					ra180.sendKeyEnt();
					ra180.sendKeys("5104");
					ra180.sendKeyEnt();
					expect(ra180.display.getPlainText()).toMatch(/^PNY:[0-9]{3} $/);
					ra180.sendKeySlt();
				});
			});
			
			it("should display title", function () {
				ra180.sendKey7();
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("  (NYK) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});

		describe("TJK", function () {
			it("should display title", function () {
				ra180.sendKey9();
				expect(ra180.display.getPlainText()).toBe("  (TJK) ");
				ra180.sendKeyEnt();
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});
	});
});
describe("Ra180", function() {
	var ra180;
	
	beforeEach(function() {
		ra180 = new Ra180ViewModel();
		ra180.synchronizationContext = new InstantSynchronizationContext();
	});

	function enterNewPny() {
		ra180.sendKey4();
		ra180.sendKeyEnt(4);
		ra180.sendKeyAnd();
		
		var calc = new Ra180PnyCalculator();
		var pn = calc.generateKeys().pn;
		for (var i=0; i < pn.length; i++) {
			ra180.sendKeys(pn[1]);
			ra180.sendKeyEnt();
		}

		ra180.sendKeySlt();
	}

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

		it("should cycle MOD", function () {
			expect(ra180.mod()).toBe(ra180.MOD_OFF);
			ra180.sendKeyMod();
			expect(ra180.mod()).toBe(ra180.MOD_KLAR);
			ra180.sendKeyMod();
			expect(ra180.mod()).toBe(ra180.MOD_SKYDD);
			ra180.sendKeyMod();
			expect(ra180.mod()).toBe(ra180.MOD_DRELAY);
			ra180.sendKeyMod();
			expect(ra180.mod()).toBe(ra180.MOD_OFF);
		});

		it("should cycle CHANNEL", function () {
			expect(ra180.channel()).toBe(1);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(2);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(3);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(4);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(5);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(6);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(7);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(8);
			ra180.sendKeyChannel();
			expect(ra180.channel()).toBe(1);
		});

		it("should cycle VOLUME", function () {
			// Volume starts by default at 4...
			expect(ra180.volume()).toBe(4);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(5);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(6);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(7);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(8);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(1);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(2);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(3);
			ra180.sendKeyVolume();
			expect(ra180.volume()).toBe(4);
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
	
	describe("Async tests", function () {
		var synchronizationContext;
	
		beforeEach(function() {
			synchronizationContext = new MockSynchronizationContext();
			ra180.synchronizationContext = synchronizationContext;
		});

		describe("Self test", function () {
			it("should perform self-test on KLAR", function () {
				ra180.setModKlar();
				expect(ra180.display.getPlainText()).toBe("TEST    ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("TEST OK ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("NOLLST  ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("        ");
			});
			
			it("should not delay change of mod observable to KLAR", function () {
				ra180.setModKlar();
				expect(ra180.mod()).toBe(ra180.MOD_KLAR);
			});
			
			it("should not delay change of mod observable to SKYDD", function () {
				ra180.setModSkydd();
				expect(ra180.mod()).toBe(ra180.MOD_SKYDD);
			});
			
			it("should not delay change of mod observable to DRELÄ", function () {
				ra180.setModDRelay();
				expect(ra180.mod()).toBe(ra180.MOD_DRELAY);
			});

			it("should not open menu until self-test is complete", function () {
				ra180.setModKlar();
				ra180.sendKey1();
				expect(ra180.display.getPlainText()).not.toMatch(/^T:/);
			});

			it("should not display NOLLST if entered KDA", function () {
				ra180.setModKlar();
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL * 3);
				enterNewPny();
				ra180.setModOff();
				ra180.setModKlar();
				expect(ra180.display.getPlainText()).toBe("TEST    ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("TEST OK ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("        ");
			});
			
			it("should perform self-test after RESET", function () {
				ra180.setModKlar();
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL * 3);
				enterNewPny();
				ra180.sendKeyReset();
				expect(ra180.display.getPlainText()).toBe("TEST    ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("TEST OK ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("NOLLST  ");
				synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				expect(ra180.display.getPlainText()).toBe("        ");
			});
		});
		
		describe("TID", function () {
			beforeEach(function () {
				ra180.setModKlar();
				ra180.synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				ra180.synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
				ra180.synchronizationContext.tick(ra180.SELFTEST_INTERVAL);
			});
			describe("should tick", function () {
				it("each second", function() {
					ra180.sendKey1();
					expect(ra180.display.getPlainText()).toBe("T:000000");
					synchronizationContext.tick(1000);
					expect(ra180.display.getPlainText()).toBe("T:000001");
					synchronizationContext.tick(1000);
					expect(ra180.display.getPlainText()).toBe("T:000002");
				});
				it("each minute", function() {
					ra180.sendKey1();
					expect(ra180.display.getPlainText()).toBe("T:000000");
					synchronizationContext.tick(60 * 1000);
					expect(ra180.display.getPlainText()).toBe("T:000100");
					synchronizationContext.tick(60 * 1000);
					expect(ra180.display.getPlainText()).toBe("T:000200");
				});
				it("each hour", function() {
					ra180.sendKey1();
					expect(ra180.display.getPlainText()).toBe("T:000000");
					synchronizationContext.tick(60 * 60 * 1000);
					expect(ra180.display.getPlainText()).toBe("T:010000");
					synchronizationContext.tick(60 * 60 * 1000);
					expect(ra180.display.getPlainText()).toBe("T:020000");
				});
				it("each day", function() {
					ra180.sendKey1();
					ra180.sendKeyEnt();
					expect(ra180.display.getPlainText()).toBe("DAT:0101");
					synchronizationContext.tick(24 * 60 * 60 * 1000);
					expect(ra180.display.getPlainText()).toBe("DAT:0102");
					synchronizationContext.tick(24 * 60 * 60 * 1000);
					expect(ra180.display.getPlainText()).toBe("DAT:0103");
				});
			});

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

			describe("when changing channel", function () {
				it("should refresh FR", function () {
					ra180.sendKey4();

					ra180.setChannel2();
					var channel2 = ra180.display.getPlainText();
					ra180.setChannel3();
					var channel3 = ra180.display.getPlainText();
					ra180.setChannel4();
					var channel4 = ra180.display.getPlainText();
					ra180.setChannel5();
					var channel5 = ra180.display.getPlainText();
					ra180.setChannel6();
					var channel6 = ra180.display.getPlainText();
					ra180.setChannel7();
					var channel7 = ra180.display.getPlainText();
					ra180.setChannel8();
					var channel8 = ra180.display.getPlainText();
					ra180.setChannel1();
					var channel1 = ra180.display.getPlainText();

					expect(channel1).not.toBe(channel2);
					expect(channel2).not.toBe(channel3);
					expect(channel3).not.toBe(channel4);
					expect(channel4).not.toBe(channel5);
					expect(channel5).not.toBe(channel6);
					expect(channel6).not.toBe(channel7);
					expect(channel7).not.toBe(channel8);
					expect(channel8).not.toBe(channel1);
				});
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
			
			it("should not allow AND when no PNY has been entered", function () {
				ra180.sendKey7();
				expect(ra180.display.getPlainText()).toBe("NYK=### ");
				ra180.sendKeyAnd();
				expect(ra180.display.getPlainText()).toBe("NYK=### ");
			});

			describe("when entered passive key", function () {
				beforeEach(function () {
					enterNewPny();
				});
				
				it("should select entered PNY", function () {
					ra180.sendKey7();
					expect(ra180.display.getPlainText()).toBe("NYK:### ");
					ra180.sendKeyAnd();
					expect(ra180.display.getPlainText()).toMatch(/^NYK:[0-9]{3} $/);
				});
				
				it("should be possible to de-select active key", function () {
					ra180.sendKey7();
					expect(ra180.display.getPlainText()).toBe("NYK:### ");
					ra180.sendKeyAnd();
					expect(ra180.display.getPlainText()).toMatch(/^NYK:[0-9]{3} $/);
					ra180.sendKeyAnd();
					expect(ra180.display.getPlainText()).toBe("NYK:### ");
				});

				describe("twice", function () {
					beforeEach(function () {
						// Select already entered passive key as the active key.
						ra180.sendKey7();
						ra180.sendKeyAnd();
						ra180.sendKeySlt();

						// Now we have no passive key, enter a new passive key.
						enterNewPny();
					});

					it("should cycle all PNYs", function () {
						ra180.sendKey7();
						ra180.sendKeyAnd();
						expect(ra180.display.getPlainText()).toMatch(/^NYK:[0-9]{3} $/);
					});
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
	
		describe("EFF", function () {
			it("is LÅG by default", function() {
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:LÅG ");
				ra180.sendKeySlt();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:LÅG ");
			});

			it("can change to NRM", function() {
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:NRM ");
				ra180.sendKeySlt();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:NRM ");
			});

			it("can change to HÖG", function() {
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:HÖG ");
				ra180.sendKeySlt();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:HÖG ");
			});

			it("can change back to LÅG", function() {
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:LÅG ");
				ra180.sendKeySlt();
				ra180.sendKeyEff();
				expect(ra180.display.getPlainText()).toBe("EFF:LÅG ");
			});
		});
	});
});

describe("MockSynchronizationContext", function () {
	var synchronizationContext;
	var listener;

	beforeEach(function() {
		synchronizationContext = new MockSynchronizationContext();
		listener = {
			callback: function() {}
		};

		spyOn(listener, 'callback');
	});

	describe("setInterval", function () {		
		it("should never call listener if interval has not elapsed", function () {
			synchronizationContext.setInterval(listener.callback, 1000);
			synchronizationContext.tick(999);
			expect(listener.callback.calls.count()).toEqual(0);
		});
		it("should call listener once", function () {
			synchronizationContext.setInterval(listener.callback, 1000);
			synchronizationContext.tick(1000);
			expect(listener.callback.calls.count()).toEqual(1);
		});
		it("should call listener once, when passed", function () {
			synchronizationContext.setInterval(listener.callback, 1000);
			synchronizationContext.tick(1999);
			expect(listener.callback.calls.count()).toEqual(1);
		});
		it("should call listener twice", function () {
			synchronizationContext.setInterval(listener.callback, 1000);
			synchronizationContext.tick(2000);
			expect(listener.callback.calls.count()).toEqual(2);
		});
		it("should cancel", function () {
			var id = synchronizationContext.setTimeout(listener.callback, 1000);
			synchronizationContext.clearInterval(id);
			synchronizationContext.tick(1000);
			expect(listener.callback.calls.count()).toEqual(0);
		});
	});

	describe("setTimeout", function () {
		it("should never call listener if delay has not elapsed", function () {
			synchronizationContext.setTimeout(listener.callback, 1000);
			synchronizationContext.tick(999);
			expect(listener.callback.calls.count()).toEqual(0);
		});
		it("should call listener once when delay has elapsed", function () {
			synchronizationContext.setTimeout(listener.callback, 1000);
			synchronizationContext.tick(1000);
			expect(listener.callback.calls.count()).toEqual(1);
		});
		it("should call listener once even if delay has elapsed multiple times", function () {
			synchronizationContext.setTimeout(listener.callback, 1000);
			synchronizationContext.tick(2000);
			expect(listener.callback.calls.count()).toEqual(1);
		});
		it("should cancel", function () {
			var id = synchronizationContext.setTimeout(listener.callback, 1000);
			synchronizationContext.clearTimeout(id);
			synchronizationContext.tick(1000);
			expect(listener.callback.calls.count()).toEqual(0);
		});
	});
});
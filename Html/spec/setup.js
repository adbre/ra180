// The specs were written for jasmine; these are the only jasmine-isms they use.
import { vi } from 'vitest';

globalThis.spyOn = function (object, method) {
    var spy = vi.spyOn(object, method);
    spy.and = { callThrough: function () { return spy; } };
    spy.calls = { count: function () { return spy.mock.calls.length; } };
    return spy;
};

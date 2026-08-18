ra180
=====

Ra180 simulation lab — a browser mockup of the Ra180 radio, and the .NET
experiments that came before it. This predates
[dart380-js](https://github.com/adbre/dart380-js), which took the same idea
further.

## Running the browser version

    npm install
    npm run dev       # http://localhost:3000, rebuilds and reloads on change
    npm test          # 140 specs

One-shot build into `Html/dist/`:

    npm run build

Click **KLAR** on the MOD dial to start the radio; it runs a self-test and
reports `TEST OK`. `1` opens TID, `4` opens KDA, `7` opens NYK, `ENT` steps
through submenus.

WebRTC voice (`Html/src/RtcRadio.js`) is disabled: it needs an XSockets.NET
broker, and both XSockets and the host it ran on are gone. Set `BROKER_URI` in
`Html/app.js` if you ever put a compatible one up.

## C42A.Ra180

The original C# implementation: a WinForms client, an XSockets host and the
`Infrastructure` library with its tests. .NET Framework era, not currently
buildable — kept for reference.

import * as esbuild from 'esbuild';

const watch = process.argv.includes('--watch');
const PORT = 3000;

const ctx = await esbuild.context({
    entryPoints: ['Html/app.js'],
    outfile: 'Html/dist/app.js',
    bundle: true,
    sourcemap: true,
    target: 'es2020',
    logLevel: 'info',
});

if (!watch) {
    await ctx.rebuild();
    await ctx.dispose();
    process.exit(0);
}

await ctx.watch();

const { hosts, port } = await ctx.serve({ servedir: 'Html', port: PORT });
console.log(`\n  Ra180 at http://${hosts[0] ?? 'localhost'}:${port}\n`);
